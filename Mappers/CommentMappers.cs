using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers
    {
        public static CommentDto ToCommentDto(this Comment commentModel)
        {
            return new CommentDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                CreatedBy = commentModel.AppUser.UserName,
                StockId = commentModel.StockId
            };
        }

        public static Comment ToCommentFromCreateDTO(this CreateCommentRequestDto stockDto, int stockId)
        {
            return new Comment
            {
                Title = stockDto.Title,
                Content = stockDto.Content,
                StockId = stockId
            };
        }

        public static Comment ToCommentFromUpdateDTO(this UpdateCommentRequestDto stockDto)
        {
            return new Comment
            {
                Title = stockDto.Title,
                Content = stockDto.Content,
            };
        }
    }
}
