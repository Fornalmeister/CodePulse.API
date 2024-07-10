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

        public async Task<BlogPost> DeleteAsync(Guid id)
        {
            var existingBlogPost =  await _dbcontext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);

            if(existingBlogPost != null)
            {
                _dbcontext.BlogPosts.Remove(existingBlogPost);
                await _dbcontext.SaveChangesAsync();
                return existingBlogPost;
            }

            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _dbcontext.BlogPosts.Include(x =>x.Categories).ToListAsync();
        }

        public async Task<BlogPost> GetByIdAsync(Guid id)
        {
            return await _dbcontext.BlogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost> GetByUrlHandleAsync(string urlHandle)
        {
            return await _dbcontext.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x=>x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await _dbcontext.BlogPosts.Include(x=>x.Categories).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            
            if(existingBlogPost == null)
            {
                return null;
            }

            _dbcontext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            existingBlogPost.Categories = blogPost.Categories;

            await _dbcontext.SaveChangesAsync();

            return blogPost;
        }
    }
}
