using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.ComponentModel;

namespace Dyploma_base
{
    class ViewStuff : INotifyPropertyChanged
    {
        #region Service Stuff

        MySqlConnection conn = new MySqlConnection("server=localhost;userid=root;password=novasik007;database=azarov1;");
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        ServiceSQL sq = new ServiceSQL();

        #endregion

        #region Properties

        #region Collections
        private ObservableCollection<Types> _types = new ObservableCollection<Types>();
        private ObservableCollection<Materials> _materials = new ObservableCollection<Materials>();
        private ObservableCollection<Additions> _additions = new ObservableCollection<Additions>();

        public ObservableCollection<Types> types
        {
            get { return _types; }
            set
            {
                _types = value;
            }
        }
        public ObservableCollection<Materials> materials
        {
            get { return _materials; }
            set
            {
                _materials = value;
            }
        }
        public ObservableCollection<Additions> additions
        {
            get { return _additions; }
            set
            {
                _additions = value;
            }
        }
        #endregion

        #region Selected
        private string selectedid = "";

        private Types _select_type = new Types();
        public Types Select_Type
        {
            get { return _select_type; }
            set
            {
                _select_type = value;
                if(value != null)
                {
                    selectedid = value.id.ToString();
                    //do stuff
                    whichbox = 0;
                    _priceIsEnabled = false;
                    tb_change_descr = value.description.ToString();
                    tb_change_price = null;
                }
                
            }
        }

