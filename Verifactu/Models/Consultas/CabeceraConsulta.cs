#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Common;

namespace Verifactu.Models.Consultas;

/// <summary>
/// Cabecera de la petición de consulta (ConsultaLR.xsd).
/// Suele incluir el obligado a la emisión y los metadatos del SIF.
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.ConsultaLR)]
public class CabeceraConsulta
{
    [XmlElement("ObligadoEmision", Namespace = VerifactuXmlNamespaces.SuministroInformacion, Order = 0)]
    public Identificacion? ObligadoEmision { get; set; }

    [XmlElement("SistemaInformatico", Namespace = VerifactuXmlNamespaces.SuministroInformacion, Order = 1)]
    public SistemaInformatico? SistemaInformatico { get; set; }
}