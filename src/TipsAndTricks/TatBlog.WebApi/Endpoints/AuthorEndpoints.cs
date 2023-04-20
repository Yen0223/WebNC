﻿using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Filters;
using System.Net;

namespace TatBlog.WebApi.Endpoints;

public static class AuthorEndpoints
{
    public static WebApplication MapAuthorEndpoints(
        this WebApplication app)
    {
        var routeGroupBuilder = app.MapGroup("/api/authors");

        routeGroupBuilder.MapGet("/", GetAuthors)
            .WithName("GetAuthors")
            .Produces<ApiResponse<PaginationResult<AuthorItem>>>();

        routeGroupBuilder.MapGet("/{id:int}", GetAuthorDetails)
            .WithName("GetAuthorsById")
            .Produces<ApiResponse<AuthorItem>>()
            .Produces(404);

        routeGroupBuilder.MapGet(
            "/{slug:regex(^[a-z0-9_-]+$)}/posts",
            GetPostsByAuthorsSlug)
            .WithName("GetPostsByAuthorsSlug")
            .Produces<ApiResponse<PaginationResult<PostDto>>>();

        routeGroupBuilder.MapPost("/", AddAuthor)
            .WithName("AddNewAuthor")
            .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
            .Produces(401)
            .Produces<ApiResponse<AuthorItem>>();

        routeGroupBuilder.MapPost("/{id:int}/picture", SetAuthorPicture)
            .WithName("SetAuthorPicture")
            .Accepts<IFormFile>("multipart/form-data")
            .Produces(401)
            .Produces<ApiResponse<string>>();

        routeGroupBuilder.MapPut("/{id:int}", UpdateAuthor)
            .WithName("UpdateAnAuthor")
            .Produces(401)
            .Produces<ApiResponse<string>>();

        routeGroupBuilder.MapDelete("/{id:int}", DeleteAuthor)
            .WithName("DeleteAnAuthor")
            .Produces(401)
            .Produces<ApiResponse<string>>();


        return app;
    }


    private static async Task<IResult> GetAuthors(
        [AsParameters] AuthorFilterModel model,
        IAuthorRepository authorRepository)
    {
        var authorsList = await authorRepository
            .GetPagedAuthorsAsync(model, model.Name);

        var paginationResult =
            new PaginationResult<AuthorItem>(authorsList);

        return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> GetAuthorDetails(
        int id,
        IAuthorRepository authorRepository,
        IMapper mapper)
    {
        var author = await authorRepository.GetCachedAuthorByIdAsync(id);
        return author == null
            ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,$"Không tìm thấy tác giả có mã số {id}"))
            : Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author)));
    }

    private static async Task<IResult> GetPostsByAuthorId(
        int id,
        [AsParameters] PagingModel pagingModel,
        IBlogRepository blogRepository)
    {
        var postQuery = new PostQuery()
        {
            AuthorId = id,
            PublishedOnly = true,
        };

        var postsList = await blogRepository.GetPagePostsAsync(
            postQuery, pagingModel,
            posts => posts.ProjectToType<PostDto>());
        var paginationResult = new PaginationResult<PostDto>(postsList);

        return Results.Ok(ApiResponse.Success(paginationResult));
    }

    public static async Task<IResult> GetPostsByAuthorsSlug(
        [FromRoute] string slug,
        [AsParameters] PagingModel pagingModel,
        IBlogRepository blogRepository)
    {
        var postQuery = new PostQuery()
        {
            AuthorSlug = slug,
            PublishedOnly = true,
        };

        var postsList = await blogRepository.GetPagePostsAsync(
            postQuery, pagingModel,
            posts => posts.ProjectToType<PostDto>());
        var paginationResult = new PaginationResult<PostDto>(postsList);

        return Results.Ok(paginationResult);
    }

    private static async Task<IResult> AddAuthor(
        AuthorEditModel model,
        IAuthorRepository authorRepository,
        IMapper mapper)
    {

        if (await authorRepository
                .IsAuthorSlugExistedAsync(0, model.UrlSlug)) 
        {
            return Results.Ok(ApiResponse.Fail(
               HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
        }

        var author = mapper.Map<Author>(model);
        await authorRepository.AddOrUpdateAsync(author);

        return Results.Ok(ApiResponse.Success(
            mapper.Map<AuthorItem>(author), HttpStatusCode.Created));
    }

    private static async Task<IResult> SetAuthorPicture(
        int id, 
        IFormFile imageFile,
        IAuthorRepository authorRepository,
        IMediaManager mediaManager)
    {
        var imageUrl = await mediaManager.SaveFileAsync(
            imageFile.OpenReadStream(),
            imageFile.FileName, 
            imageFile.ContentType);

        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return Results.Ok(ApiResponse.Fail(
               HttpStatusCode.BadRequest, "Không lưu được tập tin"));
        }

        await authorRepository.SetImageUrlAsync(id, imageUrl);
        return Results.Ok(ApiResponse.Success(imageUrl));
    }

    private static async Task<IResult> UpdateAuthor(
        int id,
        AuthorEditModel model,
        IValidator<AuthorEditModel> validator,
        IAuthorRepository authorRepository,
        IMapper mapper)
    {
        var validationResult = await validator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            return Results.Ok(ApiResponse.Fail(
                HttpStatusCode.BadRequest, validationResult));
        }

        if(await authorRepository
                .IsAuthorSlugExistedAsync(id, model.UrlSlug))
        {
            return Results.Ok(ApiResponse.Fail(
                HttpStatusCode.Conflict, $"Slug '{model.UrlSlug}' đã được sử dụng"));
        }    

        var author = mapper.Map<Author>(model);
        author.Id = id;

        return await authorRepository.AddOrUpdateAsync(author)
            ? Results.Ok(ApiResponse.Success("Tác giả đã được cập nhật", HttpStatusCode.NoContent))
            : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không thể tìm thấy tác giả"));
    }

    private static async Task<IResult> DeleteAuthor(
        int id,
        IAuthorRepository authorRepository)
    {
        return await authorRepository.DeleteAuthorAsync(id)
            ? Results.Ok(ApiResponse.Success("Tác giả đã bị xóa", HttpStatusCode.NoContent))
            : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không thể tìm thấy tác giả"));
    }
}
