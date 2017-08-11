using System.Text;
using Microsoft.Xrm.Sdk;


namespace PZone.Xrm.Testing
{
    /// <summary>
    /// Заглушка для классов, реализующих интерфейс <see cref="ITracingService"/>.
    /// </summary>
    public class FakeTracingService : ITracingService
    {
        /// <summary>
        /// Запись строки в окно трассировки.
        /// </summary>
        /// <param name="format">Формат строки.</param>
        /// <param name="args">Аргументы для подстановки в строку формата.</param>
        public void Trace(string format, params object[] args)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== Сообщение в Trasing Service ===");
            if (args == null || args.Length < 1)
                sb.AppendLine(format);
            else
                sb.AppendLine(string.Format(format, args));
            System.Diagnostics.Trace.WriteLine(sb.ToString());
        }
    }
}