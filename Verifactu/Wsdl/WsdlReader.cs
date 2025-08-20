#nullable enable
using System.Linq;
using System.Xml.Linq;

namespace Verifactu.Wsdl;

public static class WsdlReader
{
    private static readonly XNamespace nsWsdl = "http://schemas.xmlsoap.org/wsdl/";
    private static readonly XNamespace nsSoap = "http://schemas.xmlsoap.org/wsdl/soap/";
    private static readonly XNamespace nsSoap12 = "http://schemas.xmlsoap.org/wsdl/soap12/";

    // ✅ Nuevo: coge la URL del endpoint por NOMBRE de port (no el primero que aparezca)
    public static string? GetEndpointUrlByPort(string wsdlPath, string portName)
    {
        var x = XDocument.Load(wsdlPath);
        var port = x.Descendants(nsWsdl + "port")
            .FirstOrDefault(p => (string?)p.Attribute("name") == portName);
        return port?.Element(nsSoap + "address")?.Attribute("location")?.Value
               ?? port?.Element(nsSoap12 + "address")?.Attribute("location")?.Value;
    }

    // Para este WSDL, las operaciones vienen con soapAction="" → opcional/no enviar
    public static string? GetSoapAction(string wsdlPath, string operationName)
    {
        var x = XDocument.Load(wsdlPath);
        var ops =
            x.Descendants(nsWsdl + "binding")
                .Descendants(nsWsdl + "operation")
                .Select(op => new
                {
                    Name = (string?)op.Attribute("name"),
                    SoapAction11 = (string?)op.Element(nsSoap + "operation")?.Attribute("soapAction"),
                    SoapAction12 = (string?)op.Element(nsSoap12 + "operation")?.Attribute("soapAction"),
                });

        var match = ops.FirstOrDefault(o => string.Equals(o.Name, operationName, System.StringComparison.OrdinalIgnoreCase));
        return match?.SoapAction11 ?? match?.SoapAction12; // suele ser ""
    }
}