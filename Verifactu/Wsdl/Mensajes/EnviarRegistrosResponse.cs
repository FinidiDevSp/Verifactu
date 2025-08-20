#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Respuestas;

namespace Verifactu.Wsdl.Mensajes;

/// <summary>
/// Mensaje de respuesta para el envío de registros.
/// La raíz del Body será <RespuestaSuministro/> (ns RespuestaSuministro).
/// </summary>
[XmlRoot("RespuestaSuministro", Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.RespuestaSuministro)]
public class EnviarRegistrosResponse : RespuestaSuministro
{
}