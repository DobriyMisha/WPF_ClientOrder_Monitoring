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
    /// Interaction logic for FormStuff.xaml
    /// </summary>
    public partial class FormStuff : Window
    {
        public FormStuff()
        {
            InitializeComponent();
            this.DataContext = new ViewStuff();
        }
    }
}
