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
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace Курсовой.ViewModel
{
    class SettingsPageViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private string language="";
        public string Language
        {
            get
            {
                return language;
            }
            set
            {
                language = value;
                OnPropertyChanged();
            }
        }

        private string them="";
        public string Them
        {
            get
            {
                return them;
            }
            set
            {
                them = value;
                OnPropertyChanged();
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        private string newpassword;
        public string NewPassword
        {
            get
            {
                return newpassword;
            }
            set
            {
                newpassword = value;
                OnPropertyChanged();
            }
        }

        private string avatar;
        public string Avatar
        {
            get
            {
                return avatar;
            }
            set
            {
                avatar = value;
                OnPropertyChanged();
            }
        }

        public ICommand ChooseAvatar
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Filter = "JPG Format (*.jpg)|*.jpg|PNG Format (*.png)|*.png";

                    if (openFile.ShowDialog() == true)
                    {
                        Avatar = openFile.FileName;
                    }
                });
            }
        }

        public ICommand SaveClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    try
                    {
                        DBRepository<User> dBRepository = new DBRepository<User>(new BuildEntities());
                        User user = dBRepository.GetAll().Where(s => s.ID_User == CurrentUserID.getInstance(0).ID).First();
                        if (Language.Length > 0)
                        {
                            user.Language = Language;
                        }
                        if (Them.Length > 0)
                        {
                            user.Them = Them;
                        }
                        if (Password != null && NewPassword != null)
                        {
                            if (Password.Length > 0 && NewPassword.Length > 0)
                            {
                                SHA1 sHA1 = new SHA1CryptoServiceProvider();
                                if (user.Password.SequenceEqual(sHA1.ComputeHash(Encoding.ASCII.GetBytes(Password))))
                                {
                                    if (NewPassword.Length >= 5)
                                        user.Password = sHA1.ComputeHash(Encoding.ASCII.GetBytes(NewPassword));
                                    else
                                        MessageBox.Show("Password must be at least 5 characters");
                                }
                                else
                                    MessageBox.Show("Invalid password");
                            }
                        }
                        dBRepository.Update(user);
                        if (Avatar != "")
                        {
                            dBRepository.UpdateAvatar(user, Avatar);
                        }
                        MessageBox.Show("Data was saved");
                    }
                    catch
                    {
                        MessageBox.Show("Ops error, sory =(");
                    }

                });
            }
        }

            
    }
}
