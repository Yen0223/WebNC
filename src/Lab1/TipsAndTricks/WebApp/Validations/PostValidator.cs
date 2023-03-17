using FluentValidation;
using TatBlog.Services.Blogs;
using WebApp.Areas.Admin.Models;

namespace WebApp.Validations;

public class PostValidator : AbstractValidator<PostEditModel>
{
	private readonly IBlogRepository _blogRepository;

	public PostValidator(IBlogRepository blogRepository)
	{
		_blogRepository = blogRepository;

		RuleFor(x => x.Title)
			.NotEmpty()
			.MaximumLength(500);

		RuleFor(x => x.ShortDescription)
			.NotEmpty();

		RuleFor(x => x.Description)
			.NotEmpty();

		RuleFor(x => x.Meta)
			.NotEmpty()
			.MaximumLength(1000);

		RuleFor(x => x.UrlSlug)
			.NotEmpty()
			.MaximumLength(1000);

		RuleFor(x => x.UrlSlug)
			.MustAsync(async (postModel, slug, cancellationToken) =>
			!await blogRepository.IsPostSlugExistedAsync(
				postModel.Id, slug, cancellationToken))
			.WithMessage("Slug '{PropertyValue}' đã được sử dụng");

		RuleFor(x => x.CategoryId)
			.NotEmpty()
			.WithMessage("Bạn phải chọn chủ đề cho bài viết");

		RuleFor(x => x.AuthorId)
			.NotEmpty()
			.WithMessage("Bạn phải chọn tác giả của bài viết");

		RuleFor(x => x.SelectedTags)
			.Must(HasAtLeastOneTag)
			.WithMessage("Bạn phải chọn ít nhất một thẻ");

		When(x => x.Id <= 0, () =>
		{
			RuleFor(x => x.ImageFile)
			.Must(x => x is { Length: > 0 })
			.WithMessage("Bạn phải chọn hình ảnh cho bài viết");
		})
		.Otherwise(() =>
		{
			RuleFor(x => x.ImageFile)
			.MustAsync(SetImageIfNotExist)
			.WithMessage("Bạn phải chọn hình ảnh cho bài viết");
		});

	}


	//Kiểm tra post có h/a hay chưa 
	//nếu chưa, bắt buộc chọn
	private async Task<bool> SetImageIfNotExist(
		PostEditModel postModel,
		IFormFile imageFile,
		CancellationToken cancellationToken)
	{ 
		//Lấy tt post từ CSDL
		var post = await _blogRepository.GetPostByIdAsync(
			postModel.Id, false, cancellationToken);

		//Nếu bài viết đã có ảnh => k bắt buộc chọn file
		if (!string.IsNullOrWhiteSpace(post?.ImageUrl))
			return true;

		//ngược lại, kiểm tra ng dùng đã chọn chưa,nếu chưa => báo lõi
		return imageFile is { Length: > 0 };
	}

	//Kiểm tra người dùng nhập ít nhất 1 thẻ (tag)
	private bool HasAtLeastOneTag(
		PostEditModel postModel,
		string selectedTags)
	{
		return postModel.GetSelectedTags().Any();
	}
}
