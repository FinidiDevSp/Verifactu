using System.Xml.Serialization;

namespace Verifactu.Models.Common;

/// <summary>
/// Bloque &lt;PeriodoImputacion&gt; con &lt;Ejercicio&gt; (YYYY) y &lt;Periodo&gt; ("01".."12").
/// En ejemplos aparece dentro de filtros de consulta y en respuestas.
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.SuministroInformacion)]
public class PeriodoImputacion
{
    /// <summary>Ejercicio (YYYY).</summary>
    [XmlElement("Ejercicio", Order = 0)]
    public string? Ejercicio { get; set; }

    /// <summary>
    /// Periodo (dos dígitos "01".."12", ver lista L2C en la especificación).
    /// </summary>
    [XmlElement("Periodo", Order = 1)]
    public string? Periodo { get; set; }
}