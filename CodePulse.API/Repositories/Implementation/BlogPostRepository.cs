using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public BlogPostRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await _dbcontext.BlogPosts.AddAsync(blogPost);
            await _dbcontext.SaveChangesAsync();
            
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _dbcontext.BlogPosts.Include(x =>x.Categories).ToListAsync();
        }
    }
}
