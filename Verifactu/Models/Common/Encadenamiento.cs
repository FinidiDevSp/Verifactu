#nullable enable
using System.Xml.Serialization;

namespace Verifactu.Models.Common;

/// <summary>
/// Bloque &lt;Encadenamiento&gt; con &lt;RegistroAnterior&gt; que contiene la huella
/// y la referencia al registro inmediatamente anterior generado por el SIF.
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.SuministroInformacion)]
public class Encadenamiento
{
    [XmlElement("RegistroAnterior", Order = 0)]
    public RegistroAnterior? RegistroAnterior { get; set; }
}

/// <summary>
/// &lt;RegistroAnterior&gt; con Emisor, Serie, Fecha y &lt;Huella&gt;.
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.SuministroInformacion)]
public class RegistroAnterior
{
    [XmlElement("IDEmisorFactura", Order = 0)]
    public string? IDEmisorFactura { get; set; }

    [XmlElement("NumSerieFactura", Order = 1)]
    public string? NumSerieFactura { get; set; }

    /// <summary>Formato de ejemplo: "13-09-2024".</summary>
    [XmlElement("FechaExpedicionFactura", Order = 2)]
    public string? FechaExpedicionFactura { get; set; }

    /// <summary>Huella (hash) del registro anterior segun doc. de huella.</summary>
    [XmlElement("Huella", Order = 3)]
    public string? Huella { get; set; }
}
