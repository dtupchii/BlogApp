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
                UrlHandle = addBlogPostRequest.UrlHandle,
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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //getting the object from db
            var existingPost = await _blogPostRepository.GetAsync(id);
            var tags = await _tagRepository.GetAllAsync();

            if (existingPost != null)
            {
                //mapping the domain model to vm
                var vm = new EditBlogPostRequest
                {
                    Id = existingPost.Id,
                    Heading = existingPost.Heading,
                    PageTitle = existingPost.PageTitle,
                    Content = existingPost.Content,
                    Author = existingPost.Author,
                    ShortDescription = existingPost.ShortDescription,
                    FeaturedImageUrl = existingPost.FeaturedImageUrl,
                    UrlHandle = existingPost.UrlHandle,
                    PublishedDate = existingPost.PublishedDate,
                    Visible = existingPost.Visible,
                    Tags = tags.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = existingPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(vm);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            //mapping vm to domain model
            var blogPost = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                ShortDescription = editBlogPostRequest.ShortDescription,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Author = editBlogPostRequest.Author,
                Content = editBlogPostRequest.Content,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Visible = editBlogPostRequest.Visible
            };

            //Mapping tags into domain model
            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await _tagRepository.GetAsync(tag);
                    
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }    
            }

            blogPost.Tags = selectedTags;

            //updating info in db
            var updatedPost = await _blogPostRepository.UpdateAsync(blogPost);
            if (updatedPost != null)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            //deleting post from db through repository
            var deletedPost = await _blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

            if (deletedPost != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new {id =  editBlogPostRequest.Id});
        }
    }
}
