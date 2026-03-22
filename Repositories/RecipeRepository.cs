using first;
using Microsoft.EntityFrameworkCore;

public class RecipeRepository : IRecipeRepository
{
    private readonly RecipeContext _context;

    public RecipeRepository(RecipeContext context)
    {
        _context = context;
    }

    public async Task<List<RecipeDto>> GetAsync(int? Id, string? Name, string? Description)
    {
        var query = _context.Recipes
            .Include(r => r.Ingredients)
            .AsQueryable();

        if (Id != null) query = query.Where(r => r.Id == Id.Value);
        if (!string.IsNullOrEmpty(Name)) query = query.Where(r => r.Name.Contains(Name));
        if (!string.IsNullOrEmpty(Description)) query = query.Where(r => r.Description.Contains(Description));

        var recipes = await query.ToListAsync();
        return recipes.Select(r => new RecipeDto
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            Image = r.Image,
            ingredientDTOs = r.Ingredients.Select(i => new IngredientDTO
            {
                Id = i.Id,
                Name = i.Name
            }).ToList()
        }).ToList();
    }


    public async Task<bool> DeleteAsync(int Id)
    {
        var recipe = await _context.Recipes
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == Id);

        if (recipe == null)
        {
            return false;
        }

        if (recipe.Ingredients != null && recipe.Ingredients.Any())
        {
            _context.Ingredients.RemoveRange(recipe.Ingredients);
        }

        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();

        return true;
    }


    public async Task<RecipeDto> AddAsync(RecipeDto recipeDto)
    {
        var ingredientIds = recipeDto.ingredientDTOs.Where(i => i.Id != 0).Select(i => i.Id).ToList();

        var ingredientNames = recipeDto.ingredientDTOs.Where(i => i.Id == 0).Select(i => i.Name).ToList();

        var existingIngredients = await _context.Ingredients
            .Where(i => ingredientIds.Contains(i.Id) || ingredientNames.Contains(i.Name)).ToListAsync();

        var newIngredients = recipeDto.ingredientDTOs.Where(dto => dto.Id == 0 && !existingIngredients.Any(ei => ei.Name == dto.Name)).Select(dto => new Ingredient { Name = dto.Name }).ToList();

        var allIngredients = existingIngredients.Concat(newIngredients).ToList();

        var recipe = new Recipe
        {
            Name = recipeDto.Name,
            Description = recipeDto.Description,
            Image = recipeDto.Image,
            Ingredients = allIngredients
        };

        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();

        return new RecipeDto
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Description = recipe.Description,
            Image = recipe.Image,
            ingredientDTOs = allIngredients.Select(i => new IngredientDTO
            {
                Id = i.Id,
                Name = i.Name
            }).ToList()
        };
    }

    public async Task<bool> UpdateRecipeAsync(int id, RecipePUTDTO updatedRecipe)
    {
        var recipe = await _context.Recipes.Include(r => r.Ingredients).FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null)
        {
            return false;
        }

        recipe.Name = updatedRecipe.Name;
        recipe.Description = updatedRecipe.Description;

        foreach (var ingredient in updatedRecipe.Ingredients)
        {
            var existingIngredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == ingredient);

            if (existingIngredient == null)
            {
                existingIngredient = new Ingredient { Name = ingredient };
                _context.Ingredients.Add(existingIngredient);
                await _context.SaveChangesAsync();
            }

            if (!recipe.Ingredients.Any(i => i.Id == existingIngredient.Id))
            {
                recipe.Ingredients.Add(existingIngredient);
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Ingredient>> GetAllIngAsync()
    {
        var ingredients = await _context.Ingredients.ToListAsync();
        return ingredients;
    }

    public async Task<bool> DeleteIngAsync(int Id)
    {
        var ingredient = await _context.Ingredients.FirstOrDefaultAsync(r => r.Id == Id);

        if (ingredient == null)
        {
            return false;
        }
        var recipes = await _context.Recipes.Where(r => r.Ingredients.Any(i => i.Id == Id)).ToListAsync();
        foreach (var recipe in recipes)
        {
            recipe.Ingredients.Remove(ingredient);
        }
        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync();

        return true;
    }
    public async Task AddIngAsync(IngredientDTO ingredientDTO)
    {
        var ingredient = new Ingredient
        {
            Name = ingredientDTO.Name,
            Recipes = []
        };
        _context.Ingredients.Add(ingredient);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateIngAsync(int Id, IngredientDTO ingredientDTO)
    {
        var ingredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Id == Id);
        if (ingredient == null)
        {
            return false;
        }
        ingredient.Name = ingredientDTO.Name;
        await _context.SaveChangesAsync();
        return true;
    }

    //Kilistázza azokat a recepteket, amelyek a megadott kezdőbetűvel kezdődnek
    public async Task<List<RecipeDto>> GetRecipesByLetterAsync(char c)
    {
        var recipes = await _context.Recipes.Where(r => r.Name.StartsWith(c.ToString().ToLower())).Select(r => new RecipeDto
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            Image = r.Image,
            ingredientDTOs = r.Ingredients.Select(i => new IngredientDTO
            {
                Id = i.Id,
                Name = i.Name
            }).ToList()
        })
            .ToListAsync();

        return recipes;
    }

}