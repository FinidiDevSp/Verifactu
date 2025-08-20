#nullable enable
namespace Verifactu.Models.Common.Catalogos;

/// <summary>
/// Catálogo de tipos de factura. Se expone como constantes para no romper al aparecer códigos nuevos.
/// Usa estas constantes en tu lógica de negocio; las propiedades XML siguen siendo string.
/// </summary>
public static class TipoFactura
{
    // Ejemplos habituales (ajusta/añade según el catálogo oficial):
    public const string FacturaCompleta = "F1";
    public const string FacturaSimplificada = "F2";
    public const string FacturaRectificativa = "R1"; // si aplica
    public const string Sustitutiva = "S1";           // si aplica

    // Extensión segura: si AEAT añade códigos, los puedes añadir aquí sin tocar modelos.
}