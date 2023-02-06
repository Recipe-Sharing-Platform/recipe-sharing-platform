using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.CuisineExceptions
{
    public class CuisineNotFoundException : Exception
    {
        public CuisineNotFoundException(Guid cuisineId) : base($"Cuisine not found: {cuisineId}"){}
        public CuisineNotFoundException(string message) : base(message){}
    }

    public class CuisinesNotFoundException : Exception
    {
        public CuisinesNotFoundException(string message) : base(message) { }
    }
}
