﻿namespace Blog.Web.Models.Domain
{
    public class BlogPostLike
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BlogPostId { get; set; } 
    }
}
