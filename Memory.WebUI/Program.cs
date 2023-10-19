using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Memory.Business.AutoMapper;
using Memory.Business.DependencyResolvers;
using Memory.DataAccess.Concrete.EntityFramework.Context;
using Memory.Entities.Concrete;
using Memory.WebUI.BasketTransaction;
using Memory.WebUI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//AutoFac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new BusinessModule()));
// Mapper
//MapperConfiguration mapperConfiguration = new MapperConfiguration(mc => mc.AddProfile(new MapperProfile()));
//IMapper mapper = mapperConfiguration.CreateMapper();
//builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(typeof(MapperProfile));


// Context
builder.Services.AddDbContext<MemoryContext>();
// Identity
builder.Services.AddIdentity<AppIdentityUser, AppIdentityRole>().AddEntityFrameworkStores<MemoryContext>();


builder.Services.AddTransient<IBasketTransaction, BasketTransaction>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Kimlik doðrulama
app.UseAuthorization(); // Yetki doðrulama

//app.UseMiddleware<LoggingMiddleware>();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=city}/{action=Index}/{id?}");

app.Run();
