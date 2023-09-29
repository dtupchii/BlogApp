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
            _db = db;
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            Tag tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };
            _db.Tags.Add(tag);
            _db.SaveChanges();

            return View("Add");
        }

        [HttpGet]
        public IActionResult List()
        {
            //_db.
            return View();
        }
    }
}
