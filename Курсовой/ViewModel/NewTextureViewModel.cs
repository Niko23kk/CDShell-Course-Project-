﻿using System;
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
using System.Windows.Documents;
using Microsoft.Win32;
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
                    price = value;
                    OnPropertyChanged();
                }
                catch
                {
                    MessageBox.Show("It is a not number");
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
                    size = value;
                    OnPropertyChanged();
                }
                catch
                {
                    MessageBox.Show("It is a not number");
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
                            MessageBox.Show("Input title (max lenght 25)");
                            check = false;
                        }
                        if (Price == 0)
                        {
                            MessageBox.Show("Input price");
                            check = false;
                        }
                        if (Size == 0 || Size < 5)
                        {
                            MessageBox.Show("Uncorrect size");
                            check = false;
                        }

                        if (check)
                        {
                            DBRepository<Elements> dBRepository = new DBRepository<Elements>(new BuildEntities());
                            Elements element = new Elements
                            {
                                ID_User = CurrentUserID.getInstance().ID,
                                TitleEng = Title,
                                Price = Price,
                                Size=Size
                            };
                            dBRepository.CreateTexture(element, FrontImage, SideImage);
                            MessageBox.Show("Texture add");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Oop error, sorry =(");
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
