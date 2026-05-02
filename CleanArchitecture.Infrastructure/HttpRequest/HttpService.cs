using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.DTOs.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.HttpRequest
{
public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpServiceResult<TResponse>> GetAsync<TResponse>(
        string url,
        Dictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var request = CreateRequest(HttpMethod.Get, url, headers);

            var response = await _httpClient.SendAsync(request, cancellationToken);

            return await HandleResponse<TResponse>(response, cancellationToken);
        }
        catch (TaskCanceledException ex)
        {
            return HttpServiceResult<TResponse>.Fail("Timeout or request canceled: " + ex.Message);
        }
        catch (HttpRequestException ex)
        {
            return HttpServiceResult<TResponse>.Fail("HTTP request error: " + ex.Message);
        }
        catch (Exception ex)
        {
            return HttpServiceResult<TResponse>.Fail("Unexpected error: " + ex.Message);
        }
    }

    public async Task<HttpServiceResult<TResponse>> PostAsync<TRequest, TResponse>(
        string url,
        TRequest requestDto,
        Dictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default)
    {
        return await SendWithBodyAsync<TRequest, TResponse>(
            HttpMethod.Post,
            url,
            requestDto,
            headers,
            cancellationToken);
    }

    public async Task<HttpServiceResult<TResponse>> PutAsync<TRequest, TResponse>(
        string url,
        TRequest requestDto,
        Dictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default)
    {
        return await SendWithBodyAsync<TRequest, TResponse>(
            HttpMethod.Put,
            url,
            requestDto,
            headers,
            cancellationToken);
    }

    public async Task<HttpServiceResult<TResponse>> DeleteAsync<TResponse>(
        string url,
        Dictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var request = CreateRequest(HttpMethod.Delete, url, headers);

            var response = await _httpClient.SendAsync(request, cancellationToken);

            return await HandleResponse<TResponse>(response, cancellationToken);
        }
        catch (TaskCanceledException ex)
        {
            return HttpServiceResult<TResponse>.Fail("Timeout or request canceled: " + ex.Message);
        }
        catch (HttpRequestException ex)
        {
            return HttpServiceResult<TResponse>.Fail("HTTP request error: " + ex.Message);
        }
        catch (Exception ex)
        {
            return HttpServiceResult<TResponse>.Fail("Unexpected error: " + ex.Message);
        }
    }

    private async Task<HttpServiceResult<TResponse>> SendWithBodyAsync<TRequest, TResponse>(
        HttpMethod method,
        string url,
        TRequest requestDto,
        Dictionary<string, string>? headers,
        CancellationToken cancellationToken)
    {
        try
        {
            using var request = CreateRequest(method, url, headers);

            var json = JsonSerializer.Serialize(requestDto, _jsonOptions);

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request, cancellationToken);

            return await HandleResponse<TResponse>(response, cancellationToken);
        }
        catch (TaskCanceledException ex)
        {
            return HttpServiceResult<TResponse>.Fail("Timeout or request canceled: " + ex.Message);
        }
        catch (HttpRequestException ex)
        {
            return HttpServiceResult<TResponse>.Fail("HTTP request error: " + ex.Message);
        }
        catch (Exception ex)
        {
            return HttpServiceResult<TResponse>.Fail("Unexpected error: " + ex.Message);
        }
    }

    private HttpRequestMessage CreateRequest(
        HttpMethod method,
        string url,
        Dictionary<string, string>? headers)
    {
        var request = new HttpRequestMessage(method, url);

        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        if (headers == null)
            return request;

        foreach (var header in headers)
        {
            if (string.IsNullOrWhiteSpace(header.Key))
                continue;

            if (header.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase))
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            else
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        return request;
    }

    private async Task<HttpServiceResult<T>> HandleResponse<T>(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        var rawResponse = await response.Content.ReadAsStringAsync(cancellationToken);

        var result = new HttpServiceResult<T>
        {
            IsSuccess = response.IsSuccessStatusCode,
            StatusCode = (int)response.StatusCode,
            ReasonPhrase = response.ReasonPhrase,
            RawResponse = rawResponse
        };

        if (!response.IsSuccessStatusCode)
        {
            result.ErrorMessage =
                $"HTTP Error {(int)response.StatusCode} - {response.ReasonPhrase}";

            if (!string.IsNullOrWhiteSpace(rawResponse))
            {
                result.ErrorMessage += $" | Response: {rawResponse}";
            }

            return result;
        }

        if (string.IsNullOrWhiteSpace(rawResponse))
            return result;

        try
        {
            result.Data = JsonSerializer.Deserialize<T>(rawResponse, _jsonOptions);
            return result;
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.ErrorMessage = "JSON deserialize error: " + ex.Message;
            return result;
        }
    }
}
}
