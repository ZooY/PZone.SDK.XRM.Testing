// ReSharper disable CommentTypo


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;


namespace PZone.Xrm.Testing
{
    /// <summary>
    /// Заглушка для классов, реализующих интерфейс <see cref="IOrganizationService"/>.
    /// </summary>
    /// <example>
    /// <code language="cs">
    /// var service = new FakeOrganizationService
    /// {
    ///     Entities = new List&lt;Entity&gt;
    ///     {
    ///         new Entity("opportunity", Guid.NewGuid());
    ///         {
    ///             ["statecode"] = new OptionSetValue(1)
    ///         }
    ///     }
    /// }
    /// var factory = new FakeOrganizationServiceFactory(service);
    /// </code>
    /// </example>
    public class FakeOrganizationService : IOrganizationService, IDisposable
    {
        /// <summary>
        /// Записи, созданные методом <see cref="Create"/>.
        /// </summary>
        public List<Entity> CreatedEntities { get; set; } = new List<Entity>();


        /// <summary>
        /// Записи, измененные методом<see cref="Update"/>.
        /// </summary>
        public List<Entity> UpdatedEntities { get; set; } = new List<Entity>();


        /// <summary>
        /// Запросы, выполняемые методом <see cref="Execute"/>.
        /// </summary>
        public List<OrganizationRequest> ExecutedRequests { get; set; } = new List<OrganizationRequest>();


        /// <summary>
        /// Записи, удаленные методом <see cref="Delete"/>.
        /// </summary>
        public List<Guid> DeletedEntities { get; set; } = new List<Guid>();


        /// <summary>
        /// Набор записей, над которыми будут выполнятся операции методами сервиса.
        /// </summary>
        public List<Entity> Entities { get; set; } = new List<Entity>();


        /// <summary>
        /// Набор метаданных сущностей, над которыми будут выполнятся поперации методами сервиса.
        /// </summary>
        public List<EntityMetadata> EntitiesMetadata { get; set; } = new List<EntityMetadata>();


        /// <summary>
        /// Набор метаданных атрибутов, над которыми будут выполнятся поперации методами сервиса.
        /// </summary>
        public List<AttributeMetadata> AttributesMetadata { get; set; } = new List<AttributeMetadata>();


        /// <summary>
        /// Создание записи сущности.
        /// </summary>
        /// <param name="entity">Данные сущности.</param>
        /// <returns>
        /// Метод возвращает сгенерированный GUID.
        /// </returns>
        /// <remarks>
        /// Метод записывает данные сущности в окно трассировки.
        /// </remarks>
        public virtual Guid Create(Entity entity)
        {
            entity.Id = Guid.Empty;
            var sb = new StringBuilder();
            sb.AppendLine("=== Create entity ===");
            sb.AppendLine();
            sb.AppendLine(entity.ToPlainText());
            System.Diagnostics.Trace.WriteLine(sb.ToString());
            CreatedEntities.Add(entity);
            return entity.Id;
        }


        /// <summary>
        /// Запрос одной записи сущности.
        /// </summary>
        /// <param name="entityName">Имя сущности.</param>
        /// <param name="id">Идентификатор записи сущности.</param>
        /// <param name="columnSet">Набор атрибутов.</param>
        /// <returns>
        /// Метод возвращает подходящую запись из набора <see cref="Entities"/>.
        /// </returns>
        public virtual Entity Retrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== Retrieve entity ===");
            sb.AppendLine();
            sb.AppendLine($"EntityName = {entityName}");
            sb.AppendLine($"ID = {id}");
            sb.AppendLine(columnSet.AllColumns
                ? "ColumnSet = All"
                : $"ColumnSet = {string.Join(", ", columnSet.Columns)}");
            sb.AppendLine();
            Entity entity;
            try
            {
                entity = UseRetrieve(entityName, id, columnSet);
            }
            catch (Exception)
            {
                System.Diagnostics.Trace.WriteLine(sb.ToString());
                throw;
            }
            sb.AppendLine("Retrieved entity");
            sb.AppendLine();
            sb.AppendLine(entity.ToPlainText());
            System.Diagnostics.Trace.WriteLine(sb.ToString());
            return entity;
        }


