namespace first;

public class RecipeDto
{
    public int Id {get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public List<IngredientDTO> ingredientDTOs { get; set;}
}