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

//����Ĭ��·�ɹ���,���߳�����ν��������URL��ƥ�䵽��Ӧ�Ŀ������ͷ���
//·�ɵ�URLģʽΪ:
//                  ������Ĭ��ΪHome,���URL��û��ָ����������,��ʹ��Home������
//                  ָ����Action Ĭ��Ϊ ShowLogin ���URL��û��ָ��Action��ô��ʹ��ShowLogin����
//                  ID? ��ʾId�ǿ�ѡ��

//MVC������Ѱ�� Home �������е� ShowLogin ����,���ҽ�id��Ϊ��������

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=ShowLogin}/{id?}");
app.UseSession();
app.Run();
