using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace api.Repository
{
    public class CommentRepository: ICommentRepository
    {
    private readonly ApplicationDBContext _context;

    public CommentRepository(ApplicationDBContext context)
    {
      _context = context;
    }

    public async Task<Comment> CreateAsync(Comment commentModel)
    {
      await _context.Comments.AddAsync(commentModel);
      await _context.SaveChangesAsync();
      return commentModel;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
      var commentModel = _context.Comments.FirstOrDefault(x => x.Id == id);
      if (commentModel == null)
      {
        return null;
      }

      _context.Comments.Remove(commentModel);
      await _context.SaveChangesAsync();
      return commentModel;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
      return await _context.Comments.ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
      return await _context.Comments.FindAsync(id);
    }

    public async Task<Comment?> UpdateAsync(UpdateCommentDto commentDto)
    {
      var commentModel = await _context.Comments.FirstOrDefaultAsync(x => x.Id == commentDto.CommentId);

      if(commentModel == null)
      {
        return null;
      }

      commentModel.Title = commentDto.Title;
      commentModel.Content = commentDto.Content;

      await _context.SaveChangesAsync();

      return commentModel;
    }
  }
}
