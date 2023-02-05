using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.RecommendationExceptions
{
    public class RecommendationNotFoundException : Exception
    {
        public RecommendationNotFoundException(string message) : base(message){}
    }
}
