using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.CookBookExceptions
{
        public class CookBookNotFoundException : Exception
        {
            public CookBookNotFoundException(Guid collectionId) : base($"No cookbook with id: {collectionId} found") { }

            public CookBookNotFoundException(string message) : base(message) { }
        }

        public class CookBooksNotFoundException : Exception
        {
            public CookBooksNotFoundException() : base("No cookbooks found!") { }

            public CookBooksNotFoundException(string message) : base(message) { }
        }
    
}
