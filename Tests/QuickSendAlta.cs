#nullable enable
using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Verifactu.Models.Common;
using Verifactu.Models.Common.Catalogos;
using Verifactu.Models.Common.Hash;
using Verifactu.Models.Operaciones;

using Verifactu.Wsdl.Mensajes;
using Verifactu.Wsdl;
using System.Net.Security;
using System.Net;
using System.Security.Authentication;
using System.Xml;
using System.Xml.Serialization;
using Verifactu.Wsdl.Soap;

public static class QuickSendAlta
{

    private const string WsdlPath = @"C:\Users\gorka.aldasoro\Desktop\Verifactu\SistemaFacturacion.wsdl";
    private const string PortName = "SistemaVerifactuSelloPruebas";
    private const string OperationName = "RegFactuSistemaFacturacion"; // para logging/soapAction (suele ser "")

    private const string PfxPath = @"C:\DATicketBai\certificado\1234.pfx";
    private const string PfxPassword = "1234";

    public static async Task Main()
    {
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            Console.Error.WriteLine("💥 UnhandledException:\n" + (e.ExceptionObject as Exception));
            Console.ReadLine();
        };

        // 1) Endpoint del PORT correcto
        var endpointUrl = WsdlReader.GetEndpointUrlByPort(WsdlPath, PortName);
        if (string.IsNullOrWhiteSpace(endpointUrl))
        {
            Console.Error.WriteLine($"No se encontró soap:address del port '{PortName}'.");
            return;
        }
        Console.WriteLine($"Endpoint: {endpointUrl}");

        // 2) soapAction (en este binding suele venir "")
        var soapAction = WsdlReader.GetSoapAction(WsdlPath, OperationName) ?? "";
        Console.WriteLine($"SOAPAction: {(string.IsNullOrWhiteSpace(soapAction) ? "(omitido)" : soapAction)}");

        // 3) HttpClient con mTLS
        var http = CreateAeATClient(LoadClientCertificate(PfxPath, PfxPassword));

        // 4) Construir payload correcto
        var alta = BuildAlta();

        // 5) Llamada SOAP
        var soap = new SoapClient(http);
        var request = new EnviarRegistrosRequest
        {
            Cabecera = new Cabecera
            {
                ObligadoEmision = new Identificacion { NombreRazon = "mi empresa", NIF = "B20558805" }
            },
            RegistroFactura = new[]
            {
                new RegistroFactura { // 👉 aquí re-declaras el xmlns:sum en el propio nodo <sum:RegistroFactura>
                    Xmlns = new XmlSerializerNamespaces(
                        new[] { new XmlQualifiedName("sum", Verifactu.Models.Common.VerifactuXmlNamespaces.SuministroLR) }),RegistroAlta = alta }
            }
        };

        // guarda la petición serializada si quieres auditarla:
        // var tmpXml = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes("")); // placeholder
        // File.WriteAllText("last-request.xml", tmpXml);

        var response = await soap.PostAsync<EnviarRegistrosRequest, Verifactu.Models.Respuestas.RespuestaSuministro>(
            endpointUrl,
            soapAction: null, // en tu binding es "", puedes omitirlo
            payload: request
        ); ;

        if (response is null)
        {
            Console.WriteLine("Respuesta nula o deserialización fallida.");
        }
        else
        {
            Console.WriteLine($"Estado: {response.Estado}  Código: {response.CodigoResultado}  Desc: {response.DescripcionResultado}");
            if (response.Detalle != null)
            {
                foreach (var r in response.Detalle)
                {
                    var id = r.IDFactura?.NumSerieFactura ?? r.IDFacturaAnulada?.NumSerieFacturaAnulada ?? "(sin num.serie)";
                    Console.WriteLine($" - {id}: {r.Estado} {r.CodigoErrorRegistro} {r.DescripcionErrorRegistro} CSV={r.CSV}");
                }
            }
        }

