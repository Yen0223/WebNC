using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;

namespace WebApp.Extensions
{
	public static class WebApplicationExtensions
	{
		//Thêm các dv được yêu cầu bởi Mvc Framework
		public static WebApplicationBuilder ConfigureMvc(
			this WebApplicationBuilder builder) 
		{
			builder.Services.AddControllersWithViews();
			builder.Services.AddResponseCompression();

			return builder;
		}

		//Đăng ký dv bởi DI Container
		public static WebApplicationBuilder ConfigureServices(
			this WebApplicationBuilder builder) 
		{
			builder.Services.AddDbContext<BlogDbContext>(options =>
		options.UseSqlServer(
			builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddScoped<IBlogRepository, BlogRepository>();
			builder.Services.AddScoped<IDataSeeder, DataSeeder>();

			return builder;
		}

		//Cấu hình HTTP Request pipiline
		public static WebApplication UseRequestPipeline(
			this WebApplication app) 
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Blog/Error");

				/*Thêm middleware cho việc áp dụng HSTS (thêm header
				 Strict-Transport-Security vào HTTP Response)*/
				app.UseHsts();
			}

			//Thêm middleware để chuyển hướng HTTP sang HTTPS
			app.UseHttpsRedirection();

			/*Thêm middleware phục vụ các yêu cầu liên quan tới 
			 các tập tin nội dung tĩnh như image, css, ...*/
			app.UseStaticFiles();

			/* Thêm middleware luwak chọn endpoint phù hợp nhất để 
			 xử lý một HTTP request*/
			app.UseRouting();

			return app;
		}

		//Thêm dl mẫu vào CSDL
		public static IApplicationBuilder UseDataSeeder (
			this IApplicationBuilder app)
		{
			using var scope = app.ApplicationServices.CreateScope();

			try
			{
				scope.ServiceProvider
					.GetRequiredService<IDataSeeder>()
					.Initialize();
			}
			catch (Exception ex)
			{
				scope.ServiceProvider
					.GetRequiredService<ILogger<Program>>()
					.LogError(ex, "Could not insert data into database");
			}

			return app;
		}
	}
}
