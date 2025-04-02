using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository(ApplicationDbContext context) : IStockRepository
    {
        public async Task<List<Stock>> GetAllAsync([FromQuery] QueryObject query)
        {
            var stocks = context.Stocks
                .Include(s => s.Comments)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsSortDecsending
                        ? stocks.OrderByDescending(s => s.Symbol)
                        : stocks.OrderBy(s => s.Symbol);
                }
            }

            var skipNumber = (query.PageNumer - 1) * query.PageSize;

            return await stocks
                .Skip(skipNumber)
                .Take(query.PageSize)
                .ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            var stock = await context.Stocks
                .Include(s => s.Comments)
                .FirstOrDefaultAsync(s => s.Id == id);

            return stock;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await context.Stocks.AddAsync(stockModel);
            await context.SaveChangesAsync();

            return stockModel;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var stockModel = await context.Stocks.FindAsync(id);

            if (stockModel != null)
            {
                stockModel.Symbol = stockDto.Symbol;
                stockModel.CompanyName = stockDto.CompanyName;
                stockModel.Purchase = stockDto.Purchase;
                stockModel.LastDiv = stockDto.LastDiv;
                stockModel.Industry = stockDto.Industry;
                stockModel.MarketCap = stockDto.MarketCap;

                await context.SaveChangesAsync();
            }

            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await context.Stocks.FindAsync(id);

            if (stockModel != null)
            {
                context.Stocks.Remove(stockModel);
                await context.SaveChangesAsync();
            }

            return stockModel;
        }

        public Task<bool> StockExists(int id)
        {
            return context.Stocks.AnyAsync(s => s.Id == id);
        }
    }
}
