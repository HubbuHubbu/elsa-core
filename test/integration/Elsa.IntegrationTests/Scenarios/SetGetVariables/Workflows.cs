using Elsa.Workflows.Core.Abstractions;
using Elsa.Workflows.Core.Activities;
using Elsa.Workflows.Core.Contracts;
using Elsa.Workflows.Core.Memory;
using Elsa.Workflows.Core.Models;

namespace Elsa.IntegrationTests.Scenarios.SetGetVariables;

class SetGetVariableWorkflow : WorkflowBase
{
    protected override void Build(IWorkflowBuilder workflow)
    {
        var variable1 = new Variable<string>();

        workflow.Root = new Sequence
        {
            Variables = {
                variable1
            },

            Activities =
            {
                new SetVariable<string>(variable1,"Line 5"),
                new WriteLine(variable1)
            }
        };
    }
}

class SetGetVariablesWorkflow : WorkflowBase
{
    protected override void Build(IWorkflowBuilder workflow)
    {
        var variable1 = new Variable<string>();
        var variable2 = new Variable<string>();
        
        workflow.Root = new Sequence
        {    
            Variables = { variable1, variable2 },
            
            Activities =
            {
                new SetVariable
                {
                    Variable = variable1,
                    Value = new ("The value of variable 1")
                },
                new SetVariable()
                {
                    Variable = variable2,
                    Value = new Input<object?>(variable1)
                },
                new WriteLine(context => $"Variable 2: {variable2.Get(context)}")
            }
        };
    }
}