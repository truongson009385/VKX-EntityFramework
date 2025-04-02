﻿using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters long")]
        [MaxLength(280, ErrorMessage = "Title must be at most 280 characters long")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000, ErrorMessage = "Content must be at most 1000 characters long")]
        public string Content { get; set; } = string.Empty;
    }
}
