#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Operaciones;

namespace Verifactu.Wsdl.Mensajes;

/// <summary>
/// Mensaje de petición para el envío de registros (Alta/Anulación).
/// La raíz en Body suele ser el propio <RegFactuSistemaFacturacion/> (ns SuministroLR).
/// </summary>
// Wsdl/Mensajes/EnviarRegistrosRequest.cs
[XmlRoot("RegFactuSistemaFacturacion", Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.SuministroLR)]
public class EnviarRegistrosRequest : Verifactu.Models.Operaciones.RegFactuSistemaFacturacion
{
}