using System;
using Microsoft.Xrm.Sdk;


namespace PZone.Xrm.Testing
{
    /// <summary>
    /// Заглушка для классов, реализующих интерфейс <see cref="IPluginExecutionContext"/>.
    /// </summary>
    /// <example>
    /// <code language="cs">
    /// var entity = new Entity("opportunity", Guid.NewGuid());
    /// {
    ///     ["statecode"] = new OptionSetValue(1)
    /// };
    /// var postEntity = new Entity("opportunity", entity.Id)
    /// {
    ///     ["estimatedvalue"] = new Money(10000)
    /// };
    /// var context = new FakePluginExecutionContext(entity);
    /// context.PostEntityImages.Add("Image", postEntity);
    /// </code>
    /// </example>
    public class FakePluginExecutionContext : IPluginExecutionContext
    {
        private readonly Entity _primaryEntity;


        /// <summary>
        ///   <para>Gets the mode of plug-in execution.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.int32.aspx">Int32</see>The mode of plug-in execution.</para>
        /// </returns>
        /// <remarks>
        ///   <para>Allowable values are defined in the <see langword="SdkMessageProcessingStepMode" /> enumeration defined in SampleCode\CS\HelperCode\OptionSets.cs and SampleCode\VB\HelperCode\OptionSets.vb of the SDK download.</para>
        /// </remarks>
        /// Event execution pipeline
        public int Mode { get; set; }


        /// <summary>
        ///   <para>Gets a value indicating if the plug-in is executing in the sandbox.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.int32.aspx">Int32</see>Indicates if the plug-in is executing in the sandbox. </para>
        /// </returns>
        /// <remarks>
        ///   <para>Allowable values are defined in the <see langword="PluginAssemblyIsolationMode" /> enumeration defined in SampleCode\CS\HelperCode\OptionSets.cs and SampleCode\VB\HelperCode\OptionSets.vb of the SDK download.</para>
        /// </remarks>
        /// Plug-in isolation, trusts, and statistics
        public int IsolationMode { get; set; }


        /// <summary>
        ///   <para>Gets the current depth of execution in the call stack.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.int32.aspx">Int32</see>T the current depth of execution in the call stack.</para>
        /// </returns>
        /// <remarks>
        ///   <para>Used by the platform for infinite loop prevention. In most cases, this property can be ignored.</para>
        ///   <para>Every time a running plug-in or Workflow issues a message request to the Web services that triggers another plug-in or Workflow to execute, the <see cref="P:Microsoft.Xrm.Sdk.IExecutionContext.Depth" /> property of the execution context is increased. If the depth property increments to its maximum value within the configured time limit, the platform considers this behavior an infinite loop and further plug-in or Workflow execution is aborted. </para>
        ///   <para>The maximum depth (8) and time limit (one hour) are configurable by the Microsoft Dynamics 365 administrator using the PowerShell command <see langword="Set-CrmSetting" />. The setting is <see langword="WorkflowSettings.MaxDepth" />. For more information, see, “Administer the deployment using Windows PowerShell” in the <see href="http://technet.microsoft.com/en-us/library/dn531202(v=Crm.7).aspx">Deploying and administering Microsoft Dynamics CRM</see>.</para>
        /// </remarks>
        /// <seealso cref="P:Microsoft.Xrm.Sdk.IExecutionContext.CorrelationId" />
        public int Depth { get; set; }


        /// <summary>
        ///   <para>Gets the name of the Web service message that is being processed by the event execution pipeline.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.string.aspx">String</see>The name of the Web service message being processed by the event execution pipeline.</para>
        /// </returns>
        /// <remarks />
        /// Supported messages and entities for plug-ins
        public string MessageName { get; set; }


        /// <summary>
        /// Идентификатор основной сущности.
        /// </summary>
        public Guid PrimaryEntityId => _primaryEntity?.Id ?? Guid.Empty;


        /// <summary>
        /// Имя основной сущности.
        /// </summary>
        public string PrimaryEntityName => _primaryEntity?.LogicalName;


