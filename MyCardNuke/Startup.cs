using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Swashbuckle.AspNetCore;
using MyCardNuke.Domain;

using MyCardNukeDataLib.Repository;
using MyCardNukeDataLib.Context;

using Microsoft.EntityFrameworkCore;


using MediatR;

namespace MyCardNuke
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<IEventStoreCard, EventStoreCard>();
            services.AddMediatR();

            var connectionString = Configuration["ConnectionStrings:CardAccessPostgreSqlProvider"];
           
            services.AddDbContext<CardContext>(o => o.UseNpgsql(connectionString, b =>
                                                                b.MigrationsAssembly("MyCardNuke")));
            services.AddScoped<ICardRepository, CardRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "CardNuke API", Version = "v1" });
                c.DescribeAllEnumsAsStrings();

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(x =>
               {
                   x.SwaggerEndpoint("/swagger/v1/swagger.json", "CardNuke API V1");
                   x.ShowExtensions();

               });
        }
    }
}
