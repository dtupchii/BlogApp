using Blog.Web.Models.Domain;

namespace Blog.Web.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<BlogPost> Posts { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}
