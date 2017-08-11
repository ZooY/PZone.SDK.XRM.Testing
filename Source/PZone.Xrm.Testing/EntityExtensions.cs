using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;


namespace PZone.Xrm.Testing
{
    /// <summary>
    /// Расширение стандартного функционала классов, реализующих интерфейс <see cref="Entity"/>.
    /// </summary>
    public static class EntityExtensions
    // ReSharper restore CheckNamespace
    {
        /// <summary>
        /// Получение данных сущности в виде строки.
        /// </summary>
        /// <param name="entity">Экземпляр класса <see cref="Entity"/>.</param>
        /// <returns>
        /// Метод возвращает все данные сущности в виде строки для отображения в окне вывода.
        /// </returns>
        public static string EntityInfo(this Entity entity)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"LogicalName = {entity.LogicalName}");
            sb.AppendLine($"ID = {entity.Id}");
            sb.AppendLine(entity.EntityState.HasValue ? $"EntityState = {entity.EntityState.Value}" : "EntityState = null");
            foreach (var attribute in entity.Attributes)
            {
                // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
                if (attribute.Value is EntityReference)
                {
                    var entityReference = (EntityReference)attribute.Value;
                    sb.AppendLine($"{attribute.Key} = {entityReference.LogicalName} | {entityReference.Id} | {entityReference.Name}");
                }
                else if (attribute.Value is OptionSetValue)
                {
                    var optionSetValue = (OptionSetValue)attribute.Value;
                    sb.AppendLine($"{attribute.Key} = {optionSetValue.Value} | {(entity.FormattedValues.Where(v => v.Key == attribute.Key).Select(v => v.Value).FirstOrDefault())}");
                }
                else if (attribute.Value is EntityCollection)
                {
                    var entityCollection = (EntityCollection)attribute.Value;
                    sb.AppendLine(attribute.Key + " =>");
                    foreach (var entityInCollection in entityCollection.Entities)
                        sb.Append(entityInCollection.EntityInfo());
                }
                else
                    sb.AppendLine($"{attribute.Key} = {attribute.Value}");
            }
            return sb.ToString();
        }
    }
}