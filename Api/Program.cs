using Api;
using Api.Configs;
using Api.Middleware;
using Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var authSection = builder.Configuration.GetSection(AuthConfig.Position); //взятие токена из конфига по имени Postion=Auth
        var authConfig = authSection.Get<AuthConfig>(); //добавляение класса в виде конфига

        builder.Services.Configure<AuthConfig>(authSection); //добавление в контейнер, параметры берутся из объекта authConfig

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();


        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme //настройка свагера для аутентификации
            {
                Description = "Введите токен пользователя",
                Name = "Authorization",
                In = ParameterLocation.Header, //токен длжен быть в загаловке
                Type = SecuritySchemeType.ApiKey, //тип ApiKey 
                Scheme = JwtBearerDefaults.AuthenticationScheme, //указание схемы

            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement() //требование безопасности
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme,

                        },
                        Scheme = "oauth2",
                        Name = JwtBearerDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header,
                    },
                    new List<string>() //значения лист строк
                }
            });
        });


        builder.Services.AddDbContext<DAL.DataContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"), sql => { }); //указали имя строки подключения из appsettings.json
        }, contextLifetime: ServiceLifetime.Scoped);

        builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<AuthService>();

        builder.Services.AddAuthentication(o =>
        {
            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; //схема аутентификации по умолчанию JwtBearer
        }).AddJwtBearer(o => //настройка схемы
        {
            o.RequireHttpsMetadata = false; // отключение проверки сертификата ssl
            o.TokenValidationParameters = new TokenValidationParameters //параметры валидации токена
            {
                ValidateIssuer = true, //вкл проверку издателя токена
                ValidIssuer = authConfig.Issuer, //правильное название из конфига
                ValidateAudience = true, //проверка аудиенции
                ValidAudience = authConfig.Audience, //название из конфига
                ValidateLifetime = true, //время жизни
                ValidateIssuerSigningKey = true, //проверка подписи
                IssuerSigningKey = authConfig.SymmetricSecurityKey(), //генерируется методом из указанного в конфиге ключа (текста)
                ClockSkew = TimeSpan.Zero, //погрешность времени жизни токена обнуляется
            };
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ValidAccessToken", p =>  //политика проверки токена
            {
                p.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme); //добавляем настроенную аутент схему
                p.RequireAuthenticatedUser(); //требуем аутентифицированного юзера для входа в приложение
            });
        });

        var app = builder.Build();

        //выполнение автоматической миграции при запуске, полезно при обновлении разных БД
        using (var serviceScope = ((IApplicationBuilder)app).ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
        {
            if (serviceScope != null)
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DAL.DataContext>();
                context.Database.Migrate();
            }
        }

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment()) - подключение к свагеру не только во время разработки, но и, например, при использовании сервера
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseTokenValidator();
        app.MapControllers();

        app.Run();
    }
}