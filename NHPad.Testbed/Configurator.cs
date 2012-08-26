using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace NHPad.Testbed
{
    public static class Configurator
    {
        public static ISessionFactory GetSessionFactory()
        {
            var config = new Configuration();
            config.SessionFactory().Integrate.Using<SQLiteDialect>().Connected.Using("Data source=nhtest.sqlite").AutoQuoteKeywords();
            var mapper = new ConventionModelMapper();
            Map(mapper);
            config.AddDeserializedMapping(mapper.CompileMappingForAllExplicitlyAddedEntities(), "Mappings");
            SchemaMetadataUpdater.QuoteTableAndColumns(config);
            new SchemaUpdate(config).Execute(false, true);
            return config.BuildSessionFactory();
        }

        static void Map(ConventionModelMapper mapper)
        {
            mapper.Class<Blog>(cm => { });
            mapper.Class<Post>(cm => cm.Bag(x => x.Categories, bpm => { }, cer => cer.ManyToMany()));
            mapper.Class<Category>(cm => cm.Bag(x => x.Posts, bpm => bpm.Inverse(true), cer => cer.ManyToMany()));
        }
    }
}