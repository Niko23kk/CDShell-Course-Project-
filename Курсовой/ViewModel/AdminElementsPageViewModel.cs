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
using Курсовой.View;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace Курсовой.ViewModel
{
    class AdminElementsPageViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private ObservableCollection<Elements> allElements = new ObservableCollection<Elements>();
        public ObservableCollection<Elements> AllElements
        {
            get { return allElements; }
            set { allElements = value; OnPropertyChanged(); }
        }

        public AdminElementsPageViewModel()
        {
            DBRepository<Elements> dBRepository = new DBRepository<Elements>(new BuildEntities());
            foreach (var item in dBRepository.GetAll())
            {
                AllElements.Add(item);
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
            dBRepository.Dispose();
            CustomMessageBox.Show("Warning","Warning: remember that any of your changes affect customer projects, be careful", MessageBoxButton.OK);
        }

        private string title="";
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        private string type = "";
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
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
                    if (value < 0)
                    {
                        throw new Exception();
                    }
                    price = value;
                    OnPropertyChanged();
                }
                catch
                {
                    CustomMessageBox.Show("Error", "Incorrect data", MessageBoxButton.OK);
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
                    if (value <= 0)
                    {
                        throw new Exception();
                    }
                    size = value;
                    OnPropertyChanged();
                }
                catch
                {
                    CustomMessageBox.Show("Error", "Incorrect data", MessageBoxButton.OK);
                }
            }
        }
        private BitmapImage frontView;
        public BitmapImage FrontView
        {
            get
            {
                return frontView;
            }
            set
            {
                frontView = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage sideVeiw;
        public BitmapImage SideView
        {
            get
            {
                return sideVeiw;
            }
            set
            {
                sideVeiw = value;
                OnPropertyChanged();
            }
        }

        private int id;
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
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
            AllElements.Clear();
            DBRepository<Elements> dBRepository = new DBRepository<Elements>(new BuildEntities());
            foreach (var item in dBRepository.GetAll().Where(s=>s.Type.ToUpper().Contains(Find.ToUpper()) || s.Title.ToUpper().Contains(Find.ToUpper())))
            {
                AllElements.Add(item);
            }
            dBRepository.Dispose();
        }

        private Visibility updateVisible=Visibility.Visible;
        public Visibility UpdateVisible
        {
            get
            {
                return updateVisible;
            }
            set
            {
                updateVisible = value;
                OnPropertyChanged();
            }
        }

        private Visibility createVisible = Visibility.Collapsed;
        public Visibility CreateVisible
        {
            get
            {
                return createVisible;
            }
            set
            {
                createVisible = value;
                OnPropertyChanged();
            }
        }

        public ICommand CreateUpdate
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    if(CreateVisible==Visibility.Visible)
                    {
                        CreateVisible = Visibility.Collapsed;
                        UpdateVisible = Visibility.Visible;
                    }
                    else
                    {
                        CreateVisible = Visibility.Visible;
                        UpdateVisible = Visibility.Collapsed;
                    }
                });
            }
        }

        public ICommand SelectElement
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    Title = ((obj as FrameworkElement).DataContext as Elements).Title;
                    Type = ((obj as FrameworkElement).DataContext as Elements).Type;
                    Price = Convert.ToInt32(((obj as FrameworkElement).DataContext as Elements).Price);
                    Size = Convert.ToInt32(((obj as FrameworkElement).DataContext as Elements).Size);
                    ID = ((obj as FrameworkElement).DataContext as Elements).ID;

                    MemoryStream strmImg1 = new MemoryStream(((obj as FrameworkElement).DataContext as Elements).Front_view);
                    BitmapImage myBitmapImage1 = new BitmapImage();
                    myBitmapImage1.BeginInit();
                    myBitmapImage1.StreamSource = strmImg1;
                    myBitmapImage1.DecodePixelWidth = 200;
                    myBitmapImage1.EndInit();
                    FrontView = myBitmapImage1;

                    MemoryStream strmImg2 = new MemoryStream(((obj as FrameworkElement).DataContext as Elements).Side_view);
                    BitmapImage myBitmapImage2 = new BitmapImage();
                    myBitmapImage2.BeginInit();
                    myBitmapImage2.StreamSource = strmImg2;
                    myBitmapImage2.DecodePixelWidth = 200;
                    myBitmapImage2.EndInit();
                    SideView = myBitmapImage2;
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
                        FrontView= new BitmapImage(new Uri(openFile.FileName));
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
                        MemoryStream strmImg1 = new MemoryStream(((obj as FrameworkElement).DataContext as Elements).Front_view);
                        BitmapImage myBitmapImage1 = new BitmapImage();
                        myBitmapImage1.BeginInit();
                        myBitmapImage1.StreamSource = strmImg1;
                        myBitmapImage1.DecodePixelWidth = 200;
                        myBitmapImage1.EndInit();
                        FrontView = myBitmapImage1;
                    }
                });
            }
        }

        public ICommand Update
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    DBRepository<Elements> dBRepository = new DBRepository<Elements>(new BuildEntities());
                    byte[] data;
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(FrontView));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        data = ms.ToArray();
                    }

                    byte[] data1;
                    JpegBitmapEncoder encoder1 = new JpegBitmapEncoder();
                    encoder1.Frames.Add(BitmapFrame.Create(SideView));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder1.Save(ms);
                        data1 = ms.ToArray();
                    }
                    switch (Title)
                    {
                        case "Гостинная":
                            {
                                Title = "Living room";
                                break;
                            }
                        case "Спальня":
                            {
                                Title = "Bedroom";
                                break;
                            }
                        case "Кухня":
                            {
                                Title = "Kitchen";
                                break;
                            }
                        case "Ванная":
                            {
                                Title = "Bathroom";
                                break;
                            }
                        case "Техника":
                            {
                                Title = "Tecgnique";
                                break;
                            }
                        case "Сад":
                            {
                                Title = "Garden";
                                break;
                            }
                        case "Интерьер":
                            {
                                Title = "Decoration";
                                break;
                            }
                        case "Постройка":
                            {
                                Title = "Build";
                                break;
                            }
                    }

                    Elements element = new Elements
                    {
                        ID = ID,
                        Title = Title,
                        Type = Type,
                        Price = Price,
                        Size = size,
                        Front_view=data,
                        Side_view = data1
                    };
                    dBRepository.Update(element);
                    CustomMessageBox.Show("Event", "Element update", MessageBoxButton.OK);
                    AllElements.Clear();
                    foreach (var item in dBRepository.GetAll())
                    {
                        AllElements.Add(item);
                    }
                    dBRepository.Dispose();
                });
            }
        }

        public ICommand Create
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    DBRepository<Elements> dBRepository = new DBRepository<Elements>(new BuildEntities());
                    byte[] data;
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(FrontView));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        data = ms.ToArray();
                    }

                    byte[] data1;
                    JpegBitmapEncoder encoder1 = new JpegBitmapEncoder();
                    encoder1.Frames.Add(BitmapFrame.Create(SideView));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder1.Save(ms);
                        data1 = ms.ToArray();
                    }
                    switch (Title)
                    {
                        case "Гостинная":
                            {
                                Title = "Living room";
                                break;
                            }
                        case "Спальня":
                            {
                                Title = "Bedroom";
                                break;
                            }
                        case "Кухня":
                            {
                                Title = "Kitchen";
                                break;
                            }
                        case "Ванная":
                            {
                                Title = "Bathroom";
                                break;
                            }
                        case "Техника":
                            {
                                Title = "Tecgnique";
                                break;
                            }
                        case "Сад":
                            {
                                Title = "Garden";
                                break;
                            }
                        case "Интерьер":
                            {
                                Title = "Decoration";
                                break;
                            }
                        case "Постройка":
                            {
                                Title = "Build";
                                break;
                            }
                    }

                    Elements element = new Elements
                    {
                        Title = Title,
                        Type = Type,
                        Price = Price,
                        Size = size,
                        Front_view = data,
                        Side_view = data1
                    };
                    dBRepository.Create(element);
                    CustomMessageBox.Show("Event", "Element was created", MessageBoxButton.OK);
                    AllElements.Clear();
                    foreach (var item in dBRepository.GetAll())
                    {
                        AllElements.Add(item);
                    }
                    dBRepository.Dispose();
                });
            }
        }

        public ICommand DeleteElement
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    try
                    {
                        if (CustomMessageBox.Show("Deleted", "Deleted element?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            DBRepository<Elements> dBElements = new DBRepository<Elements>(new BuildEntities());
                            DBRepository<WorkField> dBField = new DBRepository<WorkField>(new BuildEntities());
                            dBField.RemoveCollection(dBField.GetAll().Where(s => s.Element_ID == ((obj as FrameworkElement).DataContext as Elements).ID));
                            dBElements.Remove((obj as FrameworkElement).DataContext as Elements);
                            CustomMessageBox.Show("Event", "Element was deleted", MessageBoxButton.OK);
                            AllElements.Clear();
                            foreach (var item in dBElements.GetAll())
                            {
                                AllElements.Add(item);
                            }
                            dBElements.Dispose();
                            dBField.Dispose();
                        }
                    }
                    catch { }
                });
            }
        }
    }
}
