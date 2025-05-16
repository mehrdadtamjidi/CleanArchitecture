namespace CleanArchitecture.Application.Common.Trace;

public class CustomTraceTokenAttribute : Attribute
{
    public CustomTraceTokenAttribute(ITraceTokenGenerator? traceTokenGenerator)
    {
        TraceTokenGenerator = traceTokenGenerator;
    }
    public ITraceTokenGenerator? TraceTokenGenerator { get; }
}