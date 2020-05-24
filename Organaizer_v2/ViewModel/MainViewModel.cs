using Organaizer_v2.Model;
using Organaizer_v2.Service;
using Organaizer_v2.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Organaizer_v2.ViewModel
{
    public class MainViewModel : BaseViewModel 
    {
        private FrameSourceService frameSourceService;
        private InvokerService invokerService;

        private Page frameSource;
        private Notification notification;
        private bool isNotify;
        private DispatcherTimer timer;
        private int decrement;

        public MainViewModel(FrameSourceService frameSourceService, InvokerService invokerService)
        {
            this.frameSourceService = frameSourceService;
            this.invokerService = invokerService;

            this.frameSourceService.FrameChangeEvent += (frame) => FrameSource = frame;

            IsNotify = false;
            timer = null;
            decrement = 0;

            this.invokerService.Receive<InitializationViewModel>(this, async(data) =>
            {
                this.frameSourceService.ChangeFrame(new Login());
            });
            this.invokerService.Receive<InvokeNotification>(this, async(data) =>
            {
                ShowNotification(data.Notification);
            });
            this.invokerService.Receive<InvokeTimerTick>(this, async(data) =>
            {
                
                decrement = data.Time;
                if (decrement == 0) { 
                    timer.Stop();
                    this.invokerService.Invoke<HomeViewModel>(new InvokeTimerTick(decrement));
                    this.invokerService.Invoke<Organaizer_v2.Window.Timer>(new InvokeTimerTick(decrement));
                    return; 
                }
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);

                timer.Tick += TimerTick;
                
                timer.Start();

               
            });

        }

        private async void ShowNotification(Notification notification)
        {
            Notification = notification;
            IsNotify = true;

            await Task.Delay(3000);

            Notification = null;
            IsNotify = false;
        }
        private void TimerTick(object sender, EventArgs e)
        {
            if (decrement > 0)
            {
                TimeSpan time = TimeSpan.FromSeconds(decrement--);

                //string str = time.ToString(@"hh\:mm\:ss");
                //TimerFiled.Text = str;

                this.invokerService.Invoke<HomeViewModel>(new InvokeTimerTick(decrement));
                this.invokerService.Invoke<Organaizer_v2.Window.Timer>(new InvokeTimerTick(decrement));

                if (decrement == 0)
                {
                    // TODO: media source
                    SoundPlayer simpleSound = new SoundPlayer(@"F:/Органайзер_v2/old-fashioned-door-bell-daniel_simon.wav");
                    simpleSound.Play();
                    //timer = new DispatcherTimer(0);
                    timer.Stop();
                }

            }

            if (decrement == 0)
            {
                // TODO: media source
                //timer = new DispatcherTimer(0);
                timer.Stop();
            }


        }

        public Notification Notification
        {
            get => this.notification;
            set
            {
                this.notification = value;
                OnPropertyChanged();
            }
        }
        public Page FrameSource
        {
            get => this.frameSource;
            set
            {
                this.frameSource = value;
                OnPropertyChanged();
            }
        }

        public bool IsNotify
        {
            get => this.isNotify;
            set
            {
                this.isNotify = value;
                OnPropertyChanged();
            }
        }
    }
}
