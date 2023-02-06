using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.CuisineExceptions
{
    public class CuisineCouldNotBeCreatedException : Exception
    {
        public CuisineCouldNotBeCreatedException(string message) : base(message){}
    }
}
