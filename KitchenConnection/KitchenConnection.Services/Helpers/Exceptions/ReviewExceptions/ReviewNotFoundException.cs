using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.ReviewExceptions
{
    public class ReviewNotFoundException : Exception
    {
        public ReviewNotFoundException(Guid reviewId) : base($"Review could not be found: {reviewId}"){}
    }
}
