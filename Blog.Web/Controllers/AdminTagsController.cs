using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository _tagRepository;
        public AdminTagsController(ITagRepository tagRepository)
        {
            //dependency injection
            _tagRepository = tagRepository;
        }

        
        [HttpGet]
        public IActionResult Add()
        {
            //just returning view
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            //creating a new Tag object with values of VM object's properties (mapping)
            Tag tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await _tagRepository.AddAsync(tag);

            //going back to List View
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            //reading all Tags from DB
            var tagsFromDb = await _tagRepository.GetAllAsync();

            return View(tagsFromDb);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //finding an object in DB by Id
            var tagFromDb = await _tagRepository.GetAsync(id);

            if (tagFromDb != null)
            {
                //mapping a VM object to show values on Edit View
                var editTagRequest = new EditTagRequest
                {
                    Id = tagFromDb.Id,
                    Name = tagFromDb.Name,
                    DisplayName = tagFromDb.DisplayName
                };
                return View(editTagRequest);
            }
            else
                return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            //"converting" VM object to Tag object
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            var updatedTag = await _tagRepository.UpdateAsync(tag);

            if (updatedTag != null)
            {
                //show success message
            }
            else
            {
                //show error message
            }

            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedTag = await _tagRepository.DeleteAsync(editTagRequest.Id);

            if (deletedTag != null)
            {
                //show success message
                return RedirectToAction("List");
            }
            else
            {
                //show an error notification
            }
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
    }
}
