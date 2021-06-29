using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Dyploma_base
{
    class ViewOrderWindow : INotifyPropertyChanged
    {

        #region Service Stuff

        MySqlConnection conn = new MySqlConnection("server=localhost;userid=mainuser;password=novasik007;database=azarov1;");
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        ServiceSQL sq = new ServiceSQL();

        #endregion

        #region Properties

        private string _ordertitle;
        public string OrderTitle
        {
            get
            {
                return _ordertitle;
            }
            set
            {
                _ordertitle = value;
            }
        }

        private string _orderid;
        public string orderid
        {
            get
            {
                return _orderid;
            }
            set
            {
                _orderid = value;
                OrderTitle = "Редактор заявки #" + _orderid + ".";
            }
        }

        private string _fio;
        public string fio
        {
            get
            {
                return _fio;
            }
            set
            {
                _fio = value;
            }
        }

        private string _status;
        public string status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        private string _opendate;
        public string opendate
        {
            get
            {
                return _opendate;
            }
            set
            {
                _opendate = value;
            }
        }

        private string _deadline;
        public string deadline
        {
            get
            {
                return _deadline;
            }
            set
            {
                _deadline = value;
            }
        }

        private string _closedate;
        public string closedate
        {
            get
            {
                return _closedate;
            }
            set
            {
                _closedate = value;
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

        private ObservableCollection<AdditionalsOnItem> _additionals = new ObservableCollection<AdditionalsOnItem>();
        public ObservableCollection<AdditionalsOnItem> additionals
        {
            get { return _additionals; }
            set
            {
                _additionals = value;
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
                update_status1();
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

        #region Commands

        private readonly DelegateCommand<string> _clickCreatePlane;
        public DelegateCommand<string> clickCreatePlane
        {
            get { return _clickCreatePlane; }
        }

        private readonly DelegateCommand<string> _clickInsertNew;
        public DelegateCommand<string> clickInsertNew
        {
            get { return _clickInsertNew; }
        }

        private readonly DelegateCommand<string> _clickUpdateOrder;
        public DelegateCommand<string> clickUpdateOrder
        {
            get { return _clickUpdateOrder; }
        }

        private readonly DelegateCommand<string> _clickRefresh;
        public DelegateCommand<string> clickRefresh
        {
            get { return _clickRefresh; }
        }

        private readonly DelegateCommand<string> _clickOrderItems;
        public DelegateCommand<string> clickOrderItems
        {
            get { return _clickOrderItems; }
        }

        private readonly DelegateCommand<string> _clickAdditionals;
        public DelegateCommand<string> clickAdditionals
        {
            get { return _clickAdditionals; }
        }

        private readonly DelegateCommand<string> _edit_details;
        public DelegateCommand<string> edit_details
        {
            get { return _edit_details; }
        }

        private OrderItems _select_request_oitems;
        public OrderItems Select_request_oitems
        {
            get { return _select_request_oitems; }
            set
            {
                _select_request_oitems = value;
                if (value != null)
                {
                    additionalsfill(value.id.ToString());
                }
                //do stuff
            }
        }

        //команда обновить статус изделия
        private readonly DelegateCommand<string> _click_update_status2;
        public DelegateCommand<string> Click_update_status2
        {
            get { return _click_update_status2; }
        }

        private readonly DelegateCommand<string> _click_todayopen;
        public DelegateCommand<string> click_todayopen
        {
            get { return _click_todayopen; }
        }

        private readonly DelegateCommand<string> _click_todayclose;
        public DelegateCommand<string> click_todayclose
        {
            get { return _click_todayclose; }
        }


        #endregion

        #region Constructor

        public ViewOrderWindow()
        {
            LoadPlaneOrder();

            #region Commands Init
            _click_todayopen = new DelegateCommand<string>(
            (s) => { /* perform some action */ opendate = DateTime.Today.ToString("yyyy-MM-dd"); }//, //Execute
            //(s) => {  } //CanExecute
            );
            _click_todayclose = new DelegateCommand<string>(
           (s) => { /* perform some action */ closeorder(); }//, //Execute
                                                             //(s) => {  } //CanExecute
           );
            _clickUpdateOrder = new DelegateCommand<string>(
           (s) => { /* perform some action */ update_order(); }//, //Execute                                                                                
           //(s) => {  } //CanExecute
           );
            _clickRefresh = new DelegateCommand<string>(
           (s) => { /* perform some action */ sq.LoadOrder(ref _orderid, ref _fio, ref _status, ref _opendate, ref _deadline, ref _closedate); if (orderid != null)
               {
                   orderitemsfill();
               }
           }//, //Execute                                                                                
           //(s) => {  } //CanExecute
           );
            _clickCreatePlane = new DelegateCommand<string>(
           (s) => { /* perform some action */ LoadPlaneOrder(); }//, //Execute                                                                                
           //(s) => {  } //CanExecute
           );
            _clickInsertNew = new DelegateCommand<string>(
          (s) => { /* perform some action */
              sq.insert_new_order(ref _orderid, ref _fio, ref _status, ref _opendate, ref _deadline, ref _closedate);
              sq.LoadOrder(ref _orderid, ref _fio, ref _status, ref _opendate, ref _deadline, ref _closedate); }//, //Execute                                                                                
                                                                                                                //(s) => {  } //CanExecute
          );
            #endregion

        }

        public ViewOrderWindow(string p_order)
        {
            if (Convert.ToInt32(p_order) == 0)
            {
                LoadPlaneOrder();
            }
            else
            {
                orderid = p_order;
                sq.LoadOrder(ref _orderid, ref _fio, ref _status, ref _opendate, ref _deadline, ref _closedate);
                if (orderid != null)
                {
                    orderitemsfill();
                }
            }

            #region Commands Init

            _click_todayopen = new DelegateCommand<string>(
            (s) => { /* perform some action */ opendate = DateTime.Today.ToString("yyyy-MM-dd"); }//, //Execute
            //(s) => {  } //CanExecute
            );
            _click_todayclose = new DelegateCommand<string>(
            (s) => { /* perform some action */ closeorder(); }//, //Execute
            //(s) => {  } //CanExecute
            );
            _clickUpdateOrder = new DelegateCommand<string>(
           (s) => { /* perform some action */ update_order(); }//, //Execute                                                                                
           //(s) => {  } //CanExecute
           );
            _clickRefresh = new DelegateCommand<string>(
           (s) => { /* perform some action */
               sq.LoadOrder(ref _orderid, ref _fio, ref _status, ref _opendate, ref _deadline, ref _closedate); if (orderid != null)
               {
                   orderitemsfill();
               }
           }//, //Execute                                                                                
           //(s) => {  } //CanExecute
           );
            _clickCreatePlane = new DelegateCommand<string>(
          (s) => { /* perform some action */ LoadPlaneOrder(); }//, //Execute                                                                                
                                                                //(s) => {  } //CanExecute
          );
            _clickInsertNew = new DelegateCommand<string>(
          (s) => { /* perform some action */ sq.insert_new_order(ref _orderid, ref _fio, ref _status, ref _opendate, ref _deadline, ref _closedate);
              sq.LoadOrder(ref _orderid, ref _fio, ref _status, ref _opendate, ref _deadline, ref _closedate); }//, //Execute                                                                                
                                                                                                                //(s) => {  } //CanExecute
          );
            _click_update_status2 = new DelegateCommand<string>(
            (s) => { /* perform some action */ update_status_item(); }//, //Execute
            //(s) => {  } //CanExecute
            );
            _edit_details = new DelegateCommand<string>(
            (s) => { /* perform some action */ editdet(); }//, //Execute
            //(s) => {  } //CanExecute
            );

            #endregion

        }

        #endregion

        #region Functions
        private void LoadPlaneOrder()
        {
            orderid = null;
            fio = null;
            status = null;
            opendate = null;
            closedate = null;
            deadline = null;
            additionals.Clear();
            orderItems.Clear();
        }

        private void editdet()
        {
            if(orderid != null)
            {
                OrderItemAdd idsos = new OrderItemAdd(orderid);
                idsos.Show();
            }
            
        }

        private void closeorder()
        {
            //closedate = DateTime.Now.ToString();
            sq.CloseOrder(ref _orderid, ref _closedate);
        }

        private void orderitemsfill()
        {
            orderItems.Clear();
            orderItems = sq.orderitemsfill(orderid);
           //LastSelected_OI = p;
        }

        private void additionalsfill(string oi)
        {
            additionals.Clear();
            additionals = sq.additionalsfill(oi);
        }


        //ТУТ ОШИБКА ОТОБРАЖЕНИЯ СТАТУСА ПОСЛЕ ЕГО ИЗМЕНЕНИЯ (ИЗМЕНЯЕТСЯ В Бд, НО НЕ ОТОБРАЖАЕТСЯ)
        private void update_status1()
        {
            if (Select_status1.Content.ToString() != null && orderid != null)
            {
                sq.StatusUpdate(_select_status1.Content.ToString(), orderid);
                //status = null;
                sq.LoadOrder(ref _orderid, ref _fio, ref _status, ref _opendate, ref _deadline, ref _closedate);
                if (orderid != null)
                {
                    orderitemsfill();
                }
            }
           
        }

        //private string LastSelected_OI;
        private void update_status_item()
        {
            if (Select_status2.Content.ToString() != null && Select_request_oitems != null)
            {
                sq.StatusItemUpdate(Select_status2.Content.ToString(), Select_request_oitems.id.ToString());
            } 

            orderitemsfill();
        }

        private void update_order()
        {
            sq.FullOrderUpdate(ref _orderid, ref _fio, ref _status, ref _opendate, ref _deadline, ref _closedate);
            sq.LoadOrder(ref _orderid, ref _fio, ref _status, ref _opendate, ref _deadline, ref _closedate);
        }


        #endregion


    }
}
