using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.TagExceptions
{
    public class TagCouldNotBeCreatedException : Exception
    {
        public TagCouldNotBeCreatedException(string message) : base(message){}
    }
}
