using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Курсовой.Model;
using Курсовой.Clases;
using Курсовой.View;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace Курсовой.ViewModel
{
    class HomePageViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        
        public HomePageViewModel()
        {
            DBRepository<User> dBUser = new DBRepository<User>(new BuildEntities());
            User user= dBUser.GetAll().Where(s=>s.ID_User==CurrentUserID.getInstance(0).ID).First();
            FirstName = user.Name;
            SecondName = user.Surname;
            Login = user.Login;
            Email = user.Email;
            DBRepository<UserProject> dBUserProject = new DBRepository<UserProject>(new BuildEntities());
            DBRepository<Project> dBProject = new DBRepository<Project>(new BuildEntities());
            var userProjects = dBUserProject.GetAll().Where(s => s.ID_User == CurrentUserID.getInstance(0).ID);
            List<Project> projectsCurentUserList=new List<Project>();
            foreach (var item in dBProject.GetAll())
            {
                foreach (var item1 in userProjects)
                {
                    if (item1.ID_Project == item.ID_Project)
                    {
                        projectsCurentUserList.Add(item);
                    }
                }
            }
            ProjectsCurentUser = projectsCurentUserList;
        }

        private List<Project> projectsCurentUser = new List<Project>();
        public List<Project> ProjectsCurentUser
        {
            get { return projectsCurentUser; }
            set { projectsCurentUser = value;OnPropertyChanged(); }
        }


        private string firstName ;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged();
            }
        }

        private string secondName ;
        public string SecondName
        {
            get { return secondName; }
            set
            {
                secondName = value;
                OnPropertyChanged();
            }
        }

        private string email ;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        private string login = "";
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddTextureClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    NewTextureWindow window = new NewTextureWindow();
                    window.Show();
                });
            }
        }

    }
}
