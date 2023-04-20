using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints;

public static class CategoryEndpoints
{
    public static WebApplication MapCategoryEndpoints(
        this WebApplication app)
    {
        var routeGroupBuilder = app.MapGroup("/api/categories");

        routeGroupBuilder.MapGet("/", GetCategory)
            .WithName("GetCategory")
            .Produces<ApiResponse<PaginationResult<CategoryItem>>>();

        routeGroupBuilder.MapGet("/{id:int}", GetCategoryDetails)
            .WithName("GetCategoryById")
            .Produces<ApiResponse<CategoryItem>>()
            .Produces(404);

        routeGroupBuilder.MapGet(
            "/{slug:regex(^[a-z0-9-]+$)}/posts",
            GetPostsByCategoriesSlug)
            .WithName("GetPostsByCategoriesSlug")
            .Produces<ApiResponse<PaginationResult<PostDto>>>();

        routeGroupBuilder.MapPost("/", AddCategory)
            .WithName("AddNewCategory")
            .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
            .Produces(401)
            .Produces<ApiResponse<CategoryItem>>();

        routeGroupBuilder.MapPut("/{id:int}", UpdateCategory)
            .WithName("UpdateCategory")
            .Produces(401)
            .Produces<ApiResponse<string>>();


        return app;
    }


    

    private static async Task<IResult> GetCategory(
        [AsParameters] CategoryFilterModel model,
        ICategoryRepository categoryRepository)
    {
        var categoriesList = await categoryRepository
            .GetPagedCategoriesAsync(model, model.Name);

        var paginationResult = new PaginationResult<CategoryItem>(categoriesList);

        return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> GetCategoryDetails(
        int id,
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {
        var category = await categoryRepository.GetCachedCategoryByIdAsync(id);
        return category == null
            ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy chủ đề có mã số {id}"))
            : Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(category)));
    }

    private static async Task<IResult> GetPostsByCategoryId(
        int id,
        [AsParameters] PagingModel pagingModel,
        IBlogRepository blogRepository)
    {
        var postQuery = new PostQuery()
        {
            CategoryId = id,
            PublishedOnly = true,
        };

        var postsList = await blogRepository.GetPagePostsAsync(
            postQuery, pagingModel,
            posts => posts.ProjectToType<PostDto>());

        var paginationResult = new PaginationResult<PostDto>(postsList);

        return Results.Ok(ApiResponse.Success(paginationResult));
    }

    public static async Task<IResult> GetPostsByCategoriesSlug(
        [FromRoute] string slug,
        [AsParameters] PagingModel pagingModel,
        IBlogRepository blogRepository)
    {
        var postQuery = new PostQuery()
        {
            CategorySlug = slug,
            PublishedOnly = true,
        };

        var postsList = await blogRepository.GetPagePostsAsync(
            postQuery, pagingModel,
            posts => posts.ProjectToType<PostDto>());
        var paginationResult = new PaginationResult<PostDto>(postsList);

        return Results.Ok(paginationResult);
    }

    private static async Task<IResult> AddCategory(
       CategoryEditModel model,
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {

        if (await categoryRepository
                .IsCategorySlugExistedAsync(0, model.UrlSlug))
        {
            return Results.Ok(ApiResponse.Fail(
               HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
        }

        var category = mapper.Map<Category>(model);
        await categoryRepository.AddOrUpdateAsync(category);

        return Results.Ok(ApiResponse.Success(
            mapper.Map<CategoryItem>(category), HttpStatusCode.Created));
    }

    private static async Task<IResult> UpdateCategory(
        int id,
        CategoryEditModel model,
        IValidator<CategoryEditModel> validator,
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {
        var validationResult = await validator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            return Results.Ok(ApiResponse.Fail(
                HttpStatusCode.BadRequest, validationResult));
        }

        if (await categoryRepository
                .IsCategorySlugExistedAsync(id, model.UrlSlug))
        {
            return Results.Ok(ApiResponse.Fail(
                HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
        }

        var category = mapper.Map<Category>(model);
        category.Id = id;

        return await categoryRepository.AddOrUpdateAsync(category)
            ? Results.Ok(ApiResponse.Success("Chủ đề đã được cập nhật", HttpStatusCode.NoContent))
            : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không thể tìm thấy chủ đề"));
    }

    private static async Task<IResult> DeleteCategory(
        int id,
        ICategoryRepository categoryRepository)
    {
        return await categoryRepository.DeleteCategoryAsync(id)
            ? Results.Ok(ApiResponse.Success("Chủ đề đã bị xóa", HttpStatusCode.NoContent))
            : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không thể tìm thấy chủ đề"));
    }
}
