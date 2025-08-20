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

public static class QuickSendAlta
{
    // ⚠️ Pon aquí la ruta real al WSDL que tú me pasaste
    private const string WsdlPath = @"C:\Users\gorka.aldasoro\Desktop\Verifactu\SistemaFacturacion.wsdl";

    // Si conoces el nombre exacto de la operación en el binding (p.ej., "EnviarRegistros"),
    // WsdlReader intentará sacar el SOAPAction. Si no, se enviará vacío ("").
    private const string OperationName = "EnviarRegistros"; // ajusta si tu WSDL usa otro nombre

    // Ruta del PFX y su password para MUTUA TLS (cliente)
    private const string CertPath = @"C:\DATicketBai\certificado\1234.pfx";
    private const string CertPassword = "1234";

    public static async Task Main()
    {
        try
        {
            // 1) Obtener endpoint (y opcionalmente SOAPAction) desde el WSDL
            var endpointUrl = WsdlReader.GetFirstEndpointUrl(WsdlPath);
            if (string.IsNullOrWhiteSpace(endpointUrl))
            {
                Console.WriteLine("No se pudo obtener la URL del endpoint desde el WSDL.");
                return;
            }

            var soapAction = WsdlReader.GetSoapAction(WsdlPath, OperationName) ?? "";

            // 2) HttpClient con certificado cliente (mutua TLS)
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12
                               | System.Security.Authentication.SslProtocols.Tls13,
                CheckCertificateRevocationList = true,
                ServerCertificateCustomValidationCallback = (req, cert, chain, sslPolicyErrors) =>
                {
                    // Log diagnóstico (NO devuelvas true a ciegas)
                    File.WriteAllText("server-cert.txt", $"Subject={cert?.Subject}\nIssuer={cert?.Issuer}\nErrors={sslPolicyErrors}");
                    return sslPolicyErrors == System.Net.Security.SslPolicyErrors.None;
                }
            };

            // Carga tu PFX
            var certCli = new X509Certificate2(
                CertPath, CertPassword,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.EphemeralKeySet);
            handler.ClientCertificates.Add(certCli);

            // Log del cliente
            File.WriteAllText("client-cert.txt", $"HasPrivateKey={certCli.HasPrivateKey}, Subject={certCli.Subject}");

            using var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(60) };

            // 3) Cliente del sistema de facturación
            var client = new SistemaFacturacionClient(http, endpointUrl);

            // 4) Construir alta mínima válida
            var hoy = DateTime.Today;
            var fechaEmision = hoy.ToString("dd-MM-yyyy");
            var fechaHoraIso = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

            var alta = new RegistroAlta
            {
                IDVersion = "1.0",
                IDFactura = new IDFactura
                {
                    IDEmisorFactura = "A12345678",
                    NumSerieFactura = "2025-0001",
                    FechaExpedicionFactura = fechaEmision
                },
                TipoFactura = TipoFactura.FacturaCompleta, // "F1"

                Destinatarios = new Verifactu.Models.Common.Destinatarios
                {
                    IDDestinatario = new[]
                    {
                    new Identificacion
                    {
                        NIF = "B76543210",
                        NombreRazon = "Cliente de Pruebas S.L."
                    }
                }
                },

                Desglose = new Verifactu.Models.Common.Desglose
                {
                    DetalleDesglose = new[]
                    {
                    new Verifactu.Models.Common.DetalleDesglose
                    {
                        Impuesto = "01",                  // IVA
                        ClaveRegimen = "01",              // Régimen general
                        CalificacionOperacion = "E",      // Entrega de bienes
                        TipoImpositivo = "21.00",
                        BaseImponibleOimporteNoSujeto = "100.00",
                        CuotaRepercutida = "21.00"
                    }
                }
                },

                CuotaTotal = "21.00",
                ImporteTotal = "121.00",

                Encadenamiento = new Verifactu.Models.Common.Encadenamiento
                {
                    RegistroAnterior = null
                },

                SistemaInformatico = new Verifactu.Models.Common.SistemaInformatico
                {
                    NombreSistemaInformatico = "MiSIF Demo",
                    IdSistemaInformatico = "1001",
                    Version = "1.0.0",
                    NumeroInstalacion = "1",
                    TipoUsoPosibleSoloVerifactu = "S",
                    TipoUsoPosibleMultiOT = "N",
                    IndicadorMultiplesOT = "N"
                },

                FechaHoraHusoGenRegistro = fechaHoraIso,
                // Calculamos la huella con nuestro helper:
                TipoHuella = "01",
                Huella = "" // la rellenamos justo abajo
            };

            alta.Huella = HuellaHelper.ComputeHuellaRegistroAlta(alta);

            var request = new EnviarRegistrosRequest
            {
                Cabecera = new Cabecera
                {
                    ObligadoEmision = new Identificacion
                    {
                        NIF = "A12345678",
                        NombreRazon = "Mi Empresa de Pruebas S.A."
                    }
                },
                RegistroFactura = new[]
                {
                new RegistroFactura { RegistroAlta = alta }
            }
            };

            // 5) Enviar
            var respuesta = await client.EnviarRegistrosAsync(request, soapAction);

            // 6) Revisar resultado
            if (respuesta is null)
            {
                Console.WriteLine("Sin respuesta SOAP o deserialización fallida.");
                return;
            }

            Console.WriteLine($"Endpoint: {endpointUrl}");
            Console.WriteLine($"SOAPAction: {(string.IsNullOrWhiteSpace(soapAction) ? "(vacío)" : soapAction)}");
            Console.WriteLine($"Estado global: {respuesta.Estado}");
            Console.WriteLine($"Cod/Desc: {respuesta.CodigoResultado} - {respuesta.DescripcionResultado}");

            if (respuesta.Detalle != null)
            {
                foreach (var r in respuesta.Detalle)
                {
                    var id = r.IDFactura?.NumSerieFactura ?? r.IDFacturaAnulada?.NumSerieFacturaAnulada ?? "(sin num.serie)";
                    Console.WriteLine($" - {id}: {r.Estado} {r.CodigoErrorRegistro} {r.DescripcionErrorRegistro}");
                    if (!string.IsNullOrEmpty(r.CSV)) Console.WriteLine($"   CSV: {r.CSV}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}
