namespace Курсовой.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WorkField")]
    public partial class WorkField
    {
        public int ID { get; set; }

        [Column("ID UP")]
        public int? ID_UP { get; set; }

        [Column("Element ID")]
        public int? Element_ID { get; set; }

        public int? PositionX { get; set; }

        public int? PositionY { get; set; }

        public int? Rotate { get; set; }

        public virtual Elements Elements { get; set; }

        public virtual UserProject UserProject { get; set; }
    }
}
