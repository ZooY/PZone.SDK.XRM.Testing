using System.Text;
using Microsoft.Crm.Sdk.Messages;

namespace PZone.Xrm.Testing
{
    /// <summary>
    /// Расширение стандартного функционала класса <see cref="SetStateRequest"/>.
    /// </summary>
    public static class SetStateRequestExtentions
    {
        /// <summary>
        /// Получение данных запроса в виде текста.
        /// </summary>
        /// <param name="request">Экземпляр класса <see cref="SetStateRequest"/>.</param>
        /// <returns>
        /// Метод возвращает все данные запроса в виде текста для отображения в окне вывода.
        /// </returns>
        public static string ToPlainText(this SetStateRequest request)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"EntityMoniker = {request.EntityMoniker.ToPlainString()}");
            sb.AppendLine($"State = {request.State.Value}");
            sb.AppendLine($"Status = {request.Status.Value}");
            return sb.ToString().TrimEnd();
        }
    }
}