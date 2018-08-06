using System;
using Microsoft.Xrm.Sdk;


namespace PZone.Xrm.Testing
{
    /// <summary>
    /// Заглушка для классов, реализующих интерфейс <see cref="IServiceProvider"/>.
    /// </summary>
    /// <example>
    /// <code language="cs">
    /// var plugin = new MyPlugin();
    ///
    /// var context = new FakePluginExecutionContext();
    /// var factory = new FakeOrganizationServiceFactory();
    /// var provider = new FakeServiceProvider(context, factory);
    /// 
    /// plugin.Execute(provider);
    /// </code>
    /// </example>
    public class FakeServiceProvider : IServiceProvider
    {
        private readonly IPluginExecutionContext _context;
        private readonly IOrganizationServiceFactory _factory;


        /// <inheritdoc />
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IPluginExecutionContext))
                return _context;
            if (serviceType == typeof(IOrganizationServiceFactory))
                return _factory;
            throw new ArgumentOutOfRangeException($@"Unknown service type ""{serviceType.FullName}"".");
        }


        /// <summary>
        /// Конструтор класса.
        /// </summary>
        /// <param name="context">Контекст выполненеия подключаемого модуля.</param>
        /// <param name="factory">Фабрика для создания сервисов.</param>
        public FakeServiceProvider(IPluginExecutionContext context, IOrganizationServiceFactory factory = null)
        {
            _context = context;
            _factory = factory ?? new FakeOrganizationServiceFactory();
        }
    }
}