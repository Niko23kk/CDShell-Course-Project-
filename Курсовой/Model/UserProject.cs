namespace Курсовой.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserProject")]
    public partial class UserProject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserProject()
        {
            WorkField = new HashSet<WorkField>();
        }

        public int ID { get; set; }

        [Column("ID User")]
        public int? ID_User { get; set; }

        [Column("ID Project")]
        public int? ID_Project { get; set; }

        public virtual Project Project { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkField> WorkField { get; set; }
    }
}
