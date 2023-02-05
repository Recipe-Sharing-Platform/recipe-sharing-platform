using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.RecipeExceptions
{
    public class RecipeCouldNotBeCreatedException : Exception
    {
        public RecipeCouldNotBeCreatedException(string message) : base(message){}
    }
}
