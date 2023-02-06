using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.CuisineExceptions
{
    public class CuisineCouldNotBeUpdatedException : Exception
    {
        public CuisineCouldNotBeUpdatedException(string message) : base(message){}
    }
}
