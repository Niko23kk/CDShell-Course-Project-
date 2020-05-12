using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовой.Model
{
    class BuildHistory
    {
        public Stack<List<HomeElements>> History { get; private set; }
        public BuildHistory()
        {
            History = new Stack<List<HomeElements>>();
        }
    }
}
