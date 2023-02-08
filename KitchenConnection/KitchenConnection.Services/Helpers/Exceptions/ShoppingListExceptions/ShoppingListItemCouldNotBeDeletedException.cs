using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.ShoppingListExceptions
{
    public class ShoppingListItemCouldNotBeDeletedException:Exception
    {
        public ShoppingListItemCouldNotBeDeletedException(string message) : base(message) { }

    }
}
