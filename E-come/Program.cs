using E_come.Model;
using E_come.services;
using E_come.services.Business;
using E_come.services.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using E_come.DTO.Options;
using E_come.DTO.JWTDTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DBContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")) 
    );
builder.Services.AddIdentity<ApplicationUser,IdentityRole>(
     options => {
         options.Tokens.ProviderMap.Add("Default", new
               TokenProviderDescriptor(typeof(IUserTwoFactorTokenProvider<ApplicationUser>)));
     }

    ).AddEntityFrameworkStores<DBContext>().AddDefaultTokenProviders();
//----------------

builder.Services.AddHttpContextAccessor();
builder.Services.AddCors();

//Custom Service --Register 
builder.Services.AddScoped<IProductRepository, ProductServisec>();
builder.Services.AddScoped<ICategoryRepository, CategoryServices>();
builder.Services.AddScoped<IUserRepository, UserServices>();
builder.Services.AddScoped<IAddressRepository, AddressServisec>();
builder.Services.AddScoped<IReviewServices, ReviewServices>();
builder.Services.AddScoped<IShopProductsRepository,ShopProductsServisec>();
builder.Services.AddScoped<ICartReposatory,CartServices>();
builder.Services.AddScoped<IOrderRepository,OrderServices>();
builder.Services.AddScoped<IEmailSender,EmailSender>();
builder.Services.AddScoped<IAuthRepository,AuthServices>();
builder.Services.AddScoped<IApplyShopRepository,ApplyShopServices>();

//[Authoriz] Used JWT Token in Check Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}
).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:key"]))

    };
});

// Enable Token Bearer functionality in Swagger
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo {  Version = "v1", Title = "ASP .NET WEb 7 API", });
    
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then valid in the text input "
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()){}

    app.UseSwagger();
    app.UseSwaggerUI();





app.UseStaticFiles();
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
