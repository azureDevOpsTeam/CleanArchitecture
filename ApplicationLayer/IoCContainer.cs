#region Usings

using ApplicationLayer.Behaviors;
using ApplicationLayer.Common.Validations;
using ApplicationLayer.Extensions;
using DomainLayer.Common.Attributes;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;
using System.Security.Claims;
using System.Text;

#endregion

namespace ApplicationLayer
{
    public static class IoCContainer
    {
        public static IServiceCollection Register(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterService(configuration);
            services.AddSingleton<IApplicationBuilder, ApplicationBuilder>();
            services.AddHttpContextAccessor();
            services.MediatRDependency();
            services.RegisterServicesAutomatically();
            services.FluentValidationConfiguration();
            services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));

            services.AddCors(opt => opt.AddPolicy("AllowSpecificOrigin", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            return services;
        }

        private static void RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigurationDependency();
            services.AddMemoryCache();
            services.SwaggerConfiguration(configuration);
            services.JwtAuthorizeConfiguration(configuration);
            services.SeriLogConfiguration(configuration);
        }

        private static void SeriLogConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var columnOptions = new ColumnOptions();
            // Define additional custom columns if needed
            //columnOptions.AdditionalColumns = new Collection<SqlColumn>
            //{
            //    new SqlColumn { ColumnName = "CustomColumn1", DataType = System.Data.SqlDbType.NVarChar, DataLength = 100 },
            //    new SqlColumn { ColumnName = "CustomColumn2", DataType = System.Data.SqlDbType.Int }
            //};

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                    connectionString: configuration.GetConnectionString("LogConnection"),
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = configuration.GetSection("Serilog:TableName").Value,
                        SchemaName = configuration.GetSection("Serilog:SchemaName").Value,
                        AutoCreateSqlTable = true
                    },
                    columnOptions: columnOptions
                )
                .CreateLogger();

            services.AddSingleton(Log.Logger);
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));
        }

        private static void SwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(configuration["Swagger:Name"], new OpenApiInfo
                {
                    Version = configuration["Swagger:Version"],
                    Title = configuration["Swagger:Title"],
                    Description = configuration["Swagger:Description"],
                    TermsOfService = new Uri(configuration["Swagger:TermsOfServiceUrl"]),
                    Contact = new OpenApiContact
                    {
                        Name = configuration["Swagger:ContactName"],
                        Url = new Uri(configuration["Swagger:ContactUrl"])
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        private static void JwtAuthorizeConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var Key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);

            var tokenValidationParameter = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.TokenValidationParameters = tokenValidationParameter;

                option.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;

                        // Map custom role claim if necessary
                        var roleClaims = claimsIdentity.FindAll("role").ToList();
                        foreach (var roleClaim in roleClaims)
                        {
                            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, roleClaim.Value));
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddSingleton(tokenValidationParameter);
        }

        private static void FluentValidationConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IValidatorProvider, ValidatorProvider>();
            services.AddValidatorsFromAssemblyContaining<RefreshTokensValidator>();
        }

        private static void ConfigurationDependency(this IServiceCollection services)
        {
            services.AddHttpClient();
        }

        private static void MediatRDependency(this IServiceCollection services)
        {
            services.AddMediatR(m => m.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        }

        public static void RegisterServicesAutomatically(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Register non-generic types
            var nonGenericTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract
                && (t.GetCustomAttribute<InjectAsScopedAttribute>() != null ||
                t.GetCustomAttribute<InjectAsTransientAttribute>() != null ||
                t.GetCustomAttribute<InjectAsSingletonAttribute>() != null)
                && t.GetInterfaces().Any(i => !i.IsGenericType))
                .Select(t => new
                {
                    ServiceType = t.GetInterfaces().FirstOrDefault(),
                    ImplementationType = t,
                    Scoped = t.GetCustomAttribute<InjectAsScopedAttribute>() != null,
                    Transient = t.GetCustomAttribute<InjectAsTransientAttribute>() != null,
                    Singleton = t.GetCustomAttribute<InjectAsSingletonAttribute>() != null
                }).Where(x => x.ServiceType != null);

            foreach (var type in nonGenericTypes)
            {
                if (type.Scoped)
                {
                    services.AddScoped(type.ServiceType, type.ImplementationType);
                }
                else if (type.Transient)
                {
                    services.AddTransient(type.ServiceType, type.ImplementationType);
                }
                else if (type.Singleton)
                {
                    services.AddSingleton(type.ServiceType, type.ImplementationType);
                }
            }
        }
    }
}