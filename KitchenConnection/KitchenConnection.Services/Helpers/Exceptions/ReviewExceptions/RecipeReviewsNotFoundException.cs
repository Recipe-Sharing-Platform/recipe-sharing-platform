using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.ReviewExceptions
{
    public class RecipeReviewsNotFoundException : Exception
    {
        public RecipeReviewsNotFoundException(Guid recipeId) : base($"Recipe Reviews not found: {recipeId}"){}

        public RecipeReviewsNotFoundException(string message) : base(message){}
    }
}
