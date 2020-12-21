using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Курсовой.Model;
using Курсовой.Classes;
using Курсовой.View;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Xml.Serialization;

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
            User user= dBUser.GetAll().Where(s=>s.ID_User==CurrentUserID.getInstance().ID).First();
            FirstName = user.Name;
            SecondName = user.Surname;
            Login = user.Login;
            Email = user.Email;
            dBUser.Dispose();
            if (user.Photo != null)
            {
                MemoryStream strmImg = new MemoryStream(user.Photo);
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.StreamSource = strmImg;
                myBitmapImage.DecodePixelWidth = 200;
                myBitmapImage.EndInit();
                Avatar = myBitmapImage;
            }

            DBRepository<UserProject> dBUserProject = new DBRepository<UserProject>(new BuildEntities());
            DBRepository<Project> dBProject = new DBRepository<Project>(new BuildEntities());
            var userProjects = dBUserProject.GetAll().Where(s => s.ID_User == CurrentUserID.getInstance().ID);
            ObservableCollection<Project> projectsCurentUserList=new ObservableCollection<Project>();
            foreach (var item in dBProject.GetAll().OrderByDescending(s=>s.Date_of_change))
            {
                foreach (var item1 in userProjects)
                {
                    if (item1.ID_Project == item.ID_Project)
                    {
                        ProjectsCurentUser.Add(item);
                    }
                }
            }
            dBUserProject.Dispose();
            dBProject.Dispose();
        }

        private ObservableCollection<Project> projectsCurentUser = new ObservableCollection<Project>();
        public ObservableCollection<Project> ProjectsCurentUser
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

        private BitmapImage avatar=new BitmapImage(new Uri("pack://application:,,,/Resources/Logo.png"));
        public BitmapImage Avatar
        {
            get { return avatar; }
            set
            {
                avatar = value;
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
                    window.ShowDialog();
                });
            }
        }

        public ICommand NewProjectClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    try
                    {
                        var windows = Application.Current.Windows;
                        MainWindowViewModel t=null;
                        foreach (var item in windows)
                        {
                            t = ((item as Window).DataContext as MainWindowViewModel);
                        }
                        t.MainPage = "WorkSpace.xaml";
                        t.ClosePanelButtonVisible = Visibility.Collapsed;
                        t.BurgerButtonVisible = Visibility.Visible;
                        t.LeftPanelWidth = new GridLength(0);
                    }
                    catch
                    {
                        CustomMessageBox.Show("Error", "Error =(", MessageBoxButton.OK);
                    }
                });
            }
        }

        public ICommand OpenProject
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    var windows = Application.Current.Windows;
                    Loading loadingwin = new Loading("Waiting...");
                    loadingwin.Show();
                    loadingwin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    MainWindowViewModel t = null;
                    foreach (var item in windows)
                    {
                        t = ((item as Window).DataContext as MainWindowViewModel);
                    }
                    DBRepository<UserProject> dBRepository = new DBRepository<UserProject>(new BuildEntities());
                    CurrentUserID.ProjectID = dBRepository.GetAll().Where(s=>s.ID_Project== ((obj as FrameworkElement).DataContext as Project).ID_Project).First().ID;
                    t.MainPage = "WorkSpace.xaml";
                    t.ClosePanelButtonVisible = Visibility.Collapsed;
                    t.BurgerButtonVisible = Visibility.Visible;
                    t.LeftPanelWidth = new GridLength(0);
                    dBRepository.Dispose();
                });
            }
        }

        public ICommand DeleteProject
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    if (CustomMessageBox.Show("Delete", "Delete project?", MessageBoxButton.OK)==MessageBoxResult.Yes)
                    {
                        DBRepository<UserProject> dBUP = new DBRepository<UserProject>(new BuildEntities());
                        DBRepository<Project> dBProject = new DBRepository<Project>(new BuildEntities());
                        DBRepository<WorkField> dbWorkField = new DBRepository<WorkField>(new BuildEntities());
                        try
                        {
                            UserProject time = dBUP.GetAll().First(s => s.ID_Project == ((obj as FrameworkElement).DataContext as Project).ID_Project);
                            dbWorkField.RemoveCollection(dbWorkField.GetAll().Where(s => s.ID_UP == time.ID));
                            dBUP.Remove(time);
                            var time1 = dBProject.GetAll().First(s => s.ID_Project == time.ID_Project);
                            dBProject.Remove(time1);
                            ProjectsCurentUser.Clear();

                            var userProjects = dBUP.GetAll().Where(s => s.ID_User == CurrentUserID.getInstance().ID);
                            foreach (var item in dBProject.GetAll().OrderByDescending(s => s.Date_of_change))
                            {
                                foreach (var item1 in userProjects)
                                {
                                    if (item1.ID_Project == item.ID_Project)
                                    {
                                        ProjectsCurentUser.Add(item);
                                    }
                                }
                            }
                            dBUP.Dispose();
                            dBProject.Dispose();
                            dbWorkField.Dispose();
                        }
                        catch
                        { }
                    }
                });
            }
        }

        public ICommand LoadProject
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    try
                    {
                        OpenFileDialog openFile = new OpenFileDialog
                        {
                            Filter = "XML Format (*.xml)|*.xml"
                        };

                        if (openFile.ShowDialog() == true)
                        {
                            try
                            {
                                List<(int? ID_UP, int? Element_ID, int? PositionX, int? PositionY, int? Rotate)> upload = new List<(int? ID_UP, int? Element_ID, int? PositionX, int? PositionY, int? Rotate)>();
                                XmlSerializer serializer = new XmlSerializer(typeof(List<(int? ID_UP, int? Element_ID, int? PositionX, int? PositionY, int? Rotate)>));
                                StreamReader file = new StreamReader(openFile.FileName);
                                upload = (List<(int? ID_UP, int? Element_ID, int? PositionX, int? PositionY, int? Rotate)>)serializer.Deserialize(file);
                                List<WorkField> save = new List<WorkField>();

                                DBRepository<Project> dbProject = new DBRepository<Project>(new BuildEntities());
                                DBRepository<UserProject> dbUP = new DBRepository<UserProject>(new BuildEntities());
                                DBRepository<WorkField> dbField = new DBRepository<WorkField>(new BuildEntities());
                                Project project = new Project
                                {
                                    Project_name = openFile.FileName.Substring(openFile.FileName.ToString().LastIndexOf(@"\") + 1, openFile.FileName.Length - openFile.FileName.ToString().LastIndexOf('.') - 1),
                                    Date_of_change = DateTime.Now,
                                    Status = 0
                                };
                                dbProject.Create(project);

                                UserProject userProject = new UserProject
                                {
                                    ID_User = CurrentUserID.getInstance().ID,
                                    ID_Project = dbProject.GetAll().OrderBy(s => s.Date_of_change).Last().ID_Project
                                };
                                dbUP.Create(userProject);

                                foreach (var item in upload)
                                {
                                    save.Add(new WorkField
                                    {
                                        ID_UP = dbUP.GetAll().OrderBy(s => s.ID).Last().ID,
                                        Element_ID = item.Element_ID,
                                        PositionX = item.PositionX,
                                        PositionY = item.PositionY,
                                        Rotate = item.Rotate
                                    });
                                }

                                dbField.AddCollection(save);
                                ProjectsCurentUser.Clear();
                                var userProjects = dbUP.GetAll().Where(s => s.ID_User == CurrentUserID.getInstance().ID);
                                ObservableCollection<Project> projectsCurentUserList = new ObservableCollection<Project>();
                                foreach (var item in dbProject.GetAll().OrderByDescending(s => s.Date_of_change))
                                {
                                    foreach (var item1 in userProjects)
                                    {
                                        if (item1.ID_Project == item.ID_Project)
                                        {
                                            ProjectsCurentUser.Add(item);
                                        }
                                    }
                                }
                                CustomMessageBox.Show("Event", "Project was loaded", MessageBoxButton.OK);
                            }
                            catch { }
                        }
                    }
                    catch
                    {
                        CustomMessageBox.Show("Error", "Error =(", MessageBoxButton.OK);
                    }
                });
            }
        }

    }
}
