using System;
using System.Collections.Generic;
using LINQPad.Extensibility.DataContext;

namespace NHPad
{
    public class NHibernateStaticDataContextDriver : StaticDataContextDriver
    {
        public override string GetConnectionDescription(IConnectionInfo cxInfo)
        {
            throw new NotImplementedException();
        }

        public override bool ShowConnectionDialog(IConnectionInfo cxInfo, bool isNewConnection)
        {
            throw new NotImplementedException();
        }

        public override string Name
        {
            get { throw new NotImplementedException(); }
        }

        public override string Author
        {
            get { throw new NotImplementedException(); }
        }

        public override List<ExplorerItem> GetSchema(IConnectionInfo cxInfo, Type customType)
        {
            throw new NotImplementedException();
        }
    }
}