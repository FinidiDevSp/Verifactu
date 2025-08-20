#nullable enable
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Verifactu.Models.Wsdl.Soap;
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
        string endpointUrl, string? soapAction, TRequest payload)
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
                    // 👇 OJO: aquí va el elemento raíz real
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

            if (!string.IsNullOrWhiteSpace(soapAction))
                req.Headers.TryAddWithoutValidation("SOAPAction", soapAction); // en tu binding es "" → puedes omitir

            using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
            var respXml = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            resp.EnsureSuccessStatusCode();


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


            // Envelope de respuesta genérico
            var envSer = new XmlSerializer(typeof(SoapEnvelopeResponse));
            using var sr = new StringReader(respXml);
            if (envSer.Deserialize(sr) is not SoapEnvelopeResponse envResp || envResp.Body?.Any is null)
                return null;

            var innerSer = new XmlSerializer(typeof(TResponse));
            using var xr = new XmlNodeReader(envResp.Body.Any);
            return innerSer.Deserialize(xr) as TResponse;
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

    private static XmlElement SerializePayloadToXmlElement<T>(T obj) where T : class
    {
        // 👉 Fuerza prefijos en el ROOT del payload
        var ns = new XmlSerializerNamespaces();
        ns.Add("sum", Verifactu.Models.Common.VerifactuXmlNamespaces.SuministroLR);
        ns.Add("sum1", Verifactu.Models.Common.VerifactuXmlNamespaces.SuministroInformacion);
        ns.Add("xd", Verifactu.Models.Common.VerifactuXmlNamespaces.XmlDsig); // opcional

        var ser = new XmlSerializer(typeof(T));
        var settings = new XmlWriterSettings { OmitXmlDeclaration = true, Encoding = new UTF8Encoding(false) };
        var sb = new StringBuilder();
        using (var xw = XmlWriter.Create(sb, settings))
            ser.Serialize(xw, obj, ns);

        var doc = new XmlDocument();
        doc.LoadXml(sb.ToString());
        return doc.DocumentElement!;
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
}

[XmlRoot("Envelope", Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.SoapEnv)]
public class SoapEnvelopeResponse
{
    [XmlElement("Body", Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.SoapEnv)]
    public SoapBodyResponse? Body { get; set; }
}
public class SoapBodyResponse { [XmlAnyElement] public XmlElement? Any { get; set; } }