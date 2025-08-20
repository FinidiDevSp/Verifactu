#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Common;

namespace Verifactu.Models.Operaciones;

/// <summary>
/// Wrapper &lt;RegistroFactura&gt; que contiene o bien &lt;RegistroAlta&gt; o bien &lt;RegistroAnulacion&gt;.
/// Este nodo pertenece al esquema de SuministroLR.
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.SuministroLR)]
public class RegistroFactura
{
    // Importante: los elementos Alta/Anulacion están en el namespace de SuministroInformacion.
    [XmlElement("RegistroAlta", Namespace = VerifactuXmlNamespaces.SuministroInformacion, Order = 0)]
    public RegistroAlta? RegistroAlta { get; set; }

    [XmlElement("RegistroAnulacion", Namespace = VerifactuXmlNamespaces.SuministroInformacion, Order = 1)]
    public RegistroAnulacion? RegistroAnulacion { get; set; }
}