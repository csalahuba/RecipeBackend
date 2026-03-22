using first;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/recipes")]

public class RecipesController : ControllerBase
{
    private readonly IRecipeService _service;

    public RecipesController(IRecipeService recipeService)
    {
        _service = recipeService;
    }

    [HttpGet("Recipes")]
    public async Task<ActionResult<IEnumerable<RecipeDto>>> GetRecipes([FromQuery] int? Id, [FromQuery] string? Name, [FromQuery] string? Description, [FromQuery] string? Image)
    {
        var recipes = await _service.GetAsync(Id, Name, Description);
        return Ok(recipes);
    }

    [HttpDelete("Recipes")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        var deletion = await _service.DeleteAsync(id);
        if (!deletion)
        {
            return NotFound(new { message = "Not found" });
        }

        return Ok(new { message = "Deleted successfully" });
    }

    [HttpPost("Recipes")]
    public async Task<IActionResult> AddRecipe([FromBody] RecipeDto recipeDto)
    {
        if (recipeDto == null)
        {
            return BadRequest(new { message = "Invalid recipe data" });
        }
        var createdRecipe = await _service.AddAsync(recipeDto);
        return Ok(createdRecipe);
    }

    [HttpPut("Recipes")]
    public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipePUTDTO updatedRecipe)
    {
        var success = await _service.UpdateRecipeAsync(id, updatedRecipe);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
    [HttpGet("Ingredients")]
    public async Task<ActionResult<IEnumerable<RecipeDto>>> GetAllIng()
    {
        var ingredients = await _service.GetAllIngAsync();
        return Ok(ingredients);
    }

    [HttpDelete("Ingredients")]
    public async Task<IActionResult> DeleteIngredient(int id)
    {
        var deletion = await _service.DeleteIngAsync(id);
        if (!deletion)
        {
            return NotFound(new { message = "Ingredient not found" });
        }

        return Ok(new { message = "Ingredient deleted successfully" });
    }
    [HttpPost("Ingredients")]
    public async Task<IActionResult> AddIngredient([FromBody] IngredientDTO ingredientDTO)
    {
        if (ingredientDTO == null)
        {
            return BadRequest(new { message = "Invalid ingredient data" });
        }

        await _service.AddIngAsync(ingredientDTO);
        return Ok(new { message = "Ingredient added successfully" });
    }

    [HttpPut("Ingredients")]
    public async Task<IActionResult> UpdateIngredient(int id, [FromBody] IngredientDTO ingredientDto)
    {
        var success = await _service.UpdateIngAsync(id, ingredientDto);

        if (!success)
        {
            return NotFound(new { message = "Ingredient not found" });
        }

        return NoContent();
    }
    [HttpGet("Recipes/ByFirstLetter")]
    public async Task<ActionResult<IEnumerable<RecipeDto>>> GetRecipesByLetter([FromQuery] char letter)
    {
        if (!char.IsLetter(letter))
        {
            return BadRequest(new { message = "Input error" });
        }

        var recipes = await _service.GetRecipesByLetterAsync(letter);
        if (!recipes.Any())
        {
            return NotFound(new { message = "Recipe not found" });
        }

        return Ok(recipes);
    }
}

