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

    [Table("Alumno")]
    public partial class Alumno
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Alumno()
        {
            Adjunto = new HashSet<Adjunto>();
            Curso = new List<Curso>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        [Required]
        [StringLength(100)]
        public string Rut { get; set; }

        [Required]
        public int Peso { get; set; }

        [Required]
        public int Edad { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Adjunto> Adjunto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Curso> Curso { get; set; }

        public List<Alumno> Listar()
        {
            var alumno = new List<Alumno>();
            try
            {
                using(var context = new TestContext())
                {
                    alumno = context.Alumno.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return alumno;
        }

        public Alumno Obtener(int id)
        {
            var alumno = new Alumno();
            try
            {
                using (var context = new TestContext())
                {
                    alumno = context.Alumno.Include("Curso").Where(x => x.id == id).Single();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return alumno;

        }

        public void Eliminar(int id)
        {

            try
            {
                using (var context = new TestContext())
                {
                    context.Entry(new Alumno { id = id}).State = EntityState.Deleted;
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
                    if( this.id == 0)
                    {
                        context.Entry(this).State = EntityState.Added;
                       
                    }

                    else
                    {
                        context.Database.ExecuteSqlCommand(
                            "DELETE FROM AlumnoCurso Where Alumno_id = @id", 
                            new SqlParameter("id", this.id)
                            );

                        var cursoBK = this.Curso;
                        this.Curso = null;
                        context.Entry(this).State = EntityState.Modified;
                        this.Curso = cursoBK;

                    }

                    foreach (var c in this.Curso)
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
