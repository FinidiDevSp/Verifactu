#nullable enable
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
    TRequest request)
    where TRequest : class
    where TResponse : class
    {
        // 1) Serializa envelope y guarda petición
        var envelope = new SoapEnvelope<TRequest>
        {
            Xmlns = BuildDefaultNamespaces(),
            Body = new SoapBody<TRequest> { Content = request }
        };

        var xml = Serialize(envelope);
        File.WriteAllText("last-request.xml", xml);

        try
        {
            using var httpReq = new HttpRequestMessage(HttpMethod.Post, endpointUrl)
            {
                Version = new Version(1, 1), // fuerza HTTP/1.1 (algunos endpoints fallan con HTTP/2)
                Content = new StringContent(xml, Encoding.UTF8, "text/xml")
            };

            httpReq.Headers.Accept.Clear();
            httpReq.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/xml"));

            // Solo si soapAction NO está vacío
            if (!string.IsNullOrWhiteSpace(soapAction))
                httpReq.Headers.TryAddWithoutValidation("SOAPAction", soapAction);

            using var resp = await _http.SendAsync(httpReq, HttpCompletionOption.ResponseContentRead)
                                        .ConfigureAwait(false);

            var respXml = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            File.WriteAllText("last-response.xml", respXml);

            if (!resp.IsSuccessStatusCode)
            {
                Console.Error.WriteLine($"HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}");
                Console.Error.WriteLine("Respuesta SOAP (posible Fault):\n" + respXml);
                return null; // o lanza una excepción con el contenido
            }

            // Si el servidor devuelve Fault con 200 OK (pasa a veces),
            // intenta detectar el nodo Fault:
            if (respXml.Contains("<Fault") || respXml.Contains(":Fault"))
            {
                Console.Error.WriteLine("SOAP Fault con HTTP 200:\n" + respXml);
                return null;
            }

            return DeserializeResponse<TResponse>(respXml);
        }
        catch (HttpRequestException ex)
        {
            File.WriteAllText("last-http-exception.txt", ex.ToString());
            Console.Error.WriteLine("❌ HttpRequestException:\n" + ex);
            return null;
        }
        catch (Exception ex)
        {
            File.WriteAllText("last-exception.txt", ex.ToString());
            Console.Error.WriteLine("❌ Exception:\n" + ex);
            return null;
        }
    }


    public static HttpClient CreateAeATClient(string pfxPath, string pfxPassword)
    {
        var handler = new HttpClientHandler
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            SslProtocols = System.Security.Authentication.SslProtocols.Tls12
                           | System.Security.Authentication.SslProtocols.Tls13,
            CheckCertificateRevocationList = true
        };

        var cert = new X509Certificate2(pfxPath, pfxPassword,
            X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.EphemeralKeySet);
        handler.ClientCertificates.Add(cert);

        // Recomendable: Timeout más holgado
        var http = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(60)
        };
        return http;
    }

    private static System.Xml.Serialization.XmlSerializerNamespaces BuildDefaultNamespaces()
    {
        var ns = new XmlSerializerNamespaces();
        ns.Add("soapenv", Verifactu.Models.Common.VerifactuXmlNamespaces.SoapEnv);
        // Puedes añadir aquí prefijos/URIs adicionales si el servidor los requiere.
        return ns;
    }

    private static string Serialize<T>(T obj)
    {
        var ser = new XmlSerializer(typeof(T));
        using var ms = new MemoryStream();
        var settings = new System.Xml.XmlWriterSettings
        {
            Encoding = new UTF8Encoding(false),
            OmitXmlDeclaration = false,
            Indent = true
        };
        using var xw = System.Xml.XmlWriter.Create(ms, settings);
        ser.Serialize(xw, obj);
        return Encoding.UTF8.GetString(ms.ToArray());
    }

    private static T? DeserializeResponse<T>(string xml) where T : class
    {
        var envSer = new XmlSerializer(typeof(SoapEnvelope<T>));
        using var sr = new StringReader(xml);
        var env = (SoapEnvelope<T>?)envSer.Deserialize(sr);
        return env?.Body?.Content; // puede ser null si el Body está vacío o es un Fault
    }
}
