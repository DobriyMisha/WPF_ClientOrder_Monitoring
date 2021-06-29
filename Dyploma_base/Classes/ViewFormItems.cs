using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.Windows.Controls;

namespace Dyploma_base
{
    class ViewFormItems : INotifyPropertyChanged
    {
        #region Service Stuff

        MySqlConnection conn = new MySqlConnection("server=localhost;userid=mainuser;password=novasik007;database=azarov1;");
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        ServiceSQL sq = new ServiceSQL();

        #endregion

        #region Commands

        private readonly DelegateCommand<string> _refreshall;
        public DelegateCommand<string> refreshall
        {
            get { return _refreshall; }
        }

        private readonly DelegateCommand<string> _changeitem;
        public DelegateCommand<string> changeitem
        {
            get { return _changeitem; }
        }

        private readonly DelegateCommand<string> _createblank;
        public DelegateCommand<string> createblank
        {
            get { return _createblank; }
        }

        private readonly DelegateCommand<string> _addnew;
        public DelegateCommand<string> addnew
        {
            get { return _addnew; }
        }

        #endregion

        #region Properties

        private ObservableCollection<Materials> _materials = new ObservableCollection<Materials>();
        public ObservableCollection<Materials> materials
        {
            get
            {
                return _materials;
            }
            set
            {
                _materials = value;
            }
        }

        private ObservableCollection<Types> _types = new ObservableCollection<Types>();
        public ObservableCollection<Types> types
        {
            get
            {
                return _types;
            }
            set
            {
                _types = value;
            }
        }

        private ObservableCollection<Items> _items = new ObservableCollection<Items>();
        public ObservableCollection<Items> items
        {
            get { return _items; }
            set
            {
                _items = value;
            }
        }

        private Items _selected_item = new Items();
        public Items Selected_Item
        {
            get { return _selected_item; }
            set
            {
                _selected_item = value;
                id = _selected_item.id;
                name = _selected_item.name;
                type = _selected_item.type;
                material = _selected_item.material;
                price = _selected_item.price;
                sizing = _selected_item.sizing;
            }
        }

        private Types _cb_type;
        public Types cb_type
        {
            get { return _cb_type; }
            set
            {
                _cb_type = value;
                //do stuff
                type = _cb_type.description.ToString();
            }
        }

        private Materials _cb_material;
        public Materials cb_material
        {
            get { return _cb_material; }
            set
            {
                _cb_material = value;
                //do stuff
                material = _cb_material.description.ToString();
            }
        }

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
        #endregion

        #region Functions

        private void loaditems()
        {
            items.Clear();
            items = sq.items();
        }

        private void clearitem()
        {
            id = null;
            name = null;
            type = null;
            material = null;
            price = null;
            sizing = null;

        }

        private void load_currentitem()
        {
            for(int i = 0; i < items.Count; i++)
            {
                if (id == items[i].id)
                {
                    //do something
                    name = items[i].name;
                    material = items[i].material;
                    type = items[i].type;
                    price = items[i].price;
                    sizing = items[i].sizing;
                }
            }
        }

        private void refresher()
        {
            _selected_item = null;
            loadmaterials();
            loadtypes();
            loaditems();
           
        }

        private void loadmaterials()
        {
            materials = sq.materials();
        }

        private void loadtypes()
        {
            types = sq.types();
        }

        #endregion

        #region Constructor

        //0 for no item, any else for item by id
        public ViewFormItems(string itemid)
        {
            if(Convert.ToInt32(itemid) == 0)
            {
                loaditems();
                clearitem();
                loadmaterials();
                loadtypes();
            }
            else if(itemid != null && itemid != "")
            {
                loaditems();
                load_currentitem();
                loadmaterials();
                loadtypes();

            }

            #region Commands Init

            _refreshall = new DelegateCommand<string>(
            (s) => { /* perform some action */ refresher(); }//, //Execute
            //(s) => {  } //CanExecute
            );
            _changeitem = new DelegateCommand<string>(
            (s) => { /* perform some action */ sq.update_item(id,name,type,material,price,sizing); }//, //Execute
            //(s) => {  } //CanExecute
            );
            _createblank = new DelegateCommand<string>(
            (s) => { /* perform some action */ clearitem(); }//, //Execute
            //(s) => {  } //CanExecute
            );
            _addnew = new DelegateCommand<string>(
            (s) => { /* perform some action */ id = sq.Insert_New_Item(name, type, material, price, sizing); }//, //Execute
            //(s) => {  } //CanExecute
            );

            #endregion
        }

        #endregion
    }
}
