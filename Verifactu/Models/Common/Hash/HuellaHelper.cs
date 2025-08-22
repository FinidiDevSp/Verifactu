#nullable enable
using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using Verifactu.Models.Operaciones;

namespace Verifactu.Models.Common.Hash;

/// <summary>
/// Helper de huella: genera un SHA-256 Base64 sobre una representación canónica
/// SIMPLE del registro de alta. Ajusta el "documento canónico" si tu guía de
/// Veri*Factu exige otro orden/normalización.
/// </summary>
public static class HuellaHelper
{
    /// <summary>
    /// Calcula la huella del <see cref="RegistroAlta"/> con un canónico predecible.
    /// </summary>
    public static string ComputeHuellaRegistroAlta(RegistroAlta alta)
    {
        // Canonización mínima y determinista de los campos críticos:
        // - IDVersion|IDEmisorFactura|NumSerie|Fecha|TipoFactura|CuotaTotal|ImporteTotal
        // - Primer destinatario (si existe): NIF o IDOtro.ID
        // - Primer detalle de desglose: Impuesto|TipoImpositivo|Base|Cuota
        // - FechaHoraHusoGenRegistro
        // *Sin espacios*, *con separador '|'* y valores nulos como "".
        string S(string? v) => v?.Trim() ?? "";
        var idDest = alta.Destinatarios?.IDDestinatario != null && alta.Destinatarios.IDDestinatario.Length > 0
            ? (S(alta.Destinatarios.IDDestinatario[0].NIF) != ""
                ? S(alta.Destinatarios.IDDestinatario[0].NIF)
                : S(alta.Destinatarios.IDDestinatario[0].IDOtro?.ID))
            : "";

        var det = alta.Desglose?.DetalleDesglose != null && alta.Desglose.DetalleDesglose.Length > 0
            ? alta.Desglose.DetalleDesglose[0]
            : null;

        var canon = string.Join("|", new[]
        {
            S(alta.IDVersion),
            S(alta.IDFactura?.IDEmisorFactura),
            S(alta.IDFactura?.NumSerieFactura),
            S(alta.IDFactura?.FechaExpedicionFactura),
            S(alta.TipoFactura),
            S(alta.CuotaTotal),
            S(alta.ImporteTotal),
            idDest,
            S(det?.Impuesto),
            S(det?.TipoImpositivo),
            S(det?.BaseImponibleOimporteNoSujeto),
            S(det?.CuotaRepercutida),
            S(alta.FechaHoraHusoGenRegistro)
        });

        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(canon);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Calcula la huella del <see cref="RegistroAnulacion"/> con un canónico predecible.
    /// </summary>
    public static string ComputeHuellaRegistroAnulacion(RegistroAnulacion anulacion)
    {
        // Canon mínimo: IDVersion|IDEmisorFactura|NumSerie|Fecha|RechazoPrevio|SinRegistroPrevio|FechaHoraHusoGenRegistro
        // *Sin espacios*, *separado por '|'* y valores nulos como "".
        string S(string? v) => v?.Trim() ?? "";

        var canon = string.Join("|", new[]
        {
            S(anulacion.IDVersion),
            S(anulacion.IDFacturaAnulada?.IDEmisorFacturaAnulada),
            S(anulacion.IDFacturaAnulada?.NumSerieFacturaAnulada),
            S(anulacion.IDFacturaAnulada?.FechaExpedicionFacturaAnulada),
            S(anulacion.RechazoPrevio),
            S(anulacion.SinRegistroPrevio),
            S(anulacion.FechaHoraHusoGenRegistro)
        });

        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(canon);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
