#nullable enable
using System.Xml.Serialization;

namespace Verifactu.Models.Common.Enums;

/// <summary>
/// Códigos de impuesto más habituales en Veri*Factu.
/// </summary>
public enum Impuesto
{
    Desconocido = 0,

    [XmlEnum("01")]
    IVA = 1,

    [XmlEnum("02")]
    IPSI = 2,

    [XmlEnum("03")]
    IGIC = 3,

    [XmlEnum("99")]
    Otros = 99
}