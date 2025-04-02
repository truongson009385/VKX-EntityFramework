﻿using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stock
{
    public class CreateStockRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol cannot exceed 10 characters")]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MaxLength(50, ErrorMessage = "Company name cannot exceed 50 characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(1, 100000000)]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Industry cannot exceed 10 characters")]
        public string Industry { get; set; } = string.Empty;

        [Range(1, 500000000)]
        public long MarketCap { get; set; }
    }
}
