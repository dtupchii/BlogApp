using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext _db;

        public BlogPostRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await _db.AddAsync(blogPost);
            await _db.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingPost = await _db.BlogPosts.FindAsync(id);

            if (existingPost != null)
            {
                _db.BlogPosts.Remove(existingPost);
                await _db.SaveChangesAsync();
                return existingPost;
            }

            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _db.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await _db.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await _db.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingPost = await _db.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingPost != null)
            {
                existingPost.Id = blogPost.Id;
                existingPost.Heading = blogPost.Heading;
                existingPost.Author = blogPost.Author;
                existingPost.ShortDescription = blogPost.ShortDescription;
                existingPost.PageTitle = blogPost.PageTitle;
                existingPost.Content =  blogPost.Content;
                existingPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingPost.UrlHandle = blogPost.UrlHandle;
                existingPost.Visible = blogPost.Visible;
                existingPost.PublishedDate = blogPost.PublishedDate;
                existingPost.Tags = blogPost.Tags;

                await _db.SaveChangesAsync();

                return existingPost;
            }
            return null;
        }
    }
}
