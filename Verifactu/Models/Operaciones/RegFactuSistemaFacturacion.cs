#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Common;

namespace Verifactu.Models.Operaciones;

/// <summary>
/// Contenedor principal &lt;RegFactuSistemaFacturacion&gt; (SuministroLR.xsd).
/// Lleva Cabecera y una lista de &lt;RegistroFactura&gt;.
/// </summary>
[XmlRoot("RegFactuSistemaFacturacion", Namespace = VerifactuXmlNamespaces.SuministroLR)]
public class RegFactuSistemaFacturacion
{
    [XmlElement("Cabecera", Order = 0)]
    public Cabecera? Cabecera { get; set; }

    [XmlElement("RegistroFactura", Order = 1)]
    public RegistroFactura[]? RegistroFactura { get; set; }
}