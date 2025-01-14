using Elsa.Http.Contracts;
using Elsa.Workflows.Core;
using Elsa.Workflows.Core.Models;

// ReSharper disable once CheckNamespace
namespace Elsa.Extensions;

internal static class HttpActivityExecutionContextExtensions
{
    public static async Task<object?> ParseContentAsync(this ActivityExecutionContext context, Stream content, string contentType, Type? returnType, CancellationToken cancellationToken)
    {
        var parsers = context.GetServices<IHttpContentParser>().OrderByDescending(x => x.Priority).ToList();
        var contentParser = parsers.FirstOrDefault(x => x.GetSupportsContentType(contentType));

        if (contentParser == null)
            return null;

        return await contentParser.ReadAsync(content, returnType, cancellationToken);
    }

    public static IEnumerable<KeyValuePair<string, string[]>> GetHeaders(this ActivityExecutionContext context, Input input)
    {
        var value = context.Get(input.MemoryBlockReference());

        return value switch
        {
            IDictionary<string, string[]> dictionary1 => dictionary1,
            IDictionary<string, string> dictionary2 => dictionary2.ToDictionary(x => x.Key, x => new[] { x.Value }),
            IDictionary<string, object> dictionary3 => dictionary3.ToDictionary(pair => pair.Key, pair => pair.Value is ICollection<object> collection ? collection.Select(x => x.ToString()!).ToArray() : new[] { pair.Value.ToString()! }),
            _ => Array.Empty<KeyValuePair<string, string[]>>()
        };
    }
}