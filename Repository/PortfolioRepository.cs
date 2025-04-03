using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository(ApplicationDbContext context) : IPortfolioRepository
    {
        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await context.Portfolios.AddAsync(portfolio);
            await context.SaveChangesAsync();

            return portfolio;
        }

        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            var portfolioModel = await context.Portfolios
                .FirstOrDefaultAsync(p => p.AppUserId == appUser.Id && p.Stock.Symbol.ToLower() == symbol.ToLower());

            if (portfolioModel != null)
            {
                context.Portfolios.Remove(portfolioModel);
                await context.SaveChangesAsync();
            }

            return portfolioModel;
        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await context.Portfolios
                .Where(p => p.AppUserId == user.Id)
                .Select(p => new Stock
                {
                    Id = p.StockId,
                    Symbol = p.Stock.Symbol,
                    CompanyName = p.Stock.CompanyName,
                    Purchase = p.Stock.Purchase,
                    Industry = p.Stock.Industry,
                    LastDiv = p.Stock.LastDiv,
                    MarketCap = p.Stock.MarketCap
                })
                .ToListAsync();
        }
    }
}
