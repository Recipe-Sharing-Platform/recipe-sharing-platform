using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.CollectionExceptions
{
    public class CollectionCouldNotBeCreatedException:Exception
    {
        public CollectionCouldNotBeCreatedException(string message) : base(message) { }
    }
}
