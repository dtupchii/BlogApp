using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminTagsController(ApplicationDbContext db)
        {
            //dependency injection
            _db = db;
        }

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

            //adding new tag to DB
            await _db.Tags.AddAsync(tag);
            await _db.SaveChangesAsync();

            //going back to List View
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            //reading all Tags from DB
            List<Tag> tagsFromDb = await _db.Tags.ToListAsync();

            return View(tagsFromDb);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //finding an object in DB by Id
            var tagFromDb = await _db.Tags.FirstOrDefaultAsync(a => a.Id == id);

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

            //finding object in db by Id of tag object
            var existingTag = await _db.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                //updating values in db
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                //saving changes
                await _db.SaveChangesAsync();

                //going back to List View
                return RedirectToAction("List");
            }
            else
                return RedirectToAction("Edit", new {id = editTagRequest.Id});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var tag = await _db.Tags.FindAsync(editTagRequest.Id);

            if (tag != null)
            {
                _db.Tags.Remove(tag);
                await _db.SaveChangesAsync();

                //show a success notification
                return RedirectToAction("List");
            }
            else

                //show an error notification
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
    }
}
