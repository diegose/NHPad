using NHibernate;

namespace NHPad
{
    public class NHibernateContext
    {
        public ISession Session { get; set; }
    }
}