#nullable enable
using System.Xml.Serialization;
using Verifactu.Models.Common;

namespace Verifactu.Models.Operaciones;

/// <summary>
/// &lt;RegistroAlta&gt; pertenece al namespace de SuministroInformacion
/// y contiene la estructura de la factura.
/// </summary>
[XmlType(AnonymousType = true, Namespace = Verifactu.Models.Common.VerifactuXmlNamespaces.SuministroInformacion)]
public class RegistroAlta
{
    // 👇 añade esto:
    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces? Xmlns { get; set; }
    // Para mantener fielmente la estructura del XSD, la "factura" es el propio contenido del nodo.
    // Es decir, todas las etiquetas que cuelgan de <RegistroAlta> están definidas en Factura.
    // Aquí la exponemos como composición para facilitar el uso en .NET (serializa inline).
    [XmlElement("IDVersion", Order = 0)]
    public string? IDVersion { get => Factura?.IDVersion; set { if (Factura == null) Factura = new(); Factura.IDVersion = value; } }

    [XmlElement("IDFactura", Order = 1)]
    public Common.IDFactura? IDFactura { get => Factura?.IDFactura; set { if (Factura == null) Factura = new(); Factura.IDFactura = value; } }

    [XmlElement("NombreRazonEmisor", Order = 2)]
    public string? NombreRazonEmisor { get => Factura?.NombreRazonEmisor; set { if (Factura == null) Factura = new(); Factura.NombreRazonEmisor = value; } }

    [XmlElement("Subsanacion", Order = 3)]
    public string? Subsanacion { get => Factura?.Subsanacion; set { if (Factura == null) Factura = new(); Factura.Subsanacion = value; } }

    [XmlElement("RechazoPrevio", Order = 4)]
    public string? RechazoPrevio { get => Factura?.RechazoPrevio; set { if (Factura == null) Factura = new(); Factura.RechazoPrevio = value; } }

    [XmlElement("TipoFactura", Order = 5)]
    public string? TipoFactura { get => Factura?.TipoFactura; set { if (Factura == null) Factura = new(); Factura.TipoFactura = value; } }

    [XmlElement("DescripcionOperacion", Order = 6)]
    public string? DescripcionOperacion { get => Factura?.DescripcionOperacion; set { if (Factura == null) Factura = new(); Factura.DescripcionOperacion = value; } }

    [XmlElement("Destinatarios", Order = 7)]
    public Common.Destinatarios? Destinatarios { get => Factura?.Destinatarios; set { if (Factura == null) Factura = new(); Factura.Destinatarios = value; } }

    [XmlElement("Desglose", Order = 8)]
    public Common.Desglose? Desglose { get => Factura?.Desglose; set { if (Factura == null) Factura = new(); Factura.Desglose = value; } }

    [XmlElement("CuotaTotal", Order = 9)]
    public string? CuotaTotal { get => Factura?.CuotaTotal; set { if (Factura == null) Factura = new(); Factura.CuotaTotal = value; } }

    [XmlElement("ImporteTotal", Order = 10)]
    public string? ImporteTotal { get => Factura?.ImporteTotal; set { if (Factura == null) Factura = new(); Factura.ImporteTotal = value; } }

    [XmlElement("Encadenamiento", Order = 11)]
    public Common.Encadenamiento? Encadenamiento { get => Factura?.Encadenamiento; set { if (Factura == null) Factura = new(); Factura.Encadenamiento = value; } }

    [XmlElement("SistemaInformatico", Order = 12)]
    public Common.SistemaInformatico? SistemaInformatico { get => Factura?.SistemaInformatico; set { if (Factura == null) Factura = new(); Factura.SistemaInformatico = value; } }

    [XmlElement("FechaHoraHusoGenRegistro", Order = 13)]
    public string? FechaHoraHusoGenRegistro { get => Factura?.FechaHoraHusoGenRegistro; set { if (Factura == null) Factura = new(); Factura.FechaHoraHusoGenRegistro = value; } }

    [XmlElement("TipoHuella", Order = 14)]
    public string? TipoHuella { get => Factura?.TipoHuella; set { if (Factura == null) Factura = new(); Factura.TipoHuella = value; } }

    [XmlElement("Huella", Order = 15)]
    public string? Huella { get => Factura?.Huella; set { if (Factura == null) Factura = new(); Factura.Huella = value; } }

    [XmlElement("RefExterna", Order = 16)]
    public string? RefExterna { get => Factura?.RefExterna; set { if (Factura == null) Factura = new(); Factura.RefExterna = value; } }

    [XmlIgnore]
    public Factura? Factura { get; set; }
}
