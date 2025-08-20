#nullable enable
using System.Xml.Serialization;

namespace Verifactu.Models.Common;

/// <summary>
/// Bloque &lt;SistemaInformatico&gt; con los metadatos del SIF.
/// Los campos opcionales varían según remisión/consulta.
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.SuministroInformacion)]
public class SistemaInformatico
{
    /// <summary>Nombre o razón social del titular del SIF (opcional).</summary>
    [XmlElement("NombreRazon", Order = 0)]
    public string? NombreRazon { get; set; }

    /// <summary>NIF del titular del SIF (opcional).</summary>
    [XmlElement("NIF", Order = 1)]
    public string? NIF { get; set; }

    /// <summary>Nombre comercial del sistema informático.</summary>
    [XmlElement("NombreSistemaInformatico", Order = 2)]
    public string? NombreSistemaInformatico { get; set; }

    /// <summary>Identificador interno del SIF (numérico en ejemplos, se modela como string).</summary>
    [XmlElement("IdSistemaInformatico", Order = 3)]
    public string? IdSistemaInformatico { get; set; }

    /// <summary>Versión del SIF (ej.: "1.0.03").</summary>
    [XmlElement("Version", Order = 4)]
    public string? Version { get; set; }

    /// <summary>Número de instalación (se modela como string para no perder formato).</summary>
    [XmlElement("NumeroInstalacion", Order = 5)]
    public string? NumeroInstalacion { get; set; }

    /// <summary>Indicador de uso sólo VERI*FACTU, valores "S" / "N".</summary>
    [XmlElement("TipoUsoPosibleSoloVerifactu", Order = 6)]
    public string? TipoUsoPosibleSoloVerifactu { get; set; }

    /// <summary>Indicador de uso Multi OT, valores "S" / "N".</summary>
    [XmlElement("TipoUsoPosibleMultiOT", Order = 7)]
    public string? TipoUsoPosibleMultiOT { get; set; }

    /// <summary>Indicador de múltiples OT, valores "S" / "N".</summary>
    [XmlElement("IndicadorMultiplesOT", Order = 8)]
    public string? IndicadorMultiplesOT { get; set; }
}
