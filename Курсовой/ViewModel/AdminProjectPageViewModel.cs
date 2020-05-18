using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Курсовой.Model;
using Курсовой.Clases;
using Курсовой.View;
using Курсовой.ViewModel;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Win32;

namespace Курсовой.ViewModel
{
    class AdminProjectPageViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        
        private ObservableCollection<UserProj> projects = new ObservableCollection<UserProj>();
        public ObservableCollection<UserProj> Projects
        {
            get { return projects; }
            set { projects = value; OnPropertyChanged(); }
        }

        public AdminProjectPageViewModel()
        {
            DBRepository<Project> dBProject = new DBRepository<Project>(new BuildEntities());
            DBRepository<User> dBUser = new DBRepository<User>(new BuildEntities());
            DBRepository<UserProject> dbUP = new DBRepository<UserProject>(new BuildEntities());
            foreach (var item in dbUP.GetAll())
            {
                Projects.Add(new UserProj(dBUser.GetAll().First(s=>s.ID_User==item.ID_User),dBProject.GetAll().First(s=>s.ID_Project==item.ID_Project)));
            }
            var windows = Application.Current.Windows;
            if (windows.Count > 1)
            {
                Window window = null;
                try
                {
                    foreach (var item in windows)
                    {
                        if (item as Loading != null)
                        {
                            window = (item as Window);
                        }
                    }
                }
                catch { }
                window.Close();
            }
            dBProject.Dispose();
            dBUser.Dispose();
            dbUP.Dispose();
        }

        public AdminProjectPageViewModel(User user)
        {
            CurUser = user;
            DBRepository<Project> dBProject = new DBRepository<Project>(new BuildEntities());
            DBRepository<UserProject> dbUP = new DBRepository<UserProject>(new BuildEntities());
            foreach (var item in dbUP.GetAll().Where(s=>s.ID_User==user.ID_User))
            {
                Projects.Add(new UserProj(user, dBProject.GetAll().First(s => s.ID_Project == item.ID_Project)));
            }
            dBProject.Dispose();
            dbUP.Dispose();
        }

        private User CurUser;

        private string find;
        public string Find
        {
            get
            {
                return find;
            }
            set
            {
                find = value;
                OnPropertyChanged();
                Filter();
            }
        }

        private void Filter()
        {
            Projects.Clear();
            DBRepository<Project> dBProject = new DBRepository<Project>(new BuildEntities());
            DBRepository<User> dBUser = new DBRepository<User>(new BuildEntities());
            DBRepository<UserProject> dbUP = new DBRepository<UserProject>(new BuildEntities());
            if (CurUser == null)
            {
                foreach (var item in dbUP.GetAll().ToList())
                {
                    var user = dBUser.GetAll().First(s=>s.ID_User==item.ID_User);
                    var project = dBProject.GetAll().First(s => s.ID_Project == item.ID_Project);
                    if (user.Login.Contains(Find) || project.ID_Project.ToString().Contains(Find) || project.Project_name.Contains(Find) || project.Date_of_change.ToString().Contains(find))
                    {
                        Projects.Add(new UserProj(dBUser.GetAll().First(s => s.ID_User == item.ID_User), dBProject.GetAll().First(s => s.ID_Project == item.ID_Project)));
                    }
                }
            }
            else
            {
                foreach (var item in dbUP.GetAll().Where(s=>s.ID_User==CurUser.ID_User))
                {
                    var project = dBProject.GetAll().First(s => s.ID_Project == item.ID_Project);
                    if (CurUser.Login.Contains(Find) || project.ID_Project.ToString().Contains(Find) || project.Project_name.Contains(Find) || project.Date_of_change.ToString().Contains(find))
                    {
                        Projects.Add(new UserProj(dBUser.GetAll().First(s => s.ID_User == item.ID_User), dBProject.GetAll().First(s => s.ID_Project == item.ID_Project)));
                    }
                }
            }
            dBProject.Dispose();
            dBUser.Dispose();
            dbUP.Dispose();
        }
        
        public ICommand SaveAs
        {
            get
            {
                return new DelegateCommand(obj => {
                    SaveFileDialog openFile = new SaveFileDialog
                    {
                        Filter = "XML Format (*.xml)|*.xml"
                    };

                    if (openFile.ShowDialog() == true)
                    {
                        try
                        {
                            using (var context = new BuildEntities())
                            {
                                List<(int? ID_UP, int? Element_ID, int? PositionX, int? PositionY, int? Rotate)> save = new List<(int? ID_UP, int? Element_ID, int? PositionX, int? PositionY, int? Rotate)>();
                                DBRepository<WorkField> dBFiled = new DBRepository<WorkField>(new BuildEntities());
                                DBRepository<UserProject> dBUP = new DBRepository<UserProject>(new BuildEntities());
                                foreach (var item in dBFiled.GetAll().Where(s=>s.ID_UP==dBUP.GetAll().First(c=>c.ID_Project== ((obj as FrameworkElement).DataContext as UserProj).Project.ID_Project).ID))
                                {
                                    save.Add((item.ID_UP, item.Element_ID, item.PositionX, item.PositionY, item.Rotate));
                                }
                                XmlSerializer serializer = new XmlSerializer(typeof(List<(int? ID_UP, int? Element_ID, int? PositionX, int? PositionY, int? Rotate)>));
                                StreamWriter file = new StreamWriter(openFile.FileName);
                                serializer.Serialize(file, save);
                                MessageBox.Show("Project save");
                                dBFiled.Dispose();
                                dBUP.Dispose();
                            }
                        }
                        catch { }
                    }
                });
            }
        }

        public ICommand DeleteProject
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    try
                    {
                        DBRepository<Project> dBProject = new DBRepository<Project>(new BuildEntities());
                        DBRepository<User> dBUser = new DBRepository<User>(new BuildEntities());
                        DBRepository<UserProject> dbUP = new DBRepository<UserProject>(new BuildEntities());
                        dbUP.Remove(dbUP.GetAll().First(s=>s.ID_Project== ((obj as FrameworkElement).DataContext as UserProj).Project.ID_Project));
                        dBProject.Remove(((obj as FrameworkElement).DataContext as UserProj).Project);
                        Projects.Clear();
                        if (CurUser == null)
                        {
                            foreach (var item in dbUP.GetAll())
                            {
                                Projects.Add(new UserProj(dBUser.GetAll().First(s => s.ID_User == item.ID_User), dBProject.GetAll().First(s => s.ID_Project == item.ID_Project)));
                            }
                        }
                        MessageBox.Show("Project has been deleted");
                        dBProject.Dispose();
                        dbUP.Dispose();
                        dBUser.Dispose();
                    }
                    catch
                    { }
                });
            }
        }

    }
}
