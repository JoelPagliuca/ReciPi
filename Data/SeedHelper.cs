using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using recipi.Models;

namespace recipi.Data
{
	public class SeedHelper
	{
		// Gimme some non-prod data
		public static void SeedDummyData(RecipiDbContext context, int numberOfRecipes = 40)
		{
			Random random = new Random();
			
			// Recipe name pool
			var recipeWord1 = new[] {"Turtle", "Swordfish", "Tofu", "Chicken", "Spectacular", "Bread", "Apple", "Yoghurt"};
			var recipeWord2 = new[] {"Stew", "Cake", "Pie", "Crumble", "Pasta", "Lasaga", "Linguini", "Dessert"};
			// Recipes
			for (int i = numberOfRecipes - 1; i >= 0 ; i--)
			{
				var w1 = recipeWord1[random.Next(recipeWord1.Length)];
				var w2 = recipeWord2[random.Next(recipeWord2.Length)];
				var steps = new List<Step>();
				for (int j = 0; j < random.Next(5); j++)
				{
					steps.Add(new Step {
						Number		= j,
						Amount		= random.Next(10),
						Instruction	= w1
					});
				}
				var r = new Recipe {
					Name	= $"{w1} {w2}",
					Steps	= steps
				};
				context.Recipes.Add(r);
			}
			context.SaveChanges();
		}
		public static bool SeedRequired(RecipiDbContext context)
		{
			context.Database.EnsureCreated();
			var t = context.Recipes.OrderByDescending(v => v.Description).FirstOrDefault();
			return t == null;
		}
}
}