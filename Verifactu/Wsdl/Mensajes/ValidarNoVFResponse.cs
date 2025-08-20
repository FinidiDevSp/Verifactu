#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Respuestas;

namespace Verifactu.Wsdl.Mensajes;

/// <summary>
/// Mensaje de respuesta de la validación de registros no Veri*Factu.
/// </summary>
[XmlRoot("RespuestaValRegistNoVeriFactu", Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.RespuestaValRegistNoVeriFactu)]
public class ValidarNoVFResponse : RespuestaValRegistNoVeriFactu
{
}