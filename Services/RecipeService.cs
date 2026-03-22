using first;

public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _repository;
    public RecipeService(IRecipeRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<RecipeDto>> GetAsync(int? Id, string? Name, string? Description)
    {
        return await _repository.GetAsync(Id, Name, Description);
    }
    public async Task<bool> DeleteAsync(int Id)
    {
        return await _repository.DeleteAsync(Id);
    }

    public async Task<RecipeDto> AddAsync(RecipeDto recipeDto)
    {
        return await _repository.AddAsync(recipeDto);
    }

    public async Task<bool> UpdateRecipeAsync(int id, RecipePUTDTO updatedRecipe)
    {
        return await _repository.UpdateRecipeAsync(id, updatedRecipe);
    }

    public async Task<List<Ingredient>> GetAllIngAsync()
    {
        return await _repository.GetAllIngAsync();
    }

    public async Task<bool> DeleteIngAsync(int Id)
    {
        return await _repository.DeleteIngAsync(Id);
    }
    public async Task AddIngAsync(IngredientDTO ingredientDTO)
    {
        await _repository.AddIngAsync(ingredientDTO);
    }
    public async Task<bool> UpdateIngAsync(int Id, IngredientDTO ingredientDTO)
    {
        return await _repository.UpdateIngAsync(Id, ingredientDTO);
    }

    public async Task<List<RecipeDto>> GetRecipesByLetterAsync(char c)
    {
        return await _repository.GetRecipesByLetterAsync(c);
    }
}