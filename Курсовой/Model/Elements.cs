//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Курсовой.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Elements
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Elements()
        {
            this.WorkField = new HashSet<WorkField>();
        }
    
        public int ID { get; set; }
        public string TitleEng { get; set; }
        public string TypeEng { get; set; }
        public string TitleRu { get; set; }
        public string TypeRu { get; set; }
        public Nullable<decimal> Price { get; set; }
        public byte[] Front_view { get; set; }
        public byte[] Side_view { get; set; }
        public Nullable<int> Size { get; set; }
        public Nullable<int> ID_User { get; set; }
    
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkField> WorkField { get; set; }
    }
}
