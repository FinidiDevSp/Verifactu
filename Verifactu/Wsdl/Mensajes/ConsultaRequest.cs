#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Consultas;

namespace Verifactu.Wsdl.Mensajes;

/// <summary>
/// Mensaje de petición de consulta.
/// La raíz del Body será <ConsultaFactuSistemaFacturacion/> (ns ConsultaLR).
/// </summary>
[XmlRoot("ConsultaFactuSistemaFacturacion", Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.ConsultaLR)]
public class ConsultaRequest : ConsultaFactuSistemaFacturacion
{
}