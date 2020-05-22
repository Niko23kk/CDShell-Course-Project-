using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Курсовой.Model;
using Курсовой.Classes;
using Курсовой.View;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.Serialization.Json;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.IO;

namespace Курсовой.ViewModel
{
    class WorkSpaceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private ObservableCollection<Elements> elementsOfType = new ObservableCollection<Elements>();
        public ObservableCollection<Elements> ElementsOfType
        {
            get
            {
                return elementsOfType;
            }
            set
            {
                elementsOfType = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<HomeElements> homeCollection = new ObservableCollection<HomeElements>();
        public ObservableCollection<HomeElements> HomeCollection
        {
            get
            {
                return homeCollection;
            }
            set
            {
                homeCollection = value;
                OnPropertyChanged();
            }
        }

        private UserProject CurrentProject;
        public BuildHistory BackHistory = new BuildHistory();
        public BuildHistory ForwardHistory = new BuildHistory();

        private bool Choose { get; set; } = false;
        private bool Change { get; set; } = false;
        private System.Windows.Point CursorPoint { get; set; }
        private System.Windows.Point SecondCursorPoint { get; set; }
        private HomeElements ChangeWorkField;
        private HomeElements BufferElement;
        private bool Block=false;


        public WorkSpaceViewModel()
        {
            if (CurrentUserID.ProjectID != default(int))
            {
                DBRepository<UserProject> dBProject = new DBRepository<UserProject>(new BuildEntities());
                CurrentProject = dBProject.GetAll().Where(s => s.ID == CurrentUserID.ProjectID).First();
                CurrentUserID.ProjectID = default(int);
                DBRepository<WorkField> dBWorkField = new DBRepository<WorkField>(new BuildEntities());
                DBRepository<Elements> dbElement = new DBRepository<Elements>(new BuildEntities());
                foreach (var item in dBWorkField.GetAll().Where(s => s.ID_UP == CurrentProject.ID))
                {
                    try
                    {
                        HomeCollection.Add(new HomeElements(item, dbElement.GetAll().Where(s => s.ID == item.Element_ID).First()));
                        TotalPrice = (Convert.ToDecimal(TotalPrice) + (decimal)item.Elements.Price).ToString();
                    }
                    catch { }
                }
                dBProject.Dispose();
                dBWorkField.Dispose();
                dbElement.Dispose();
            }
            BackHistory.History.Push(HomeCollection.ToList());
            var windows=Application.Current.Windows;
            if (windows.Count > 1)
            {
                try
                {
                    Window window = null;
                    foreach (var item in windows)
                    {
                        if (item as Loading!=null)
                            window = (item as Loading);
                    }
                    window.Close();
                }
                catch { }
            }
        }

        private string totalPrice="0";
        public string TotalPrice
        {
            get
            {
                return totalPrice;
            }
            set
            {
                totalPrice = value;
                OnPropertyChanged();
            }
        }
        
        private Elements currentElement;
        public Elements CurrentElement
        {
            get
            {
                return currentElement;
            }
            set
            {
                currentElement = value;
                OnPropertyChanged();
            }
        }

        private string lockUnlock= "pack://application:,,,/Resourses/Images/Unlock.png";
        public string LockUnlock
        {
            get
            {
                return lockUnlock;
            }
            set
            {
                lockUnlock = value;
                OnPropertyChanged();
            }
        }

        public ICommand TypeClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    DBRepository<Elements> dBRepository = new DBRepository<Elements>(new BuildEntities());
                    ElementsOfType.Clear();
                    switch ((obj as string))
                    {
                        case "Гостинная":
                            {
                                obj = "Living room";
                                break;
                            }
                        case "Спальня":
                            {
                                obj = "Bedroom";
                                break;
                            }
                        case "Кухня":
                            {
                                obj = "Kitchen";
                                break;
                            }
                        case "Ванная":
                            {
                                obj = "Bathroom";
                                break;
                            }
                        case "Техника":
                            {
                                obj = "Tecgnique";
                                break;
                            }
                        case "Сад":
                            {
                                obj = "Garden";
                                break;
                            }
                        case "Интерьер":
                            {
                                obj = "Decoration";
                                break;
                            }
                        case "Постройка":
                            {
                                obj = "Build";
                                break;
                            }
                    }
                    if ((obj as string) == "User")
                    {
                        var result = dBRepository.GetAll().Where(s => s.Type == (obj as string) && s.ID_User==CurrentUserID.getInstance().ID);
                        foreach (var item in result)
                        {
                            ElementsOfType.Add(item);
                        }
                    }
                    else
                    {
                        var result = dBRepository.GetAll().Where(s => s.Type == (obj as string));
                        foreach (var item in result)
                        {
                            ElementsOfType.Add(item);
                        }
                    }
                    dBRepository.Dispose();
                });
            }
        }

        public ICommand SelectElement
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    CurrentElement = (obj as ListBoxItem).DataContext as Elements;
                    Choose = true;
                    Change = false;
                });
            }
        }

        public ICommand DropItemToCanvas
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    if (Choose && !Change)
                    {
                        CursorPoint = Mouse.GetPosition(obj as Canvas);
                        if (CurrentElement.Title != "Floor" && !CurrentElement.Title.Contains("wall"))
                        {
                            DropSelectedIcon();
                        }
                        else
                        {
                            CustomMessageBox.Show("Help", "Choose second point (click RMB)", MessageBoxButton.OK);
                        }
                    }
                    else if(!Choose && !Change)
                    {
                        try
                        {
                            CursorPoint = Mouse.GetPosition(obj as Canvas);
                            try
                            {
                                ChangeWorkField = HomeCollection.Last(s => (s.Field.PositionX < CursorPoint.X && s.Field.PositionX + s.Element.Size > CursorPoint.X) &&
                                  (s.Field.PositionY < CursorPoint.Y && s.Field.PositionY + s.Element.Size > CursorPoint.Y && s.Element.Type != "Build" && s.Element.Title!="Floor"));
                                Change = true;
                                Choose = true;
                            }
                            catch
                            {
                                try
                                {
                                    ChangeWorkField = HomeCollection.Last(s => (s.Field.PositionX < CursorPoint.X && s.Field.PositionX + s.Element.Size > CursorPoint.X) &&
                                      (s.Field.PositionY < CursorPoint.Y && s.Field.PositionY + s.Element.Size > CursorPoint.Y && s.Element.Type != "Build"));
                                    Change = true;
                                    Choose = true;
                                }
                                catch {
                                    if (ChangeWorkField == null && !Block)
                                    {
                                        try
                                        {
                                            ChangeWorkField = HomeCollection.Last(s => (s.Field.PositionX < CursorPoint.X && s.Field.PositionX + s.Element.Size > CursorPoint.X) &&
                                          (s.Field.PositionY < CursorPoint.Y && s.Field.PositionY + s.Element.Size > CursorPoint.Y));
                                            Change = true;
                                            Choose = true;
                                        }
                                        catch { ChangeWorkField = null; Change = false;Choose = false; }
                                    }
                                }
                            }
                        }
                        catch
                        { }
                    }
                    else
                    {
                        CursorPoint = Mouse.GetPosition(obj as Canvas);
                        DropSelectedIcon();
                    }
                });
            }
        }

        public ICommand SecondPointSelect
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    var windows = Application.Current.Windows;
                    Loading loadingwin = new Loading("Waiting...");
                    loadingwin.Show();
                    DBRepository<Elements> dBRepository = new DBRepository<Elements>(new BuildEntities());
                    SecondCursorPoint = Mouse.GetPosition(obj as Canvas);
                    Elements elem = dBRepository.GetAll().Where(s => s.ID == CurrentElement.ID).First();
                    dBRepository.Dispose();
                    int x = (int)CursorPoint.X;
                    int y = (int)CursorPoint.Y;
                    int saveY = y;
                    if (elem.Title == "Floor")
                    {
                        int indexX = x < SecondCursorPoint.X ? 1 : -1;
                        int indexY = y < SecondCursorPoint.Y ? 1 : -1;
                        for (; x*indexX < SecondCursorPoint.X*indexX; x += (int)CurrentElement.Size * indexX)
                        {
                            for (y = saveY; y*indexY < SecondCursorPoint.Y*indexY; y += (int)CurrentElement.Size * indexY)
                            {
                                WorkField workField = new WorkField
                                {
                                    Element_ID = CurrentElement.ID,
                                    Rotate = 0,
                                    PositionX = x,
                                    PositionY = y
                                };
                                if (CurrentProject != null)
                                {
                                    workField.ID_UP = CurrentProject.ID;
                                }
                                HomeCollection.Add(new HomeElements(workField, elem));
                                TotalPrice = (Convert.ToDecimal(TotalPrice)+(decimal)CurrentElement.Price).ToString();
                            }
                        }
                        BackHistory.History.Push(HomeCollection.ToList());
                    }
                    if (elem.Title.Contains("wall"))
                    {
                        if (elem.Title.Contains("Horizontal"))
                        {
                            int indexX = x < SecondCursorPoint.X ? 1 : -1;
                            if(indexX==-1)
                            {
                                x -= (int)CurrentElement.Size;
                            }
                            for (; x * indexX < SecondCursorPoint.X * indexX - (CurrentElement.Size / 2 * indexX); x += (int)CurrentElement.Size * indexX)
                            {
                                WorkField workField = new WorkField
                                {
                                    Element_ID = CurrentElement.ID,
                                    Rotate = 0,
                                    PositionX = x,
                                    PositionY = y
                                };
                                if (CurrentProject != null)
                                {
                                    workField.ID_UP = CurrentProject.ID;
                                }
                                HomeCollection.Add(new HomeElements(workField, elem));
                                TotalPrice = (Convert.ToDecimal(TotalPrice) + (decimal)CurrentElement.Price).ToString();
                            }
                        }
                        if (elem.Title.Contains("Vertical"))
                        {
                            int indexY = y < SecondCursorPoint.Y ? 1 : -1;
                            for (y = saveY; y * indexY < SecondCursorPoint.Y * indexY; y += (int)CurrentElement.Size * indexY)
                            {
                                WorkField workField = new WorkField
                                {
                                    Element_ID = CurrentElement.ID,
                                    Rotate = 90,
                                    PositionX = x - CurrentElement.Size / 2,
                                    PositionY = y
                                };
                                if (CurrentProject != null)
                                {
                                    workField.ID_UP = CurrentProject.ID;
                                }
                                HomeCollection.Add(new HomeElements(workField, elem));
                                TotalPrice = (Convert.ToDecimal(TotalPrice) + (decimal)CurrentElement.Price).ToString();
                            }
                        }
                        BackHistory.History.Push(HomeCollection.ToList());
                    }
                    loadingwin.Close();
                    Choose = false;
                    Change = false;
                    ChangeWorkField = null;
                });
            }
        }


        public ICommand SaveScrean
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    int height = (int)(obj as ItemsControl).ActualHeight;
                    int width = (int)(obj as ItemsControl).ActualWidth;
                    RenderTargetBitmap rtb = new RenderTargetBitmap(width+600, height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
                    rtb.Render(obj as ItemsControl);
                    BitmapEncoder pngEncoder = new PngBitmapEncoder();
                    pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();

                    pngEncoder.Save(ms);
                    ms.Close();

                    SaveFileDialog openFile = new SaveFileDialog
                    {
                        Filter = "PNG Format (*.png)|*.png"
                    };

                    if (openFile.ShowDialog() == true)
                    {
                        try
                        {
                            System.IO.File.WriteAllBytes($"{openFile.FileName}", ms.ToArray());
                            CustomMessageBox.Show("Event", "Screan was saved", MessageBoxButton.OK);
                        }
                        catch { }
                    }
                });
            }
        }

        public ICommand SaveProject
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    SaveProject saveProject = new SaveProject();
                    saveProject.ShowDialog();
                    if ((saveProject.DataContext as SaveProjectViewModel).Save == true)
                    {
                        DBRepository<Project> dBProject = new DBRepository<Project>(new BuildEntities());
                        Project project;
                        if (CurrentProject == null)
                        {
                            project = new Project
                            {
                                Project_name = (saveProject.DataContext as SaveProjectViewModel).Name,
                                Date_of_change = DateTime.Now,
                                Status = (saveProject.DataContext as SaveProjectViewModel).Finish?1:0 
                            };
                            dBProject.Create(project);

                            DBRepository<UserProject> dBUserProject = new DBRepository<UserProject>(new BuildEntities());
                            dBUserProject.Create(CurrentProject = new UserProject
                            {
                                ID_Project = project.ID_Project,
                                ID_User = CurrentUserID.getInstance().ID
                            });
                            CurrentProject = dBUserProject.GetAll().OrderBy(s => s.ID).Last();
                            foreach (var item in HomeCollection)
                            {
                                item.Field.ID_UP = CurrentProject.ID;
                            }
                            dBUserProject.Dispose();
                        }
                        else
                        {
                            project = dBProject.GetAll().Where(s => s.ID_Project == CurrentProject.ID_Project).First();
                        }
                        int height = (int)(obj as ItemsControl).ActualHeight;
                        int width = (int)(obj as ItemsControl).ActualWidth;
                        RenderTargetBitmap rtb = new RenderTargetBitmap(width + 600, height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
                        rtb.Render(obj as ItemsControl);
                        BitmapEncoder pngEncoder = new PngBitmapEncoder();
                        pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
                        System.IO.MemoryStream ms = new System.IO.MemoryStream();
                        pngEncoder.Save(ms);
                        ms.Close();

                        project.Preview = ms.ToArray();
                        project.Date_of_change = DateTime.Now;
                        dBProject.Update(project);

                        DBRepository<WorkField> dBRepository = new DBRepository<WorkField>(new BuildEntities());
                        Collection<WorkField> workFields = new Collection<WorkField>();
                        var elemnts = dBRepository.GetAll().Where(s => s.ID_UP == CurrentProject.ID);
                        if (elemnts.Count() > 0)
                        {
                            foreach (var item in HomeCollection)
                            {
                                var t = elemnts.Where(s => s.ID == item.Field.ID).First();
                            }
                            dBRepository.RemoveCollection(elemnts);
                        }
                        foreach (var item in HomeCollection)
                        {
                            workFields.Add(item.Field);
                        }
                        dBRepository.AddCollection(workFields);
                        CustomMessageBox.Show("Event", "Project save", MessageBoxButton.OK);
                        dBRepository.Dispose();
                        dBProject.Dispose();
                    }
                });
            }
        }

        public ICommand SaveAs
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    SaveFileDialog openFile = new SaveFileDialog
                    {
                        Filter = "XML Format (*.xml)|*.xml"
                    };

                    if (openFile.ShowDialog() == true)
                    {
                        try
                        {
                            using (var context = new BuildEntities())
                            {
                                List<(int? ID_UP, int? Element_ID, int? PositionX, int? PositionY, int? Rotate)> save = new List<(int? ID_UP, int? Element_ID, int? PositionX, int? PositionY, int? Rotate)>();
                                foreach (var item in HomeCollection)
                                {
                                    save.Add((item.Field.ID_UP,item.Field.Element_ID,item.Field.PositionX,item.Field.PositionY,item.Field.Rotate));
                                }
                                XmlSerializer serializer = new XmlSerializer(typeof(List<(int? ID_UP, int? Element_ID, int? PositionX, int? PositionY, int? Rotate)>));
                                StreamWriter file = new StreamWriter(openFile.FileName);
                                serializer.Serialize(file, save);
                                CustomMessageBox.Show("Event", "Project save", MessageBoxButton.OK);
                            }
                        }
                        catch { }
                    }
                });
            }
        }

        public ICommand DeleteElement
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    if (ChangeWorkField != null)
                    {
                        HomeCollection.Remove(ChangeWorkField);
                        TotalPrice = (Convert.ToDecimal(TotalPrice) - (decimal)ChangeWorkField.Element.Price).ToString();
                        ChangeWorkField = null;
                        Choose = false;
                        Change = false;
                    }
                });
            }
        }

        public ICommand LeftRotate
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    if (ChangeWorkField != null)
                    {
                        HomeCollection.Remove(ChangeWorkField);
                        ChangeWorkField.Field.Rotate -= 90;
                        HomeCollection.Add(ChangeWorkField);
                        Choose = false;
                        Change = false;
                    }
                });
            }
        }

        public ICommand RightRorate
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    if (ChangeWorkField != null)
                    {
                        HomeCollection.Remove(ChangeWorkField);
                        ChangeWorkField.Field.Rotate += 90;
                        HomeCollection.Add(ChangeWorkField);
                        Choose = false;
                        Change = false;
                    }
                });
            }
        }

        public ICommand BackHistoryClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    try
                    {
                        if (BackHistory.History.Count > 1)
                        {
                            ForwardHistory.History.Push(BackHistory.History.Pop());
                            var t=ForwardHistory.History.First().Except(BackHistory.History.First());
                            foreach (var item in t)
                            {
                                TotalPrice = (Convert.ToDecimal(TotalPrice) - (decimal)item.Element.Price).ToString();
                                HomeCollection.Remove(item);
                            }
                        }
                        else if(BackHistory.History.Count==1)
                        {
                            var save = BackHistory.History.Pop();
                            ForwardHistory.History.Push(save);
                            foreach (var item in save)
                            {
                                TotalPrice = (Convert.ToDecimal(TotalPrice) - (decimal)item.Element.Price).ToString();
                                HomeCollection.Remove(item);
                            }
                        }
                    }
                    catch { }
                });
            }
        }
        
        public ICommand ForwardHistoryClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    try
                    {
                        if (BackHistory.History.Count > 0)
                        {
                            var save = ForwardHistory.History.Pop();
                            var t = save.Except(BackHistory.History.First());
                            foreach (var item in t)
                            {
                                TotalPrice = (Convert.ToDecimal(TotalPrice) + (decimal)item.Element.Price).ToString();
                                HomeCollection.Add(item);
                            }
                            BackHistory.History.Push(save);
                        }
                        else
                        {
                            var save = ForwardHistory.History.Pop();
                            foreach (var item in save)
                            {
                                TotalPrice = (Convert.ToDecimal(TotalPrice) + (decimal)item.Element.Price).ToString();
                                HomeCollection.Add(item);
                            }
                            BackHistory.History.Push(save);
                        }
                    }
                    catch { }
                });
            }
        }

        public ICommand Copy
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    try
                    {
                        if(ChangeWorkField!=null)
                        {
                            BufferElement = new HomeElements(new WorkField {
                                Element_ID =ChangeWorkField.Field.Element_ID,
                                ID_UP = ChangeWorkField.Field.ID_UP,
                                PositionX=ChangeWorkField.Field.PositionX+20,
                                PositionY=ChangeWorkField.Field.PositionY+20,
                                Rotate=ChangeWorkField.Field.Rotate,
                                UserProject=ChangeWorkField.Field.UserProject
                            },ChangeWorkField.Element);
                        }
                    }
                    catch { }
                });
            }
        }

        public ICommand Past
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    try
                    {
                        if (BufferElement != null)
                        {
                            HomeCollection.Add(BufferElement);
                            TotalPrice = (Convert.ToDecimal(TotalPrice) + (decimal)CurrentElement.Price).ToString();
                            BackHistory.History.Push(HomeCollection.ToList());
                            ChangeWorkField = null;
                            Choose = false;
                            Change = false;
                        }
                    }
                    catch { }
                });
            }
        }

        public ICommand LockUnlockClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    if(LockUnlock== "pack://application:,,,/Resourses/Images/Lock.png")
                    {
                        LockUnlock = "pack://application:,,,/Resourses/Images/Unlock.png";
                        Block = false;
                    }
                    else
                    {
                        LockUnlock = "pack://application:,,,/Resourses/Images/Lock.png";
                        Block = true;
                    }
                });
            }
        }

        private void DropSelectedIcon()
        {
            if (currentElement != null || ChangeWorkField!=null)
            {
                DBRepository<Elements> dBRepository = new DBRepository<Elements>(new BuildEntities());
                try
                {
                    if (Choose && !Change)
                    {
                        int x = (int)CursorPoint.X - (int)CurrentElement.Size / 2;
                        int y = (int)CursorPoint.Y;
                        WorkField workField = new WorkField
                        {
                            Element_ID = CurrentElement.ID,
                            Rotate = 0,
                            PositionX = x,
                            PositionY = y
                        };
                        if (CurrentProject != null)
                        {
                            workField.ID_UP = CurrentProject.ID;
                        }
                        HomeCollection.Add(new HomeElements(workField, dBRepository.GetAll().Where(s => s.ID == CurrentElement.ID).First()));
                        Choose = false;
                        Change = false;
                        ChangeWorkField = null;
                        TotalPrice = (Convert.ToDecimal(TotalPrice) + (decimal)CurrentElement.Price).ToString();
                        BackHistory.History.Push(HomeCollection.ToList());
                        if (BackHistory.History.Count()>30)
                        {
                            BuildHistory helpstack = new BuildHistory();
                            foreach (var item in BackHistory.History)
                            {
                                if(helpstack.History.Count<30)
                                {
                                    helpstack.History.Push(item);
                                }
                            }
                            BackHistory.History.Clear();
                            foreach (var item in helpstack.History)
                            {
                                BackHistory.History.Push(item);
                            }
                        }
                    }
                    else if (Choose && Change)
                    {
                        HomeCollection.Remove(ChangeWorkField);
                        int x = (int)CursorPoint.X - (int)ChangeWorkField.Element.Size / 2;
                        int y = (int)CursorPoint.Y;
                        ChangeWorkField.Field.PositionX = x;
                        ChangeWorkField.Field.PositionY = y;
                        HomeCollection.Add(ChangeWorkField);
                        Choose = false;
                        Change = false;
                        ChangeWorkField = null;
                    }
                }
                catch { }
                dBRepository.Dispose();
            }
        }
        
    }
}
