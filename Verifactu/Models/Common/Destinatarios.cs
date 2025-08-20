#nullable enable
using System.Xml.Serialization;

namespace Verifactu.Models.Common;

/// <summary>
/// Contenedor &lt;Destinatarios&gt; con uno o varios &lt;IDDestinatario&gt;.
/// Cada IDDestinatario reutiliza Identificacion (NombreRazon/NIF/IDOtro).
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.SuministroInformacion)]
public class Destinatarios
{
    [XmlElement("IDDestinatario", Order = 0)]
    public Identificacion[]? IDDestinatario { get; set; }
}
