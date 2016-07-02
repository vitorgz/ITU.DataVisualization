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
            //Database.SetInitializer(new DropCreateDatabaseAlways<VisualizationDBContext>());
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<VisualizationDBContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<VisualizationDBContext, Configuration>());

        }

        public DbSet<VisualizationDB> Visualizations { get; set; }


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
