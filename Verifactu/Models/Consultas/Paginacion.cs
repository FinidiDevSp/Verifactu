#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Common;

namespace Verifactu.Models.Consultas;

/// <summary>
/// Parámetros de paginación / orden (cuando el servicio los admita).
/// MaxRegistros: límite por página. SentidoOrdenacion: "ASC"/"DESC".
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.ConsultaLR)]
public class Paginacion
{
    [XmlElement("MaxRegistros", Order = 0)]
    public int? MaxRegistros { get; set; }

    [XmlElement("SentidoOrdenacion", Order = 1)]
    public string? SentidoOrdenacion { get; set; } // "ASC" / "DESC"
}