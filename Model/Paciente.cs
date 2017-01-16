namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Data.SqlClient;
    using System.Linq;

    [Table("Paciente")]
    public partial class Paciente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Paciente()
        {
            Adjunto = new HashSet<Adjunto>();
            Examen = new List<Examen>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        [StringLength(50)]
        public string Rut { get; set; }

        public int? Peso { get; set; }

        public int? Edad { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Adjunto> Adjunto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Examen> Examen { get; set; }

        public List<Paciente> Listar()
        {
            var paciente = new List<Paciente>();
            try
            {
                using (var context = new TestContext())
                {
                    paciente = context.Paciente.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return paciente;
        }

        public Paciente Obtener(int id)
        {
            var paciente = new Paciente();
            try
            {
                using (var context = new TestContext())
                {
                    paciente = context.Paciente.Include("Examen").Where(x => x.id == id).Single();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return paciente;

        }

        public void Eliminar(int id)
        {

            try
            {
                using (var context = new TestContext())
                {
                    context.Entry(new Paciente { id = id }).State = EntityState.Deleted;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Guardar()
        {

            try
            {
                using (var context = new TestContext())
                {
                    if (this.id == 0)
                    {
                        context.Entry(this).State = EntityState.Added;

                    }

                    else
                    {
                        context.Database.ExecuteSqlCommand(
                            "DELETE FROM PacienteExamen Where Paciente_id = @id",
                            new SqlParameter("id", this.id)
                            );

                        var examenBK = this.Examen;
                        this.Examen = null;
                        context.Entry(this).State = EntityState.Modified;
                        this.Examen = examenBK;

                    }

                    foreach (var c in this.Examen)
                        context.Entry(c).State = EntityState.Unchanged;
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
