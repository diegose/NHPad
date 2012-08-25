using System;
using System.Collections.Generic;

namespace NHPad.Testbed
{
    public class Blog
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}