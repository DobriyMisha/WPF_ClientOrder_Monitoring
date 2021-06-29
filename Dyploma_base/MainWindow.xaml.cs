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
using System.Data;

namespace Dyploma_base
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainClass();
        }


        #region Events

        #endregion

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void dataGrid1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                switch (((Orders)(e.Row.DataContext)).status.ToString())
                {
                    case "3. Готов":
                        e.Row.Background = new SolidColorBrush(Colors.LightGreen);
                        break;
                    case "5. Закрыт":
                        e.Row.Background = new SolidColorBrush(Colors.Gray);
                        //e.Row.Visibility = (Visibility)0;
                        break;
                    case "2. Обработан":
                        e.Row.Background = new SolidColorBrush(Colors.Aquamarine);
                        break;
                    case "1. Новый":
                        e.Row.Background = new SolidColorBrush(Colors.LightBlue);
                        break;
                    case "4. Проблема":
                        e.Row.Background = new SolidColorBrush(Colors.IndianRed);
                        break;
                    default:
                        e.Row.Background = new SolidColorBrush(Colors.LightGray);
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGrid2_LoadingRow(object sender, DataGridRowEventArgs e)
        {
           try
            {
                switch (((OrderItems)(e.Row.DataContext)).statusitem.ToString())
                {
                    case "3. Готов":
                        e.Row.Background = new SolidColorBrush(Colors.LightGreen);
                        break;
                    case "2. В работе":
                        e.Row.Background = new SolidColorBrush(Colors.Aquamarine);
                        break;
                    case "1. В очереди":
                        e.Row.Background = new SolidColorBrush(Colors.LightBlue);
                        break;
                    case "4. Проблема":
                        e.Row.Background = new SolidColorBrush(Colors.IndianRed);
                        break;
                    default:
                        e.Row.Background = new SolidColorBrush(Colors.LightGray);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
