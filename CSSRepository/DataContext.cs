using System.Data.Entity;
using CSSEntity;



namespace CSSRepository
{
    public class DataContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<LoginReport> LoginReports { get; set; }
        public DbSet<MailModel> MailModels { get; set; }
        public DbSet<RequestUpdate> RequestUpdates { get; set; }

        private DataContext() { }
        public static DataContext context = null;
        public static DataContext getInstance()
        {
            if (context == null)
            {
                context = new DataContext();
                return context;
            }
            return context;
        }
    }
}
