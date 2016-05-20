﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITU.Ckan.DataVisualization.EFDataBase
{
    public class VisualizationDBContext: DbContext
    {
        public VisualizationDBContext() : base("name=VisualizationDBConnection")
        {
        }


        public DbSet<Visualization> Visualizations  { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Source> Sources  { get; set; }
        public DbSet<Row> Rows { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<NamedElement> NamedElements { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Graph> Graphs { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<DataSet> DataSets { get; set; }
        public DbSet<Column> Columns { get; set; }

    }
}
