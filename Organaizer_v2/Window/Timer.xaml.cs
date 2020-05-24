using Organaizer_v2.Model;
using Organaizer_v2.Service;
using Organaizer_v2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Organaizer_v2.Window
{
    /// <summary>
    /// Логика взаимодействия для Timer.xaml
    /// </summary>
    public partial class Timer : System.Windows.Window
    {
        private InvokerService invokerService;

        public Timer(InvokerService invokerService)
        {
            this.invokerService = invokerService;
            InitializeComponent();

            this.invokerService.Receive<InvokeTimerTick>(this, async (data) =>
            {
                TimeSpan time = TimeSpan.FromSeconds(data.Time);
                StartButton.IsEnabled = false;
                if (time.TotalSeconds == 0)
                {
                    StartButton.IsEnabled = true;
                    EndButton.IsEnabled = false;
                }
                string str = time.ToString(@"hh\:mm\:ss");
                TimerFiled.Text = str;

            });
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTime;
            if (!DateTime.TryParse(PresetTimePicker.Text, out dateTime))
            {
                MessageBox.Show("Введены неверные данные");
                return;
            }

            this.invokerService.Invoke<MainViewModel>(new InvokeTimerTick((int)dateTime.TimeOfDay.TotalSeconds));

            StartButton.IsEnabled = false;
            EndButton.IsEnabled = true;
        }

        private void End_Click(object sender, RoutedEventArgs e)
        { 

            this.invokerService.Invoke<MainViewModel>(new InvokeTimerTick(0));
        }
    }
}
