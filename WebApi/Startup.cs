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
using Akka;
using Akka.Actor;
using Akka.Routing;
using WebApi.Actors;

namespace WebApi
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
            services.AddSingleton<ActorSystem>(ctx => {
                return ActorSystem.Create(Configuration["ActorSystem:Name"]);
            });
            services.AddSingleton<IActorRef>(ctx => {                
                ActorSystem actorSystem = ctx.GetRequiredService<ActorSystem>();

                Props actorRouter = 
                    Props
                        .Create<PredictActor>(Configuration["MLModel:Path"])
                        .WithRouter(new RoundRobinPool(Convert.ToInt32(Configuration["ActorSystem:PoolInstances"])));

                IActorRef actorPool = actorSystem.ActorOf(actorRouter,Configuration["ActorSystem:PoolName"]);

                return actorPool;
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
        }
    }
}
