using System;

namespace NHPad.Testbed
{
    public class Post
    {
        public virtual Guid Id { get; set; }
        public virtual Blog Blog { get; set; }
    }
}