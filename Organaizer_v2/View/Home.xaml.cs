using Organaizer_v2.Service;
using Organaizer_v2.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Organaizer_v2.View
{
    /// <summary>
    /// Логика взаимодействия для Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        private InvokerService invokerService;
        public Home(InvokerService invokerService)
        {
            this.invokerService = invokerService;
            InitializeComponent();
        }

        private void OpenAboutWindow(object sender, RoutedEventArgs e)
        {
            Author author = new Author();
            author.Show();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenGeneratePassword(object sender, RoutedEventArgs e)
        {
            PasswordGeneration genPassword = new PasswordGeneration();
            genPassword.Show();
        }

        private void OpenTimer(object sender, RoutedEventArgs e)
        {
            Timer timer = new Timer(this.invokerService);
            timer.Show();
        }
    }
}
