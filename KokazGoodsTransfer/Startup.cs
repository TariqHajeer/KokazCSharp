using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using KokazGoodsTransfer.HubsConfig;
using Microsoft.AspNetCore.SignalR;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.DAL.Infrastructure.Concret;

namespace KokazGoodsTransfer
{
    public class Startup
    {
        // Scaffold-DbContext "Server=.;Database=Kokaz;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -F
        //> dotnet ef dbcontext scaffold "Server=.;Database=Kokaz;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -F
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        //This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            //services.AddDbContext<KokazContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SmartAsp")));
            services.AddDbContext<KokazContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Local")));
            //services.AddDbContext<KokazContext>(options => options.UseSqlServer(Configuration.GetConnectionString("goldenWingsDB")));

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder
                    .SetIsOriginAllowed(_ => true)
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .WithExposedHeaders("x-paging");
                });
            });



            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;


            }).AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/NotificationHub")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

            }

            );

            services.AddAuthorization(option =>
            {
                option.AddPolicy("Employee", policy =>
                {
                    policy.RequireClaim("Type", "Employee");
                });
                option.AddPolicy("Client", policy =>
                {
                    policy.RequireClaim("Type", "Client");

                });
                option.AddPolicy("Agent", policy =>
                {
                    policy.RequireClaim("Type", "Agent");

                });
            });
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            services.AddSingleton<NotificationHub, NotificationHub>();
            services.AddSignalR();
            services.AddSwaggerGen(s =>
            {

                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Kokaz APi",
                });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        BearerFormat="JWT",
                    },
                    Array.Empty<string>()
                }
            });

            });
            services.AddMemoryCache();
            services.AddScoped(typeof(IIndexRepository<>), typeof(IndexRepository<>));
            services.AddAutoMapper(typeof(Startup));
            RegiserServices(services);
            services.AddScoped<Logging, Logging>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("EnableCORS");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/NotificationHub");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Test");
            });
        }




        private void RegiserServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(ICashedRepository<>), typeof(CashedRepository<>));
            services.AddScoped(typeof(ICountryCashedRepository), typeof(CountryCashedRepository));
        }
    }
}
