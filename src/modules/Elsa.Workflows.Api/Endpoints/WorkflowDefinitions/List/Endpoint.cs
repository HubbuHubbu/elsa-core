using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Models;
using Elsa.Workflows.Persistence.Services;
using FastEndpoints;
using Open.Linq.AsyncExtensions;

namespace Elsa.Workflows.Api.Endpoints.WorkflowDefinitions.List;

public class List : Endpoint<Request, Response>
{
    private readonly IWorkflowDefinitionStore _store;

    public List(IWorkflowDefinitionStore store)
    {
        _store = store;
    }

    public override void Configure()
    {
        Get("/workflow-definitions");
    }

    public override async Task<Response> ExecuteAsync(Request request, CancellationToken cancellationToken)
    {
        var pageArgs = new PageArgs(request.Page, request.PageSize);
        var versionOptions = request.VersionOptions != null ? VersionOptions.FromString(request.VersionOptions) : default(VersionOptions?);
        var splitIds = request.DefinitionIds ?? Array.Empty<string>();

        if (splitIds.Any())
        {
            var summaries = await _store.FindManySummariesAsync(splitIds, versionOptions, cancellationToken).ToList();
            return new Response(summaries, summaries.Count);
        }
        else
        {
            var summaries = await _store.ListSummariesAsync(versionOptions, request.MaterializerName, pageArgs, cancellationToken);
            return new Response(summaries.Items, summaries.TotalCount);
        }
    }
}