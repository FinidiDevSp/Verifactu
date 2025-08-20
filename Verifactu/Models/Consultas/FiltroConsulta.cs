#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Common;

namespace Verifactu.Models.Consultas;

/// <summary>
/// Filtros de la consulta. Incluye periodo y criterios habituales:
/// - PeriodoImputacion (Ejercicio/Periodo)
/// - IDFactura (para consulta puntual)
/// - Destinatarios, TipoFactura, rangos y paginación (cuando aplique)
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.ConsultaLR)]
public class FiltroConsulta
{
    // Periodo estándar (YYYY + "01".."12")
    [XmlElement("PeriodoImputacion", Namespace = VerifactuXmlNamespaces.SuministroInformacion, Order = 0)]
    public PeriodoImputacion? PeriodoImputacion { get; set; }

    // Consulta por identificador completo de factura (alternativa a otros filtros)
    [XmlElement("IDFactura", Namespace = VerifactuXmlNamespaces.SuministroInformacion, Order = 1)]
    public IDFactura? IDFactura { get; set; }

    // Opcional: restringir por tipo de factura (códigos oficiales, modelado como string)
    [XmlElement("TipoFactura", Namespace = VerifactuXmlNamespaces.SuministroInformacion, Order = 2)]
    public string? TipoFactura { get; set; }

    // Opcional: limitar a un destinatario concreto
    [XmlElement("Destinatarios", Namespace = VerifactuXmlNamespaces.SuministroInformacion, Order = 3)]
    public Destinatarios? Destinatarios { get; set; }

    // Opcional: rango de importes (como string para mantener literal)
    [XmlElement("ImporteMin", Namespace = VerifactuXmlNamespaces.SuministroInformacion, Order = 4)]
    public string? ImporteMin { get; set; }

    [XmlElement("ImporteMax", Namespace = VerifactuXmlNamespaces.SuministroInformacion, Order = 5)]
    public string? ImporteMax { get; set; }

    // Opcional: paginación (clave de última fila devuelta por AEAT)
    [XmlElement("ClavePaginacion", Namespace = VerifactuXmlNamespaces.SuministroInformacion, Order = 6)]
    public ClavePaginacion? ClavePaginacion { get; set; }
}