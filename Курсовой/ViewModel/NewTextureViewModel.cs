using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Курсовой.Model;
using Курсовой.Classes;
using Курсовой.View;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;

namespace Курсовой.ViewModel
{
    class NewTextureViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private string title="";
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        private int price;
        public int Price
        {
            get { return price; }
            set
            {
                try
                {
                    if(value<0)
                    {
                        throw new Exception();
                    }
                    price = value;
                    OnPropertyChanged();
                }
                catch
                {
                    CustomMessageBox.Show("Error", "It is not a number", MessageBoxButton.OK);
                }
            }
        }

        private int size;
        public int Size
        {
            get { return size; }
            set
            {
                try
                {
                    if (value < 0)
                    {
                        throw new Exception();
                    }
                    size = value;
                    OnPropertyChanged();
                }
                catch
                {
                    CustomMessageBox.Show("Error", "It is not a number", MessageBoxButton.OK);
                }
            }
        }

        private string frontImage;
        public string FrontImage
        {
            get { return frontImage; }
            set
            {
                frontImage = value;
                OnPropertyChanged();
            }
        }

        private string sideImage;
        public string SideImage
        {
            get { return sideImage; }
            set
            {
                sideImage = value;
                OnPropertyChanged();
            }
        }


        public ICommand AddTextureClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    try
                    {
                        bool check = true;

                        if (Title.Length == 0 || Title.Length > 25)
                        {
                            CustomMessageBox.Show("Error", "Input title (max lenght 25)", MessageBoxButton.OK);
                            check = false;
                        }
                        if (Price == 0)
                        {
                            CustomMessageBox.Show("Error", "Input price", MessageBoxButton.OK);
                            check = false;
                        }
                        if (Size == 0 || Size < 5)
                        {
                            CustomMessageBox.Show("Error", "Uncorrect size", MessageBoxButton.OK);
                            check = false;
                        }

                        if (check)
                        {
                            DBRepository<Elements> dBRepository = new DBRepository<Elements>(new BuildEntities());
                            byte[] data;
                            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(new BitmapImage(new Uri(FrontImage))));
                            using (MemoryStream ms = new MemoryStream())
                            {
                                encoder.Save(ms);
                                data = ms.ToArray();
                            }

                            byte[] data1;
                            JpegBitmapEncoder encoder1 = new JpegBitmapEncoder();
                            encoder1.Frames.Add(BitmapFrame.Create(new BitmapImage(new Uri(SideImage))));
                            using (MemoryStream ms = new MemoryStream())
                            {
                                encoder1.Save(ms);
                                data1 = ms.ToArray();
                            }

                            Elements element = new Elements
                            {
                                ID_User = CurrentUserID.getInstance().ID,
                                Title = Title,
                                Price = Price,
                                Type="User",
                                Size=Size,
                                Front_view=data,
                                Side_view=data1
                            };
                            dBRepository.Create(element);
                            CustomMessageBox.Show("Event", "Object add", MessageBoxButton.OK);
                            dBRepository.Dispose();
                        }
                    }
                    catch
                    {
                        CustomMessageBox.Show("Error", "Oop error, sorry =(", MessageBoxButton.OK);
                    }
                });
            }
        }

        public ICommand FrontImageClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Filter = "JPG Format (*.jpg)|*.jpg|PNG Format (*.png)|*.png";

                    if (openFile.ShowDialog() == true)
                    {
                        FrontImage=openFile.FileName;
                    }
                });
            }
        }

        public ICommand SideImageClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Filter = "JPG Format (*.jpg)|*.jpg|PNG Format (*.png)|*.png";

                    if (openFile.ShowDialog() == true)
                    {
                        SideImage = openFile.FileName;
                    }
                });
            }
        }

    }
}
