using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Курсовой.Model;
using Курсовой.Clases;
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
                    HomeCollection.Add(new HomeElements(item,dbElement.GetAll().Where(s=>s.ID==item.Element_ID).First()));
                }
            }
        }
        
        private Elements currentElement;
        public Elements CurrentElements
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

        public double Zoom { get; set; } = 1;

        private ScaleTransform zoomProperty;
        public ScaleTransform ZoomProperty
        {
            get
            {
                return zoomProperty;
            }
            set
            {
                zoomProperty = value;
                OnPropertyChanged();
            }
        }

        public ICommand TypeClick
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    DBRepository<Elements> dBRepositoryEl = new DBRepository<Elements>(new BuildEntities());
                    ElementsOfType.Clear();
                    var result= dBRepositoryEl.GetAll().Where(s => s.TypeEng == (obj as string));
                    foreach (var item in result)
                    {
                        ElementsOfType.Add(item);
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
                    CurrentElements = (obj as ListBoxItem).DataContext as Elements;
                    Choose = true;
                    Change = false;
                });
            }
        }

        public ICommand ZoomCanvas
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    (obj as Canvas).InputBindings.ToString();
                    MouseWheelGesture gesture = new MouseWheelGesture();
                    gesture.Matches(null, null);
                    MessageBox.Show(gesture.Direction.ToString());
                    if (MouseWheelGesture.CtrlDown.Direction == MouseWheelGesture.WheelDirection.Down)
                    {
                        MessageBox.Show("Down");
                    }
                    else
                        MessageBox.Show("");
                });
            }
        }
        
        public ICommand ZoomPlusCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    Zoom += 0.1;
                    ZoomProperty = new ScaleTransform(Zoom, Zoom, Zoom*100, Zoom*100);
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
                        if (CurrentElements.TitleEng != "Floor")
                        {
                            DropSelectedIcon();
                        }
                        else
                        {
                            MessageBox.Show("Choose second point (click RMB)");
                        }
                    }
                    else if(!Choose && !Change)
                    {
                        try
                        {
                            CursorPoint = Mouse.GetPosition(obj as Canvas);
                            ChangeWorkField = HomeCollection.Where(s => (s.Field.PositionX < CursorPoint.X && s.Field.PositionX + s.Element.Size > CursorPoint.X) &&
                              (s.Field.PositionY < CursorPoint.Y && s.Field.PositionY + s.Element.Size > CursorPoint.Y)).First();
                            Change = true;
                            Choose = true;
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
                    DBRepository<Elements> dBRepository = new DBRepository<Elements>(new BuildEntities());
                    SecondCursorPoint = Mouse.GetPosition(obj as Canvas);
                    int x = (int)CursorPoint.X;
                    int y = (int)CursorPoint.Y;
                    int saveY = y;
                    Elements elem = dBRepository.GetAll().Where(s => s.ID == CurrentElements.ID).First();
                    int indexX = x < SecondCursorPoint.X ? 1 : -1;
                    int indexY = y < SecondCursorPoint.Y ? 1 : -1;
                    for (; x < SecondCursorPoint.X; x += (int)CurrentElements.Size*indexX)
                    {
                        for (y=saveY; y < SecondCursorPoint.Y; y += (int)CurrentElements.Size*indexY)
                        {
                            WorkField workField = new WorkField
                            {
                                Element_ID = CurrentElements.ID,
                                Rotate = 0,
                                PositionX = x,
                                PositionY = y
                            };
                            if (CurrentProject != null)
                            {
                                workField.ID_UP = CurrentProject.ID;
                            }
                            HomeCollection.Add(new HomeElements(workField, elem));
                        }
                    }
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
                        Filter = "XML Format (*.png)|*.png"
                    };

                    if (openFile.ShowDialog() == true)
                    {
                        try
                        {
                            System.IO.File.WriteAllBytes($"{openFile.FileName}", ms.ToArray());
                            MessageBox.Show("Screan was saved");
                        }
                        catch { }
                    }
                    
                    //System.Drawing.Bitmap croppedImage = new System.Drawing.Bitmap(@"D:\Учеба\4_сем\Курсач\123.png");
                    //var windows = Application.Current.Windows;
                    //Window window = null;
                    //foreach (var item in windows)
                    //{
                    //    window = item as Window;
                    //}
                    //System.Drawing.Rectangle destinationsRect = new System.Drawing.Rectangle(300, 0, width, height);
                    //croppedImage = croppedImage.Clone(destinationsRect, croppedImage.PixelFormat);

                    //System.Drawing.Image image = croppedImage.Clone(destinationsRect, System.Drawing.Imaging.PixelFormat.Undefined);
                    //croppedImage.Dispose();
                    //image.Save(@"D:\Учеба\4_сем\Курсач\123.png");
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
                                Status = (saveProject.DataContext as SaveProjectViewModel).Save?1:0 
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
                        MessageBox.Show("Project save");
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
                                MessageBox.Show("Project save");
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
                    HomeCollection.Remove(ChangeWorkField);
                    ChangeWorkField = null;
                    Choose = false;
                    Change = false;
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
                        ForwardHistory.History.Push(BackHistory.History.Pop());
                        HomeCollection.Clear();
                        foreach (var item in BackHistory.History.First())
                        {
                            HomeCollection.Add(item);
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
                        var time = ForwardHistory.History.Pop();
                        BackHistory.History.Push(time);
                        HomeCollection.Clear();
                        foreach (var item in time)
                        {
                            HomeCollection.Add(item);
                        }
                    }
                    catch { }
                });
            }
        }

        private void DropSelectedIcon()
        {
            if (currentElement != null || ChangeWorkField!=null)
            {
                DBRepository<Elements> dBRepository = new DBRepository<Elements>(new BuildEntities());
                if (Choose && !Change)
                {
                    int x = (int)CursorPoint.X -(int)CurrentElements.Size / 2;
                    int y = (int)CursorPoint.Y;
                    WorkField workField = new WorkField
                    {
                        Element_ID=CurrentElements.ID,
                        Rotate=0,
                        PositionX = x,
                        PositionY = y
                    };
                    if(CurrentProject!=null)
                    {
                        workField.ID_UP = CurrentProject.ID;
                    }
                    HomeCollection.Add(new HomeElements(workField, dBRepository.GetAll().Where(s => s.ID == CurrentElements.ID).First()));
                    Choose = false;
                    Change = false;
                    ChangeWorkField = null;
                }
                else if(Choose && Change)
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

                BackHistory.History.Push(HomeCollection.ToList());
            }
        }
        
        //private Double zoomMax = 5;
        //private Double zoomMin = 0.5;
        //private Double zoomSpeed = 0.001;
        //private Double zoom = 1;
        //// Image
        //private Image backgroung;
        

        //// Zoom on Mouse wheel
        //private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    zoom += zoomSpeed * e.Delta; // Ajust zooming speed (e.Delta = Mouse spin value )
        //    if (zoom < zoomMin) { zoom = 0.5; } // Limit Min Scale
        //    if (zoom > zoomMax) { zoom = 5; } // Limit Max Scale

        //    Point mousePos = e.GetPosition(canvas_Draw);

        //    if (zoom > 1)
        //    {
        //        canvas_Draw.RenderTransform = new ScaleTransform(zoom, zoom, mousePos.X, mousePos.Y); // transform Canvas size from mouse position
        //    }
        //    else
        //    {
        //        canvas_Draw.RenderTransform = new ScaleTransform(zoom, zoom); // transform Canvas size
        //    }
        //}

    }
}
