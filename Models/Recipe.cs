namespace first;

public class Recipe
{
    public int Id {get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public List<Ingredient> Ingredients { get; set; }
}