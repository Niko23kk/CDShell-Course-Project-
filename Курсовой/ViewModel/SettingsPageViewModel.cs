using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Курсовой.Model;
using Курсовой.Classes;
using Курсовой.View;
using Курсовой.ViewModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.IO;

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
                foreach (var item in Application.Current.Windows)
                {
                    try
                    {
                        (((((item as Window).Content as Grid).FindName("Frame") as Frame).Content as Page).FindName("OldPassword") as PasswordBox).Tag = "";
                    }
                    catch { }
                }
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
                foreach (var item in Application.Current.Windows)
                {
                    try
                    {
                        (((((item as Window).Content as Grid).FindName("Frame") as Frame).Content as Page).FindName("NewPassword") as PasswordBox).Tag = "";
                    }
                    catch { }
                }
                newpassword = value;
                OnPropertyChanged();
            }
        }

        private string avatar="";
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
                        if (Avatar != "")
                        {
                            byte[] data;
                            JpegBitmapEncoder encoder1 = new JpegBitmapEncoder();
                            encoder1.Frames.Add(BitmapFrame.Create(new BitmapImage(new Uri(Avatar))));
                            using (MemoryStream ms = new MemoryStream())
                            {
                                encoder1.Save(ms);
                                data = ms.ToArray();
                            }
                            user.Photo = data;
                        }
                        if (Them != "")
                        {
                            switch (Them)
                            {
                                case "Стандартная":
                                    {
                                        Them = "Default";
                                        return;
                                    }
                                case "Темная":
                                    {
                                        Them = "Dark";
                                        return;
                                    }
                            }
                            user.Them = Them;
                        }
                        if (Language != "")
                        {
                            user.Language = Language;
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
                                        CustomMessageBox.Show("Error", "Password must be at least 5 characters", MessageBoxButton.OK);
                                }
                                else
                                {
                                    CustomMessageBox.Show("Error", "Invalid password", MessageBoxButton.OK);
                                    return;
                                }
                            }
                        }
                        dBRepository.Update(user);
                        Application.Current.Resources.Remove(Application.Current.Resources.Contains("Language"));
                        if(user.Language==null || user.Language.Contains("Eng"))
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
                        CustomMessageBox.Show("Event", "Data was saved", MessageBoxButton.OK);
                        dBRepository.Dispose();
                    }
                    catch
                    {
                        CustomMessageBox.Show("Error", "Ops error, sory =(", MessageBoxButton.OK);
                    }

                });
            }
        }

            
    }
}
