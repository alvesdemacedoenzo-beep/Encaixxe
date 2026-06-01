using Encaixxe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Encaixxe.Application.Abstractions.Repositories;
using Encaixxe.Infrastructure.Repositories;

namespace Encaixxe.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProdutoRepository, EfProdutoRepository>();
        services.AddScoped<ICategoriaRepository, EfCategoriaRepository>();
        services.AddScoped<IMarcaRepository, EfMarcaRepository>();

        return services;
    }
}