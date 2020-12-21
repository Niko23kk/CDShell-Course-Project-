namespace Курсовой.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Elements
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Elements()
        {
            WorkField = new HashSet<WorkField>();
        }

        public int ID { get; set; }

        [StringLength(30)]
        public string Title { get; set; }

        [StringLength(20)]
        public string Type { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Price { get; set; }

        [Column("Front view", TypeName = "image")]
        [MaxLength]
        public byte[] Front_view { get; set; }

        [Column("Side view", TypeName = "image")]
        [MaxLength]
        public byte[] Side_view { get; set; }

        public int? Size { get; set; }

        [Column("ID User")]
        public int? ID_User { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkField> WorkField { get; set; }
    }
}
