using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Verifactu.Models.Common;

/// <summary>
/// Identificación genérica (ObligadoEmision / Destinatario / Contraparte) usada por los XSD de Veri*Factu.
/// Mapea a los elementos en el namespace de SuministroInformacion.xsd:
/// - &lt;NombreRazon>
/// - &lt;NIF>
/// - &lt;IDOtro&gt;(&lt;CodigoPais>, &lt;IDType>, &lt;ID&gt;)
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.SuministroInformacion)]
public class Identificacion
{
    /// <summary>Nombre o razón social (hasta 120 chars en especificación).</summary>
    [XmlElement("NombreRazon", Order = 0)]
    public string? NombreRazon { get; set; }

    /// <summary>NIF nacional español (formato 9 chars). Opcional si se usa IDOtro.</summary>
    [XmlElement("NIF", Order = 1)]
    public string? NIF { get; set; }

    /// <summary>
    /// Identificación alternativa (extranjera) en país de residencia.
    /// Esquema admite Código de país (ISO 3166-1 alpha-2), tipo y valor.
    /// </summary>
    [XmlElement("IDOtro", Order = 2)]
    public IDOtro? IDOtro { get; set; }

    /// <summary>
    /// Indica si el nodo tiene alguna identificación válida (NIF o IDOtro.ID).
    /// Útil en validaciones de dominio (no participa en serialización).
    /// </summary>
    [XmlIgnore]
    public bool TieneIdentificacion =>
        !string.IsNullOrWhiteSpace(NIF) || (IDOtro is { } o && !string.IsNullOrWhiteSpace(o.ID));
}

/// <summary>
/// Bloque &lt;IDOtro&gt; para identificación extranjera.
/// </summary>
[XmlType(AnonymousType = true, Namespace = VerifactuXmlNamespaces.SuministroInformacion)]
public class IDOtro
{
    /// <summary>Código de país (ISO 3166-1 alpha-2), ej.: ES, FR, PT.</summary>
    [XmlElement("CodigoPais", Order = 0)]
    public string? CodigoPais { get; set; }

    /// <summary>Tipo de identificación en el país de residencia (códigos L7 según la especificación).</summary>
    [XmlElement("IDType", Order = 1)]
    public string? IDType { get; set; }

    /// <summary>Número/valor de identificación en el país de residencia.</summary>
    [XmlElement("ID", Order = 2)]
    public string? ID { get; set; }
}
