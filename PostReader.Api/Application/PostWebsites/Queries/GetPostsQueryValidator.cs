using FluentValidation;

namespace PostReader.Api.Application.PostWebsites.Queries
{
    public class GetPostsQueryValidator : AbstractValidator<GetPostsQuery>
    {
        public GetPostsQueryValidator()
        {
            RuleFor(v => v.Sentence)
                .MinimumLength(3).WithMessage("The minimum sentence length is 3 characters")
                .MaximumLength(250).WithMessage("The maximum sentence length is 250 characters");
        }
    }
}
