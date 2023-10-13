using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            //creating a new tag object with values of VM object's properties (mapping)
            Tag tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            //adding new tag to DB
            _db.Tags.Add(tag);
            _db.SaveChanges();

            //going back to List View
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult List()
        {
            //reading all Tags from DB
            List<Tag> tagsFromDb = _db.Tags.ToList();

            return View(tagsFromDb);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            //finding an object in DB by Id
            var tagFromDb = _db.Tags.FirstOrDefault(a => a.Id == id);

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
        public IActionResult Edit(EditTagRequest editTagRequest)
        {
            //"converting" VM object to Tag object
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            //finding object in db by Id of tag object
            var existingTag = _db.Tags.Find(tag.Id);

            if (existingTag != null)
            {
                //updating values in db
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                //saving changes
                _db.SaveChanges();

                //going back to List View
                return RedirectToAction("List");
            }
            else
                return RedirectToAction("Edit", new {id = editTagRequest.Id});
        }
    }
}
