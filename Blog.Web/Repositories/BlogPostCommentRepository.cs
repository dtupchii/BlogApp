using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly ApplicationDbContext _db;

        public BlogPostCommentRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<BlogPostComment> AddAsync(BlogPostComment comment)
        {
            await _db.BlogPostComments.AddAsync(comment);
            await _db.SaveChangesAsync();
            return comment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId)
        {
            return await _db.BlogPostComments.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }
    }
}
