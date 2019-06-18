using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using recipi.Models;
using recipi.Data;

namespace recipi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RecipeController : ControllerBase
	{
		private readonly RecipiDbContext _context;

		public RecipeController(RecipiDbContext context)
		{
			_context = context;
			if (_context.Recipes.Count() == 0)
			{
				// dummy data
				_context.Recipes.Add(new Recipe { Name = "Hamburger", Description = "Ham-Bur-Ger" });
				_context.SaveChanges();
			}
		}

		// GET Recipe
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
		{
			return await _context.Recipes.ToListAsync();
		}

		// Get Recipe/9
		[HttpGet("{recipeId}")]
		public async Task<ActionResult<Recipe>> GetRecipe(int recipeId)
		{
			var recipe = await _context.Recipes
				.Include(r => r.Steps)
				.AsNoTracking()
				.FirstOrDefaultAsync(r => r.Id == recipeId);
			
			if (recipe == null)
			{
				return NotFound();
			}
			return recipe;
		}
	}
}