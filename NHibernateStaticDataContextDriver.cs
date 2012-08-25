using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using LINQPad.Extensibility.DataContext;
using NHibernate;

namespace NHPad
{
    public class NHibernateStaticDataContextDriver : StaticDataContextDriver
    {
        public override string GetConnectionDescription(IConnectionInfo cxInfo)
        {
            return cxInfo.CustomTypeInfo.GetCustomTypeDescription();
        }

        public override bool ShowConnectionDialog(IConnectionInfo cxInfo, bool isNewConnection)
        {
            cxInfo.CustomTypeInfo.CustomTypeName = typeof(NHibernateContext).FullName;
            cxInfo.CustomTypeInfo.CustomAssemblyPath = Assembly.GetExecutingAssembly().Location;
            return new ConnectionDialog(cxInfo).ShowDialog().GetValueOrDefault();
        }

        public override string Name
        {
            get { return "NHibernate"; }
        }

        public override string Author
        {
            get { return "Diego Mijelshon"; }
        }

        public override List<ExplorerItem> GetSchema(IConnectionInfo cxInfo, Type customType)
        {
            return NHibernateSchemaReader.GetSchema(GetSessionFactory(cxInfo));
        }

        public override IEnumerable<string> GetNamespacesToAdd(IConnectionInfo cxInfo)
        {
            yield return "NHibernate";
            yield return "NHibernate.Criterion";
            yield return "NHibernate.Linq";
            var entityNamespaces = GetSessionFactory(cxInfo).GetAllClassMetadata()
                        .Select(x => x.Value.GetMappedClass(EntityMode.Poco).Namespace).Distinct();
            foreach (var entityNamespace in entityNamespaces)
                yield return entityNamespace;
        }

        public override IEnumerable<string> GetAssembliesToAdd(IConnectionInfo cxInfo)
        {
            yield return GetClientAssembly(cxInfo).FullName;
        }

        public override void InitializeContext(IConnectionInfo cxInfo, object context, QueryExecutionManager executionManager)
        {
            GetClientAssembly(cxInfo);
            var nhContext = (NHibernateContext)context;
            nhContext.Session = GetSessionFactory(cxInfo).OpenSession();
        }

        public override bool AreRepositoriesEquivalent(IConnectionInfo c1, IConnectionInfo c2)
        {
            return XNode.DeepEquals(c1.DriverData, c2.DriverData);
        }

        ISessionFactory GetSessionFactory(IConnectionInfo cxInfo)
        {
            var assembly = GetClientAssembly(cxInfo);
            var method = assembly.GetExportedTypes().SelectMany(x => x.GetMethods())
                .First(x => x.IsStatic &&
                            x.ReturnType == typeof(ISessionFactory));
            return (ISessionFactory)method.Invoke(null, null);
        }

        static Assembly GetClientAssembly(IConnectionInfo cxInfo)
        {
            return LoadAssemblySafely((string)cxInfo.DriverData.Element("ClientAssembly"));
        }
    }
}