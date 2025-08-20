#nullable enable
using System.Xml.Serialization;

namespace Verifactu.Models.Operaciones
{
    [XmlType(AnonymousType = true, Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.SuministroLR)]
    public class RegistroFactura
    {
        // 👇 añade esto:
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces? Xmlns { get; set; }

        [XmlElement("RegistroAlta", Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.SuministroInformacion)]
        public RegistroAlta? RegistroAlta { get; set; }

        [XmlElement("RegistroAnulacion", Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.SuministroInformacion)]
        public RegistroAnulacion? RegistroAnulacion { get; set; }
    }
}