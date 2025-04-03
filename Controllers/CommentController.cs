using api.Data;
using api.Dtos.Comment;
using api.Extensions;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await commentRepo.GetAllAsync();

            var commentDtos = comments.Select(x => x.ToCommentDto());

            return Ok(commentDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await commentRepo.GetByIdAsync(id);

            if (comment == null)
                return NotFound();

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDto commentDto)
        {
            if (!await stockRepo.StockExists(stockId))
                return BadRequest("Stock does not exist");

            var username = User.GetUsername();
            var appUser = await userManager.FindByNameAsync(username);

            var commentModel = commentDto.ToCommentFromCreateDTO(stockId);
            commentModel.AppUserId = appUser.Id;

            await commentRepo.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentDto)
        {
            var commentModel = await commentRepo.UpdateAsync(id, commentDto.ToCommentFromUpdateDTO());

            if (commentModel == null)
                return NotFound();

            return Ok(commentModel.ToCommentDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var commentModel = await commentRepo.DeleteAsync(id);

            if (commentModel == null)
                return NotFound();

            return NoContent();
        }
    }
}
