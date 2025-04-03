using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController(
        UserManager<AppUser> userManager,
        IStockRepository stockRepo,
        IPortfolioRepository portfolioRepo
    ) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await userManager.FindByNameAsync(username);
            var userPortfolio = await portfolioRepo.GetUserPortfolio(appUser);

            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await userManager.FindByNameAsync(username);
            var stock = await stockRepo.GetBySymbolAsync(symbol);

            if (stock == null)
                return NotFound("Stock not found");

            var userPortfolio = await portfolioRepo.GetUserPortfolio(appUser);

            if (userPortfolio.Any(s => s.Symbol.ToLower() == stock.Symbol.ToLower()))
                return BadRequest("Stock already in portfolio");

            var portfolioModel = new Portfolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id,
            };

            await portfolioRepo.CreateAsync(portfolioModel);

            if (portfolioModel == null)
            {
                return StatusCode(500, "Could not create");
            }
            else
            {
                return Created();
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemovePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await userManager.FindByNameAsync(username);
            
            var userPortfolio = await portfolioRepo.GetUserPortfolio(appUser);

            var filteredPortfolio = userPortfolio
                .Where(s => s.Symbol.ToLower() == symbol.ToLower())
                .ToList();

            if (filteredPortfolio.Count() == 1)
                await portfolioRepo.DeletePortfolio(appUser, symbol);
            else
                return BadRequest("Stock not in your portfolio");

            return Ok();
        }
    }
}
