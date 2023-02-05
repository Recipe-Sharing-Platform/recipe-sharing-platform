using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.CookBookExceptions
{
    public class CookBookCouldNotBeCreatedException : Exception
    {
        public CookBookCouldNotBeCreatedException(string message) : base(message) { }
    }
}
