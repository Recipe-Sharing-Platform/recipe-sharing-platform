using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.TagExceptions
{
    public class TagNotFoundException : Exception
    {
        public TagNotFoundException(Guid tagId) : base($"Tag not found: {tagId}"){}
        public TagNotFoundException(string message) : base(message){}
    }

    public class TagsNotFoundException : Exception
    {
        public TagsNotFoundException(string message) : base(message) { }
    }
}
