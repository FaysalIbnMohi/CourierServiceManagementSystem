using Injection;
using Injection.Interfaces;
using CSSService;

namespace courier_service_system
{
    public class Injector
    {
        public static IInjectionContainer Container { get; set; }
        static Injector()
        {
            Container = new Container();
        }

        public static void Configure()
        {
            Container.Register<IEmployeeService, EmployeeService>().Singleton();
            Container.Register<ICustomerService, CustomerService>().Singleton();
            Container.Register<ILoginReportService, LoginReportService>().Singleton();
            Container.Register<IOfficeService, OfficeService>().Singleton();
            Container.Register<IMailModelService, MailModelService>().Singleton();
            Container.Register<IProductService, ProductService>().Singleton();
            Container.Register<ITripService, TripService>().Singleton();
            Container.Register<IVehicleService, VehicleService>().Singleton();
            Container.Register<IRequestUpdateService, RequestUpdateService>().Singleton();

        }
    }
}