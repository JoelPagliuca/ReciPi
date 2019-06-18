using System.Collections.Generic;

namespace recipi.Models
{
	public class Recipe
	{
		public enum DifficultyLevel
		{
			Breezy,
			Tricky,
			Espert
		}
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DifficultyLevel Difficulty { get; set; }
		public int Serves { get; set; }
		public int TimePrep { get; set; }
		public int TimeCook { get; set; }
		public int TimeOther { get; set; }
		
		public List<Step> Steps { get; set; }
	}

}