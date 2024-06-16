using Application.Configuration;
using Application.Deductibles;
using Application.Services;
using Application.TaxCalculators;
using Core.Configuration;
using Core.Deductibles;
using Core.Services;
using Core.TaxCalculators;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;

            // Add services to the container.

            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddTransient<IConfigurationFactory, AppSettingsConfigurationFactory>();
            services.AddTransient<IDeductibleFactory, DeductibleFactory>();
            services.AddTransient<ITaxCalculatorService, TaxCalculatorService>();

            // Tax calculator and Deductible calculator Services
            services.AddTransient<ITaxCalculator, DefaultIncomeTaxCalculator>();
            services.AddTransient<ITaxCalculator, DefaultSocialContributionCalculator>();
            services.AddTransient<IDeductibleCalculator, DefaultCharityDeductibleCalculator>();

            services.AddTransient<ICacheService, InMemoryCacheService>();
            services.AddMemoryCache();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
