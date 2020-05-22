using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Курсовой.Classes;
using System.Windows;

namespace Курсовой.ViewModel
{
    class SaveProjectViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public string name="";
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public bool finish = false;
        public bool Finish
        {
            get
            {
                return finish;
            }
            set
            {
                finish = value;
                OnPropertyChanged();
            }
        }

        public bool save = false;
        public bool Save
        {
            get
            {
                return save;
            }
            set
            {
                save = value;
                OnPropertyChanged();
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
                        Save = true;
                        var window = Application.Current.Windows[1];
                        window.Close();
                    }
                    catch
                    { }
                });
            }
        }


    }
}
