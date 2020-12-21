using System;
using System.Collections.Generic;
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


namespace Курсовой.ViewModel
{
    class MainWindowViewModel:INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            MainPage = "HomePage.xaml";
            DBRepository<User> dBRepository = new DBRepository<User>(new BuildEntities());
            User user=dBRepository.GetAll().Where(s => s.ID_User == CurrentUserID.getInstance().ID).First();
            LoginCurrenUser = user.Login;
            if (user.Language == null || user.Language == "English")
            {
                ResourceDictionary resourceDict = Application.LoadComponent(new Uri("/Resources/EngLanguage.xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary;
                Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            }
            else
            {
                ResourceDictionary resourceDict = Application.LoadComponent(new Uri("/Resources/RuLanguage.xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary;
                Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            }
            if (user.Them == null || user.Them == "Default")
            {
                ResourceDictionary resourceDict = Application.LoadComponent(new Uri("/Resources/DefaultThem.xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary;
                Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            }
            else
            {
                ResourceDictionary resourceDict = Application.LoadComponent(new Uri("/Resources/BlackThem.xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary;
                Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            }
            FirstCharLoginCurrenUser = user.Name.ToUpper().First().ToString()+user.Surname.ToUpper().First().ToString();
            dBRepository.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private string loginCurrenUser;
        public string LoginCurrenUser
        {
            get
            {
                return loginCurrenUser;
            }
            set
            {
                loginCurrenUser = value;
                OnPropertyChanged();
            }
        }

        private string firstCharLoginCurrenUser;
        public string FirstCharLoginCurrenUser
        {
            get
            {
                return firstCharLoginCurrenUser;
            }
            set
            {
                firstCharLoginCurrenUser = value;
                OnPropertyChanged();
            }
        }


        private string mainPage;
        public string MainPage
        {
            get
            {
                return mainPage;
            }
            set
            {
                mainPage = value;
                OnPropertyChanged();
            }
        }

        private int minWidthWindow=700;
        public int MinWidthWindow
        {
            get
            {
                return minWidthWindow;
            }
            set
            {
                minWidthWindow = value;
                OnPropertyChanged();
            }
        }

        private int widthWindow=1200;
        public int WidthWindow
        {
            get
            {
                return widthWindow;
            }
            set
            {
                widthWindow = value;
                OnPropertyChanged();
            }
        }

        private GridLength leftPanelWidth;
        public GridLength LeftPanelWidth
        {
            get
            {
                return leftPanelWidth;
            }
            set
            {
                leftPanelWidth = value;
                OnPropertyChanged();
            }
        }

        private Visibility closePanelButtonVisible = Visibility.Visible;
        public Visibility ClosePanelButtonVisible
        {
            get { return closePanelButtonVisible; }
            set{ closePanelButtonVisible = value;OnPropertyChanged();}
        }

        private Visibility burgerButtonVisible = Visibility.Collapsed;
        public Visibility BurgerButtonVisible
        {
            get { return burgerButtonVisible; }
            set{burgerButtonVisible = value;OnPropertyChanged();}
        }

        public ICommand HomeClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    WidthWindow = 1200;
                    MinWidthWindow = 950;
                    MainPage = "HomePage.xaml";
                });
            }
        }

        public ICommand SettingsClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    MinWidthWindow = 800;
                    MainPage = "SettingsPage.xaml";
                });
            }
        }

        public ICommand InfoClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    MinWidthWindow = 800;
                    MainPage = "InfoPage.xaml";
                });
            }
        }

        public ICommand BurgerButtonClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    LeftPanelWidth = new GridLength(300);
                    BurgerButtonVisible = Visibility.Collapsed;
                    ClosePanelButtonVisible = Visibility.Visible;
                });
            }
        }

        public ICommand CloseButtonClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    if (MainPage == "WorkSpace.xaml")
                    {
                        LeftPanelWidth = new GridLength(0);
                        BurgerButtonVisible = Visibility.Visible;
                        ClosePanelButtonVisible = Visibility.Collapsed;
                    }
                    else
                    {
                        LeftPanelWidth = new GridLength(100);
                        BurgerButtonVisible = Visibility.Visible;
                        ClosePanelButtonVisible = Visibility.Collapsed;
                    }
                });
            }
        }

        public ICommand LogoffClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    CurrentUserID.LogOut();
                    SignInWindow signIn = new SignInWindow();
                    signIn.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    signIn.Show();
                    (obj as Window).Close();
                });
            }
        }

    }
}
