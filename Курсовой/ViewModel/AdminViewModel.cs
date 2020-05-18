using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using Курсовой.Clases;
using Курсовой.Model;
using Курсовой.View;

namespace Курсовой.ViewModel
{
    class AdminViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
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
            set { closePanelButtonVisible = value; OnPropertyChanged(); }
        }

        private Visibility burgerButtonVisible = Visibility.Collapsed;
        public Visibility BurgerButtonVisible
        {
            get { return burgerButtonVisible; }
            set { burgerButtonVisible = value; OnPropertyChanged(); }
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
                    LeftPanelWidth = new GridLength(100);
                    BurgerButtonVisible = Visibility.Visible;
                    ClosePanelButtonVisible = Visibility.Collapsed;
                });
            }
        }

        public ICommand UserClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    Loading loadingwin = new Loading("Waiting...");
                    loadingwin.Show();
                    loadingwin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    MainPage = "AdminUserPage.xaml";
                });
            }
        }

        public ICommand ProjectsClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    Loading loadingwin = new Loading("Waiting...");
                    loadingwin.Show();
                    loadingwin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    MainPage = "AdminProjectPage.xaml";
                });
            }
        }

        public ICommand ElementsClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    Loading loadingwin = new Loading("Waiting...");
                    loadingwin.Show();
                    loadingwin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    MainPage = "AdminElementsPage.xaml";
                });
            }
        }

        public ICommand SettingsClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    MainPage = "SettingsPage.xaml";
                });
            }
        }

        public ICommand LogoffClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    SignInWindow signIn = new SignInWindow();
                    signIn.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    signIn.Show();
                    CurrentUserID.LogOut();
                    (obj as Window).Close();
                });
            }
        }
    }
}