        public virtual Entity UseRetrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            if (DeletedEntities.Contains(id))
                throw new Exception("The record was previously removed");

            var entity = Entities.FirstOrDefault(e => e.Id == id);
            if (entity == null)
                throw new Exception($"Record \"{entityName}\" with ID = {id} is not found");
            return entity;
        }


        /// <summary>
        /// Обновление записи сущности.
        /// </summary>
        /// <param name="entity">Данные записи.</param>
        /// <remarks>
        /// Метод не производит действий с записью, а лишь отображает информацию о записи в окне трассировки.
        /// </remarks>
        public virtual void Update(Entity entity)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== Update entity ===");
            sb.AppendLine();
            sb.AppendLine(entity.ToPlainText());
            System.Diagnostics.Trace.WriteLine(sb.ToString());
            UpdatedEntities.Add(entity);
        }


        /// <summary>
        /// Удаление записи сущности.
        /// </summary>
        /// <param name="entityName">Имя сущности.</param>
        /// <param name="id">Идентификатор сущности.</param>
        /// <remarks>
        /// Метод записывает данные сущности в окно трассировки.
        /// </remarks>
        public void Delete(string entityName, Guid id)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Delete entity");
            sb.AppendLine($"EntityName = {entityName}");
            sb.AppendLine($"ID = {id}");
            System.Diagnostics.Trace.WriteLine(sb.ToString());
            DeletedEntities.Add(id);
        }


        #region Execute


        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="request">Содержимое запроса.</param>
        /// <returns>
        /// Метод возвращает ответ, на запрос.
        /// </returns>
        /// <remarks>
        /// <para>Поддерживаемые типы запросов:</para>
        /// <list type="bullet">
        /// <item><see cref="Microsoft.Xrm.Sdk.Messages.RetrieveAttributeRequest"/></item>
        /// <item><see cref="RetrieveEntityRequest"/></item>
        /// <item><see cref="RetrieveAllEntitiesRequest"/></item>
        /// <item><see cref="Microsoft.Xrm.Sdk.Messages.ExecuteMultipleRequest"/></item>
        /// </list>
        /// </remarks>
        public virtual OrganizationResponse Execute(OrganizationRequest request)
        {
            ExecutedRequests.Add(request);
            var sb = new StringBuilder();
            OrganizationResponse response;
            try
            {
                switch (request)
                {
                    case RetrieveAttributeRequest retrieveAttributeRequest:
                        sb.AppendLine("=== Retrieve Attribute Request ===");
                        sb.AppendLine();
                        response = UseRetrieveAttributeRequest(retrieveAttributeRequest);
                        break;
                    case RetrieveEntityRequest retrieveEntityRequest:
                        sb.AppendLine("=== Retrieve Entity Request ===");
                        sb.AppendLine();
                        response = UseRetrieveEntityRequest(retrieveEntityRequest);
                        break;
                    case RetrieveAllEntitiesRequest retrieveAllEntitiesRequest:
                        sb.AppendLine("=== Retrieve All Entities Request ===");
                        sb.AppendLine();
                        response = UseRetrieveAllEntitiesRequest(retrieveAllEntitiesRequest);
                        break;
                    case ExecuteMultipleRequest executeMultipleRequest:
                        sb.AppendLine("=== Set State Request ===");
                        sb.AppendLine();
                        response = UseExecuteMultipleRequest(executeMultipleRequest);
                        break;
                    case SetStateRequest setStateRequest:
                        sb.AppendLine("=== Set State Request ===");
                        sb.AppendLine();
                        sb.AppendLine(setStateRequest.ToPlainText());
                        response = UseSetStateRequest(setStateRequest);
                        break;
                    default:
                        throw new NotImplementedException(request.GetType().ToString());
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Trace.WriteLine(sb.ToString());
                throw;
            }
            System.Diagnostics.Trace.WriteLine(sb.ToString());
            return response;
        }


        public virtual OrganizationResponse UseRetrieveAttributeRequest(RetrieveAttributeRequest retrieveAttributeRequest)
        {
            var attributeMetadata = AttributesMetadata.FirstOrDefault(e => e.LogicalName == retrieveAttributeRequest.LogicalName);
            if (attributeMetadata == null)
                throw new Exception($"Attribute metadata with logical name = {retrieveAttributeRequest.LogicalName} is not found");
            var response = new RetrieveAttributeResponse { ["AttributeMetadata"] = attributeMetadata };
            return response;
        }


        public virtual OrganizationResponse UseRetrieveEntityRequest(RetrieveEntityRequest retrieveEntityRequest)
        {
            var entityMetadata = EntitiesMetadata.FirstOrDefault(e => e.LogicalName == retrieveEntityRequest.LogicalName);
            if (entityMetadata == null)
                throw new Exception($"Entity metadata with logical name = {retrieveEntityRequest.LogicalName} is not found");
            var response = new RetrieveEntityResponse { ["EntityMetadata"] = entityMetadata };
            return response;
        }


        public virtual OrganizationResponse UseRetrieveAllEntitiesRequest(RetrieveAllEntitiesRequest retrieveAllEntitiesRequest)
        {
            return new RetrieveAllEntitiesResponse { ["EntityMetadata"] = EntitiesMetadata.ToArray() };
        }


        public virtual OrganizationResponse UseExecuteMultipleRequest(ExecuteMultipleRequest executeMultipleRequest)
        {
            var collection = new ExecuteMultipleResponseItemCollection();
            var i = 0;
            foreach (var requestItem in executeMultipleRequest.Requests)
            {
                if (requestItem is CreateRequest createRequest)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Create entity");
                    createRequest.Target.Id = Guid.Empty;
                    sb.AppendLine(createRequest.Target.ToPlainText());
                    System.Diagnostics.Trace.Write(sb.ToString());
                    var createResponse = new CreateResponse { ["Id"] = Guid.Empty };
                    collection.Add(new ExecuteMultipleResponseItem
                    {
                        RequestIndex = i++,
                        Response = createResponse,
                        Fault = null
                    });
                    continue;
                }

                throw new NotImplementedException(requestItem.GetType().ToString());
            }

            var response = new ExecuteMultipleResponse { Results = { ["Responses"] = collection } };
            return response;
        }


        public virtual OrganizationResponse UseSetStateRequest(SetStateRequest setStateRequest)
        {
            return new OrganizationResponse();
        }
        

        #endregion


        /// <summary>
        /// Связывание записей связью M:M.
        /// </summary>
        /// <param name="entityName">Имя основной сущности.</param>
        /// <param name="entityId">Идентификатор основной записи сущности.</param>
        /// <param name="relationship">Параметры связи.</param>
        /// <param name="relatedEntities">Набор связываемых записей.</param>
        /// <remarks>
        /// <note type="caution">Метод не реализован!</note>
        /// </remarks>
        public void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== Associate entities ===");
            sb.AppendLine();
            sb.AppendLine($"Entity = {entityName} | {entityId}");
            sb.AppendLine($"Relationship = {relationship.SchemaName})");
            sb.AppendLine("Related Entities:");
            foreach (var entityRef in relatedEntities)
                sb.AppendLine("    " + entityRef.ToPlainString());
            System.Diagnostics.Trace.WriteLine(sb.ToString());
        }


        /// <summary>
        /// Разрыв связей между записями со связями М:М.
        /// </summary>
        /// <param name="entityName">Имя основной сущности.</param>
        /// <param name="entityId">Идентификатор основной записи сущности.</param>
        /// <param name="relationship">Параметры связи.</param>
        /// <param name="relatedEntities">Набор связанных записей.</param>
        /// <remarks>
        /// <note type="caution">Метод не реализован!</note>
        /// </remarks>
        public void Disassociate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== Disassociate entities ===");
            sb.AppendLine();
            sb.AppendLine($"Entity = {entityName} | {entityId}");
            sb.AppendLine($"Relationship = {relationship.SchemaName})");
            sb.AppendLine("Related Entities:");
            foreach (var entityRef in relatedEntities)
                sb.AppendLine("    " + entityRef.ToPlainString());
            System.Diagnostics.Trace.WriteLine(sb.ToString());
        }


        #region RetrieveMultiple


        /// <summary>
        /// Поиск записей.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <returns>
        /// Метод возвращает подходящую запись из набора <see cref="Entities"/>.
        /// </returns>
        /// <remarks>
        /// <para>Поддерживаемые типы запросов:</para>
        /// <list type="bullet">
        /// <item><see cref="QueryByAttribute"/>: поиск происходит по значениям атрибутов.</item>
        /// <item><see cref="QueryExpression"/>: возвращаются все записи подходящего типа.</item>
        /// <item><see cref="FetchExpression"/>: возвращаются все записи подходящего типа.</item>
        /// </list>
        /// </remarks>
        public virtual EntityCollection RetrieveMultiple(QueryBase query)
        {
            var sb = new StringBuilder();
            EntityCollection entities;
            try
            {
                switch (query)
                {
                    case QueryExpression queryExpression:
                        sb.AppendLine("=== Retrieve entity collection by QueryExpression ===");
                        sb.AppendLine();
                        sb.AppendLine(queryExpression.ToPlainText());
                        entities = UseQueryExpression(queryExpression);
                        break;
                    case FetchExpression fetchExpression:
                        sb.AppendLine("=== Retrieve entity collection by FetchExpression ===");
                        sb.AppendLine();
                        sb.AppendLine(fetchExpression.Query.Trim());
                        entities = UseFetchExpression(fetchExpression);
                        break;
                    case QueryByAttribute queryByAttribute:
                        sb.AppendLine("=== Retrieve entity collection by QueryByAttribute ===");
                        sb.AppendLine();
                        sb.AppendLine(queryByAttribute.ToPlainText());
                        entities = UseQueryByAttribute(queryByAttribute);
                        break;
                    default:
                        throw new NotImplementedException("RetrieveMultiple для данного вида запроса не реализован.");
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Trace.WriteLine(sb.ToString());
                throw;
            }
            sb.AppendLine();
            System.Diagnostics.Trace.WriteLine(sb.ToString());
            LogRetrieved(entities);
            return entities;
        }


        public virtual EntityCollection UseQueryExpression(QueryExpression query)
        {
            var entities = Entities.Where(e => e.LogicalName == query.EntityName).ToList();
            return new EntityCollection(entities);
        }


        public virtual EntityCollection UseFetchExpression(FetchExpression query)
        {
            var entityName = GetFetchXmlEntity(query.Query);
            var entities = Entities.Where(e => e.LogicalName == entityName).ToList();
            return new EntityCollection(entities);
        }


        public virtual EntityCollection UseQueryByAttribute(QueryByAttribute query)
        {
            var entities = new List<Entity>();
            foreach (var entity in Entities.Where(e => e.LogicalName == query.EntityName))
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                for (var i = 0; i < query.Attributes.Count; i++)
                {
                    var attribute = query.Attributes[i];
                    if (entity.Contains(attribute) && entity[attribute].Equals(query.Values[i]))
                        entities.Add(entity);
                }
            }
            return new EntityCollection(entities);
        }


        protected string GetFetchXmlEntity(string fetchXml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(fetchXml);
            var entityNode = xmlDoc.SelectSingleNode("/fetch/entity");
            if (entityNode?.Attributes == null)
                throw new Exception("Некорректный FetchXML-запрос.");
            return entityNode.Attributes["name"].Value;
        }


        protected void LogRetrieved(EntityCollection entities)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Retrieved " + entities.Entities.Count + " entities");
            foreach (var entity in entities.Entities)
            {
                sb.AppendLine();
                sb.AppendLine(entity.ToPlainText());
            }
            System.Diagnostics.Trace.WriteLine(sb.ToString().TrimEnd());
        }

        
        #endregion


        /// <inheritdoc />
        public void Dispose()
        {
            System.Diagnostics.Trace.WriteLine("Dispose " + GetType());
        }
    }
}