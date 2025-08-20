#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Common;

namespace Verifactu.Models.Respuestas;

/// <summary>
/// Respuesta al envío de altas/anulaciones (RespuestaSuministro.xsd).
/// Suele contener un resumen y un detalle por cada registro suministrado.
/// </summary>
[XmlRoot("RespuestaSuministro", Namespace = VerifactuXmlNamespaces.RespuestaSuministro)]
public class RespuestaSuministro
{
    /// <summary>Estado global del fichero/carga (p.ej. "Correcto", "ParcialmenteCorrecto", "Incorrecto").</summary>
    [XmlElement("Estado", Order = 0)]
    public string? Estado { get; set; }

    /// <summary>Código de resultado global (cuando aplique).</summary>
    [XmlElement("CodigoResultado", Order = 1)]
    public string? CodigoResultado { get; set; }

    /// <summary>Descripción del resultado global.</summary>
    [XmlElement("DescripcionResultado", Order = 2)]
    public string? DescripcionResultado { get; set; }

    /// <summary>Número total de registros recibidos/aceptados/rechazados (si el servicio los devuelve).</summary>
    [XmlElement("NumRegistrosRecibidos", Order = 3)]
    public string? NumRegistrosRecibidos { get; set; }

    [XmlElement("NumRegistrosAceptados", Order = 4)]
    public string? NumRegistrosAceptados { get; set; }

    [XmlElement("NumRegistrosRechazados", Order = 5)]
    public string? NumRegistrosRechazados { get; set; }

    /// <summary>Detalle uno-a-uno por registro enviado.</summary>
    [XmlArray("Detalle", Order = 6)]
    [XmlArrayItem("RespuestaRegistro")]
    public RespuestaRegistro[]? Detalle { get; set; }
}

/// <summary>
/// Resultado por cada registro (alta o anulación) incluido en el suministro.
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.RespuestaSuministro)]
public class RespuestaRegistro
{
    // La AEAT devuelve la identificación según sea Alta o Anulación; modelamos ambos y el que no aplique vendrá nulo.
    [XmlElement("IDFactura")]
    public IDFactura? IDFactura { get; set; }

    [XmlElement("IDFacturaAnulada")]
    public IDFacturaAnulada? IDFacturaAnulada { get; set; }

    /// <summary>Estado del registro concreto (Correcto/Incorrecto).</summary>
    [XmlElement("Estado")]
    public string? Estado { get; set; }

    /// <summary>CSV asignado por AEAT para el registro (cuando aplica).</summary>
    [XmlElement("CSV")]
    public string? CSV { get; set; }

    /// <summary>Código de error del registro si ha sido rechazado.</summary>
    [XmlElement("CodigoErrorRegistro")]
    public string? CodigoErrorRegistro { get; set; }

    /// <summary>Descripción del error asociado al código.</summary>
    [XmlElement("DescripcionErrorRegistro")]
    public string? DescripcionErrorRegistro { get; set; }

    /// <summary>Huella/Hash que AEAT puede devolver asociado al registro.</summary>
    [XmlElement("Huella")]
    public string? Huella { get; set; }

    /// <summary>Fecha/hora que la AEAT asocia al procesamiento del registro.</summary>
    [XmlElement("FechaHoraProcesamiento")]
    public string? FechaHoraProcesamiento { get; set; }
}
