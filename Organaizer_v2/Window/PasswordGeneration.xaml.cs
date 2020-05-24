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
using System.Windows.Shapes;

namespace Organaizer_v2.Window
{
    /// <summary>
    /// Логика взаимодействия для PasswordGeneration.xaml
    /// </summary>
    public partial class PasswordGeneration : System.Windows.Window
    {
        public PasswordGeneration()
        {
            InitializeComponent();
            create.IsEnabled = false;
            copy.IsEnabled = false;
        }
        private void count_TextChanged(object sender, TextChangedEventArgs e)
        {
            create.IsEnabled = true;
            copy.IsEnabled = true;
        }

        private void CreatePass(object sender, RoutedEventArgs e)
        {
            try
            {
                if (count.Text != "")
                {
                    create.IsEnabled = true;

                    txt.Text = "";
                    bool rr = Int32.TryParse(count.Text, out int IntCount);
                    if (rr == false)
                    {
                        throw new Exception("Введите цифры!");
                    }

                    string abc = "0";
                    if (lowcase.IsChecked == true)//использовать маленькие буквыs
                        abc += "qwertyuiopasdfghjklzxcvbnm";
                    if (uppercase.IsChecked == true)//использовать заглавные
                        abc += "QWERTYUIOPASDFGHJKLZXCVBNM";
                    if (spec.IsChecked == true)//использовать спецсимволы
                        abc += "!@#$%^&*()";
                    if (numeral.IsChecked == true)//юзать цифры
                        abc += "123456789";
                    Random rnd = new Random();
                    for (int i = 0; i < int.Parse(count.Text); i++)
                        txt.Text += abc[rnd.Next(abc.Length)];
                }
                else
                {
                    //ErrorSymbols erroremp;
                    //erroremp = new ErrorSymbols();
                    //erroremp.Show();
                    create.IsEnabled = false;
                    copy.IsEnabled = false;
                }

            }
            catch (Exception)
            {
                //ErrorSymbols errors;
                //errors = new ErrorSymbols();
                //errors.Show();
                count.Text = "";
            }
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                copy.IsEnabled = false;
                if (txt.Text != "")
                {
                    copy.IsEnabled = true;
                }
                Clipboard.SetText(txt.Text);
            }
            catch (Exception) { }
        }
    }
}
