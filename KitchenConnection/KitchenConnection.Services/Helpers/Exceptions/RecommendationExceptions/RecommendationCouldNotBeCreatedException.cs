using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.RecommendationExceptions
{
    public class RecommendationCouldNotBeCreatedException : Exception
    {
        public RecommendationCouldNotBeCreatedException(string message) : base(message) { }
    }
}
