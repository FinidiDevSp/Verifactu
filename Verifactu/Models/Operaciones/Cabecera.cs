#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Common;

namespace Verifactu.Models.Operaciones;

/// <summary>
/// Cabecera del envío de suministro (SuministroLR.xsd).
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.SuministroLR)]
public class Cabecera
{
    [XmlElement("ObligadoEmision", Namespace = VerifactuXmlNamespaces.SuministroInformacion, Order = 0)]
    public Identificacion? ObligadoEmision { get; set; }

    // Para “bajo requerimiento” (opcional)
    [XmlElement("RemisionRequerimiento", Namespace = VerifactuXmlNamespaces.SuministroInformacion, Order = 1)]
    public RemisionRequerimiento? RemisionRequerimiento { get; set; }
}

/// <summary>
/// Bloque para remisión bajo requerimiento (opcional en cabecera).
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.SuministroInformacion)]
public class RemisionRequerimiento
{
    /// <summary>Referencia del requerimiento de la AEAT.</summary>
    [XmlElement("RefRequerimiento", Order = 0)]
    public string? RefRequerimiento { get; set; }

    /// <summary>Marca de fin de requerimiento ("S" / "N").</summary>
    [XmlElement("FinRequerimiento", Order = 1)]
    public string? FinRequerimiento { get; set; }
}