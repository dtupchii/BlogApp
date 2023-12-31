﻿using Blog.Web.Models.Domain;

namespace Blog.Web.Repositories
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync (BlogPostComment comment);
        Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync (Guid blogPostId);
    }
}
