using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using WebApp.Extensions;
using WebApp.Mapsters;

var builder = WebApplication.CreateBuilder(args);
{
	builder
		.ConfigureMvc()
		.ConfigureServices()
		.ConfigureMapster();
}

//Thêm dữ liêu mẫu vào CSDL


var app = builder.Build();
{
	app.UseRequestPipeline();
	app.UseBlogRoutes();
	app.UseDataSeeder();
	
}


app.Run();
