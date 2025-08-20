#nullable enable
using System.Xml.Serialization;

namespace Verifactu.Models.Wsdl.Soap;

[XmlRoot("Envelope", Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.SoapEnv)]
public class SoapEnvelope
{
    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces? Xmlns { get; set; }

    [XmlElement("Header", Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.SoapEnv)]
    public SoapHeader? Header { get; set; }

    [XmlElement("Body", Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.SoapEnv)]
    public SoapBody Body { get; set; } = new();
}

public class SoapHeader { }

public class SoapBody
{
    // Inserta el elemento real tal cual (RegFactuSistemaFacturacion)
    [XmlAnyElement]
    public System.Xml.XmlElement? Payload { get; set; }
}