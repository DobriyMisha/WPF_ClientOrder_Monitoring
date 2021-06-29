using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dyploma_base
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        public OrderWindow(string order)
        {
            InitializeComponent();
            this.DataContext = new ViewOrderWindow(order);
        }

        private void datagridOI_LoadingRow(object sender, DataGridRowEventArgs e)
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
