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
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace Курсовой.ViewModel
{
    class RecoveryViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public RecoveryViewModel(User user,int code)
        {
            MCode = code;
            CurUser = user;
        }

        int MCode;
        User CurUser;

        private string code;
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
                OnPropertyChanged();
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

        public ICommand Change
        {
            get
            {
                return new DelegateCommand(obj => {
                    try
                    {
                        if (Convert.ToUInt32(Code) == MCode)
                        {
                            bool check = true;
                            try
                            {
                                if (Password.Length < 5)
                                {
                                    CustomMessageBox.Show("Error", "Check your Password (minimal lenght 5)", MessageBoxButton.OK);
                                    check = false;
                                }
                                if (ConPassword.Length < 5)
                                {
                                    CustomMessageBox.Show("Error", "Check you Confirm Password (minimal lenght 5)", MessageBoxButton.OK);
                                    check = false;
                                }
                                if (check)
                                {
                                    if (Password != ConPassword)
                                    {
                                        CustomMessageBox.Show("Error", "Password and Confirm Password are different", MessageBoxButton.OK);
                                        return;
                                    }
                                    SHA1 sHA1 = new SHA1CryptoServiceProvider();
                                    CurUser.Password = sHA1.ComputeHash(Encoding.ASCII.GetBytes(Password));
                                    DBRepository<User> dBRepository = new DBRepository<User>(new BuildEntities());
                                    dBRepository.Update(CurUser);
                                    CustomMessageBox.Show("Event", "Password was changed", MessageBoxButton.OK);
                                    var window = Application.Current.Windows[Application.Current.Windows.Count - 1];
                                    (window as SignInWindow).Frame.Navigate(new SignIn());
                                    dBRepository.Dispose();
                                }   
                            }
                            catch
                            {
                                CustomMessageBox.Show("Error", "Input Password and Confirm Password", MessageBoxButton.OK);
                            }
                        }
                        else
                        {
                            CustomMessageBox.Show("Error", "Code is not right", MessageBoxButton.OK);
                        }
                    }
                    catch
                    {
                        CustomMessageBox.Show("Error", "Check you code", MessageBoxButton.OK);
                    };
                });
            }
        }
    }
}
