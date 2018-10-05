using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace PZone.Xrm.Testing
{
    /// <summary>
    /// Расширение стандартного функционала класса <see cref="QueryByAttribute"/>.
    /// </summary>
    public static class QueryByAttributeExtentions
    {
        /// <summary>
        /// Получение данных запроса в виде текста.
        /// </summary>
        /// <param name="query">Экземпляр класса <see cref="QueryByAttribute"/>.</param>
        /// <returns>
        /// Метод возвращает все данные запроса в виде текста для отображения в окне вывода.
        /// </returns>
        public static string ToPlainText(this QueryByAttribute query)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"EntityName = {query.EntityName}");
            sb.AppendLine(query.ColumnSet.AllColumns
                ? "ColumnSet = All"
                : $"ColumnSet = {string.Join(", ", query.ColumnSet.Columns)}");
            sb.AppendLine("Criteria");
            for (var i = 0; i < query.Attributes.Count; i++)
                sb.AppendLine($"\t{query.Attributes[i]} = {query.Values[i]}");
            return sb.ToString().TrimEnd();
        }
    }
}