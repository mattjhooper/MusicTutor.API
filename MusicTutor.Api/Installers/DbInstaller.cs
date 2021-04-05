using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicTutor.Api.Validators.Pupils;
using MusicTutor.Core.Services;
using MusicTutor.Data.EFCore;
using MusicTutor.Data.EFCore.Services;

namespace MusicTutor.Api.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MusicTutorDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default"), x => x.MigrationsAssembly("MusicTutor.Data")));

            //services.AddDbContext<MusicTutorDbContext>(opt =>
            //    opt.UseInMemoryDatabase("MusicTutorFull")); 

            services.AddScoped<IMusicTutorDbContext>(provider => provider.GetService<MusicTutorDbContext>());

            services.AddHealthChecks().AddDbContextCheck<MusicTutorDbContext>();

            services.AddScoped<IDataService, DataServiceEFCore>();

            services.AddScoped<IDbValidator, DbValidator>();
        }
    }
}