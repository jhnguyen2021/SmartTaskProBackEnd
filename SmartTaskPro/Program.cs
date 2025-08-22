using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartTaskPro.Data;
using SmartTaskPro.Mappings;
using SmartTaskPro.Services;
using SmartTaskPro.Services.Interfaces;
using System.Text;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Identity (GUID keys)
//builder.Services.AddIdentity<User, IdentityRole<Guid>>(opt =>
//{
//    opt.Password.RequiredLength = 6;
//    opt.Password.RequireNonAlphanumeric = false;
//})
//.AddEntityFrameworkStores<AppDbContext>()
//.AddDefaultTokenProviders();



builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwt = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt.Issuer,
        ValidAudience = jwt.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key))
    };
});

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AppProfile>());
//builder.Services.AddAutoMapper(typeof(Program).Assembly);
//builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<SmartTaskPro.Validation.ProjectCreateValidator>();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();


//builder.Services.AddControllers();
builder.Services
    .AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITaskService, TaskService>();
//builder.Services.AddScoped<ITaskRepository, TaskRepository>();
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();

//builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<JwtHelper>();
//builder.Services.AddScoped<PasswordHasher>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(kvp => kvp.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

        var problem = new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed."
        };
        return new BadRequestObjectResult(problem);
    };
});

builder.Services.AddRouting(o => o.LowercaseUrls = true);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


//using (var scope = app.Services.CreateScope())
//{
//    await IdentitySeeder.SeedAsync(scope.ServiceProvider);
//}


app.Run();
