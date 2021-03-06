﻿using System.Collections.Generic;
using System.Linq;
using LINQPad.Extensibility.DataContext;
using NHibernate;
using NHibernate.Metadata;
using NHibernate.Persister.Collection;
using NHibernate.Type;

namespace NHPad
{
    public static class NHibernateSchemaReader
    {
        public static List<ExplorerItem> GetSchema(ISessionFactory sessionFactory)
        {
            var items = sessionFactory.GetAllClassMetadata().Values
                .Select(x => new ExplorerItem(x.EntityName, ExplorerItemKind.QueryableObject, ExplorerIcon.Table)
                                 {
                                     Children = GetId(x)
                                         .Concat(x.PropertyNames.Select(p => GetPropertyItem(sessionFactory, x, p))).ToList(),
                                     Tag = x.GetMappedClass(EntityMode.Poco)
                                 }).ToList();
            foreach (var property in items.SelectMany(x => x.Children))
            {
                var type = (IType)property.Tag;
                var manyToOne = type as ManyToOneType;
                if (manyToOne != null)
                    property.HyperlinkTarget = items.Single(x => Equals(x.Tag, type.ReturnedClass));
                if (type.IsCollectionType && type.ReturnedClass.IsGenericType)
                    property.HyperlinkTarget =
                        items.Single(x => Equals(x.Tag, type.ReturnedClass.GetGenericArguments().Single()));
            }
            return items;
        }

        private static IEnumerable<ExplorerItem> GetId(IClassMetadata classMetadata)
        {
            var propertyName = classMetadata.IdentifierPropertyName;
            if (propertyName != null)
                yield return new ExplorerItem(propertyName, ExplorerItemKind.Property, ExplorerIcon.Key)
                                 {
                                     Tag = classMetadata.GetPropertyType(propertyName)
                                 };
        }

        private static ExplorerItem GetPropertyItem(ISessionFactory sessionFactory, IClassMetadata classMetadata, string propertyName)
        {
            var kind = GetKind(classMetadata, propertyName);
            var propertyType = classMetadata.GetPropertyType(propertyName);
            return new ExplorerItem(kind == ExplorerItemKind.Property
                                        ? string.Format("{0} ({1})", propertyName, propertyType.ReturnedClass.Name)
                                        : propertyName,
                                    kind,
                                    GetIcon(sessionFactory, classMetadata, propertyName))
                       {
                           Tag = classMetadata.GetPropertyType(propertyName)
                       };
        }

        static ExplorerItemKind GetKind(IClassMetadata classMetadata, string propertyName)
        {
            var propertyType = classMetadata.GetPropertyType(propertyName);
            return propertyType.IsCollectionType
                       ? ExplorerItemKind.CollectionLink
                       : propertyType.IsAssociationType
                             ? ExplorerItemKind.ReferenceLink
                             : ExplorerItemKind.Property;
        }

        static ExplorerIcon GetIcon(ISessionFactory sessionFactory, IClassMetadata classMetadata, string propertyName)
        {
            var propertyType = classMetadata.GetPropertyType(propertyName);
            return propertyType.IsCollectionType
                       ? ((ICollectionPersister)sessionFactory.GetCollectionMetadata(
                           string.Format("{0}.{1}", classMetadata.GetMappedClass(EntityMode.Poco), propertyName)))
                             .IsManyToMany
                             ? ExplorerIcon.ManyToMany
                             : ExplorerIcon.OneToMany
                       : propertyType.IsAssociationType
                             ? ExplorerIcon.ManyToOne
                             : ExplorerIcon.Column;
        }
    }
}