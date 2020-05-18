using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовой.Model
{
    class CurrentUserID
    {
        private static CurrentUserID instance;
        public int ID;
        public static int ProjectID;

        private CurrentUserID(int id)
        {
            ID = id;
        }

        public static CurrentUserID getInstance(int id=0)
        {
            if (instance == null)
                instance = new CurrentUserID(id);
            return instance;
        }

        public static void LogOut()
        {
            instance = null;
        }
    }
}
