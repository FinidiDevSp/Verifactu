#nullable enable
using System;
using System.Globalization;
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
    // === Alta ===
    public static string CalcularHuellaAlta(
        string idEmisorFactura,
        string numSerieFactura,
        string fechaExpedicionFactura,      // "dd-MM-yyyy"
        string tipoFactura,
        string? cuotaTotal,                // admite null
        decimal? importeTotal,              // admite null
        string huellaRegistroAnterior,      // null/"" si es el primer registro
        string fechaHoraHusoGenRegistro     // ISO 8601, p.ej. "2024-01-01T19:20:30+01:00"
    )
    {
        string cadena =
            "IDEmisorFactura=" + TrimOrEmpty(idEmisorFactura) + "&" +
            "NumSerieFactura=" + TrimOrEmpty(numSerieFactura) + "&" +
            "FechaExpedicionFactura=" + TrimOrEmpty(fechaExpedicionFactura) + "&" +
            "TipoFactura=" + TrimOrEmpty(tipoFactura) + "&" +
            "CuotaTotal=" + TrimOrEmpty( cuotaTotal) + "&" +
            "ImporteTotal=" + FormatDecimalOrEmpty(importeTotal) + "&" +
            "Huella=" + TrimOrEmpty(huellaRegistroAnterior) + "&" +
            "FechaHoraHusoGenRegistro=" + TrimOrEmpty(fechaHoraHusoGenRegistro);

        return Sha256HexUpper(cadena);
    }

    // === Anulación ===
    public static string CalcularHuellaAnulacion(
        string idEmisorFacturaAnulada,
        string numSerieFacturaAnulada,
        string fechaExpedicionFacturaAnulada,  // "dd-MM-yyyy"
        string huellaRegistroAnterior,
        string fechaHoraHusoGenRegistro        // ISO 8601
    )
    {
        string cadena =
            "IDEmisorFacturaAnulada=" + TrimOrEmpty(idEmisorFacturaAnulada) + "&" +
            "NumSerieFacturaAnulada=" + TrimOrEmpty(numSerieFacturaAnulada) + "&" +
            "FechaExpedicionFacturaAnulada=" + TrimOrEmpty(fechaExpedicionFacturaAnulada) + "&" +
            "Huella=" + TrimOrEmpty(huellaRegistroAnterior) + "&" +
            "FechaHoraHusoGenRegistro=" + TrimOrEmpty(fechaHoraHusoGenRegistro);

        return Sha256HexUpper(cadena);
    }

    // === Evento ===
    public static string CalcularHuellaEvento(
        string nifSistemaInformatico,     // si no se informa, pasa "" y usa ID
        string idSistemaInformaticoOtro,  // ID de IDOtro; si no se informa, pasa ""
        string idSistemaInformatico,      // IdSistemaInformatico
        string version,
        string numeroInstalacion,
        string nifObligadoEmision,
        string tipoEvento,
        string huellaEventoAnterior,
        string fechaHoraHusoGenEvento
    )
    {
        string cadena =
            "NIF=" + TrimOrEmpty(nifSistemaInformatico) + "&" +
            "ID=" + TrimOrEmpty(idSistemaInformaticoOtro) + "&" +
            "IdSistemaInformatico=" + TrimOrEmpty(idSistemaInformatico) + "&" +
            "Version=" + TrimOrEmpty(version) + "&" +
            "NumeroInstalacion=" + TrimOrEmpty(numeroInstalacion) + "&" +
            "NIF=" + TrimOrEmpty(nifObligadoEmision) + "&" +
            "TipoEvento=" + TrimOrEmpty(tipoEvento) + "&" +
            "HuellaEvento=" + TrimOrEmpty(huellaEventoAnterior) + "&" +
            "FechaHoraHusoGenEvento=" + TrimOrEmpty(fechaHoraHusoGenEvento);

        return Sha256HexUpper(cadena);
    }

    // === Helpers ===

    private static string TrimOrEmpty(string? s) =>
        (s ?? string.Empty).Trim();

    // Formatea con punto decimal y sin ceros a la derecha innecesarios
    // Cumple que 123.1 y 123.10 se tratan indistintamente para el hash.
    private static string FormatDecimalOrEmpty(decimal? value)
    {
        if (!value.HasValue) return string.Empty;
        // Usa InvariantCulture y hasta 2 decimales, sin ceros de relleno a la derecha
        // (ajústalo si en tu sistema hay más de 2 decimales).
        string s = value.Value.ToString("0.##", CultureInfo.InvariantCulture);
        return s.Trim();
    }

    private static string Sha256HexUpper(string input)
    {
        byte[] data = Encoding.UTF8.GetBytes(input);
        using var sha = SHA256.Create();
        byte[] hash = sha.ComputeHash(data);
        var sb = new StringBuilder(hash.Length * 2);
        foreach (var b in hash)
            sb.Append(b.ToString("X2")); // mayúsculas
        return sb.ToString();
    }
}
