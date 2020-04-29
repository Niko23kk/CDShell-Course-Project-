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
    class NewTextureViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public NewTextureViewModel()
        {

        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        private string info;
        public string Info
        {
            get { return info; }
            set
            {
                info = value;
                OnPropertyChanged();
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        private string price;
        public string Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged();
            }
        }

        private string sezeX;
        public string SizeX
        {
            get { return sezeX; }
            set
            {
                SizeX = value;
                OnPropertyChanged();
            }
        }

        private string sezeY;
        public string SizeY
        {
            get { return sezeY; }
            set
            {
                SizeY = value;
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
