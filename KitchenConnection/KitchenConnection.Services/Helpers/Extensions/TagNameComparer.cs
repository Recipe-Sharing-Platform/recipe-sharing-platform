using KitchenConnection.DataLayer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenConnection.BusinessLogic.Helpers.Extensions
{
    public class TagNameComparer : IEqualityComparer<Tag>
    {
        public bool Equals(Tag left, Tag right)
        {
            if(string.Equals(left.Name, right.Name, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(Tag tag)
        {
            return tag.Name.GetHashCode();
        }
    }
}
