using Dapper;
using Microsoft.Data.SqlClient;
using RecipesWithFunk.Model;
using RecipesWithFunk.Model.RequestModels;
using RecipesWithFunk.Properties;
using System.Data;

namespace RecipesWithFunk.Data.Repositories;

public interface IRecipeRepository
{
    Task<Recipe?> AddRecipe(AddRecipeRequest recipe, CancellationToken cancellationToken = default);
    Task<bool> DeleteRecipe(int recipeId, CancellationToken cancellationToken = default);
    Task<List<Recipe>> GetAllRecipes(CancellationToken cancellationToken = default);
    Task<List<string>> GetAllTypes(CancellationToken cancellationToken = default);
    Task<Recipe?> GetRecipeById(int recipeId, CancellationToken cancellationToken = default);
    Task<bool> RecipeExists(string recipeName, CancellationToken cancellationToken = default);
    Task<bool> UpdateRecipe(int recipeId, UpdateRecipeRequest recipe, CancellationToken cancellationToken = default);
}

public class RecipeRepository : IRecipeRepository
{
    private string? connectionString = string.Empty;

    public RecipeRepository()
    {
        connectionString = $"Data Source={Settings.Default.ServerName};Initial Catalog={Settings.Default.InitialCatalog};Integrated Security=true;" +
               $"Connection Lifetime=15;Min Pool Size=1;Max Pool Size=200;Connection Timeout=180;TrustServerCertificate=true;";
        if (connectionString is null)
            throw new Exception("Connection String was empty");
    }

