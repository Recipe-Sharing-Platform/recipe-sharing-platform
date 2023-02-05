using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.RecipeExceptions
{
    public class RecipeNutrientsNotFoundException : Exception
    {
        public RecipeNutrientsNotFoundException(Guid? recipeId) : base($"Could not find nutrients for Recipe: {recipeId}"){}
    }
}
