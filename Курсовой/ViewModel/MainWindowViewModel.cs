using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Курсовой.Model;
using Курсовой.Clases;
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
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop="")
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
                return new Com(obj =>
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
                return new Com(obj =>
                {
                    LeftPanelWidth = new GridLength(100);
                    BurgerButtonVisible = Visibility.Visible;
                    ClosePanelButtonVisible = Visibility.Collapsed;
                });
            }
        }

    }
}
