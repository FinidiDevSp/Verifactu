#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Common;

namespace Verifactu.Models.Operaciones;

/// <summary>
/// &lt;RegistroAnulacion&gt; (SuministroInformacion.xsd).
/// Incluye identificación de la factura a anular y los metadatos comunes.
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.SuministroInformacion)]
public class RegistroAnulacion
{
    [XmlElement("IDVersion", Order = 0)]
    public string? IDVersion { get; set; }

    [XmlElement("IDFactura", Order = 1)]
    public IDFacturaAnulada? IDFacturaAnulada { get; set; }

    /// <summary>Marca en caso de anulación tras rechazo previo de la propia anulación ("S"/"N").</summary>
    [XmlElement("RechazoPrevio", Order = 2)]
    public string? RechazoPrevio { get; set; }

    /// <summary>Marca en caso de anulación sin registro previo en AEAT ("S"/"N").</summary>
    [XmlElement("SinRegistroPrevio", Order = 3)]
    public string? SinRegistroPrevio { get; set; }

    [XmlElement("Encadenamiento", Order = 4)]
    public Encadenamiento? Encadenamiento { get; set; }

    [XmlElement("SistemaInformatico", Order = 5)]
    public SistemaInformatico? SistemaInformatico { get; set; }

    [XmlElement("FechaHoraHusoGenRegistro", Order = 6)]
    public string? FechaHoraHusoGenRegistro { get; set; }

    [XmlElement("TipoHuella", Order = 7)]
    public string? TipoHuella { get; set; }

    [XmlElement("Huella", Order = 8)]
    public string? Huella { get; set; }

    [XmlElement("RefExterna", Order = 9)]
    public string? RefExterna { get; set; }
}