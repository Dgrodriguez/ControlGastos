using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;



namespace ControlGastos.Data
{
    public class ControlGastosContext : DbContext
    {
        public ControlGastosContext() : base("name=ControlGastosDB") { 
            Database.SetInitializer<ControlGastosContext>(null); // 🔹 Evita migraciones automáticas
        }
        public DbSet<TiposGasto> TipoGasto { get; set; }
        public DbSet<FondosMonetario> FondoMonetario { get; set; }
        public DbSet<TipoFondosMonetario> TipoFondoMonetario { get; set; }
        public DbSet<TipoDocumentoGasto> TipoDocumento { get; set; }
        public DbSet<Presupuesto> Presupuestos { get; set; }
        public DbSet<Gasto> Gasto { get; set; }
        public DbSet<GastosDetalle> GastosDetalle { get; set; }
        public DbSet<Deposito> Deposito { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TiposGasto>()
                .Property(t => t.FechaCreacion)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<FondosMonetario>()
                .Property(t => t.FechaCreacion)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Presupuesto>()
                .Property(t => t.FechaCreacion)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            modelBuilder.Entity<Gasto>()
                .Property(t => t.FechaCreacion)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            modelBuilder.Entity<GastosDetalle>()
                .Property(t => t.FechaCreacion)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            modelBuilder.Entity<Deposito>()
            .Property(t => t.FechaCreacion)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Conventions.Remove<ForeignKeyIndexConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


    }
}