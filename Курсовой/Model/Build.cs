namespace Курсовой.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BuildEntities : DbContext
    {
        public BuildEntities()
            : base("name=BuildEntities")
        {
        }

        public virtual DbSet<Elements> Elements { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserProject> UserProject { get; set; }
        public virtual DbSet<WorkField> WorkField { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Elements>()
                .Property(e => e.Price)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Elements>()
                .HasMany(e => e.WorkField)
                .WithOptional(e => e.Elements)
                .HasForeignKey(e => e.Element_ID);

            modelBuilder.Entity<UserProject>()
                .HasMany(e => e.WorkField)
                .WithOptional(e => e.UserProject)
                .HasForeignKey(e => e.ID_UP);
        }
    }
}
