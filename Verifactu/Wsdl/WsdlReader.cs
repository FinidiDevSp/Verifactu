#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Verifactu.Wsdl;

/// <summary>
/// Lector simple de WSDL para extraer la URL del endpoint y, si procede, SOAPAction por operación.
/// </summary>
public static class WsdlReader
{
    private static readonly XNamespace nsWsdl = "http://schemas.xmlsoap.org/wsdl/";
    private static readonly XNamespace nsSoap = "http://schemas.xmlsoap.org/wsdl/soap/";
    private static readonly XNamespace nsSoap12 = "http://schemas.xmlsoap.org/wsdl/soap12/";

    public static string? GetFirstEndpointUrl(string wsdlPath)
    {
        var x = XDocument.Load(wsdlPath);
        // Busca <service>/<port>/<soap:address location="...">
        var addr = x.Descendants(nsSoap + "address").Attributes("location").Select(a => a.Value).FirstOrDefault()
                ?? x.Descendants(nsSoap12 + "address").Attributes("location").Select(a => a.Value).FirstOrDefault();
        return addr;
    }

    /// <summary>
    /// Intenta obtener el SOAPAction de una operación por su nombre de operación (binding).
    /// </summary>
    public static string? GetSoapAction(string wsdlPath, string operationName)
    {
        var x = XDocument.Load(wsdlPath);

        // Busca operaciones en soap 1.1 o 1.2
        var soapOps11 = x.Descendants(nsWsdl + "binding")
            .Descendants(nsWsdl + "operation")
            .Select(op => new
            {
                Name = (string?)op.Attribute("name"),
                SoapAction = (string?)op.Element(nsSoap + "operation")?.Attribute("soapAction")
            });

        var soapOps12 = x.Descendants(nsWsdl + "binding")
            .Descendants(nsWsdl + "operation")
            .Select(op => new
            {
                Name = (string?)op.Attribute("name"),
                SoapAction = (string?)op.Element(nsSoap12 + "operation")?.Attribute("soapAction")
            });

        var ops = soapOps11.Concat(soapOps12);
        var match = ops.FirstOrDefault(o => string.Equals(o.Name, operationName, StringComparison.OrdinalIgnoreCase));
        return match?.SoapAction;
    }
}
