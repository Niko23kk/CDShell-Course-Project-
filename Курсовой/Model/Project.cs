namespace Курсовой.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Project")]
    public partial class Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {
            UserProject = new HashSet<UserProject>();
        }

        [Key]
        [Column("ID Project")]
        public int ID_Project { get; set; }

        [Column("Date of change")]
        public DateTime? Date_of_change { get; set; }

        [Column("Project name")]
        [StringLength(25)]
        public string Project_name { get; set; }

        public int? Status { get; set; }

        [Column(TypeName = "image")]
        [MaxLength]
        public byte[] Preview { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserProject> UserProject { get; set; }
    }
}
