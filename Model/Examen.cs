namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class Examen
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Examen()
        {
            Paciente = new HashSet<Paciente>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Paciente> Paciente { get; set; }

        public List<Examen> Todo()
        {
            var examenes = new List<Examen>();
            try
            {
                using (var context = new TestContext())
                {
                    examenes = context.Examen.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return examenes;
        }
    }
}