        private Materials _select_mat = new Materials();
        public Materials Select_Mat
        {
            get { return _select_mat; }
            set
            {
                _select_mat = value;
                if(value != null)
                {
                    selectedid = _select_mat.id.ToString();
                    //do stuff
                    whichbox = 1;
                    _priceIsEnabled = true;
                    tb_change_descr = "";
                    tb_change_price = "";
                    try
                    {
                        tb_change_descr = _select_mat.description.ToString();
                        tb_change_price = _select_mat.price.ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                }
               
            }
        }

        private Additions _select_ad = new Additions();
        public Additions Select_Ad
        {
            get { return _select_ad; }
            set
            {
                _select_ad = value;
                if(value != null)
                {
                    selectedid = _select_ad.id.ToString();
                    //do stuff
                    whichbox = 2;
                    _priceIsEnabled = true;
                    tb_change_descr = value.description.ToString();
                    tb_change_price = value.price.ToString();
                }
               
            }
        }

        private bool _priceIsEnabled;
        public bool priceIsEnabled
        {
            get { return _priceIsEnabled; }
            set
            {
                _priceIsEnabled = value;
            }
        }

        #endregion

        #region Other
        private int whichbox;

        //type add tb
        private string _tb_addtype;
        public string tb_addtype
        {
            get { return _tb_addtype; }
            set
            {
                _tb_addtype = value;
            }
        }
        //material add tb
        private string _tb_addmat_descr;
        public string tb_addmat_descr
        {
            get { return _tb_addmat_descr; }
            set
            {
                _tb_addmat_descr = value;
            }
        }
        //price add tb
        private string _tb_addmat_price;
        public string tb_addmat_price
        {
            get { return _tb_addmat_price; }
            set
            {
                _tb_addmat_price = value;
            }
        }
        //additional add tb 
        private string _tb_addad_descr;
        public string tb_addad_descr
        {
            get { return _tb_addad_descr; }
            set
            {
                _tb_addad_descr = value;
            }
        }
        private string _tb_addad_price;
        public string tb_addad_price
        {
            get { return _tb_addad_price; }
            set
            {
                _tb_addad_price = value;
            }
            
        }
        //changing stuff tb
        private string _tb_change_descr;
        public string tb_change_descr
        {
            get { return _tb_change_descr; }
            set
            {
                _tb_change_descr = value;
            }
        }
        private string _tb_change_price;
        public string tb_change_price
        {
            get { return _tb_change_price; }
            set
            {
                _tb_change_price = value;
            }
        }

        #endregion

        #endregion

        #region Commands

        //команда клик по номеру заказа
        private readonly DelegateCommand<string> _click_add_type;
        public DelegateCommand<string> Click_add_type
        {
            get { return _click_add_type; }
        }

        //команда клик по номеру заказа
        private readonly DelegateCommand<string> _click_add_mat;
        public DelegateCommand<string> Click_add_mat
        {
            get { return _click_add_mat; }
        }

        //команда клик по номеру заказа
        private readonly DelegateCommand<string> _click_add_ad;
        public DelegateCommand<string> Click_add_ad
        {
            get { return _click_add_ad; }
        }

        //команда клик по номеру заказа
        private readonly DelegateCommand<string> _click_change_stuff;
        public DelegateCommand<string> Click_change_stuff
        {
            get { return _click_change_stuff; }
        }

        private readonly DelegateCommand<string> _click_items_stuff;
        public DelegateCommand<string> click_items_stuff
        {
            get { return _click_items_stuff; }
        }

        #endregion

        #region Function

        private void types_fill()
        {
            types.Clear();
            types = sq.types();
        }
        private void materials_fill()
        {
            materials.Clear();
            materials = sq.materials();
        }
        private void additions_fill()
        {
            additions.Clear();
            additions = sq.additions();
        }
        private void changestuff()
        {
            switch (whichbox)
            {
                case 0:
                    sq.update_itemtype(selectedid, _tb_change_descr);
                    types_fill();
                    break;
                case 1:
                    sq.update_material(selectedid, _tb_change_descr, _tb_change_price);
                    materials_fill();
                    break;
                case 2:
                    sq.update_additional(selectedid, _tb_change_descr, _tb_change_price);
                    additions_fill();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Constructor

        public ViewStuff()
        {
            types_fill();
            materials_fill();
            additions_fill();
            _priceIsEnabled = true;
            #region Init Commands

            //команда добавления нового типа
            _click_add_type = new DelegateCommand<string>(
            (s) => { /* perform some action */ sq.insert_service_itemtype(tb_addtype); types_fill(); }//, //Execute
            //(s) => {  } //CanExecute
            );
            //команда добавления нового материала
            _click_add_mat = new DelegateCommand<string>(
            (s) => { /* perform some action */ sq.insert_service_materials(tb_addmat_descr,tb_addmat_price); materials_fill(); }//, //Execute
            //(s) => {  } //CanExecute
            );
            //команда добавление новой услуги
            _click_add_ad = new DelegateCommand<string>(
            (s) => { /* perform some action */ sq.insert_service_additions(tb_addad_descr,tb_addad_price); additions_fill(); }//, //Execute
            //(s) => {  } //CanExecute
            );
            //команда клика на рефреш
            _click_change_stuff = new DelegateCommand<string>(
            (s) => { /* perform some action */ changestuff(); }//, //Execute
            //(s) => {  } //CanExecute
            );
            _click_items_stuff = new DelegateCommand<string>(
            (s) => { /* perform some action */ ItemWindow iw = new ItemWindow("0"); iw.Show(); }//, //Execute
            //(s) => {  } //CanExecute
            );

            #endregion

        }

        #endregion

    }

    class Types
    {

        private string _id;
        private string _description;

        public string id
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
    }

    class Materials
    {

        private string _id;
        private string _description;
        private string _price;

        public string id
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

        public string price
        {
            get { return _price; }
            set
            {
                _price = value;
            }
        }

    }

    class Additions
    {
        private string _id;
        private string _description;
        private string _price;

        public string id
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

        public string price
        {
            get { return _price; }
            set
            {
                _price = value;
            }
        }
    }

    class Items
    {
        private string _id;
        private string _name;
        private string _type;
        private string _material;
        private string _price;
        private string _sizing;

        public string id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;    
            }
        }
        public string type
        {
            get { return _type; }
            set
            {
                _type = value;
            }
        }
        public string material
        {
            get { return _material; }
            set
            {
                _material = value;
            }
        }
        public string price
        {
            get { return _price; }
            set
            {
                _price = value;
            }
        }
        public string sizing
        {
            get { return _sizing; }
            set
            {
                _sizing = value;
            }
        }
    }

}
