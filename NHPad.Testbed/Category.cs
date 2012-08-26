using System.Collections.Generic;

namespace NHPad.Testbed
{
    public class Category
    {
        public virtual ICollection<Post> Posts { get; set; } 
    }
}