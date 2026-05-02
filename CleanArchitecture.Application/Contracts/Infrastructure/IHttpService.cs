using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.DTOs.V1;

namespace CleanArchitecture.Application.Contracts.Infrastructure
{
    public interface IHttpService
    {
        Task<HttpServiceResult<TResponse>> GetAsync<TResponse>(
            string url,
            Dictionary<string, string>? headers = null,
            CancellationToken cancellationToken = default);

        Task<HttpServiceResult<TResponse>> PostAsync<TRequest, TResponse>(
            string url,
            TRequest request,
            Dictionary<string, string>? headers = null,
            CancellationToken cancellationToken = default);

        Task<HttpServiceResult<TResponse>> PutAsync<TRequest, TResponse>(
            string url,
            TRequest request,
            Dictionary<string, string>? headers = null,
            CancellationToken cancellationToken = default);

        Task<HttpServiceResult<TResponse>> DeleteAsync<TResponse>(
            string url,
            Dictionary<string, string>? headers = null,
            CancellationToken cancellationToken = default);
    }
}
