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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            return await _context.Recipes.ToListAsync();
        }
    }
}