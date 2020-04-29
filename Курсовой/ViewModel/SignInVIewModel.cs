using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Курсовой.Model;
using Курсовой.Clases;
using Курсовой.ViewModel;
using System.Windows.Input;
using System.Windows;
using Курсовой.View;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Security.Cryptography;

namespace Курсовой.ViewModel
{
    class SignInViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        User user = new User();
        private string login="";
        public string Login
        {
            get { return login; }
            set
            {
                user.Login = value;
                login = value;
                OnPropertyChanged();

            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public ICommand SignInClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    bool check = true;
                    if (Login.Length == 0)
                    {
                        MessageBox.Show("Input your Login");
                        check = false;
                    }
                    if (Password.Length <= 5)
                    {
                        MessageBox.Show("Input your Password (minimal lenght 5)");
                    }
                    if (check)
                    {
                        bool find = false;
                        SHA1 sHA1 = new SHA1CryptoServiceProvider();
                        DBRepository<User> db = new DBRepository<User>(new BuildEntities());
                        User user = db.GetAll().Where(s => s.Login == Login).First();
                        if (user != null)
                        {
                            if (user.Password.SequenceEqual(sHA1.ComputeHash(Encoding.ASCII.GetBytes(Password))))
                            {
                                CurrentUserID userID = CurrentUserID.getInstance(user.ID_User);
                                find = true;
                                MainWindow mainWindow = new MainWindow();
                                mainWindow.Show();
                                Application.Current.MainWindow.Close();
                            }

                            if (!find)
                                MessageBox.Show("We not found account =(");
                        }
                    }
                });
            }
        }

        public ICommand RegistrationClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    (obj as Page).NavigationService.Navigate(new Uri("pack://application:,,,/View/Registration.xaml"), UriKind.RelativeOrAbsolute);
                });
            }
        }
    }
}
