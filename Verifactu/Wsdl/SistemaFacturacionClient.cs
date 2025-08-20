#nullable enable
using System.Net.Http;
using System.Threading.Tasks;
using Verifactu.Wsdl.Mensajes;
using Verifactu.Wsdl.Soap;

namespace Verifactu.Wsdl;

/// <summary>
/// Cliente de alto nivel para el sistema de facturación Veri*Factu.
/// Wrapea las llamadas SOAP y expone métodos typed.
/// </summary>
public sealed class SistemaFacturacionClient
{
    private readonly SoapClient _soap;
    private readonly string _endpointUrl;

    /// <param name="httpClient">HttpClient configurado (timeout, certs, etc.).</param>
    /// <param name="endpointUrl">URL del servicio (del WSDL/binding).</param>
    public SistemaFacturacionClient(HttpClient httpClient, string endpointUrl)
    {
        _soap = new SoapClient(httpClient);
        _endpointUrl = endpointUrl;
    }

    /// <summary>
    /// Envía altas/anulaciones.
    /// </summary>
    /// <param name="request">RegFactuSistemaFacturacion con la cabecera y los registros.</param>
    /// <param name="soapAction">
    /// SOAPAction del WSDL para esta operación (p.ej. "urn:EnviarRegistros" o similar).
    /// Si no estás seguro, prueba con "" (algunos servidores lo ignoran).
    /// </param>
    public Task<EnviarRegistrosResponse?> EnviarRegistrosAsync(
        EnviarRegistrosRequest request,
        string soapAction = "")
        => _soap.PostAsync<EnviarRegistrosRequest, EnviarRegistrosResponse>(_endpointUrl, soapAction, request);

    /// <summary>
    /// Realiza una consulta de registros.
    /// </summary>
    public Task<ConsultaResponse?> ConsultarAsync(
        ConsultaRequest request,
        string soapAction = "")
        => _soap.PostAsync<ConsultaRequest, ConsultaResponse>(_endpointUrl, soapAction, request);

    /// <summary>
    /// (Opcional) Valida registros no Veri*Factu si el WSDL lo contempla.
    /// </summary>
    public Task<ValidarNoVFResponse?> ValidarNoVerifactuAsync(
        ValidarNoVFRequest request,
        string soapAction = "")
        => _soap.PostAsync<ValidarNoVFRequest, ValidarNoVFResponse>(_endpointUrl, soapAction, request);
}
