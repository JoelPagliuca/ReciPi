namespace recipi.Models
{
	public class Step
	{
		public int Number { get; set; }
		public string Ingredient { get; set; }
		public string Unit { get; set; }
		public float Amount { get; set; }
		public string Instruction { get; set; }

		public int RecipeId { get; set; }
		public Recipe Recipe { get; set; }
	}
}