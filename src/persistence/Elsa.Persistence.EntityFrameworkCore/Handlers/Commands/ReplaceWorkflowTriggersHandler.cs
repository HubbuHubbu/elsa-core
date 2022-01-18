using Elsa.Mediator.Contracts;
using Elsa.Persistence.Commands;
using Elsa.Persistence.Entities;
using Elsa.Persistence.EntityFrameworkCore.Contracts;

namespace Elsa.Persistence.EntityFrameworkCore.Handlers.Commands;

public class ReplaceWorkflowTriggersHandler : ICommandHandler<ReplaceWorkflowTriggers>
{
    private readonly IStore<WorkflowTrigger> _store;
    public ReplaceWorkflowTriggersHandler(IStore<WorkflowTrigger> store) => _store = store;

    public async Task<Unit> HandleAsync(ReplaceWorkflowTriggers command, CancellationToken cancellationToken)
    {
        var workflowDefinitionIds = command.WorkflowTriggers.Select(x => x.WorkflowDefinitionId).Distinct().ToList();
        await _store.DeleteWhereAsync(x => workflowDefinitionIds.Contains(x.WorkflowDefinitionId), cancellationToken);
        await _store.SaveManyAsync(command.WorkflowTriggers, cancellationToken);

        return Unit.Instance;
    }
}