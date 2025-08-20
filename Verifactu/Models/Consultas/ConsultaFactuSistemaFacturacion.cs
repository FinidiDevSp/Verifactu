#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Common;

namespace Verifactu.Models.Consultas;

/// <summary>
/// Raíz de la petición de consulta (ConsultaLR.xsd).
/// Incluye la cabecera, los filtros de consulta y, si procede, parámetros de paginación.
/// </summary>
[XmlRoot("ConsultaFactuSistemaFacturacion", Namespace = VerifactuXmlNamespaces.ConsultaLR)]
public class ConsultaFactuSistemaFacturacion
{
    [XmlElement("Cabecera", Order = 0)]
    public CabeceraConsulta? Cabecera { get; set; }

    [XmlElement("FiltroConsulta", Order = 1)]
    public FiltroConsulta? FiltroConsulta { get; set; }

    [XmlElement("Paginacion", Order = 2)]
    public Paginacion? Paginacion { get; set; }
}