    public async Task<Recipe?> AddRecipe(AddRecipeRequest recipe, CancellationToken cancellationToken = default)
    {
        Recipe? ret = null;
        int? res = null;

        const string iSql = @"
INSERT INTO Recipe.Recipe (CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, [Name], [Description], Directions, Ingredients, Note, [Type], Rating, Servings)
OUTPUT INSERTED.RecipeId
VALUES (@by, @now, @by, @now, @name, @description, @directions, @ingredients, @note, @type, @rating, @servings)
";

        using IDbConnection conn = new SqlConnection(connectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        using IDbTransaction trx = conn.BeginTransaction();

        var parms = new
        {
            By = "System",
            DateTime.Now,
            recipe.Description,
            recipe.Directions,
            recipe.Ingredients,
            recipe.Name,
            recipe.Note,
            recipe.Rating,
            recipe.Servings,
            recipe.Type
        };

        try
        {
            CommandDefinition iCmd = new(iSql, parms, trx, 150, cancellationToken: cancellationToken);
            res = await conn.QuerySingleAsync<int>(iCmd);

            trx.Commit();
        }
        catch (Exception ex)
        {
            trx.Rollback();
        }
        finally
        {
            conn.Close();
        }

        if (res is not null)
            ret = await GetRecipeById(res.Value, cancellationToken);
        return ret;
    }

    public async Task<bool> DeleteRecipe(int recipeId, CancellationToken cancellationToken = default)
    {
        int? res = null;

        const string dSql = @"
DELETE FROM Recipe.Recipe WHERE RecipeId = @recipeId
";

        using IDbConnection conn = new SqlConnection(connectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        using IDbTransaction trx = conn.BeginTransaction();

        var parms = new { recipeId };

        try
        {
            CommandDefinition iCmd = new(dSql, parms, trx, 150, cancellationToken: cancellationToken);
            res = await conn.ExecuteAsync(iCmd);

            trx.Commit();
        }
        catch (Exception ex)
        {
            trx.Rollback();
        }
        finally
        {
            conn.Close();
        }

        return res > 0;
    }

    public async Task<List<Recipe>> GetAllRecipes(CancellationToken cancellationToken = default)
    {
        List<Recipe> ret = [];

        const string sSql = @"
SELECT r.RecipeId, r.CreatedBy, r.CreatedDate, r.ModifiedBy, r.ModifiedDate, r.[Name], r.[Description], r.Directions, r.Ingredients, r.Note, r.[Type], r.Rating, r.Servings
FROM Recipe.Recipe AS r
";

        using IDbConnection conn = new SqlConnection(connectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        try
        {
            CommandDefinition sCmd = new(sSql, null, null, 150, cancellationToken: cancellationToken);
            IEnumerable<Recipe> res = await conn.QueryAsync<Recipe>(sCmd);

            if (res is not null)
                ret.AddRange(res);
        }
        catch (Exception ex)
        {
            var test = ex.Message;
        }
        finally
        {
            conn.Close();
        }

        return ret;
    }

    public async Task<List<string>> GetAllTypes(CancellationToken cancellationToken = default)
    {
        List<string> ret = [];

        const string sSql = @"
SELECT DISTINCT [Type]
FROM Recipe.Recipe
";

        using IDbConnection conn = new SqlConnection(connectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        try
        {
            CommandDefinition sCmd = new(sSql, null, null, 150, cancellationToken: cancellationToken);
            IEnumerable<string> res = await conn.QueryAsync<string>(sCmd);

            if (res is not null)
                ret.AddRange(res);
        }
        catch (Exception ex)
        {
            var test = ex.Message;
        }
        finally
        {
            conn.Close();
        }

        return ret;
    }

    public async Task<Recipe?> GetRecipeById(int recipeId, CancellationToken cancellationToken = default)
    {
        Recipe? ret = null;

        const string sSql = @"
SELECT r.RecipeId, r.CreatedBy, r.CreatedDate, r.ModifiedBy, r.ModifiedDate, r.[Name], r.[Description], r.Directions, r.Ingredients, r.Note, r.[Type], r.Rating, r.Servings
FROM Recipe.Recipe AS r
WHERE r.RecipeId = @recipeId
";

        var parms = new { recipeId };

        using IDbConnection conn = new SqlConnection(connectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        try
        {
            CommandDefinition sCmd = new(sSql, parms, null, 150, cancellationToken: cancellationToken);
            IEnumerable<Recipe> res = await conn.QueryAsync<Recipe>(sCmd);

            if (res is not null)
                ret = res.FirstOrDefault();
        }
        catch (Exception ex)
        {
            var test = ex.Message;
        }
        finally
        {
            conn.Close();
        }

        return ret;
    }

    public async Task<bool> RecipeExists(string recipeName, CancellationToken cancellationToken = default)
    {
        bool ret = false;

        const string sSql = @"
SELECT r.RecipeId, r.CreatedBy, r.CreatedDate, r.ModifiedBy, r.ModifiedDate, r.[Name], r.[Description], r.Directions, r.Ingredients, r.Note, r.[Type], r.Rating, r.Servings
FROM Recipe.Recipe AS r
WHERE r.[Name] = @recipeName
";

        var parms = new { recipeName };

        using IDbConnection conn = new SqlConnection(connectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        try
        {
            CommandDefinition sCmd = new(sSql, parms, null, 150, cancellationToken: cancellationToken);
            IEnumerable<Recipe> res = await conn.QueryAsync<Recipe>(sCmd);

            ret = res.Any();
        }
        catch (Exception ex)
        {
            var test = ex.Message;
        }
        finally
        {
            conn.Close();
        }

        return ret;
    }

    public async Task<bool> UpdateRecipe(int recipeId, UpdateRecipeRequest recipe, CancellationToken cancellationToken = default)
    {
        int? res = null;

        const string uSql = @"
UPDATE Recipe.Recipe SET CreatedBy = @by,
    CreatedDate = @now,
    ModifiedBy = @by,
    ModifiedDate = @now,
    [Name] = @name,
    [Description] = @description,
    Directions = @directions,
    Ingredients = @ingredients,
    Note = @note,
    [Type] = @type,
    Rating = @rating,
    Servings = @servings
WHERE RecipeId = @recipeId
";

        using IDbConnection conn = new SqlConnection(connectionString);
        if (conn.State != ConnectionState.Open)
            conn.Open();
        using IDbTransaction trx = conn.BeginTransaction();

        var parms = new
        {
            By = "System",
            DateTime.Now,
            recipe.Description,
            recipe.Directions,
            recipe.Ingredients,
            recipe.Name,
            recipe.Note,
            recipe.Rating,
            recipe.Servings,
            recipeId,
            recipe.Type
        };

        try
        {
            CommandDefinition iCmd = new(uSql, parms, trx, 150, cancellationToken: cancellationToken);
            res = await conn.ExecuteAsync(iCmd);

            trx.Commit();
        }
        catch (Exception ex)
        {
            trx.Rollback();
        }
        finally
        {
            conn.Close();
        }

        return res > 0;
    }
}
