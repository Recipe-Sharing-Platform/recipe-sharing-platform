using KitchenConnection.DataLayer.Models.DTOs.Recipe;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.RecipeExceptions
{
    public class RecipeCouldNotBeUpdatedException : Exception
    {
        public RecipeCouldNotBeUpdatedException(string message) : base(message){}
    }
}
