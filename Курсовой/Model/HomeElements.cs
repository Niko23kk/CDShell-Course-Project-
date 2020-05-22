using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовой.Model
{
    [Serializable]
    public class HomeElements
    {
        public WorkField Field { get; set; }
        public Elements Element { get; set; }
        public HomeElements(WorkField field,Elements element)
        {
            Field = field;
            Element = element;
        }
    }
}
