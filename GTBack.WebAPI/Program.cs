using FluentValidation;
using GTBack.Core.Entities;
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Services.Restourant;
using GTBack.Service.Mapping.Resourant;
using GTBack.Service.Services.RestourantServices;
using GTBack.Service.Services.SharedServices;
using GTBack.WebAPI;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.Data.SqlClient;
using IClientService = Google.Apis.Services.IClientService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


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
SqlConnectionStringBuilder mySql = new SqlConnectionStringBuilder();
mySql.DataSource = "database-2.cfcokfalhlyk.eu-central-1.rds.amazonaws.com"; 
mySql.UserID = "admin";            
mySql.Password = "Bthntncr81.";     
mySql.InitialCatalog = "database-2";

builder.Services.AddHangfire((sp, config) =>
{
    config.UseSqlServerStorage(mySql.ConnectionString).SetDataCompatibilityLevel(CompatibilityLevel.Version_180).UseSimpleAssemblyNameTypeSerializer().UseRecommendedSerializerSettings();
});
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("/Users/omerbatuhantuncer/Documents/GitHub/GT-webAPI/GTBack.WebAPI/private_key.json")
});
builder.Services.AddHangfireServer();
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddScoped<RefreshTokenRepository>();
builder.Services.AddTransient<IValidatorFactory, ServiceProviderValidatorFactory>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddScoped(typeof(IJwtTokenService<BaseRegisterDTO>), typeof(JwtTokenService<BaseRegisterDTO>));
builder.Services.AddScoped(typeof(IRefreshTokenService), typeof(RefreshTokenService));
builder.Services.AddAppConfiguration(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient(typeof(IListingServiceI<,>),typeof(ListService<,>));
builder.Services.AddScoped(typeof(IUserService), typeof(UserService));
builder.Services.AddScoped(typeof(GTBack.Core.Services.Restourant.IClientService), typeof(ClientService));
builder.Services.AddScoped(typeof(IEventService), typeof(EventService));
builder.Services.AddScoped(typeof(IMenuAndCategoryService), typeof(MenuAndCategoryService));
builder.Services.AddScoped(typeof(IAdditionAndOrderService), typeof(AdditionAndOrderService));
builder.Services.AddScoped(typeof(ITableAndAreaService), typeof(TableAndAreaService));
builder.Services.AddScoped(typeof(IRoleService<RoleCreateDTO,RoleListDTO>), typeof(RoleService));
builder.Services.AddScoped(typeof(IRestoCompanyService<CompanyAddDTO,CompanyListDTO>), typeof(CompanyService));
builder.Services.AddScoped(typeof(IDepartmentService<DepartmentAddDTO,DepartmentListDTO>), typeof(DepartmentService));
builder.Services.AddScoped(typeof(IEmployeeService), typeof(EmployeeService));
builder.Services.AddScoped(typeof(IService<>),typeof(Service<>));
builder.Services.AddScoped(typeof(IEventTypeService),typeof(EventTypeService));
builder.Services.AddAutoMapper(typeof(RestourantMapProfile));
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.LoadValidators();
builder.Services.AddMemoryCache();

var appConfig = builder.Configuration.Get<GoThereAppConfig>();





builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(mySql.ConnectionString, option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });


});


var app = builder.Build();

// Configure the HTTP request pipeline.



app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GoThere API v1");
    c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
});
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter
        {
            User = app.Configuration.GetSection("HangfireOptions:User").Value,
            Pass = app.Configuration.GetSection("HangfireOptions:Pass").Value
        }
    }
});
app.UseAuthentication();


app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
