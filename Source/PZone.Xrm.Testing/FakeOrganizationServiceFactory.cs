using System;
using Microsoft.Xrm.Sdk;


namespace PZone.Xrm.Testing
{
    /// <summary>
    /// Заглушка для классов, реализующих интерфейс <see cref="IOrganizationServiceFactory"/>.
    /// </summary>
    public class FakeOrganizationServiceFactory : IOrganizationServiceFactory
    {
        private readonly IOrganizationService _service;


        /// <summary>
        /// Получение экземпляра сервиса.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>
        /// Метод возвращает сервис, указанный в конструкторе <see cref="FakeOrganizationServiceFactory(IOrganizationService)"/>.
        /// </returns>
        public IOrganizationService CreateOrganizationService(Guid? userId)
        {
            return _service;
        }


        /// <summary>
        /// Конструтор класса без параметров.
        /// </summary>
        public FakeOrganizationServiceFactory()
        {
            _service = new FakeOrganizationService();
        }


        /// <summary>
        /// Конструтор класса с указанием экземпляра сервиса.
        /// </summary>
        /// <param name="service">Реальный или фейковый Organization Service.</param>
        public FakeOrganizationServiceFactory(IOrganizationService service)
        {
            _service = service;
        }
    }
}