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
        public virtual DbSet<Alumno> Alumno { get; set; }
        public virtual DbSet<Curso> Curso { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adjunto>()
                .Property(e => e.Archivo)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.Apellido)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.Rut)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.Peso);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.Edad);

            modelBuilder.Entity<Alumno>()
                .HasMany(e => e.Adjunto)
                .WithRequired(e => e.Alumno)
                .HasForeignKey(e => e.Alumno_id);

            modelBuilder.Entity<Alumno>()
                .HasMany(e => e.Curso)
                .WithMany(e => e.Alumno)
                .Map(m => m.ToTable("AlumnoCurso"));

            modelBuilder.Entity<Curso>()
                .Property(e => e.Nombre)
                .IsUnicode(false);
        }
    }
}
