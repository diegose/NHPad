using System;
using System.Collections.Generic;

namespace NHPad.Testbed
{
    public class Post
    {
        public virtual Blog Blog { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}