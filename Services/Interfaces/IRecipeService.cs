using first;

public interface IRecipeService{
    public Task<List<RecipeDto>> GetAsync(int? Id, string? Name, string? Description);
    public Task<bool> DeleteAsync(int Id);
    public Task<RecipeDto> AddAsync(RecipeDto recipeDto);
    public Task<bool> UpdateRecipeAsync(int id, RecipePUTDTO updatedRecipe);
    public Task<List<Ingredient>> GetAllIngAsync();
    public Task<bool> DeleteIngAsync(int Id);
    public Task AddIngAsync(IngredientDTO ingredientDTO);
    public Task<bool> UpdateIngAsync(int Id, IngredientDTO ingredientDTO);
    public Task<List<RecipeDto>> GetRecipesByLetterAsync(char c);
}
