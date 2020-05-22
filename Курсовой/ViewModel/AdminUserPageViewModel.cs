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
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace Курсовой.ViewModel
{
    class AdminUserPageViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public AdminUserPageViewModel()
        {
            DBRepository<User> dBUser = new DBRepository<User>(new BuildEntities());
            foreach (var item in dBUser.GetAll().Where(s=>s.Type!="Admin"))
            {
                Users.Add(item);
            }
            var windows = Application.Current.Windows;
            if (windows.Count > 1)
            {
                Window window = null;
                try
                {
                    foreach (var item in windows)
                    {
                        if (item as Loading != null)
                        {
                            window = (item as Window);
                        }
                    }
                }
                catch { }
                window.Close();
            }
            dBUser.Dispose();
        }

        private ObservableCollection<User> users = new ObservableCollection<User>();
        public ObservableCollection<User> Users
        {
            get { return users; }
            set { users = value; OnPropertyChanged(); }
        }

        private string firstName = "";
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

        private string secondName = "";
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

        private string email = "";
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

        private string login = "";
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

        private string language;
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

        private string them;
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

        private string newLogin = "";
        public string NewLogin
        {
            get { return newLogin; }
            set
            {
                if (value.Length <= 25)
                {
                    newLogin = value;
                    OnPropertyChanged();
                }
                else CustomMessageBox.Show("Error", "Check your Login (max lenght 25)", MessageBoxButton.OK);
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
                    foreach (var item in Application.Current.Windows)
                    {
                        ((((item as SignInWindow).Content as Frame).Content as Page).FindName("Password") as PasswordBox).Tag = "";
                    }
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

        private bool adminStatus;
        public bool AdminStatus
        {
            get
            {
                return adminStatus;
            }
            set
            {
                adminStatus = value;
                OnPropertyChanged();
            }
        }

        private string find;
        public string Find
        {
            get
            {
                return find;
            }
            set
            {
                find = value;
                OnPropertyChanged();
                Filter();
            }
        }

        private void Filter()
        {
            Users.Clear();
            DBRepository<User> dBRepository = new DBRepository<User>(new BuildEntities());
            foreach (var item in dBRepository.GetAll().Where(s=>s.Login.ToUpper().Contains(Find.ToUpper()) || s.Name.ToUpper().Contains(Find.ToUpper()) || s.Surname.ToUpper().Contains(Find.ToUpper()) || s.Email.ToUpper().Contains(Find.ToUpper()) && s.Type!="Admin"))
            {
                Users.Add(item);
            }
            dBRepository.Dispose();
        }

        public ICommand ChooseUser
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    try
                    {
                        FirstName = ((obj as FrameworkElement).DataContext as User).Name;
                        SecondName = ((obj as FrameworkElement).DataContext as User).Surname;
                        Login = ((obj as FrameworkElement).DataContext as User).Login;
                        Email = ((obj as FrameworkElement).DataContext as User).Email;
                        Language = ((obj as FrameworkElement).DataContext as User).Language;
                        Them = ((obj as FrameworkElement).DataContext as User).Them;
                    }
                    catch
                    { }
                });
            }
        }

        public ICommand ShowProject
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    var window = Application.Current.Windows[Application.Current.Windows.Count - 1];
                    (window as Admin).Frame.Navigate(new AdminProjectPage((obj as FrameworkElement).DataContext as User));
                });
            }
        }

        public ICommand DeleteUser
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    try
                    {
                        if (CustomMessageBox.Show("Delete", "Deleted user?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            DBRepository<User> dBRepository = new DBRepository<User>(new BuildEntities());
                            dBRepository.Remove((obj as FrameworkElement).DataContext as User);
                            Users.Clear();
                            foreach (var item in dBRepository.GetAll().Where(s => s.Type != "Admin"))
                            {
                                Users.Add(item);
                            }
                            CustomMessageBox.Show("Delete", "User has been deleted", MessageBoxButton.OK);
                            dBRepository.Dispose();
                        }
                    }
                    catch
                    { }
                });
            }
        }

        public ICommand Update
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    bool check = true;
                    if (FirstName.Length == 0)
                    {
                        CustomMessageBox.Show("Error", "Input First name", MessageBoxButton.OK);
                        check = false;
                    }
                    if (SecondName.Length == 0)
                    {
                        CustomMessageBox.Show("Error", "Input Second name", MessageBoxButton.OK);
                        check = false;
                    }
                    if (!Regex.IsMatch(Email, @"^[-a-z0-9!#$%&'*+/=?^_`{|}~]+(\.[-a-z0-9!#$%&'*+/=?^_`{|}~]+)*@([a-z0-9]([-a-z0-9]{0,61}[a-z0-9])?\.)" +
                        "*(aero|arpa|asia|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel|[a-z][a-z])$"))
                    {
                        CustomMessageBox.Show("Error", "Uncorrectly format in email", MessageBoxButton.OK);
                        check = false;
                    }
                    try
                    {
                        if (Password.Length < 5 && Password.Length > 0)
                        {
                            CustomMessageBox.Show("Error", "Check Password (minimal lenght 5)", MessageBoxButton.OK);
                            check = false;
                        }
                    }
                    catch { }


                    if (check)
                    {
                        SHA1 sHA1 = new SHA1CryptoServiceProvider();

                        DBRepository<User> dBRepository = new DBRepository<User>(new BuildEntities());
                        User user = dBRepository.GetAll().First(s => s.Login == Login);
                        user.Name = FirstName;
                        user.Surname = SecondName;
                        user.Email = Email;
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
                        user.Them=Them;
                        user.Language = Language;
                        if (NewLogin != "" && dBRepository.GetAll().Where(s => s.Login == NewLogin).Count() == 0)
                        {
                            user.Login = NewLogin;
                        }
                        if (!String.IsNullOrEmpty(Password))
                        {
                            user.Password = sHA1.ComputeHash(Encoding.ASCII.GetBytes(Password));
                        }
                        user.Type = AdminStatus ? "Admin" : "User";
                        dBRepository.Update(user);
                        CustomMessageBox.Show("Event", "Info was updating", MessageBoxButton.OK);
                        Users.Clear();
                        foreach (var item in dBRepository.GetAll().Where(s => s.Type != "Admin"))
                        {
                            Users.Add(item);
                        }
                        dBRepository.Dispose();
                    }
                });
            }
        }

    }
}
