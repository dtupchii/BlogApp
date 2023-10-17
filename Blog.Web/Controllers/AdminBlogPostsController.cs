using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IBlogPostRepository _blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            _tagRepository = tagRepository;
            _blogPostRepository = blogPostRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //get tags from repository
            var tags = await _tagRepository.GetAllAsync();

            //populating tags for View
            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            //map view model to domain model
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHendle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible
            };

            var selectedTags = new List<Tag>();
            //map tags from selected tags
            foreach (var SelectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(SelectedTagId);
                var existingTag = await _tagRepository.GetAsync(selectedTagIdAsGuid);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }
            //map tags back to domain model
            blogPost.Tags = selectedTags;

            await _blogPostRepository.AddAsync(blogPost);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var allPosts = await _blogPostRepository.GetAllAsync();

            return View(allPosts);
        }
    }
}
