#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Common;

namespace Verifactu.Models.Respuestas;

/// <summary>
/// Respuesta del servicio de validación de registros no Veri*Factu
/// (RespuestaValRegistNoVeriFactu.xsd).
/// </summary>
[XmlRoot("RespuestaValRegistNoVeriFactu", Namespace = VerifactuXmlNamespaces.RespuestaValRegistNoVeriFactu)]
public class RespuestaValRegistNoVeriFactu
{
    [XmlElement("Estado", Order = 0)]
    public string? Estado { get; set; }

    [XmlElement("CodigoResultado", Order = 1)]
    public string? CodigoResultado { get; set; }

    [XmlElement("DescripcionResultado", Order = 2)]
    public string? DescripcionResultado { get; set; }

    [XmlArray("Detalle", Order = 3)]
    [XmlArrayItem("RespuestaRegistro")]
    public RespuestaValNoVFRegistro[]? Detalle { get; set; }
}

/// <summary>
/// Detalle por registro validado en el servicio “no Veri*Factu”.
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.RespuestaValRegistNoVeriFactu)]
public class RespuestaValNoVFRegistro
{
    [XmlElement("IDFactura", Namespace = VerifactuXmlNamespaces.SuministroInformacion)]
    public IDFactura? IDFactura { get; set; }

    [XmlElement("Estado")]
    public string? Estado { get; set; }

    [XmlElement("CodigoErrorRegistro")]
    public string? CodigoErrorRegistro { get; set; }

    [XmlElement("DescripcionErrorRegistro")]
    public string? DescripcionErrorRegistro { get; set; }
}