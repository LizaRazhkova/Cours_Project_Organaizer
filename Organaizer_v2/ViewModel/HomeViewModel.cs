using Organaizer_v2.Model;
using Organaizer_v2.Service;
using Organaizer_v2.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Organaizer_v2.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        private FrameSourceService frameSourceService;
        private InvokerService invokerService;

        private string timer;

        public HomeViewModel(FrameSourceService frameSourceService, InvokerService invokerService)
        {
            this.frameSourceService = frameSourceService;
            this.invokerService = invokerService;

            this.timer = string.Empty;

            this.invokerService.Receive<InvokeTimerTick>(this, async (data) =>
            {
                TimeSpan time = TimeSpan.FromSeconds(data.Time);
                string str = time.ToString(@"hh\:mm\:ss");
                Timer = str;
            });
        }
        public ICommand ExitCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    this.invokerService.DisableAllInvokers();
                });
            }
        }
        public ICommand GoToGallery
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    this.invokerService.Invoke<GalleryViewModel>(new LoadNeccessaryData());
                    this.frameSourceService.ChangeFrame(new View.Gallery());
                });
            }
        }

        public ICommand OpenSettings
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    this.frameSourceService.ChangeFrame(new View.Setting());
                });
            }
        }

        public ICommand GoToManagerPass
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    this.invokerService.Invoke<ManagerPassViewModel>(new LoadNeccessaryData());
                    this.frameSourceService.ChangeFrame(new View.ManagerPass());
                });
            }
        }
        
        public ICommand GoToTaskManager
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    this.invokerService.Invoke<TaskManagerViewModel>(new LoadNeccessaryData());
                    this.frameSourceService.ChangeFrame(new View.TaskManager());
                });
            }
        }



        public string Timer
        {
            get => this.timer;
            set
            {
                this.timer = value;
                OnPropertyChanged();
            }
        }
    }
}
