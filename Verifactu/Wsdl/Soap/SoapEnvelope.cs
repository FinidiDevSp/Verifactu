#nullable enable
using System.Xml.Serialization;

namespace Verifactu.Wsdl.Soap;

public static class SoapNamespaces
{
    public const string SoapEnv = Verifactu.Models.Common.VerifactuXmlNamespaces.SoapEnv;
}

[XmlRoot("Envelope", Namespace = SoapNamespaces.SoapEnv)]
public class SoapEnvelope
{
    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces? Xmlns { get; set; }

    [XmlElement("Header", Namespace = SoapNamespaces.SoapEnv, Order = 0)]
    public SoapHeader? Header { get; set; }

    [XmlElement("Body", Namespace = SoapNamespaces.SoapEnv, Order = 1)]
    public SoapBody Body { get; set; } = new();
}

public class SoapHeader { }

public class SoapBody
{
    // 👇 Clave: el payload se inserta tal cual, sin nombre "Any" ni "Content"
    [XmlAnyElement]
    public System.Xml.XmlElement? Payload { get; set; }
}