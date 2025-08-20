#nullable enable
using System;
using System.Globalization;

namespace Verifactu.Models.Common.Enums;

public static class EnumExtensions
{
    // ---- SiNo ----
    public static SiNo ToSiNo(this string? value)
        => string.Equals(value, "S", StringComparison.OrdinalIgnoreCase) ? SiNo.S
         : string.Equals(value, "N", StringComparison.OrdinalIgnoreCase) ? SiNo.N
         : SiNo.Desconocido;

    public static string FromSiNo(this SiNo value)
        => value switch
        {
            SiNo.S => "S",
            SiNo.N => "N",
            _ => ""
        };

    // ---- Impuesto ----
    public static Impuesto ToImpuesto(this string? value)
        => value?.Trim() switch
        {
            "01" => Impuesto.IVA,
            "02" => Impuesto.IPSI,
            "03" => Impuesto.IGIC,
            "99" => Impuesto.Otros,
            _ => Impuesto.Desconocido
        };

    public static string FromImpuesto(this Impuesto value)
        => value switch
        {
            Impuesto.IVA => "01",
            Impuesto.IPSI => "02",
            Impuesto.IGIC => "03",
            Impuesto.Otros => "99",
            _ => ""
        };

    // ---- SentidoOrdenacion ----
    public static SentidoOrdenacion ToSentidoOrdenacion(this string? value)
        => string.Equals(value, "ASC", StringComparison.OrdinalIgnoreCase) ? SentidoOrdenacion.ASC
         : string.Equals(value, "DESC", StringComparison.OrdinalIgnoreCase) ? SentidoOrdenacion.DESC
         : SentidoOrdenacion.Desconocido;

    public static string FromSentidoOrdenacion(this SentidoOrdenacion value)
        => value switch
        {
            SentidoOrdenacion.ASC => "ASC",
            SentidoOrdenacion.DESC => "DESC",
            _ => ""
        };

    // ---- Helpers numéricos seguros (por si quieres parsear importes a decimal) ----
    public static bool TryParseDecimal(string? s, out decimal value)
        => decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out value);

    public static string ToInvariant(this decimal value) => value.ToString(CultureInfo.InvariantCulture);
}
