namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Curso")]
    public partial class Curso
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Curso()
        {
            Alumno = new HashSet<Alumno>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Alumno> Alumno { get; set; }

        public List<Curso> Todo()
        {
            var cursos = new List<Curso>();
            try
            {
                using (var context = new TestContext())
                {
                    cursos = context.Curso.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return cursos;
        }


    }
}
