using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовой.Model
{
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