        /// <summary>
        ///   <para>Gets the GUID of the request being processed by the event execution pipeline.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/System.Nullable.aspx">Nullable</see>&lt;<see href="https://msdn.microsoft.com/library/system.guid.aspx">Guid</see>&gt;The GUID of the request being processed by the event execution pipeline. This corresponds to the <see cref="P:Microsoft.Xrm.Sdk.OrganizationRequest.RequestId" /> property, which is the primary key for the <see cref="T:Microsoft.Xrm.Sdk.OrganizationRequest" /> class from which specialized request classes are derived.</para>
        /// </returns>
        /// <remarks />
        /// Use messages (request and response classes) with the Execute methodEvent execution pipeline
        public Guid? RequestId { get; set; } = Guid.NewGuid();


        /// <summary>
        ///   <para>Gets the name of the secondary entity that has a relationship with the primary entity.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.string.aspx">String</see>The name of the secondary entity that has a relationship with the primary entity.</para>
        /// </returns>
        /// <remarks />
        /// <seealso cref="P:Microsoft.Xrm.Sdk.IExecutionContext.PrimaryEntityName" />
        public string SecondaryEntityName { get; set; }


        /// <summary>
        ///   <para>Gets the parameters of the request message that triggered the event that caused the plug-in to execute.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:Microsoft.Xrm.Sdk.ParameterCollection" />The parameters of the request message that triggered the event that caused the plug-in to execute.</para>
        /// </returns>
        /// <remarks />
        /// Input and output parameters<seealso cref="P:Microsoft.Xrm.Sdk.IExecutionContext.OutputParameters" />
        /// Use messages (request and response classes) with the Execute method
        public ParameterCollection InputParameters { get; set; } = new ParameterCollection();


        /// <summary>
        ///   <para>Gets the parameters of the response message after the core platform operation has completed.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:Microsoft.Xrm.Sdk.ParameterCollection" />The parameters of the response message after the core platform operation has completed.</para>
        /// </returns>
        /// <remarks />
        /// Input and output parameters<seealso cref="P:Microsoft.Xrm.Sdk.IExecutionContext.InputParameters" />
        /// Use messages (request and response classes) with the Execute method
        public ParameterCollection OutputParameters { get; set; } = new ParameterCollection();


        /// <summary>
        ///   <para>Gets the custom properties that are shared between plug-ins.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:Microsoft.Xrm.Sdk.ParameterCollection" />The custom properties that are shared between plug-ins.</para>
        /// </returns>
        /// <remarks />
        /// Pass data between plug-ins
        public ParameterCollection SharedVariables { get; set; } = new ParameterCollection();


        /// <summary>
        ///   <para>Gets the GUID of the system user for whom the plug-in invokes web service methods on behalf of.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.guid.aspx">Guid</see>The GUID of the system user for whom the plug-in invokes web service methods on behalf of. This property corresponds to the <see langword="SystemUserId" /> property, which is the primary key for the <see langword="SystemUser" /> entity.</para>
        /// </returns>
        /// <remarks />
        /// User and team entities
        public Guid UserId { get; set; } = Guid.NewGuid();


        /// <summary>
        ///   <para>Gets the GUID of the system user account under which the current pipeline is executing.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.guid.aspx">Guid</see>The GUID of the system user account under which the current pipeline is executing. This property corresponds to the <see langword="SystemUserId" /> property, which is the primary key for the <see langword="SystemUser" /> entity.</para>
        /// </returns>
        /// <remarks>
        ///   <para />
        /// </remarks>
        /// Impersonation in plug-insUser and team entities
        public Guid InitiatingUserId { get; set; } = Guid.NewGuid();


        /// <summary>
        ///   <para>Gets the GUIDGUID of the business unit that the user making the request, also known as the calling user, belongs to.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.guid.aspx">Guid</see>The GUID of the business unit. This property corresponds to the <see langword="BusinessUnitId" /> property, which is the primary key for the <see langword="BusinessUnit" /> entity.</para>
        /// </returns>
        /// <remarks>
        ///   <para />
        /// </remarks>
        /// BusinessUnit entity
        public Guid BusinessUnitId { get; set; } = Guid.NewGuid();


        /// <summary>
        ///   <para>Gets the GUID of the organization that the entity belongs to and the plug-in executes under.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.guid.aspx">Guid</see>The GUID of the organization that the entity belongs to and the plug-in executes under. This corresponds to the <see langword="OrganizationId" /> attribute, which is the primary key for the <see langword="Organization" /> entity.</para>
        /// </returns>
        /// <remarks />
        /// Organization entities
        public Guid OrganizationId { get; set; } = Guid.NewGuid();


