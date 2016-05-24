using ITU.Ckan.DataVisualization.EFDataBase.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.EFDataBase
{
    public class VisualizationDBContext: DbContext
    {
        public VisualizationDBContext() : base("name=VisualizationDBConnection")
        {
           // Database.SetInitializer(new DropCreateDatabaseIfModelChanges<VisualizationDBContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<VisualizationDBContext, Configuration>());

        }
        
        public DbSet<Visualization> Visualizations  { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Row> Rows { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        //public DbSet<NamedElement> NamedElements { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Graph> Graphs { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<DataSet> DataSets { get; set; }
        public DbSet<Column> Columns { get; set; }

        public void Detach(object entity)
        {
            ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.ChangeObjectState(entity, EntityState.Detached);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //WCF can't serialize lazy loaded entities
            Configuration.LazyLoadingEnabled = false;

            //Disable one-to-many cascade delete.
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

    }
}
