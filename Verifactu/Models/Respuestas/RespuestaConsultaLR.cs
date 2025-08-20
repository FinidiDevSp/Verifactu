#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Common;

namespace Verifactu.Models.Respuestas;

/// <summary>
/// Respuesta a la petición de consulta (RespuestaConsultaLR.xsd).
/// Incluye resumen, total de registros y la lista de resultados con paginación.
/// </summary>
[XmlRoot("RespuestaConsultaLR", Namespace = VerifactuXmlNamespaces.RespuestaConsultaLR)]
public class RespuestaConsultaLR
{
    /// <summary>Estado global de la consulta.</summary>
    [XmlElement("Estado", Order = 0)]
    public string? Estado { get; set; }

    /// <summary>Código/Descripción global del resultado (si procede).</summary>
    [XmlElement("CodigoResultado", Order = 1)]
    public string? CodigoResultado { get; set; }

    [XmlElement("DescripcionResultado", Order = 2)]
    public string? DescripcionResultado { get; set; }

    /// <summary>Total de registros encontrados.</summary>
    [XmlElement("TotalRegistros", Order = 3)]
    public string? TotalRegistros { get; set; }

    /// <summary>Clave para solicitar la siguiente página (si hay más resultados).</summary>
    [XmlElement("SiguienteClavePaginacion", Namespace = VerifactuXmlNamespaces.SuministroInformacion, Order = 4)]
    public ClavePaginacion? SiguienteClavePaginacion { get; set; }

    /// <summary>Resultados devueltos en esta página.</summary>
    [XmlArray("Resultados", Order = 5)]
    [XmlArrayItem("Registro")]
    public ResultadoConsulta[]? Resultados { get; set; }
}

/// <summary>
/// Fila/entrada de la lista de resultados en la respuesta de consulta.
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.RespuestaConsultaLR)]
public class ResultadoConsulta
{
    [XmlElement("IDFactura", Namespace = VerifactuXmlNamespaces.SuministroInformacion, Order = 0)]
    public IDFactura? IDFactura { get; set; }

    /// <summary>Nombre/Razón del emisor si el servicio lo devuelve.</summary>
    [XmlElement("NombreRazonEmisor", Order = 1)]
    public string? NombreRazonEmisor { get; set; }

    /// <summary>Tipo de factura (código oficial) si está presente.</summary>
    [XmlElement("TipoFactura", Order = 2)]
    public string? TipoFactura { get; set; }

    /// <summary>Importe total según conste en AEAT.</summary>
    [XmlElement("ImporteTotal", Order = 3)]
    public string? ImporteTotal { get; set; }

    /// <summary>Estado de ese registro en AEAT (Aceptado/Rechazado, etc.).</summary>
    [XmlElement("Estado", Order = 4)]
    public string? Estado { get; set; }

    /// <summary>CSV del registro si aplica.</summary>
    [XmlElement("CSV", Order = 5)]
    public string? CSV { get; set; }

    /// <summary>Huella (hash) almacenada.</summary>
    [XmlElement("Huella", Order = 6)]
    public string? Huella { get; set; }

    /// <summary>Fecha/hora asociada al registro en AEAT.</summary>
    [XmlElement("FechaHora", Order = 7)]
    public string? FechaHora { get; set; }
}
