using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;


namespace PZone.Xrm.Testing
{
    /// <summary>
    /// Расширение стандартного функционала класса <see cref="Entity"/>.
    /// </summary>
    public static class EntityExtensions
        // ReSharper restore CheckNamespace
    {
        /// <summary>
        /// Получение данных сущности в виде текста.
        /// </summary>
        /// <param name="entity">Экземпляр класса <see cref="Entity"/>.</param>
        /// <returns>
        /// Метод возвращает все данные сущности в виде текста для отображения в окне вывода.
        /// </returns>
        public static string ToPlainText(this Entity entity)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"LogicalName = {entity.LogicalName}");
            sb.AppendLine($"ID = {entity.Id}");
            foreach (var attribute in entity.Attributes)
            {
                switch (attribute.Value)
                {
                    case EntityReference entityReference:
                        sb.AppendLine($"{attribute.Key} = {entityReference.ToPlainString()}");
                        break;
                    case OptionSetValue optionSetValue:
                        sb.AppendLine($"{attribute.Key} = {optionSetValue.Value} | {(entity.FormattedValues.Where(v => v.Key == attribute.Key).Select(v => v.Value).FirstOrDefault())}");
                        break;
                    case EntityCollection entityCollection:
                    {
                        sb.AppendLine(attribute.Key + " =>");
                        foreach (var entityInCollection in entityCollection.Entities)
                            sb.Append(entityInCollection.ToPlainText());
                        break;
                    }
                    case Money money:
                        sb.AppendLine($"{attribute.Key} = {money.Value}");
                        break;
                    case AliasedValue alias:
                        switch (alias.Value)
                        {
                            //case EntityReference entityReference:
                            //    sb.AppendLine($"{attribute.Key} = {entityReference.ToPlainString()}");
                            //    break;
                            //case OptionSetValue optionSetValue:
                            //    sb.AppendLine($"{attribute.Key} = {optionSetValue.Value} | {(entity.FormattedValues.Where(v => v.Key == attribute.Key).Select(v => v.Value).FirstOrDefault())}");
                            //    break;
                            //case EntityCollection entityCollection:
                            //{
                            //    sb.AppendLine(attribute.Key + " =>");
                            //    foreach (var entityInCollection in entityCollection.Entities)
                            //        sb.Append(entityInCollection.ToPlainText());
                            //    break;
                            //}
                            case Money money:
                                sb.AppendLine($"{attribute.Key} = {money.Value}");
                                break;
                            default:
                                sb.AppendLine($"{attribute.Key} = {attribute.Value}");
                                break;
                        }

                        break;
                    default:
                        sb.AppendLine($"{attribute.Key} = {attribute.Value}");
                        break;
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}