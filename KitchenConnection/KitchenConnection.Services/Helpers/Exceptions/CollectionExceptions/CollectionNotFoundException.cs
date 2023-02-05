using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Exceptions.CollectionExceptions
{
        public class CollectionNotFoundException : Exception
        {
            public CollectionNotFoundException(Guid collectionId) : base($"User has no collection with id: {collectionId}") { }

            public CollectionNotFoundException(string message) : base(message) { }
        }

        public class CollectionsNotFoundException : Exception
        {
            public CollectionsNotFoundException() : base("User has no collections") { }

            public CollectionsNotFoundException(string message) : base(message) { }
        }
    
}
