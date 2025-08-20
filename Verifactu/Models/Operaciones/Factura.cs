#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Common;

namespace Verifactu.Models.Operaciones;

/// <summary>
/// Contenido de &lt;RegistroAlta&gt; (SuministroInformacion.xsd).
/// Incluye identificadores, destinatarios, desglose, totales y metadatos.
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.SuministroInformacion)]
public class Factura
{
    /// <summary>Versión del esquema (ej.: "1.0").</summary>
    [XmlElement("IDVersion", Order = 0)]
    public string? IDVersion { get; set; }

    /// <summary>Identificador único del registro.</summary>
    [XmlElement("IDFactura", Order = 1)]
    public IDFactura? IDFactura { get; set; }

    /// <summary>Nombre/Razón social del emisor (opcional en algunos flujos).</summary>
    [XmlElement("NombreRazonEmisor", Order = 2)]
    public string? NombreRazonEmisor { get; set; }

    /// <summary>Indicador de subsanación ("S"/"N").</summary>
    [XmlElement("Subsanacion", Order = 3)]
    public string? Subsanacion { get; set; }

    /// <summary>Indicador de rechazo previo (p.ej. "S"/"N" o "X" según casuística de alta por rechazo).</summary>
    [XmlElement("RechazoPrevio", Order = 4)]
    public string? RechazoPrevio { get; set; }

    /// <summary>Tipo de factura (p.ej., F1, F2...). Lista de códigos.</summary>
    [XmlElement("TipoFactura", Order = 5)]
    public string? TipoFactura { get; set; }

    /// <summary>Descripción libre de la operación.</summary>
    [XmlElement("DescripcionOperacion", Order = 6)]
    public string? DescripcionOperacion { get; set; }

    /// <summary>Lista de destinatarios (uno o varios).</summary>
    [XmlElement("Destinatarios", Order = 7)]
    public Destinatarios? Destinatarios { get; set; }

    /// <summary>Desglose de impuestos y bases.</summary>
    [XmlElement("Desglose", Order = 8)]
    public Desglose? Desglose { get; set; }

    /// <summary>Cuota total (suma de cuotas).</summary>
    [XmlElement("CuotaTotal", Order = 9)]
    public string? CuotaTotal { get; set; }

    /// <summary>Importe total de la factura.</summary>
    [XmlElement("ImporteTotal", Order = 10)]
    public string? ImporteTotal { get; set; }

    /// <summary>Cadena de encadenamiento: referencia a registro anterior y su huella.</summary>
    [XmlElement("Encadenamiento", Order = 11)]
    public Encadenamiento? Encadenamiento { get; set; }

    /// <summary>Datos del sistema informático de facturación que generó el registro.</summary>
    [XmlElement("SistemaInformatico", Order = 12)]
    public SistemaInformatico? SistemaInformatico { get; set; }

    /// <summary>Fecha/hora con huso del servidor del SIF en formato ISO-8601.</summary>
    [XmlElement("FechaHoraHusoGenRegistro", Order = 13)]
    public string? FechaHoraHusoGenRegistro { get; set; }

    /// <summary>Tipo de huella (p.ej. "01").</summary>
    [XmlElement("TipoHuella", Order = 14)]
    public string? TipoHuella { get; set; }

    /// <summary>Huella (hash) del registro.</summary>
    [XmlElement("Huella", Order = 15)]
    public string? Huella { get; set; }

    /// <summary>Referencia externa opcional, útil para correlación con el SIF.</summary>
    [XmlElement("RefExterna", Order = 16)]
    public string? RefExterna { get; set; }
}
