namespace ExternalSearch.KnowledgeGraph.Integration.Tests
{
    public class KnowledgeGraphTests
    {
        // private TestContext testContext;
        // TODO Issue 170 - Unit Test Failures

        // [Fact]
        // public void Test()
        // {
        //     // Arrange
        //     this.testContext = new TestContext();
        //     IEntityMetadata entityMetadata = new EntityMetadataPart()
        //     {
        //         Name = "Microsoft",
        //         EntityType = EntityType.Organization
        //     };

        //     var externalSearchProvider = new Mock<KnowledgeGraphExternalSearchProvider>(MockBehavior.Loose);
        //     var clues = new List<CompressedClue>();

        //     externalSearchProvider.CallBase = true;

        //     this.testContext.ProcessingHub.Setup(h => h.SendCommand(It.IsAny<ProcessClueCommand>())).Callback<IProcessingCommand>(c => clues.Add(((ProcessClueCommand)c).Clue));
        //     this.testContext.Container.Register(Component.For<IExternalSearchProvider>().UsingFactoryMethod(() => externalSearchProvider.Object));

        //     var context = this.testContext.Context.ToProcessingContext();
        //     var command = new ExternalSearchCommand();
        //     var actor = new ExternalSearchProcessingAccessor(context.ApplicationContext);
        //     var workflow = new Mock<Workflow>(MockBehavior.Loose, context, new EmptyWorkflowTemplate<ExternalSearchCommand>());

        //     workflow.CallBase = true;

        //     command.With(context);
        //     command.OrganizationId = context.Organization.Id;
        //     command.EntityMetaData = entityMetadata;
        //     command.Workflow = workflow.Object;
        //     context.Workflow = command.Workflow;

        //     // Act
        //     var result = actor.ProcessWorkflowStep(context, command);
        //     Assert.Equal(WorkflowStepResult.Repeat.SaveResult, result.SaveResult);

        //     result = actor.ProcessWorkflowStep(context, command);
        //     Assert.Equal(WorkflowStepResult.Success.SaveResult, result.SaveResult);
        //     context.Workflow.AddStepResult(result);

        //     context.Workflow.ProcessStepResult(context, command);

        //     // Assert
        //     this.testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

        //     Assert.True(clues.Count > 0);
        // }

        // TODO Issue 170 - Unit Test Failures
        //[Fact]
        //public void Test2()
        //{
        //    // Arrange
        //    this.testContext = new TestContext();
        //    IEntityMetadata entityMetadata = new EntityMetadataPart()
        //    {
        //        Name = "Bang & Olufsen",
        //        EntityType = EntityType.Organization
        //    };

        //    var externalSearchProvider = new Mock<KnowledgeGraphExternalSearchProvider>(MockBehavior.Loose);
        //    var clues = new List<CompressedClue>();

        //    externalSearchProvider.CallBase = true;

        //    this.testContext.ProcessingHub.Setup(h => h.SendCommand(It.IsAny<ProcessClueCommand>())).Callback<IProcessingCommand>(c => clues.Add(((ProcessClueCommand)c).Clue));
        //    this.testContext.Container.Register(Component.For<IPropertyTranslationService>().UsingFactoryMethod(() => new PropertyTranslationService()));

        //    var context = this.testContext.Context.ToProcessingContext();
        //    var command = new ExternalSearchCommand();
        //    var actor = new ExternalSearchProcessing(context.ApplicationContext);
        //    var workflow = new Mock<Workflow>(MockBehavior.Loose, context, new EmptyWorkflowTemplate<ExternalSearchCommand>());

        //    workflow.CallBase = true;

        //    command.With(context);
        //    command.OrganizationId = context.Organization.Id;
        //    command.EntityMetaData = entityMetadata;
        //    command.Workflow = workflow.Object;
        //    context.Workflow = command.Workflow;

        //    // Act
        //    var result = actor.ProcessWorkflowStep(context, command);
        //    Assert.Equal(WorkflowStepResult.Repeat.SaveResult, result.SaveResult);

        //    result = actor.ProcessWorkflowStep(context, command);
        //    Assert.Equal(WorkflowStepResult.Success.SaveResult, result.SaveResult);
        //    context.Workflow.AddStepResult(result);

        //    context.Workflow.ProcessStepResult(context, command);

        //    // Assert
        //    this.testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

        //    Assert.True(clues.Count > 0);
        //}
    }
}
