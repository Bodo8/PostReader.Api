using PostReader.Api.Application.PostWebsites.Queries;
using PostReader.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PostReader.Api.Common.CommonModels;

namespace PostReader.Api.Controllers
{
    public class WebsitesReaderController : ApiControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet()]
        public async Task<IActionResult> Search([FromQuery] GetPostsQuery query)
        {
            if (!ModelState.IsValid)
                return View("Index");

            var posts = await Mediator.Send(query);

            return View(posts);
        }

    }
}
