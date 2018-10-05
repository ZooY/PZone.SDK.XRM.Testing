using System.Text;
using Microsoft.Xrm.Sdk;

namespace PZone.Xrm.Testing
{
    /// <summary>
    /// Расширение стандартного функционала класса <see cref="EntityReference"/>.
    /// </summary>
    public static class EntityReferenceExtensions
    {
        /// <summary>
        /// Получение данных ссылки на сущность в виде строки.
        /// </summary>
        /// <param name="entityRef">Экземпляр класса <see cref="EntityReference"/>.</param>
        /// <param name="indent">Отступ блока текста слева.</param>
        /// <returns>
        /// Метод возвращает все данные ссылки на сущность в виде строки для отображения в окне вывода.
        /// </returns>
        public static string ToPlainText(this EntityReference entityRef, int indent = 0)
        {
            var sb = new StringBuilder();
            var indentString = new string(' ', indent);
            sb.AppendLine($"{indentString}LogicalName = {entityRef.LogicalName}");
            sb.AppendLine($"{indentString}ID = {entityRef.Id}");
            sb.AppendLine($"{indentString}Name = {entityRef.Name}");
            return sb.ToString().TrimEnd();
        }


        /// <summary>
        /// Получение данных ссылки на сущность в виде строки.
        /// </summary>
        /// <param name="entityRef">Экземпляр класса <see cref="EntityReference"/>.</param>
        /// <returns>
        /// Метод возвращает все данные ссылки на сущность в виде строки для отображения в окне вывода.
        /// </returns>
        public static string ToPlainString(this EntityReference entityRef)
        {
            var sb = new StringBuilder();
            sb.Append($"{entityRef.LogicalName} | {entityRef.Id} | {entityRef.Name}");
            return sb.ToString();
        }
    }
}