#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Respuestas;

namespace Verifactu.Wsdl.Mensajes;

/// <summary>
/// Mensaje de respuesta de consulta.
/// La raíz del Body será <RespuestaConsultaLR/> (ns RespuestaConsultaLR).
/// </summary>
[XmlRoot("RespuestaConsultaLR", Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.RespuestaConsultaLR)]
public class ConsultaResponse : RespuestaConsultaLR
{
}