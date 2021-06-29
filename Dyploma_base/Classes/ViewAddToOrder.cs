using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Dyploma_base
{
    class ViewAddToOrder : INotifyPropertyChanged
    {
        #region Service Stuff

        MySqlConnection conn = new MySqlConnection("server=localhost;userid=mainuser;password=novasik007;database=azarov1;");
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        ServiceSQL sq = new ServiceSQL();

        #endregion

        #region Properties

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
            }
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

        private OrderItems _select_request_additionals;
        public OrderItems Select_request_additionals
        {
            get { return _select_request_additionals; }
            set
            {
                _select_request_additionals = value;
                
                //do stuff
            }
        }

        private Items _select_items;
        public Items Select_items
        {
            get { return _select_items; }
            set
            {
                _select_items = value;
                
                //do stuff
            }
        }

        private Additions _select_additions;
        public Additions Select_additions
        {
            get { return _select_additions; }
            set
            {
                _select_additions = value;

                //do stuff
            }
        }

        private string _quantity;
        public string quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
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

        private ObservableCollection<Items> _Items = new ObservableCollection<Items>();
        public ObservableCollection<Items> Items
        {
            get { return _Items; }
            set
            {
                _Items = value;
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

        private ObservableCollection<Additions> _additions = new ObservableCollection<Additions>();
        public ObservableCollection<Additions> additions
        {
            get { return _additions; }
            set
            {
                _additions = value;
            }
        }

        #endregion

        #region Commands


        private readonly DelegateCommand<string> _click_update_quan;
        public DelegateCommand<string> click_update_quan
        {
            get { return _click_update_quan; }
        }

        private readonly DelegateCommand<string> _click_add_oi;
        public DelegateCommand<string> click_add_oi
        {
            get { return _click_add_oi; }
        }

        private readonly DelegateCommand<string> _click_del_oi;
        public DelegateCommand<string> click_del_oi
        {
            get { return _click_del_oi; }
        }

        private readonly DelegateCommand<string> _click_add_ad;
        public DelegateCommand<string> click_add_ad
        {
            get { return _click_add_ad; }
        }

        private readonly DelegateCommand<string> _click_del_ad;
        public DelegateCommand<string> click_del_ad
        {
            get { return _click_del_ad; }
        }

        #endregion

        #region Functions
        private void loaditems()
        {
            Items.Clear();
            Items = sq.items();
        }
        private void additions_fill()
        {
            additions.Clear();
            additions = sq.additions();
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
        private void add_oi()
        {
            if(_select_items != null)
            {
                sq.insert_orderitem(orderid, _select_items.id.ToString());
                orderitemsfill();
            }
            
        }
        private void del_oi()
        {
            sq.remove_orderitem(_select_request_oitems.id.ToString());
            orderitemsfill();
        }
        private void add_ad()
        {
            sq.insert_oi_additional(_select_request_oitems.id.ToString(), _select_additions.id.ToString());
            additionalsfill(_select_request_oitems.id.ToString());
        }
        private void del_ad()
        {
            sq.remove_oi_additional(_select_request_additionals.id.ToString());
            additionalsfill(_select_request_oitems.id.ToString());
        }
        private void updatequan()
        {
            sq.update_orderitem_quantity(_select_request_oitems.id.ToString(), quantity); 
            orderitemsfill();
        }

        #endregion

        #region Constructor

        public ViewAddToOrder(string oi)
        {
            
            loaditems();
            additions_fill();
            orderid = oi;
            orderitemsfill();

            _click_update_quan = new DelegateCommand<string>(
            (s) => { /* perform some action */ updatequan(); }//, //Execute
            //(s) => {  } //CanExecute
            );
            _click_add_oi = new DelegateCommand<string>(
           (s) => { /* perform some action */ add_oi(); }//, //Execute
           //(s) => {  } //CanExecute
           );
            _click_del_oi = new DelegateCommand<string>(
           (s) => { /* perform some action */ del_oi(); }//, //Execute
           //(s) => {  } //CanExecute
           );
            _click_add_ad = new DelegateCommand<string>(
           (s) => { /* perform some action */ add_ad(); }//, //Execute
           //(s) => {  } //CanExecute
           );
            _click_del_ad = new DelegateCommand<string>(
           (s) => { /* perform some action */ del_ad(); }//, //Execute
           //(s) => {  } //CanExecute
           );
        }

        #endregion
    }
}
