using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTM.QuoteAPI.Model;
using CTM.QuoteAPI.Model.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace CTM.QuoteAPI
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

            // Register redis quote store
            services.AddSingleton<IQuoteStore, QuoteStore>();

            // Register aggregator
            services.AddSingleton<IQuoteAggregator, QuoteAggregator>();

            // Register individual quote providers
            services.AddSingleton<IEnumerable<IQuoteProvider>>(new List<IQuoteProvider>
            {
                new GenericQuoteProvider("QuoteProviderA"),
                new GenericQuoteProvider("QuoteProviderB")
            });

            // Register redis connection
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
