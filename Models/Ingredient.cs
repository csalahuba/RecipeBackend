namespace first;

public class Ingredient
{
    public int Id {get; set; }
    public string Name { get; set; }
    public List<Recipe> Recipes { get; set;} = new List<Recipe>();
}