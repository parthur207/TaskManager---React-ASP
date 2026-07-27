using MimeKit.Cryptography;
using TaskManager.Core.UseCases.AI.Interfaces;

namespace TaskManager.API.Facades
{
    public class AiUseCaseCaseFacade
    {
        public AiUseCaseCaseFacade(IAI_PriorityTasksUseCase aI_PriorityTasksUseCase, IAI_RefineTaskAttributesUseCase aI_RefineTaskAttributesUseCase,
            IAI_GenerateExecutionPlanUseCase aI_GenerateExecutionPlanUseCase, IAI_GenerateChildTaskUseCase aI_IGenerateSubTaskUseCase)
        {
            this.aI_PriorityTasksUseCase = aI_PriorityTasksUseCase;
            this.aI_RefineTaskAttributesUseCase = aI_RefineTaskAttributesUseCase;
            this.aI_GenerateExecutionPlanUseCase = aI_GenerateExecutionPlanUseCase;
            this.aI_IGenerateSubTaskUseCase = aI_IGenerateSubTaskUseCase;
        }

        public IAI_PriorityTasksUseCase aI_PriorityTasksUseCase { get; }
        public IAI_RefineTaskAttributesUseCase aI_RefineTaskAttributesUseCase { get; }
        public IAI_GenerateExecutionPlanUseCase aI_GenerateExecutionPlanUseCase { get; }
        public IAI_GenerateChildTaskUseCase aI_IGenerateSubTaskUseCase { get; }
    }
}
