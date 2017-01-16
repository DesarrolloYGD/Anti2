namespace Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TestContext : DbContext
    {
        public TestContext()
            : base("name=TestContext")
        {
        }

        public virtual DbSet<Adjunto> Adjunto { get; set; }
        public virtual DbSet<Examen> Examen { get; set; }
        public virtual DbSet<Paciente> Paciente { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adjunto>()
                .Property(e => e.Archivo)
                .IsUnicode(false);

            modelBuilder.Entity<Examen>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Paciente>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Paciente>()
                .Property(e => e.Apellido)
                .IsUnicode(false);

            modelBuilder.Entity<Paciente>()
                .Property(e => e.Rut)
                .IsUnicode(false);

            modelBuilder.Entity<Paciente>()
                .HasMany(e => e.Adjunto)
                .WithRequired(e => e.Paciente)
                .HasForeignKey(e => e.Alumno_id);

            modelBuilder.Entity<Paciente>()
                .HasMany(e => e.Examen)
                .WithMany(e => e.Paciente)
                .Map(m => m.ToTable("PacienteExamen"));
        }
    }
}
