var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//配置默认路由规则,告诉程序如何解析传入的URL并匹配到对应的控制器和方法
//路由的URL模式为:
//                  控制器默认为Home,如果URL中没有指定控制器名,就使用Home控制器
//                  指定了Action 默认为 ShowLogin 如果URL中没有指定Action那么就使用ShowLogin方法
//                  ID? 表示Id是可选的

//MVC将尝试寻找 Home 控制器中的 ShowLogin 方法,并且将id作为参数传入

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=ShowLogin}/{id?}");
app.UseSession();
app.Run();