        /// <summary>
        ///   <para>Gets the unique name of the organization that the entity currently being processed belongs to and the plug-in executes under.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.string.aspx">String</see>The unique name of the organization that the entity currently being processed belongs to and the plug-in executes under.</para>
        /// </returns>
        /// <remarks>
        ///   <para>See the <see langword="Name" /> attribute of the <see langword="Organization" /> entity.</para>
        /// </remarks>
        /// Organization entities
        public string OrganizationName { get; set; }


        /// <summary>
        ///   <para>Gets the properties of the primary entity before the core platform operation has begins.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:Microsoft.Xrm.Sdk.EntityImageCollection" />The properties of the primary entity before the core platform operation has begins.</para>
        /// </returns>
        /// <remarks />
        /// <seealso cref="P:Microsoft.Xrm.Sdk.IExecutionContext.PostEntityImages" />
        /// Pre and post entity images
        public EntityImageCollection PreEntityImages { get; set; } = new EntityImageCollection();


        /// <summary>
        ///   <para>Gets the properties of the primary entity after the core platform operation has been completed.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:Microsoft.Xrm.Sdk.EntityImageCollection" />The properties of the primary entity after the core platform operation has been completed.</para>
        /// </returns>
        /// <remarks />
        /// <seealso cref="P:Microsoft.Xrm.Sdk.IExecutionContext.PreEntityImages" />
        /// Pre and post entity images
        public EntityImageCollection PostEntityImages { get; set; } =new EntityImageCollection();


        /// <summary>
        ///   <para>Gets a reference to the related <see langword="SdkMessageProcessingingStep" /> or <see langword="ServiceEndpoint" />.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:Microsoft.Xrm.Sdk.EntityReference" />A reference to the related <see langword="SdkMessageProcessingingStep" /> or <see langword="ServiceEndpoint" />.entity.</para>
        /// </returns>
        /// <remarks>
        ///   <para>An <see langword="SdkMessageProcessingingStep" /> entity is used for plug-in registration and a <see langword="ServiceEndpoint" /> entity is used for Microsoft Azure integration.</para>
        /// </remarks>
        /// Plug-in registration entitiesServiceEndpoint entity messages and methodsWork with Dynamics 365 data in your Azure solution
        public EntityReference OwningExtension { get; set; }


        /// <summary>
        ///   <para>Gets the GUID for tracking plug-in or custom workflow activity execution. </para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.guid.aspx">Guid</see>The GUID for tracking plug-in or custom workflow activity execution.</para>
        /// </returns>
        /// <remarks>
        ///   <para>This property is used by the platform for infinite loop prevention. In most cases, this property can be ignored.</para>
        /// </remarks>
        /// <seealso cref="P:Microsoft.Xrm.Sdk.IExecutionContext.Depth" />
        public Guid CorrelationId { get; set; } = Guid.NewGuid();


        /// <summary>
        ///   <para>Gets whether the plug-in is executing from the Microsoft Dynamics 365 for Microsoft Office Outlook with Offline Access client while it is offline. </para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.boolean.aspx">Boolean</see>true if the plug-in is executing from the Microsoft Dynamics 365 for Microsoft Office Outlook with Offline Access client while it is offline; otherwise, false.</para>
        /// </returns>
        /// <remarks />
        /// Write a plug-in<seealso cref="P:Microsoft.Xrm.Sdk.IExecutionContext.IsOfflinePlayback" />
        public bool IsExecutingOffline { get; set; }


        /// <summary>
        ///   <para>Gets a value indicating if the plug-in is executing as a result of the Microsoft Dynamics 365 for Microsoft Office Outlook with Offline Access client transitioning from offline to online and synchronizing with the Microsoft Dynamics 365 server.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.boolean.aspx">Boolean</see>true if the the plug-in is executing as a result of the Microsoft Dynamics 365 for Microsoft Office Outlook with Offline Access client transitioning from offline to online; otherwise, false.</para>
        /// </returns>
        /// <remarks />
        /// <seealso cref="P:Microsoft.Xrm.Sdk.IExecutionContext.IsExecutingOffline" />
        public bool IsOfflinePlayback { get; set; }


