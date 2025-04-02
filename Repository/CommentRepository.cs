using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository(ApplicationDbContext context) : ICommentRepository
    {
        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await context.Comments.AddAsync(commentModel);
            await context.SaveChangesAsync();

            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var comment = await context.Comments.FindAsync(id);

            if (comment != null)
            {
                context.Comments.Remove(comment);
                await context.SaveChangesAsync();
            }

            return comment;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            var comments = await context.Comments.ToListAsync();

            return comments;
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            var comment = await context.Comments.FindAsync(id);

            return comment;
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var existingComment = await context.Comments.FindAsync(id);

            if (existingComment != null)
            {
                existingComment.Title = commentModel.Title;
                existingComment.Content = commentModel.Content;
                await context.SaveChangesAsync();
            }

            return existingComment;
        }
    }
}
