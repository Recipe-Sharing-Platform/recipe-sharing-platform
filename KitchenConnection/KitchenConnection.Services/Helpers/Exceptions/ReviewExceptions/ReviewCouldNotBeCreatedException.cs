using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.ReviewExceptions
{
    public class ReviewCouldNotBeCreatedException : Exception
    {
        public ReviewCouldNotBeCreatedException(string message) : base(message){}
    }
}
