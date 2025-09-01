using Finanzauto.Application.Interfaces;
using Finanzauto.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Finanzauto.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IShipperService, ShipperService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            return services;
        }
    }
}
