using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TPShoes.Datos;
using TPShoes.Datos.Interfaces;
using TPShoes.Datos.Repositorios;
using TPShoes.Servicios.Interfaces;
using TPShoes.Servicios.Servicios;

namespace TPShoes.IoC
{
    public static class DI
    {
        public static void ConfigurarServicios(IServiceCollection servicios, IConfiguration configuration)
        {
         
            //todos los repositorios
            servicios.AddScoped<IRepositorioBrands,
                RepositorioBrands>();
            servicios.AddScoped<IRepositorioColours,
                RepositorioColours>();
            servicios.AddScoped<IRepositorioGenres,
                RepositorioGenres>();
            servicios.AddScoped<IRepositorioShoes,
                RepositorioShoes>();
            servicios.AddScoped<IRepositorioSports,
                RepositorioSports>();
            servicios.AddScoped<IRepositorioSizes,
                RepositorioSizes>();
            servicios.AddScoped<IRepositorioSizeShoes,
              RepositorioSizeShoes>();

            // todos los servicios
            servicios.AddScoped<IBrandsServicio,
               BrandsServicio>();
            servicios.AddScoped<IColoursServicio,
              ColoursServicio>();
            servicios.AddScoped<IGenresServicio,
              GenresServicio>();
            servicios.AddScoped<IShoesServicio,
              ShoesServicio>();
            servicios.AddScoped<ISportsServicio,
              SportsServicio>();
            servicios.AddScoped<ISizesServicio,
              SizesServicio>();
            servicios.AddScoped<ISizeShoesServicio,
             SizeShoesServicio>();

            //UnityOfWork
            servicios.AddScoped<IUnitOfWork, UnitOfWork>();

            servicios.AddDbContext<DBContextShoes>(optiones =>
            {
                optiones.UseSqlServer(configuration.GetConnectionString("MyConn")); // La cadena de conexion queda en appsettings.json
            });

            
        }

    }

}
