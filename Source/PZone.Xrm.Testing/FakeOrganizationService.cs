using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;


namespace PZone.Xrm.Testing
{
    /// <summary>
    /// Заглушка для классов, реализующих интерфейс <see cref="IOrganizationService"/>.
    /// </summary>
    public class FakeOrganizationService : IOrganizationService, IDisposable
    {
        /// <summary>
        /// Записи, удаленные методом <see cref="Delete"/>.
        /// </summary>
        protected List<Guid> DeletedEntities = new List<Guid>();


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
            sb.AppendLine("Create entity");
            sb.AppendLine(entity.EntityInfo());
            System.Diagnostics.Trace.Write(sb.ToString());
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
        public Entity Retrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== Retrieve entity ===");
            sb.AppendLine($"EntityName = {entityName}");
            sb.AppendLine($"ID = {id}");
            sb.AppendLine(columnSet.AllColumns
                ? "ColumnSet = All"
                : $"ColumnSet = {string.Join(", ", columnSet.Columns)}");
            System.Diagnostics.Trace.WriteLine(sb.ToString());
            if (DeletedEntities.Contains(id))
            {
                throw new Exception("The record was previously removed");
            }
            var entity = Entities.FirstOrDefault(e => e.Id == id);
            if (entity == null)
                throw new Exception($"Record with ID = {id} is not found");
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
            sb.AppendLine("Update entity");
            sb.AppendLine(entity.EntityInfo());
            System.Diagnostics.Trace.WriteLine(sb.ToString());
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
        /// <item><see cref="RetrieveAttributeRequest"/></item>
        /// <item><see cref="RetrieveEntityRequest"/></item>
        /// <item><see cref="RetrieveAllEntitiesRequest"/></item>
        /// <item><see cref="ExecuteMultipleRequest"/></item>
        /// </list>
        /// </remarks>
        public virtual OrganizationResponse Execute(OrganizationRequest request)
        {
            var retrieveAttributeRequest = request as RetrieveAttributeRequest;
            if (retrieveAttributeRequest != null)
            {
                var attributeMetadata = AttributesMetadata.FirstOrDefault(e => e.LogicalName == retrieveAttributeRequest.LogicalName);
                if (attributeMetadata == null)
                    throw new Exception($"Attribute metadata with logical name = {retrieveAttributeRequest.LogicalName} is not found");
                var response = new RetrieveAttributeResponse { ["AttributeMetadata"] = attributeMetadata };
                return response;
            }
            var retrieveEntityRequest = request as RetrieveEntityRequest;
            if (retrieveEntityRequest != null)
            {
                var entityMetadata = EntitiesMetadata.FirstOrDefault(e => e.LogicalName == retrieveEntityRequest.LogicalName);
                if (entityMetadata == null)
                    throw new Exception($"Entity metadata with logical name = {retrieveEntityRequest.LogicalName} is not found");
                var response = new RetrieveEntityResponse { ["EntityMetadata"] = entityMetadata };
                return response;
            }
            if (request is RetrieveAllEntitiesRequest)
            {
                var response = new RetrieveAllEntitiesResponse { ["EntityMetadata"] = EntitiesMetadata.ToArray() };
                return response;
            }

            var executeMultipleRequest = request as ExecuteMultipleRequest;
            if (executeMultipleRequest != null)
            {
                var collection = new ExecuteMultipleResponseItemCollection();
                var i = 0;
                foreach (var requestItem in executeMultipleRequest.Requests)
                {

                    var createRequest = requestItem as CreateRequest;
                    if (createRequest != null)
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine("Create entity");
                        createRequest.Target.Id = Guid.Empty;
                        sb.AppendLine(createRequest.Target.EntityInfo());
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
            throw new NotImplementedException(request.GetType().ToString());
        }


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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }


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
            // ReSharper disable CanBeReplacedWithTryCastAndCheckForNull
            if (query is QueryExpression)
                return RetrieveMultiple((QueryExpression)query);
            if (query is FetchExpression)
                return RetrieveMultiple((FetchExpression)query);
            if (query is QueryByAttribute)
                return RetrieveMultiple((QueryByAttribute)query);
            throw new NotImplementedException("RetrieveMultiple для данного вида запроса не реализован.");
            // ReSharper restore CanBeReplacedWithTryCastAndCheckForNull
        }


        private EntityCollection RetrieveMultiple(QueryByAttribute query)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== Retrieve entity collection by QueryByAttribute ===");
            sb.AppendLine(QueryExpressionInfo(query));
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

            sb.AppendLine("Retrieved " + entities.Count + " entities");
            System.Diagnostics.Trace.WriteLine(sb.ToString());
            return new EntityCollection(entities);
        }


        private EntityCollection RetrieveMultiple(QueryExpression query)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== Retrieve entity collection by QueryExpression ===");
            sb.AppendLine(QueryExpressionInfo(query));
            var entities = Entities.Where(e => e.LogicalName == query.EntityName).ToList();
            sb.AppendLine("Retrieved " + entities.Count + " entities");
            System.Diagnostics.Trace.WriteLine(sb.ToString());
            return new EntityCollection(entities);
        }


        private EntityCollection RetrieveMultiple(FetchExpression query)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== Retrieve entity collection by FetchExpression ===");
            sb.AppendLine(query.Query);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(query.Query);
            var entityNode = xmlDoc.SelectSingleNode("/fetch/entity");
            if (entityNode?.Attributes == null)
                throw new Exception("Некорректный FetchXML-запрос.");
            var entityName = entityNode.Attributes["name"].Value;
            var entities = Entities.Where(e => e.LogicalName == entityName).ToList();
            sb.AppendLine("Retrieved " + entities.Count + " entities");
            System.Diagnostics.Trace.WriteLine(sb.ToString());
            return new EntityCollection(entities);
        }


        private string QueryExpressionInfo(QueryExpression query)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"EntityName = {query.EntityName}");
            sb.AppendLine(query.ColumnSet.AllColumns
                ? "ColumnSet = All"
                : $"ColumnSet = {string.Join(", ", query.ColumnSet.Columns)}");
            if (query.Criteria != null)
            {
                sb.AppendLine($"Criteria (FilterOperator = {query.Criteria.FilterOperator})");
                foreach (var condition in query.Criteria.Conditions)
                {
                    sb.AppendLine($"\t{condition.AttributeName} {condition.Operator} \"{string.Join("\", \"", condition.Values)}\"");
                }
            }
            return sb.ToString();
        }


        private string QueryExpressionInfo(QueryByAttribute query)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"EntityName = {query.EntityName}");
            sb.AppendLine(query.ColumnSet.AllColumns
                ? "ColumnSet = All"
                : $"ColumnSet = {string.Join(", ", query.ColumnSet.Columns)}");
            sb.AppendLine("Criteria");
            for (var i = 0; i < query.Attributes.Count; i++)
                sb.AppendLine($"\t{query.Attributes[i]} = {query.Values[i]}");
            return sb.ToString();
        }


        /// <inheritdoc />
        public void Dispose()
        {
            System.Diagnostics.Trace.WriteLine("Dispose " + GetType());
        }
    }
}