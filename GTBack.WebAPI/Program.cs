using FluentValidation;
using GTBack.Core.Repositories;
using GTBack.Core.Services;
using GTBack.Core.UnitOfWorks;
using GTBack.Repository;
using GTBack.Repository.Repositories;
using GTBack.Repository.UnitOfWorks;
using GTBack.Service.Configurations;
using GTBack.Service.Mapping;
using GTBack.Service.Services;
using GTBack.Service.Utilities.Jwt;
using GTBack.WebAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using GTBack.Core.DTO;
using GTBack.Core.Services.coach;
using GTBack.Core.Services.Coach;
using GTBack.Core.Services.Ecommerce;
using GTBack.Core.Services.Shopping;
using GTBack.Service.Mapping.Resourant;
using GTBack.Service.Services.Coach;
using GTBack.Service.Services.Ecommerce;
using GTBack.Service.Services.SharedServices;
using GTBack.Service.Services.ShoppingService;
using GTBack.WebAPI;
using Hangfire;
using Hangfire.PostgreSql;
using Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.




builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});



builder.Services.AddSignalR();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GoThere API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
            },
            new List<string>()
        }
    });
});
// Console.WriteLine(builder.Configuration.GetSection("ConnectionStrings:defaultConnection").Value);
builder.Services.AddHangfire((config) =>
{
    config.UsePostgreSqlStorage(builder.Configuration.GetSection("ConnectionStrings:defaultConnection").Value);
});



builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<RefreshTokenRepository>();
builder.Services.AddTransient<IValidatorFactory, ServiceProviderValidatorFactory>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddScoped(typeof(IJwtTokenService<BaseRegisterDTO>), typeof(JwtTokenService<BaseRegisterDTO>));
builder.Services.AddScoped(typeof(IRefreshTokenService), typeof(RefreshTokenService));
builder.Services.AddAppConfiguration(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient(typeof(IListingServiceI<,>), typeof(ListService<,>));
builder.Services.AddScoped(typeof(IShoppingCompany), typeof(ShoppingCompanyService));
builder.Services.AddScoped(typeof(ILessonService), typeof(LessonService));
builder.Services.AddScoped(typeof(IQuestionImageService), typeof(QuestionImageService));
builder.Services.AddScoped(typeof(IClassroomService), typeof(ClasroomService));
builder.Services.AddScoped(typeof(ISubjectService), typeof(SubjectService));
builder.Services.AddScoped(typeof(ICoachAuthService), typeof(CoachService));
builder.Services.AddScoped(typeof(IStudentAuthService), typeof(StudentAuthService));
builder.Services.AddScoped(typeof(IShoppingUserService), typeof(ShoppingUserService));
builder.Services.AddScoped(typeof(IAuthService), typeof(ClientAuthService));
builder.Services.AddScoped(typeof(IEmployeeAuthService), typeof(ClientEmployeeService));
builder.Services.AddScoped(typeof(IEcommerceProductService), typeof(EcommerceProductService));
builder.Services.AddScoped(typeof(IEcommerceCompanyService), typeof(EcommerceCompanyService));
builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
builder.Services.AddScoped(typeof(IShoppingOrderService), typeof(ShoppinOrderService));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddAutoMapper(typeof(RestourantMapProfile));
builder.Services.AddAutoMapper(typeof(ShoppingMapProfile));
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.LoadValidators();
builder.Services.AddMemoryCache();


if (FirebaseApp.DefaultInstance == null)
{
   
}

var appConfig = builder.Configuration.Get<GoThereAppConfig>();



builder.Services.AddDbContext<AppDbContext>(x =>
{
   x.UseNpgsql(builder.Configuration.GetSection("ConnectionStrings:defaultConnection").Value);
});


var app = builder.Build();

// Configure the HTTP request pipeline.

FirebaseApp.Create(new AppOptions()
{
    Credential =
        GoogleCredential.FromFile(
            "private_key.json")
});

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GoThere API v1");
    c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
});
app.UseHangfireServer();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
});
app.UseAuthentication();


app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<PayHub>("/pay-hub");
    endpoints.MapControllers();
});


// app.MapControllers();
//
// app.MapHub<PayHub>("/pay-hub").RequireCors("AllowAll");;


app.Run();