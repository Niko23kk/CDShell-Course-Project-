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

    public class UserProj
    {
        public User User { get; set; }
        public Project Project { get; set; }

        public UserProj()
        { }

        public UserProj(User user, Project project)
        {
            User = user;
            Project = project;
        }
    }
}
