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
    // === SHA-256(UTF-8(cadena)) -> HEX mayúsculas ===
    public static string GetHashVerifactu(string msg)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(msg); // UTF-8 requerido
        var hash = sha.ComputeHash(bytes);
        var sb = new StringBuilder(hash.Length * 2);
        foreach (var b in hash) sb.Append(b.ToString("X2"));
        return sb.ToString();
    }

    // === Helpers de construcción de la cadena ===
    private static string GetValorCampo(string nombre, string? valor, bool separador)
    {
        string campo = nombre + "=" + ((valor == null) ? "" : valor.Trim());
        return separador ? campo + "&" : campo;
    }

    // Decimal con punto y sin ceros de relleno innecesarios (123.10 -> 123.1)
    private static string GetValorCampoDecimal(string nombre, decimal? valor, bool separador)
    {
        string s = (valor.HasValue) ? valor.Value.ToString("0.##", CultureInfo.InvariantCulture) : "";
        return GetValorCampo(nombre, s, separador);
    }

    // === Referencia (cadena) para ALTA ===
    public static string GetReferenciaRegistroAlta(
        string nifEmisor,
        string numFacturaSerie,
        string fechaExpedicion,        // "dd-MM-yyyy" tal como va al XML
        string tipoFactura,
        string cuotaTotal,
        string importeTotal,
        string huellaAnterior,         // "" si primer registro
        string fechaHoraHusoRegistro   // "yyyy-MM-ddTHH:mm:sszzz" (ISO 8601 con offset)
    )
    {
        return
            "IDEmisorFactura=" + (nifEmisor?.Trim() ?? "") + "&" +
            "NumSerieFactura=" + (numFacturaSerie?.Trim() ?? "") + "&" +
            "FechaExpedicionFactura=" + (fechaExpedicion?.Trim() ?? "") + "&" +
            "TipoFactura=" + (tipoFactura.Trim() ?? "") + "&" +
            "CuotaTotal=" + (cuotaTotal.Trim() ?? "") + "&" +
            "ImporteTotal=" + (importeTotal?.Trim() ?? "") + "&" +
            "Huella=" + (huellaAnterior?.Trim() ?? "") + "&" +
            "FechaHoraHusoGenRegistro=" + (fechaHoraHusoRegistro?.Trim() ?? "");
    }

    // === Huella ALTA (atajo) ===
    public static string CalcularHuellaAlta(
        string nifEmisor, string numFacturaSerie, string fechaExpedicion, string tipoFactura,
       string cuotaTotal, string importeTotal, string huellaAnterior, string fechaHoraHusoRegistro)
    {
        string referencia = GetReferenciaRegistroAlta(nifEmisor, numFacturaSerie, fechaExpedicion, tipoFactura,
                                                      cuotaTotal, importeTotal, huellaAnterior, fechaHoraHusoRegistro);
        return GetHashVerifactu(referencia);
    }

    // === Referencia (cadena) para ANULACIÓN ===
    public static string GetReferenciaRegistroAnulacion(
        string nifEmisorFacturaAnulada,
        string numSerieFacturaAnulada,
        string fechaExpedicionFacturaAnulada, // "dd-MM-yyyy"
        string huellaAnterior,
        string fechaHoraHusoRegistro          // ISO 8601 con offset
    )
    {
        var sb = new StringBuilder();
        sb.Append(GetValorCampo("IDEmisorFacturaAnulada", nifEmisorFacturaAnulada, true))
          .Append(GetValorCampo("NumSerieFacturaAnulada", numSerieFacturaAnulada, true))
          .Append(GetValorCampo("FechaExpedicionFacturaAnulada", fechaExpedicionFacturaAnulada, true))
          .Append(GetValorCampo("Huella", huellaAnterior, true))
          .Append(GetValorCampo("FechaHoraHusoGenRegistro", fechaHoraHusoRegistro, false));
        return sb.ToString();
    }

    public static string CalcularHuellaAnulacion(
        string nifEmisorFacturaAnulada, string numSerieFacturaAnulada, string fechaExpedicionFacturaAnulada,
        string huellaAnterior, string fechaHoraHusoRegistro)
    {
        string referencia = GetReferenciaRegistroAnulacion(
            nifEmisorFacturaAnulada, numSerieFacturaAnulada, fechaExpedicionFacturaAnulada,
            huellaAnterior, fechaHoraHusoRegistro);
        return GetHashVerifactu(referencia);
    }

    // === Helper de fecha/hora con offset: DateTime -> "yyyy-MM-ddTHH:mm:sszzz" ===
    public static string FormatearFechaHoraOffset(DateTime dt) => dt.ToString("yyyy-MM-ddTHH:mm:sszzz");
}