        Console.WriteLine("Fin. ENTER para salir."); Console.ReadLine();
    }

    private static EnviarRegistrosRequest BuildRequest(RegistroAlta alta) => new()
    {
        Cabecera = new Cabecera
        {
            ObligadoEmision = new Identificacion { NombreRazon = "mi empresa", NIF = "mi cif" }
        },
        RegistroFactura = new[] { new RegistroFactura { RegistroAlta = alta } }
    };

    private static RegistroAlta BuildAlta()
    {
        var alta = new RegistroAlta
        {
            // y redeclarar xmlns sum1 en RegistroAlta:
            Xmlns = new XmlSerializerNamespaces(
                    new[] { new XmlQualifiedName("sum1", Verifactu.Models.Common.VerifactuXmlNamespaces.SuministroInformacion) }),

            IDVersion = "1.0",
            NombreRazonEmisor = "PRUEBA",
            DescripcionOperacion = "Factura de prueba para Verifactu",
            IDFactura = new Verifactu.Models.Common.IDFactura
            {
                IDEmisorFactura = "B20558805",
                NumSerieFactura = "2025-0002",
                FechaExpedicionFactura = "20-08-2025",
            },
            TipoFactura = "F1",
            Destinatarios = new Verifactu.Models.Common.Destinatarios
            {
                IDDestinatario = new[]
                    {
                        new Verifactu.Models.Common.Identificacion
                        {
                            NombreRazon = "Cliente de Pruebas S.L.",
                            NIF = "B20558813"
                        }
                    }
            },
            Desglose = new Verifactu.Models.Common.Desglose
            {
                DetalleDesglose = new[]
                    {
                        new Verifactu.Models.Common.DetalleDesglose
                        {
                            Impuesto = "01",
                            ClaveRegimen = "01",
                            CalificacionOperacion = "S1",
                            TipoImpositivo = "21.00",
                            BaseImponibleOimporteNoSujeto = "100.00",
                            CuotaRepercutida = "21.00"
                        }
                    }
            },
            CuotaTotal = "21.00",
            ImporteTotal = "121.00",
           /* Encadenamiento = new Encadenamiento
            {
                PrimerRegistro = "S",
                RegistroAnterior = null
            },*/
            
            Encadenamiento = new Encadenamiento
            {
                //PrimerRegistro = "N",
                RegistroAnterior = new RegistroAnterior
                {
                    IDEmisorFactura = "B20558805",
                    NumSerieFactura = "2025-0001",
                    FechaExpedicionFactura = "20-08-2025",
                    Huella = "89ACF6A4CD91356A1D120B2..."
                }
            },
            

            SistemaInformatico = new Verifactu.Models.Common.SistemaInformatico
            {
                NombreSistemaInformatico = "MiSIF Demo",
                NombreRazon = "PRIUEBA",
                NIF = "B20558805",
                IdSistemaInformatico = "77",
                Version = "1.0.0",
                NumeroInstalacion = "1",
                TipoUsoPosibleSoloVerifactu = "S",
                TipoUsoPosibleMultiOT = "N",
                IndicadorMultiplesOT = "N"
            },
            FechaHoraHusoGenRegistro = "2025-08-20T09:30:17Z",
        };

        alta.TipoHuella = "01";
        alta.Huella = HuellaHelper.CalcularHuellaAlta(alta.IDFactura.IDEmisorFactura,alta.IDFactura.NumSerieFactura,alta.IDFactura.FechaExpedicionFactura,alta.TipoFactura,alta.CuotaTotal,alta.ImporteTotal,alta.Encadenamiento.RegistroAnterior.Huella,DateTime.Now);
        return alta;
    }

    private static X509Certificate2 LoadClientCertificate(string pfxPath, string? password)
        => new(pfxPath, password,
               X509KeyStorageFlags.PersistKeySet |
               X509KeyStorageFlags.MachineKeySet |
               X509KeyStorageFlags.Exportable);

    private static HttpClient CreateAeATClient(X509Certificate2 clientCert)
    {
        var handler = new HttpClientHandler
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            CheckCertificateRevocationList = true,
            SslProtocols = SslProtocols.Tls12
        };
        handler.ClientCertificates.Add(clientCert);

        var http = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(90)
        };
        http.DefaultRequestVersion = System.Net.HttpVersion.Version11;
        http.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;

        return http;
    }
}
