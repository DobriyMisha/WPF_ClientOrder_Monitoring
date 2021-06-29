using System;
using System.ComponentModel;
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
using MySql.Data.MySqlClient;

namespace Dyploma_base
{
    //класс, описывающий представление view_orders
    public class Orders : INotifyPropertyChanged
    {
        //interface for fody inotify
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        private int _id;
        private string _FIO;
        private string _status;
        private string _created;
        private string _deadline;
        private string _closed;

        #region Public Properties
        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }
        public string FIO
        {
            get { return _FIO; }
            set
            {
                _FIO = value;
            }
        }
        public string status
        {
            get { return _status; }
            set
            {
                _status = value;
            }
        }
        public string created
        {
            get { return _created; }
            set
            {
                _created = value;
            }
        }
        public string deadline
        {
            get { return _deadline; }
            set
            {
                _deadline = value;
            }
        }
        public string closed
        {
            get { return _closed; }
            set
            {
                _closed = value;
            }
        }
        #endregion

        public Orders()
        {

        }

        
    }

    public class OrderItems : INotifyPropertyChanged
    {
        private int _id;
        private int _orderid;
        private int _quantity;
        private string _itemname;
        private int _price;
        private string _statusitem;

        #region Public Properties

        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }

        public int orderid
        {
            get { return _orderid; }
            set
            {
                _orderid = value;
            }
        }

        public int quantity
        {
            get { return _quantity; }
            set
            {
                if(value.ToString() == null) _quantity = 1;
                else _quantity = value;
            }
        }

        public string itemname
        {
            get { return _itemname; }
            set
            {
                _itemname = value;
            }
        }

        public int price
        {
            get { return _price; }
            set
            {
                _price = value;
            }
        }

        public string statusitem
        {
            get { return _statusitem; }
            set
            {
                _statusitem = value;
            }
        }

        #endregion

        public OrderItems()
        {

        }

        //interface for fody inotify
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }

    public class AdditionalsOnItem : INotifyPropertyChanged
    {
        private int _id;
        private string _description;
        private int _price;

        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }
        
        public string description
        {
            get { return _description; }
            set
            {
                _description = value;
            }
        }
        
        public int price
        {
            get { return _price; }
            set
            {
                _price = value;
            }
        }

        //interface for fody inotify
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }

}

