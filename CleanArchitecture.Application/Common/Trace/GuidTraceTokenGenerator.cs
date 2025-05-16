namespace CleanArchitecture.Application.Common.Trace;

public class GuidTraceTokenGenerator : ITraceTokenGenerator
{
    public string GenerateToken() => Guid.NewGuid().ToString("N");
}
