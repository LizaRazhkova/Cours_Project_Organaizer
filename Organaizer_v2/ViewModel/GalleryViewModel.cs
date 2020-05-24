using Organaizer_v2.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using Organaizer_v2.View;
using System.Runtime.CompilerServices;
using Organaizer_v2.Model;
using System.Data.Entity;

namespace Organaizer_v2.ViewModel
{
    public class GalleryViewModel : BaseViewModel
    {
        private FrameSourceService frameSourceService;
        private InvokerService invokerService;
        private AuthService authService;


        public Gallery SelectedItem { get; set; }


        public ObservableCollection<Gallery> CollectionOfPhotos { get; private set; }
        public GalleryViewModel(FrameSourceService frameSourceService, InvokerService invokerService, AuthService authService)
        {
            this.frameSourceService = frameSourceService;
            this.invokerService = invokerService;
            this.authService = authService;
            this.invokerService.Receive<LoadNeccessaryData>(this, async (data) => LoadPicture());

            CollectionOfPhotos = new ObservableCollection<Gallery>();
            
        }
        private byte[] ConvertImageToBinary(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
        private async Task<Gallery> SaveIntoDB(string filename, Image img)
        {
            Users user = this.authService.User;

            using (Organaizer_v2Entities db = new Organaizer_v2Entities())
            {


                Gallery photo = new Gallery()
                {
                    FileName = filename,
                    Data = ConvertImageToBinary(img),

                    Id = user.id
                };
                db.Gallery.Add(photo);
                await db.SaveChangesAsync();
                return photo;
            }
        }

        private void LoadPicture()
        {
            using (Organaizer_v2Entities db = new Organaizer_v2Entities())
            {
                try
                {
                    Users user = this.authService.User;
                    var list = db.Gallery.ToList();
                    List<Gallery> photosList = new List<Gallery>();


                    for (int i=0; i<list.Count(); i++)
                    {
                        if(list[i].Id == user.id)
                        {
                            photosList.Add(list[i]);
                        }
                    }
                    CollectionOfPhotos = new ObservableCollection<Gallery>(photosList);
                    OnPropertyChanged("CollectionOfPhotos");
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        public ICommand SelecctFolder
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

                    if(openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        CollectionOfPhotos.Add(await SaveIntoDB(openFileDialog.FileName, Image.FromFile(openFileDialog.FileName)));
                    }
                });
            }
        }

        public ICommand GoToHome
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    this.frameSourceService.ChangeFrame(new Home(this.invokerService));
                });
            }
        }

        public ICommand DeleteElem
        {
            get
            {
                return new RelayCommand(async (obj) =>
                {
                   var item = SelectedItem as Gallery;

                    await DeleteIntoDB(item.IdImage);
                    CollectionOfPhotos.Remove(item);

                    int i = CollectionOfPhotos.Count();

                });
            }
        }

        private async Task<Gallery> DeleteIntoDB(int id)
        {
            using (Organaizer_v2Entities db = new Organaizer_v2Entities())
            {
                Gallery photo = (Gallery) db.Gallery.FirstOrDefault(Id => Id.IdImage == id);
                db.Gallery.Remove(photo);
                await db.SaveChangesAsync();
                return photo;
            }
        }
    }
}