        /// <summary>
        ///   <para>Gets a value indicating if the plug-in is executing within the database transaction.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.boolean.aspx">Boolean</see>true if the plug-in is executing within the database transaction; otherwise, false.</para>
        /// </returns>
        /// <remarks />
        /// Event execution pipeline
        public bool IsInTransaction { get; set; }


        /// <summary>
        ///   <para>Gets the GUID of the related <see langword="System Job" />.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.guid.aspx">Guid</see>The GUID of the related <see langword="System Job" />. This corresponds to the <see langword="AsyncOperationId" /> attribute, which is the primary key for the <see langword="System Job" /> entity.</para>
        /// </returns>
        /// <remarks>
        ///   <para>This property is used for the Microsoft Dynamics 365 to Microsoft Azure integration feature. </para>
        /// </remarks>
        /// <seealso cref="P:Microsoft.Xrm.Sdk.IExecutionContext.OperationCreatedOn" />
        /// AsyncOperation (system job) entityAzure integration with Microsoft Dynamics 365
        public Guid OperationId { get; set; } = Guid.NewGuid();


        /// <summary>
        ///   <para>Gets the date and time that the related <see langword="System Job" /> was created.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.datetime.aspx">DateTime</see>The date and time that the related <see langword="System Job" /> was created.</para>
        /// </returns>
        /// <remarks>
        ///   <para>This property is used for the Microsoft Dynamics 365 to Microsoft Azure integration feature. It contains the same date and time value as the <see langword="CreatedOn" /> property of the related <see langword="System Job" /> entity.</para>
        /// </remarks>
        /// <seealso cref="P:Microsoft.Xrm.Sdk.IExecutionContext.OperationId" />
        /// AsyncOperation (system job) entityAzure integration with Microsoft Dynamics 365
        public DateTime OperationCreatedOn { get; set; }


        /// <summary>
        ///   <para>Gets the stage in the execution pipeline that a synchronous plug-in is registered for. </para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.int32.aspx">Int32</see>.  Valid values are 10 (pre-validation), 20 (pre-operation), 40 (post-operation), and 50 (post-operation, deprecated).</para>
        /// </returns>
        /// <remarks>
        ///   <para>A stage identifies when a registered synchronous plug-in is to execute during the processing of a message request. Plug-ins can be further ordered within a stage using the <see langword="SdkMessageProcessingStep.Rank" /> attribute.</para>
        ///   <para>An enumeration named <see langword="SdkMessageProcessingStepStage" /> that defines the supported range of values for this property is available by including the file SampleCode\CS\HelperCode\OptionSets.cs or SampleCode\VB\HelperCode\OptionSets.vb in your project.</para>
        /// </remarks>
        /// Plug-in registration entitiesEvent execution pipeline
        public int Stage { get; set; }


        /// <summary>
        ///   <para>Gets the execution context from the parent pipeline operation.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:Microsoft.Xrm.Sdk.IPluginExecutionContext" />The execution context from the parent pipeline operation.</para>
        /// </returns>
        /// <remarks>
        ///   <para>This property may be populated for a plug-in registered in stage 20 or 40 when a Create, Update, Delete, or RetrieveExchangeRate request is processed by the execution pipeline.</para>
        ///   <para>Certain message requests require the platform to internally generate another message request, and hence another execution pipeline, to complete processing of the original request. For example, pipeline execution of the <see cref="T:Microsoft.Crm.Sdk.Messages.AssignRequest" /> results in the internal generation of an <see cref="T:Microsoft.Xrm.Sdk.Messages.UpdateRequest" />. Any plug-in registered in stage 20 or 40 for an update request will have the ParentContext property set to the execution context of the original assign request.</para>
        /// </remarks>
        /// Event execution pipelinePass data between plug-ins
        public IPluginExecutionContext ParentContext { get; set; }


        /// <summary>
        /// Конструтор класса без параметров.
        /// </summary>
        public FakePluginExecutionContext()
        {
        }


        /// <summary>
        /// Конструтор класса с указанием основной сущности.
        /// </summary>
        /// <param name="primaryEntity">Основная сущность.</param>
        public FakePluginExecutionContext(Entity primaryEntity) : this()
        {
            _primaryEntity = primaryEntity;
            InputParameters = new ParameterCollection
            {
                { "Target", _primaryEntity }
            };
        }
    }
}