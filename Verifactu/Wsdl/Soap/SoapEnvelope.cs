#nullable enable
using System.Xml.Serialization;

namespace Verifactu.Wsdl.Soap;

/// <summary>
/// Envelope SOAP 1.1 para serializar peticiones/respuestas.
/// </summary>
[XmlRoot("Envelope", Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.SoapEnv)]
public class SoapEnvelope<TBody>
{
    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces? Xmlns { get; set; }

    [XmlElement("Header", Order = 0)]
    public SoapHeader? Header { get; set; }

    [XmlElement("Body", Order = 1)]
    public SoapBody<TBody>? Body { get; set; }
}

public class SoapHeader
{
    // Puedes añadir aquí cabeceras personalizadas si el WSDL las define.
    // De momento lo dejamos genérico.
}

public class SoapBody<T>
{
    // Contenido real del cuerpo SOAP.
    [XmlElement(Order = 0)]
    public T? Content { get; set; }
}