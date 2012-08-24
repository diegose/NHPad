using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LINQPad.Extensibility.DataContext;

namespace NHPad
{
    public class NHibernateStaticDataContextDriver : StaticDataContextDriver
    {
        public override string GetConnectionDescription(IConnectionInfo cxInfo)
        {
            return cxInfo.DisplayName;
        }

        public override bool ShowConnectionDialog(IConnectionInfo cxInfo, bool isNewConnection)
        {
            cxInfo.CustomTypeInfo.CustomTypeName = typeof(NHibernateContext).FullName;
            cxInfo.CustomTypeInfo.CustomAssemblyPath = Assembly.GetExecutingAssembly().Location;
            return true;
        }

        public override string Name
        {
            get { return "NHibernate Driver"; }
        }

        public override string Author
        {
            get { return "Diego Mijelshon"; }
        }

        public override List<ExplorerItem> GetSchema(IConnectionInfo cxInfo, Type customType)
        {
            return new List<ExplorerItem>();
        }
    }
}