using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
//using System.Data.DataSetExtensions.dll;

namespace Dyploma_base
{
    class MainClass : INotifyPropertyChanged
    {

        #region Service Stuff

        MySqlConnection conn = new MySqlConnection("server=localhost;userid=mainuser;password=novasik007;database=azarov1;");
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        ServiceSQL sq = new ServiceSQL();

        #endregion

        private string LastSelected_OI;

        #region Properties
        public string OrderFedos
        {
            get;
            set;
        }

        private ObservableCollection<Orders> _orders = new ObservableCollection<Orders>();
        public ObservableCollection<Orders> orders
        {
            get { return _orders; }
            set
            {
                _orders = value;
            }
        }

        private ObservableCollection<OrderItems> _orderItems = new ObservableCollection<OrderItems>();
        public ObservableCollection<OrderItems> orderItems
        {
            get { return _orderItems; }
            set
            {
                _orderItems = value;
            }
        }

        //текстбокс поиск по номеру заказа
        private string _tbSearchBy_OrderNumber;
        public string tbSearchBy_OrderNumber
        {
            get
            {
                return this._tbSearchBy_OrderNumber;  
            }
            set
            {
                this._tbSearchBy_OrderNumber = value;
               
            }
        }

        //текстбокс поиск по имени клиента
        private string _tbSearchBy_Name;
        public string tbSearchBy_Name
        {
            get
            {
                return this._tbSearchBy_Name;
            }
            set
            {
                this._tbSearchBy_Name = value;

            }
        }

        #endregion

        #region Commands

        //команда клик рефреш основного датагрида
        private readonly DelegateCommand<string> _clickCommandRefresh;
        public DelegateCommand<string> ButtonClickCommandRefresh
        {
            get { return _clickCommandRefresh; }
        }

        //команда клик по номеру заказа
        private readonly DelegateCommand<string> _clickSearchBy_OrderNumber;
        public DelegateCommand<string> Click_SearchBy_OrderNumber
        {
            get { return _clickSearchBy_OrderNumber; }
        }

        //команда клик по имени
        private readonly DelegateCommand<string> _clickSearchBy_Name;
        public DelegateCommand<string> Click_clickSearchBy_Name
        {
            get { return _clickSearchBy_Name; }
        }

        //команда обновить статус заявки
        private readonly DelegateCommand<string> _click_update_status1;
        public DelegateCommand<string> Click_update_status1
        {
            get { return _click_update_status1; }
        }

        //команда обновить статус изделия
        private readonly DelegateCommand<string> _click_update_status2;
        public DelegateCommand<string> Click_update_status2
        {
            get { return _click_update_status2; }
        }

        //команда обновить статус изделия
        private readonly DelegateCommand<string> _click_open_redactor_select;
        public DelegateCommand<string> Click_open_redactor_select
        {
            get { return _click_open_redactor_select; }
        }

        private readonly DelegateCommand<string> _click_items_stuff;
        public DelegateCommand<string> click_items_stuff
        {
            get { return _click_items_stuff; }
        }


        //селект в основном датагриде для вывода изделий
        private Orders _select_request;
        public Orders Select_Request
        {
            get { return _select_request; }
            set
            {
                _select_request = value;
                if(value!=null)
                {
                    orderitemsfill(value.id.ToString());
                    SelectedOrder = value.id.ToString();
                }
               //do stuff
                
            }
        }

        private readonly DelegateCommand<string> _serviceOpen;
        public DelegateCommand<string> serviceOpen
        {
            get { return _serviceOpen; }
        }

        private string SelectedOrder;

        //селект во вторичном датагриде 
        private OrderItems _select_request_oitems;
        public OrderItems Select_request_oitems
        {
            get { return _select_request_oitems; }
            set
            {
                _select_request_oitems = value;

            }
        }

        //селект в первом боксе статусов
        private ComboBoxItem _select_status1;
        public ComboBoxItem Select_status1
        {
            get { return _select_status1; }
            set
            {
                _select_status1 = value;
                
                //do stuff

            }
        }

