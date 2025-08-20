#nullable enable
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Verifactu.Wsdl.Soap;

namespace Verifactu.Wsdl.Soap;

/// <summary>
/// Cliente SOAP 1.1 ligero. Serializa objetos a XML dentro del Envelope y hace POST.
/// </summary>
public sealed class SoapClient
{
    private readonly HttpClient _http;
    public SoapClient(HttpClient http) => _http = http;

    /// <summary>
    /// Envía una petición SOAP 1.1 y deserializa la respuesta.
    /// </summary>
    /// <typeparam name="TRequest">Tipo del objeto de la petición (el que va en Body).</typeparam>
    /// <typeparam name="TResponse">Tipo del objeto de la respuesta (el que viene en Body).</typeparam>
    /// <param name="endpointUrl">URL del servicio SOAP.</param>
    /// <param name="soapAction">Cabecera SOAPAction (puede ser requerida por el servidor). Si no lo sabes, déjalo "".</param>
    /// <param name="request">Objeto de petición.</param>
    public async Task<TResponse?> PostAsync<TRequest, TResponse>(
        string endpointUrl,
        string? soapAction,
        TRequest payload)
        where TRequest : class
        where TResponse : class
    {
        try
        {
            var env = new SoapEnvelope
            {
                Xmlns = new XmlSerializerNamespaces(new[]
                {
                    new XmlQualifiedName("soapenv", Verifactu.Models.Common.VerifactuXmlNamespaces.SoapEnv)
                }),
                Body = new SoapBody
                {
                    // Insertamos el elemento raíz real (RegFactuSistemaFacturacion, etc.)
                    Payload = SerializePayloadToXmlElement(payload)
                }
            };

            var xml = Serialize(env);
            try
            {
                File.WriteAllText("last-request.xml", xml);
            }
            catch (Exception logEx)
            {
                Console.Error.WriteLine("No se pudo guardar last-request.xml:\n" + logEx);
            }

            using var req = new HttpRequestMessage(HttpMethod.Post, endpointUrl)
            {
                Version = System.Net.HttpVersion.Version11,
                Content = new StringContent(xml, Encoding.UTF8, "text/xml")
            };

            // En este binding el SOAPAction suele ser "", puedes omitirlo:
            if (!string.IsNullOrWhiteSpace(soapAction))
                req.Headers.TryAddWithoutValidation("SOAPAction", soapAction);

            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
            var respXml = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            try
            {
                File.WriteAllText("last-response.xml", respXml);
            }
            catch (Exception logEx)
            {
                Console.Error.WriteLine("No se pudo guardar last-response.xml:\n" + logEx);
            }

            if (!resp.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}\n{respXml}");

            }


            // Deserializamos el Envelope de respuesta capturando el primer hijo del Body
            var ser = new XmlSerializer(typeof(SoapEnvelopeResponse));
            using var sr = new StringReader(respXml);
            if (ser.Deserialize(sr) is not SoapEnvelopeResponse envResp || envResp.Body?.Any is null)
                return null;

            // Deserializa el elemento real del Body al tipo esperado de respuesta
            var innerSer = new XmlSerializer(typeof(TResponse));
            using var innerReader = new XmlNodeReader(envResp.Body.Any);
            return innerSer.Deserialize(innerReader) as TResponse;
        }
        catch (HttpRequestException ex)
        {
            try { File.WriteAllText("last-http-exception.txt", ex.ToString()); } catch { }
            Console.Error.WriteLine("❌ HttpRequestException:\n" + ex);
            return null;
        }
        catch (Exception ex)
        {
            try { File.WriteAllText("last-exception.txt", ex.ToString()); } catch { }
            Console.Error.WriteLine("❌ Exception:\n" + ex);
            return null;
        }
    }

    private static string Serialize<T>(T obj)
    {
        var ser = new XmlSerializer(typeof(T));
        var settings = new System.Xml.XmlWriterSettings
        {
            Encoding = new UTF8Encoding(false),
            OmitXmlDeclaration = false,
            Indent = true
        };
        using var ms = new MemoryStream();
        using var xw = System.Xml.XmlWriter.Create(ms, settings);
        ser.Serialize(xw, obj);
        return Encoding.UTF8.GetString(ms.ToArray());
    }

    // Serializa el payload con sus propios namespaces sum/sum1 para que salgan en su raíz
    private static XmlElement SerializePayloadToXmlElement<T>(T obj) where T : class
    {
        var ns = new XmlSerializerNamespaces();
        ns.Add("sum", Verifactu.Models.Common.VerifactuXmlNamespaces.SuministroLR);
        ns.Add("sum1", Verifactu.Models.Common.VerifactuXmlNamespaces.SuministroInformacion);
        ns.Add("xd", Verifactu.Models.Common.VerifactuXmlNamespaces.XmlDsig); // opcional

        var ser = new XmlSerializer(typeof(T));
        var settings = new System.Xml.XmlWriterSettings { OmitXmlDeclaration = true, Encoding = new UTF8Encoding(false) };
        var sb = new StringBuilder();
        using (var xw = XmlWriter.Create(sb, settings))
            ser.Serialize(xw, obj, ns);

        var doc = new XmlDocument();
        doc.LoadXml(sb.ToString());
        return doc.DocumentElement!;
    }
}

// Envelope solo para leer respuestas (captura "cualquier" hijo en el Body)
[XmlRoot("Envelope", Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.SoapEnv)]
public class SoapEnvelopeResponse
{
    [XmlElement("Body", Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.SoapEnv)]
    public SoapBodyResponse? Body { get; set; }
}

public class SoapBodyResponse
{
    [XmlAnyElement]
    public XmlElement? Any { get; set; }
}