#nullable enable
using System.Xml.Serialization;

namespace Verifactu.Models.Common;

/// <summary>
/// Identificador de registro de facturación (alta / consulta).
/// &lt;IDEmisorFactura&gt; + &lt;NumSerieFactura&gt; + &lt;FechaExpedicionFactura&gt;.
/// Ojo: la fecha en los ejemplos usa formato dd-MM-yyyy, por lo que
/// aquí se modela como string para mantener el formato literal.
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.SuministroInformacion)]
public class IDFactura
{
    [XmlElement("IDEmisorFactura", Order = 0)]
    public string? IDEmisorFactura { get; set; }

    [XmlElement("NumSerieFactura", Order = 1)]
    public string? NumSerieFactura { get; set; }

    /// <summary>Formato de ejemplo: "13-09-2024".</summary>
    [XmlElement("FechaExpedicionFactura", Order = 2)]
    public string? FechaExpedicionFactura { get; set; }
}

/// <summary>
/// Variante usada en &lt;RegistroAnulacion&gt;:
/// &lt;IDEmisorFacturaAnulada&gt;, &lt;NumSerieFacturaAnulada&gt;, &lt;FechaExpedicionFacturaAnulada&gt;.
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.SuministroInformacion)]
public class IDFacturaAnulada
{
    [XmlElement("IDEmisorFacturaAnulada", Order = 0)]
    public string? IDEmisorFacturaAnulada { get; set; }

    [XmlElement("NumSerieFacturaAnulada", Order = 1)]
    public string? NumSerieFacturaAnulada { get; set; }

    /// <summary>Formato de ejemplo: "13-09-2024".</summary>
    [XmlElement("FechaExpedicionFacturaAnulada", Order = 2)]
    public string? FechaExpedicionFacturaAnulada { get; set; }
}

/// <summary>
/// Bloque &lt;ClavePaginacion&gt; que la AEAT usa para respuestas y para la siguiente página en consultas.
/// Idéntico a IDFactura estándar (sin sufijo "Anulada").
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.SuministroInformacion)]
public class ClavePaginacion
{
    [XmlElement("IDEmisorFactura", Order = 0)]
    public string? IDEmisorFactura { get; set; }

    [XmlElement("NumSerieFactura", Order = 1)]
    public string? NumSerieFactura { get; set; }

    [XmlElement("FechaExpedicionFactura", Order = 2)]
    public string? FechaExpedicionFactura { get; set; }
}
