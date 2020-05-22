using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Курсовой.Model;
using Курсовой.Classes;
using Курсовой.ViewModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Курсовой.View;

namespace Курсовой.ViewModel
{
    class RegistrateViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        
        private string firstName="";
        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (value.Length <= 25)
                {
                    firstName = value;
                    OnPropertyChanged();
                }
                else
                    CustomMessageBox.Show("Error", "Check your First name (max lenght 25)", MessageBoxButton.OK);
            }
        }

        private string secondName="";
        public string SecondName
        {
            get { return secondName; }
            set
            {
                if (value.Length <= 25)
                {
                    secondName = value;
                    OnPropertyChanged();
                }
                else
                    CustomMessageBox.Show("Error", "Check your Second name (max lenght 25)", MessageBoxButton.OK);
            }
        }

        private string email="";
        public string Email
        {
            get { return email; }
            set
            {
                if (value.Length <= 30)
                {
                    email = value;
                    OnPropertyChanged();
                }
                else
                    CustomMessageBox.Show("Error", "Check your email (max lenght 30)", MessageBoxButton.OK);
            }
        }

        private string login="";
        public string Login
        {
            get { return login; }
            set
            {
                if (value.Length <= 25)
                {
                    login = value;
                    OnPropertyChanged();
                }
                else
                    CustomMessageBox.Show("Error", "Check your Login (max lenght 25)", MessageBoxButton.OK);
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                try
                {
                    if (value.Length > 0)
                    {
                        foreach (var item in Application.Current.Windows)
                        {
                            ((((item as Window).Content as Frame).Content as Page).FindName("Password") as PasswordBox).Tag = "";
                        }
                    }
                    password = value;
                    OnPropertyChanged();
                }
                catch { }
            }
        }

        private string conpassword;
        public string ConPassword
        {
            get { return conpassword; }
            set
            {
                try
                {
                    if (value.Length > 0)
                    {
                        foreach (var item in Application.Current.Windows)
                        {
                            ((((item as Window).Content as Frame).Content as Page).FindName("ConfirmPassword") as PasswordBox).Tag = "";
                        }
                    }
                    conpassword = value;
                    OnPropertyChanged();
                }
                catch { }
            }
        }

        public ICommand SignInClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    (obj as Page).NavigationService.Navigate(new Uri("pack://application:,,,/View/SignIn.xaml"), UriKind.RelativeOrAbsolute);
                });
            }
        }

        public ICommand RegistrationClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    bool check = true;
                    if (FirstName.Length == 0)
                    {
                        CustomMessageBox.Show("Error", "Input your First name", MessageBoxButton.OK);
                        check = false;
                    }
                    if (SecondName.Length == 0)
                    {
                        CustomMessageBox.Show("Error", "Input your Second name", MessageBoxButton.OK);
                        check = false;
                    }
                    if (!Regex.IsMatch(Email, @"^[-a-z0-9!#$%&'*+/=?^_`{|}~]+(\.[-a-z0-9!#$%&'*+/=?^_`{|}~]+)*@([a-z0-9]([-a-z0-9]{0,61}[a-z0-9])?\.)"+
                        "*(aero|arpa|asia|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel|[a-z][a-z])$"))
                    {
                        CustomMessageBox.Show("Error", "Uncorrectly format in email", MessageBoxButton.OK);
                        check = false;
                    }
                    if (Login.Length == 0)
                    {
                        CustomMessageBox.Show("Error", "Input your login", MessageBoxButton.OK);
                        check = false;
                    }
                    try
                    {
                        if (Password.Length <5)
                        {
                            CustomMessageBox.Show("Error", "Check your Password (minimal lenght 5)", MessageBoxButton.OK);
                            check = false;
                        }
                        if (ConPassword.Length <5)
                        {
                            CustomMessageBox.Show("Error", "Check you Confirm Password (minimal lenght 5)", MessageBoxButton.OK);
                            check = false;
                        }
                    }
                    catch
                    {
                        CustomMessageBox.Show("Error", "Input Password and Confirm Password", MessageBoxButton.OK);
                    }
                    if (Password != ConPassword)
                    {
                        CustomMessageBox.Show("Error", "Password and Confirm Password are different", MessageBoxButton.OK);
                        check = false;
                    }


                    if (check)
                    {
                        DBRepository<User> dBRepository = new DBRepository<User>(new BuildEntities());
                        SHA1 sHA1 = new SHA1CryptoServiceProvider();
                        User user = new User
                        {
                            Name = FirstName,
                            Surname = SecondName,
                            Login = Login,
                            Email = Email,
                            Password = sHA1.ComputeHash(Encoding.ASCII.GetBytes(Password)),
                            Type="User"
                        };
                        try
                        {
                            dBRepository.Create(user);
                            CustomMessageBox.Show("Event", "Registration was successful", MessageBoxButton.OK);
                            (obj as Page).NavigationService.Navigate(new Uri("pack://application:,,,/View/SignIn.xaml"), UriKind.RelativeOrAbsolute);
                        }
                        catch
                        {
                            CustomMessageBox.Show("Error", "This Login is busy", MessageBoxButton.OK);
                        }
                        dBRepository.Dispose();

                    }
                });
            }
        }

    }
}
