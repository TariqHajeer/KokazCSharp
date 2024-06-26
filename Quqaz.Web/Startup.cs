using System;
using System.Threading.Tasks;
using Quqaz.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Quqaz.Web.HubsConfig;
using Microsoft.AspNetCore.SignalR;
using Quqaz.Web.Helpers;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.DAL.Infrastructure.Concret;
using Quqaz.Web.Services.Interfaces;
using Quqaz.Web.Services.Concret;
using Quqaz.Web.Middlewares;
using Wkhtmltopdf.NetCore;
using Quqaz.Web.Services.Interfaces.Additional;
using Quqaz.Web.Services.Concret.Additional;
using System.IO;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using OfficeOpenXml;

namespace Quqaz.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostEnvironment _hostEnvironment;
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }



        //This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            services.AddControllers();
            services.AddDbContext<KokazContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BranchDB")));
            services.AddHttpContextAccessor();
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
            services.AddHttpContextAccessor();
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
                option.AddPolicy("TreasuryOfficial", policy =>
                {
                    policy.RequireClaim("TreasuryOfficial", true.ToString());
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
                    Title = "Quqaz",
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
                s.OperationFilter<AddRequiredHeaderParameter>();
            });
            services.AddMemoryCache();
            services.AddScoped(typeof(IIndexRepository<>), typeof(IndexRepository<>));
            services.AddAutoMapper(typeof(Startup));
            RegiserServices(services);
            services.AddWkhtmltopdf("wkhtmltopdf");
            services.AddScoped<Logging, Logging>();
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(_hostEnvironment.ContentRootPath, Configuration["firebaseFileName"]))
            });
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
            app.UseMiddleware<ErrorMiddlewares>();
            app.UseAuthentication();
            app.UseMiddleware<PrivilegesMiddlewares>();
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
            if (env.IsDevelopment())
            {
                using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
                scope?.ServiceProvider.GetRequiredService<KokazContext>().Database.Migrate();
            }
        }




        private void RegiserServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ICountryCashedService, CountryCashedService>();
            services.AddScoped<IRegionCashedService, RegionCashedService>();
            services.AddScoped<IUserCashedService, UserCashedSerivce>();
            services.AddScoped<IClientCashedService, ClientCashedService>();
            services.AddScoped<IIncomeTypeSerive, IncomeTypeSerivce>();
            services.AddScoped<IOutcomeTypeService, OutcomeTypeService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped(typeof(IIndexService<>), typeof(IndexService<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITreasuryService, TreasuryService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IIncomeService, IncomeService>();
            services.AddScoped<IOutcomeService, OutcomeService>();
            services.AddScoped<IUintOfWork, UnitOfWork>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IAgentPrintService, AgentPrintService>();
            services.AddScoped<IPointSettingService, PointSettingService>();
            services.AddScoped<IHttpContextAccessorService, HttpContextAccessorService>();
            services.AddScoped<IPaymentRequestSerivce, PaymentRequestSerivce>();
            services.AddScoped<IPaymentWayService, PaymentWayService>();
            services.AddScoped<IEditRequestService, EditRequestService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddScoped<IOrderTypeCashService, OrderTypeCashService>();
            services.AddScoped<IReceiptService, ReceiptService>();
            services.AddScoped<IOrderClientSerivce, OrderClientSerivce>();
            services.AddScoped<IClientPaymentService, ClientPaymentService>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IClientMessageService, ClientMessageService>();
        }
    }
}