        //селект во втором боксе статусов
        private ComboBoxItem _select_status2;
        public ComboBoxItem Select_status2
        {
            get { return _select_status2; }
            set
            {
                _select_status2 = value;
                
                //do stuff

            }
        }


        #endregion

        #region Constructor

        //КОНСТРУКТОР
        public MainClass()
        {

            conn.Open();

            MySqlCommand command = new MySqlCommand("SELECT b.fio FROM azarov1.orders a left join azarov1.clients b on a.clientid = b.id where a.id = 1;", conn);
            OrderFedos = command.ExecuteScalar().ToString();

            conn.Close();

            ordersfill();

            #region Commands Initialization

            //команда клика на рефреш
            _clickCommandRefresh = new DelegateCommand<string>(
            (s) => { /* perform some action */ ordersfill(); }//, //Execute
            //(s) => {  } //CanExecute
            );

            //команда клика на поиск по номеру заказа
            _clickSearchBy_OrderNumber = new DelegateCommand<string>(
            (s) => { /* perform some action */ searchby_ordernumb(tbSearchBy_OrderNumber); }//, //Execute
            //(s) => {  } //CanExecute
            );

            //команда клика на поиск по имени
            _clickSearchBy_Name = new DelegateCommand<string>(
            (s) => { /* perform some action */ searchby_fio(tbSearchBy_Name); }//, //Execute
            //(s) => {  } //CanExecute
            );

            _click_update_status1 = new DelegateCommand<string>(
            (s) => { /* perform some action */ update_status1();  }//, //Execute
            //(s) => {  } //CanExecute
            );

            _click_update_status2 = new DelegateCommand<string>(
            (s) => { /* perform some action */ update_status_item(); }//, //Execute
            //(s) => {  } //CanExecute
            );

            _click_open_redactor_select = new DelegateCommand<string>(
            (s) => { /* perform some action */ open_redactor(); }//, //Execute
            //(s) => {  } //CanExecute
            );

            _serviceOpen = new DelegateCommand<string>(
            (s) => { /* perform some action */ FormStuff fs = new FormStuff(); fs.Show(); }//, //Execute
            //(s) => {  } //CanExecute
            );

            _click_items_stuff = new DelegateCommand<string>(
           (s) => { /* perform some action */ ItemWindow iw = new ItemWindow("0"); iw.Show(); }//, //Execute
           //(s) => {  } //CanExecute
           );
            #endregion

        }

        #endregion

        #region Functions

        private void ordersfill()
        {
            orders.Clear();
            orders = sq.orders_Fill();
        }
        
        private void orderitemsfill(string p)
        {
            orderItems.Clear();
            orderItems = sq.orderitemsfill(p);
            LastSelected_OI = p;
        }

        private void searchby_ordernumb(string par_order)
        {
            orders.Clear();
            ordersfill();
            orders = sq.searchby_ordernumb(_orders,par_order);
        }

        private void searchby_fio(string par_fio)
        {
            orders.Clear();
            ordersfill();
            orders = sq.searchby_name(_orders, par_fio);
        }

        private void update_status1()
        {
            if(Select_status1.Content.ToString() != null && Select_Request != null)
            {
                sq.StatusUpdate(Select_status1.Content.ToString(), Select_Request.id.ToString());
            }
            orders.Clear();
            ordersfill();
        }
        private void update_status_item()
        {
            if (Select_status2.Content.ToString() != null && Select_request_oitems != null)
            {
                sq.StatusItemUpdate(Select_status2.Content.ToString(), Select_request_oitems.id.ToString());
            }
            
            orderitemsfill(LastSelected_OI);
        }

        private void open_redactor()
        {
            if(SelectedOrder != null)
            {
                OrderWindow ow = new OrderWindow(SelectedOrder);
                ow.Show();
            }
            else
            {
                MessageBox.Show("Перед нажатием на кнопку, выберите заявку в окне заявок!", "Не выбрана заявка", MessageBoxButton.OK);
            }
           
        }

        #endregion
    }

    #region DelegateCommand Class

    public class DelegateCommand<T> : System.Windows.Input.ICommand where T : class
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public DelegateCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public DelegateCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            return _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }

    #endregion


}
