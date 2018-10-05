using System.Text;
using Microsoft.Xrm.Sdk.Query;

namespace PZone.Xrm.Testing
{
    /// <summary>
    /// Расширение стандартного функционала класса <see cref="QueryExpression"/>.
    /// </summary>
    public static class QueryExpressionExtensions
    {
        /// <summary>
        /// Получение данных запроса в виде текста.
        /// </summary>
        /// <param name="query">Экземпляр класса <see cref="QueryExpression"/>.</param>
        /// <returns>
        /// Метод возвращает все данные запроса в виде текста для отображения в окне вывода.
        /// </returns>
        public static string ToPlainText(this QueryExpression query)
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
    }
}