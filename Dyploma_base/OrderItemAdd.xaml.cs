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
    /// Interaction logic for OrderItemAdd.xaml
    /// </summary>
    public partial class OrderItemAdd : Window
    {
        public OrderItemAdd(string p)
        {
            InitializeComponent();
            if (p != null)
            {
                this.DataContext = new ViewAddToOrder(p);
            }
        }
    }
}
