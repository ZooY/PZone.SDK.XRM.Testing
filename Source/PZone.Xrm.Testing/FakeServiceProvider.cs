using System;
using Microsoft.Xrm.Sdk;


namespace PZone.Xrm.Testing
{
    /// <summary>
    /// Заглушка для классов, реализующих интерфейс <see cref="IServiceProvider"/>.
    /// </summary>
    public class FakeServiceProvider : IServiceProvider
    {
        private readonly IPluginExecutionContext _context;
        private readonly IOrganizationService _service;
        private readonly ITracingService _tracingService;


        /// <inheritdoc />
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IPluginExecutionContext))
                return _context;
			if (serviceType == typeof(IOrganizationService))
                return _service;
            if (serviceType == typeof(ITracingService))
                return _tracingService;
            throw new ArgumentOutOfRangeException($@"Неизвестный тип сервиса ""{serviceType.FullName}"".");
        }


        /// <summary>
        /// Конструтор класса.
        /// </summary>
        /// <param name="context">Контекст выполненеия подключаемого модуля.</param>
        public FakeServiceProvider(IPluginExecutionContext context, IOrganizationService service = null, ITracingService tracingService = null)
        {
            _context = context;
            _service = service;
            _tracingService = tracingService ?? new FakeTracingService();
        }
    }
}