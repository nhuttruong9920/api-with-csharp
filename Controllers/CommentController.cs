using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController :ControllerBase
    {
      private readonly ICommentRepository _commentRepo;
      private readonly IStockRepository _stockRepo;
      private readonly UserManager<AppUser> _userManager;
      public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
      {
        _commentRepo = commentRepo;
        _stockRepo = stockRepo;
        _userManager = userManager;
      }

      [HttpGet]
      public async Task<IActionResult> GetAll()
      {
        var comments = await _commentRepo.GetAllAsync();
        var commentDto = comments.Select(s => s.ToCommentDto());
      return Ok(commentDto);
      }

      [HttpGet("{id}")]
      public async Task<IActionResult> GetById([FromRoute] int id)
      {
      var comment = await _commentRepo.GetByIdAsync(id);

        if (comment == null)
        {
          return NotFound(new { Message = "Comment not found" });
        }
        return Ok(comment.ToCommentDto());
      }

      [HttpPost]
      public async Task<IActionResult> Create([FromBody] CreateCommentDto commentDto)
      {
        if(!await _stockRepo.StockExists(commentDto.StockId)){
          return BadRequest("Stock does not exist");
        }

      var username = User.GetUserName();

      var appUser = await _userManager.FindByNameAsync(username);

        var commentModel = commentDto.ToCommentFromCreate();
        commentModel.AppUserId = appUser.Id;
        await _commentRepo.CreateAsync(commentModel);
        return CreatedAtAction(nameof(GetById), new {id = commentModel}, commentModel.ToCommentDto());
      }

      [HttpPut]
      public async Task<IActionResult> Update([FromBody] UpdateCommentDto commentDto)
      {
      var commentModel = await _commentRepo.UpdateAsync(commentDto);

      if(commentModel == null){
        return NotFound(new { Message = "Comment not found" });
      }

      return Ok(commentModel.ToCommentDto());
      }

      [HttpDelete]
      public async Task<IActionResult> Delete([FromBody] int id){
        var commentModel = await _commentRepo.DeleteAsync(id);

        if(commentModel == null){
          return NotFound(new { Message = "Stock not found" });
        }

        return Ok(new { Message = "Delete success!" });
      }
    }
}
