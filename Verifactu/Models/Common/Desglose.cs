#nullable enable
using System.Xml.Serialization;

namespace Verifactu.Models.Common;

/// <summary>
/// Contenedor &lt;Desglose&gt; con N elementos &lt;DetalleDesglose&gt;.
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.SuministroInformacion)]
public class Desglose
{
    [XmlElement("DetalleDesglose", Order = 0)]
    public DetalleDesglose[]? DetalleDesglose { get; set; }
}

/// <summary>
/// Línea de desglose con impuestos y bases.
/// Nota: se modela todo como string para preservar el formato exacto tal y como va por XML.
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.SuministroInformacion)]
public class DetalleDesglose
{
    /// <summary>Impuesto: 01=IVA, 02=IPSI, 03=IGIC, 99=Otros (según especificación).</summary>
    [XmlElement("Impuesto", Order = 0)]
    public string? Impuesto { get; set; }

    [XmlElement("ClaveRegimen", Order = 1)]
    public string? ClaveRegimen { get; set; }

    [XmlElement("CalificacionOperacion", Order = 2)]
    public string? CalificacionOperacion { get; set; }

    [XmlElement("TipoImpositivo", Order = 3)]
    public string? TipoImpositivo { get; set; }

    [XmlElement("BaseImponibleOimporteNoSujeto", Order = 4)]
    public string? BaseImponibleOimporteNoSujeto { get; set; }

    [XmlElement("CuotaRepercutida", Order = 5)]
    public string? CuotaRepercutida { get; set; }
}