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
using Prometheus;
using Jaeger;
using Jaeger.Samplers;
using OpenTracing.Util;
using OpenTracing.Contrib.NetCore;
using System.Reflection;
using Jaeger.Senders;
using Jaeger.Reporters;
using OpenTracing;

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
            var cm = ConnectionMultiplexer.Connect("localhost");
            var quoteStore = new QuoteStore(cm);
            // Register redis quote store
            services.AddSingleton<IQuoteStore>(quoteStore);

            // Register aggregator
            services.AddSingleton<IQuoteAggregator, QuoteAggregator>();

            // Register redis connection
            services.AddSingleton<IConnectionMultiplexer>(cm);

            var quoteResponseReceiver = new QuoteResponseReceiver(cm, quoteStore, "QuoteResult");
            quoteResponseReceiver.StartReceiving();

            services.AddSingleton(serviceProvider =>
            {
                var serviceName = Assembly.GetEntryAssembly().GetName().Name;
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var sampler = new ConstSampler(sample: true);
                ITracer tracer = new Tracer.Builder(serviceName)
                    .WithLoggerFactory(loggerFactory)
                    .WithSampler(sampler)
                    .Build();

                GlobalTracer.Register(tracer);
                return tracer;
            });

            services.AddOpenTracing();
            // Register individual quote providers
            services.AddSingleton<IEnumerable<IQuoteProvider>>(new List<IQuoteProvider>
            {
                new GenericQuoteProvider(GlobalTracer.Instance, cm, "QuoteProviderA"),
                new GenericQuoteProvider(GlobalTracer.Instance, cm, "QuoteProviderB")
            });


            services
                .AddControllers()
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {       
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseMetricServer();
            app.UseHttpMetrics();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
