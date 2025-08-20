#nullable enable
using System.Xml.Serialization;

namespace Verifactu.Wsdl.Mensajes;

/// <summary>
/// Mensaje de petición para validar registros no Veri*Factu (si usas ese endpoint).
/// La raíz/namespace exactos dependen del WSDL concreto.
/// Si el servicio espera otro nombre, ajusta el XmlRoot.
/// </summary>
[XmlRoot("ValidarRegistrosNoVerifactu", Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.SuministroLR)]
public class ValidarNoVFRequest
{
    // Define aquí las propiedades que exija el WSDL para la validación.
    // Puedes reutilizar tipos Common/ si el XSD coincide.
}