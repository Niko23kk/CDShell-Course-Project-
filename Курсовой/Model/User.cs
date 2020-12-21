namespace Курсовой.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Elements = new HashSet<Elements>();
            UserProject = new HashSet<UserProject>();
        }

        [Key]
        [Column("ID User")]
        public int ID_User { get; set; }

        [StringLength(25)]
        public string Name { get; set; }

        [StringLength(25)]
        public string Surname { get; set; }

        [StringLength(30)]
        public string Email { get; set; }

        [StringLength(25)]
        public string Login { get; set; }

        [MaxLength(20)]
        public byte[] Password { get; set; }

        [StringLength(20)]
        public string Language { get; set; }

        [StringLength(20)]
        public string Them { get; set; }

        [StringLength(10)]
        public string Type { get; set; }

        [Column(TypeName = "image")]
        [MaxLength]
        public byte[] Photo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Elements> Elements { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserProject> UserProject { get; set; }
    }
}
