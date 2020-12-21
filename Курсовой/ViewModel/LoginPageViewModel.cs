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
using System.Net;
using System.IO;
using System.Net.Mail;

namespace Курсовой.ViewModel
{
    class LoginPageViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private string login;
        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
                OnPropertyChanged();
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

        public ICommand GetCode
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    DBRepository<User> dBRepository = new DBRepository<User>(new BuildEntities());
                    User currentUser = null;
                    try
                    {
                        currentUser=dBRepository.GetAll().First(s => s.Login == Login);
                    }
                    catch
                    {
                        CustomMessageBox.Show("Error", "We don't find this login", MessageBoxButton.OK);
                    }
                    try
                    {
                        Random rnd = new Random();
                        int value = rnd.Next(1000, 9999);
                        MailAddress from = new MailAddress("cdshellcompany@mail.ru", "CD SHELL");
                        MailAddress to = new MailAddress(dBRepository.GetAll().First(s => s.Login == Login).Email);
                        MailMessage m = new MailMessage(from, to);
                        m.Subject = "Password recovery";
                        m.Body = $"Your activated code: {value}";

                        SmtpClient smtp = new SmtpClient("smtp.mail.ru", 25);
                        smtp.EnableSsl = true;
                        smtp.Credentials = new NetworkCredential("cdshellcompany@mail.ru", "NIKO123wow");
                        smtp.Send(m);
                        var window = Application.Current.Windows[Application.Current.Windows.Count - 1];
                        (window as SignInWindow).Frame.Navigate(new Recovery(dBRepository.GetAll().First(s => s.Login == Login),value));
                    }
                    catch
                    {
                        CustomMessageBox.Show("Error", "We don't find this account", MessageBoxButton.OK);
                    }
                    dBRepository.Dispose();
                });
            }
        }

    }
}
