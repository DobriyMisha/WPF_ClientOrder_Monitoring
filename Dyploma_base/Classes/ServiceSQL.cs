using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Linq;

namespace Dyploma_base
{
    class ServiceSQL
    {
        private string MyConString = "server = localhost; userid=root;password=novasik007;database=azarov1;CharSet=utf8";

        #region Filling Tables

        private ObservableCollection<Orders> _orders_Fill = new ObservableCollection<Orders>();

        public ObservableCollection<Orders> orders_Fill()
        {
            string sql = "SELECT * FROM azarov1.view_orders";
            DataTable dt = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(MyConString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand cmdSel = new MySqlCommand(sql, connection))
                    {
                        MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
                        da.Fill(dt);
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        var game = new Orders
                        {
                            id = Convert.ToInt32(row["id"]),
                            FIO = row["FIO"].ToString(),
                            status = row["description"].ToString(),
                            created = row["created"].ToString(),
                            deadline = row["deadline"].ToString(),
                            closed = row["closed"].ToString(),
                        };
                        _orders_Fill.Add(game);
                    }
                    //обрезаем время у datetime для отображения только даты
                    foreach (Orders ord in _orders_Fill)
                    {
                        ord.created = String.Concat(ord.created.Reverse().Skip(8).Reverse());
                        ord.closed = String.Concat(ord.closed.Reverse().Skip(8).Reverse());
                        ord.deadline = String.Concat(ord.deadline.Reverse().Skip(8).Reverse());
                    }
                    return _orders_Fill;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    return _orders_Fill;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private ObservableCollection<OrderItems> _orderitemsfill = new ObservableCollection<OrderItems>();
        public ObservableCollection<OrderItems> orderitemsfill(string orderid)
        {

            if (orderid != null)
            {
                string sql = "call proc_order_items(" + orderid + ")";
                DataTable dt = new DataTable();
                using (MySqlConnection connection = new MySqlConnection(MyConString))
                {
                    try
                    {
                        connection.Open();
                        using (MySqlCommand cmdSel = new MySqlCommand(sql, connection))
                        {
                            MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
                            da.Fill(dt);
                        }
                        foreach (DataRow row in dt.Rows)
                        {
                            var orderitem1 = new OrderItems
                            {
                                id = Convert.ToInt32(row["id"]),
                                orderid = Convert.ToInt32(row["orderid"]),
                                quantity = Convert.ToInt32(row["quantity"]),
                                itemname = row["itemname"].ToString(),
                                price = Convert.ToInt32(row["price"]),
                                statusitem = row["statusitem"].ToString(),
                            };
                            _orderitemsfill.Add(orderitem1);
                        }
                        return _orderitemsfill;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                        return _orderitemsfill;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            else return null;

        }

        private ObservableCollection<AdditionalsOnItem> _additionalsfill = new ObservableCollection<AdditionalsOnItem>();
        public ObservableCollection<AdditionalsOnItem> additionalsfill(string orderitem)
        {

            if (Convert.ToInt32(orderitem) != 0)
            {
                using (MySqlConnection connection = new MySqlConnection(MyConString))
                {
                    try
                    {
                        string msg_in = "select  * from view_added_additionals where orderitemsid =" + orderitem + ";";
                        connection.Open();
                        MySqlDataAdapter SDA2 = new MySqlDataAdapter(msg_in, connection);
                        DataTable DATA2 = new DataTable();
                        SDA2.Fill(DATA2);
                        foreach (DataRow row in DATA2.Rows)
                        {
                            var add = new AdditionalsOnItem
                            {
                                id = Convert.ToInt32(row["id"]),
                                description = row["description"].ToString(),
                                price = Convert.ToInt32(row["price"]),
                            };
                            _additionalsfill.Add(add);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();

                    }
                }

            }
            return _additionalsfill;
        }

        private ObservableCollection<Additions> _additions = new ObservableCollection<Additions>();
        public ObservableCollection<Additions> additions()
        {
            using (MySqlConnection connection = new MySqlConnection(MyConString))
            {
                try
                {
                    string msg_in = "select  * from azarov1.additionals;";
                    connection.Open();
                    MySqlDataAdapter SDA2 = new MySqlDataAdapter(msg_in, connection);
                    DataTable DATA2 = new DataTable();
                    SDA2.Fill(DATA2);
                    foreach (DataRow row in DATA2.Rows)
                    {
                        var add = new Additions
                        {
                            id = row["id"].ToString(),
                            description = row["description"].ToString(),
                            price = row["price"].ToString(),
                        };
                        _additions.Add(add);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return _additions;
        }

        private ObservableCollection<Types> _types = new ObservableCollection<Types>();
        public ObservableCollection<Types> types()
        {
            using (MySqlConnection connection = new MySqlConnection(MyConString))
            {
                try
                {
                    string msg_in = "select  * from itemtypes;";
                    connection.Open();
                    MySqlDataAdapter SDA2 = new MySqlDataAdapter(msg_in, connection);
                    DataTable DATA2 = new DataTable();
                    SDA2.Fill(DATA2);
                    foreach (DataRow row in DATA2.Rows)
                    {
                        var add = new Types
                        {
                            id = row["id"].ToString(),
                            description = row["description"].ToString(),
                        };
                        _types.Add(add);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return _types;
        }

        private ObservableCollection<Materials> _materials = new ObservableCollection<Materials>();
        public ObservableCollection<Materials> materials()
        {
            using (MySqlConnection connection = new MySqlConnection(MyConString))
            {
                try
                {
                    string msg_in = "select  * from azarov1.materials ;";
                    connection.Open();
                    MySqlDataAdapter SDA2 = new MySqlDataAdapter(msg_in, connection);
                    DataTable DATA2 = new DataTable();
                    SDA2.Fill(DATA2);
                    foreach (DataRow row in DATA2.Rows)
                    {
                        var add = new Materials
                        {
                            id = row["id"].ToString(),
                            description = row["description"].ToString(),
                            price = row["price"].ToString(),
                        };
                        _materials.Add(add);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return _materials;
        }

        private ObservableCollection<Items> _items = new ObservableCollection<Items>();
        public ObservableCollection<Items> items()
        {
            using (MySqlConnection connection = new MySqlConnection(MyConString))
            {
                try
                {
                    string msg_in = "SELECT * FROM azarov1.view_items_full";
                    connection.Open();
                    MySqlDataAdapter SDA2 = new MySqlDataAdapter(msg_in, connection);
                    DataTable DATA2 = new DataTable();
                    SDA2.Fill(DATA2);
                    foreach (DataRow row in DATA2.Rows)
                    {
                        var add = new Items
                        {
                            id = row["id"].ToString(),
                            name = row["Наименование"].ToString(),
                            type = row["Тип изделия"].ToString(),
                            material = row["Материал"].ToString(),
                            price = row["Цена"].ToString(),
                            sizing = row["Размеры"].ToString(),
                        };
                        _items.Add(add);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return _items;
        }

        //загрузка заказа
        public void LoadOrder(ref string orderid, ref string fio, ref string status, ref string opendate, ref string deadline, ref string closedate)
        {
            using (MySqlConnection conn = new MySqlConnection(MyConString))
            {
                try
                {
                    string name = null;
                    conn.Open();
                    // запрос
                    string sql = "SELECT a.fio FROM azarov1.Clients a inner join azarov1.orders b on b.clientid = a.id where b.id =" + orderid + ";";
                    // объект для выполнения SQL-запроса
                    MySqlCommand command = new MySqlCommand("SELECT b.fio FROM azarov1.orders a left join azarov1.clients b on a.clientid = b.id where a.id = " + orderid + ";", conn);
                    // выполняем запрос и получаем ответ
                    name = command.ExecuteScalar().ToString();
                    fio = name.ToString();

                    try
                    {
                        sql = "SELECT a.created FROM azarov1.orders a where a.id =" + orderid + ";";
                        // объект для выполнения SQL-запроса
                        command = new MySqlCommand(sql, conn);
                        // выполняем запрос и получаем ответ
                        string pars1 = command.ExecuteScalar().ToString();
                        if (pars1 != null)
                        {
                            DateTime datestring = DateTime.Parse(pars1);
                            //datestring = String.Concat(datestring.Reverse().Skip(8).Reverse());
                            opendate = datestring.ToString("yyyy-MM-dd");
                        }
                    }
                    catch
                    {

                    }

                    try
                    {
                        sql = "SELECT a.deadline FROM azarov1.orders a where a.id =" + orderid + ";";
                        // объект для выполнения SQL-запроса
                        command = new MySqlCommand(sql, conn);
                        // выполняем запрос и получаем ответ
                        string pars2 = command.ExecuteScalar().ToString();
                        if (pars2 != null && pars2 != "")
                        {
                            DateTime date2 = DateTime.Parse(pars2);
                            deadline = date2.ToString("yyyy-MM-dd");
                        }
                    }
                    catch
                    {

                    }

                    try
                    {
                        sql = "SELECT a.closed FROM azarov1.orders a where a.id =" + orderid + ";";
                        // объект для выполнения SQL-запроса
                        command = new MySqlCommand(sql, conn);
                        // выполняем запрос и получаем ответ
                        string pars3 = command.ExecuteScalar().ToString();
                        if (pars3 != null && pars3 != "")
                        {
                            DateTime date3 = DateTime.Parse(pars3);
                            closedate = date3.ToString("yyyy-MM-dd");
                        }
                    }
                    catch
                    {
                    }

                    try
                    {
                        sql = "select a.description from azarov1.statustable a right join azarov1.orders b on b.orderstatus = a.id where b.id = " + orderid + ";";
                        command = new MySqlCommand(sql, conn);
                        status = command.ExecuteScalar().ToString();
                    }
                    catch
                    {

                    }
                    
                }
                catch (Exception ex)
                {
                    // MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                    // orderitemsfill();
                }
            }


        }

        #endregion

        #region Inserting New

        //заводим новый заказ
        public void insert_new_order(ref string orderid, ref string fio, ref string status, ref string opendate, ref string deadline, ref string closedate)
        {
            //string ordernum;
            string lii = null;
            string lii_f = null;
            string sql = "insert into Clients (FIO) values ('" + fio + "');";
            string sql3 = "select a.id from azarov1.clients a where fio = '" + fio + "';";

            using (MySqlConnection conn = new MySqlConnection(MyConString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(sql, conn);
                    if (fio != null)
                    {
                        //insert fio
                        command = new MySqlCommand(sql, conn);
                        command.ExecuteNonQueryAsync();

                        //insert contacts with client id
                        command = new MySqlCommand("select a.id from azarov1.clients a where a.fio = '" + fio + "';", conn);
                        lii_f = command.ExecuteScalar().ToString();

                        string sqlc = "insert into contacts (clientid) values (" + lii_f + ");";
                        command = new MySqlCommand(sqlc, conn);
                        command.ExecuteNonQueryAsync();

                        //create new order by inserting client id to it
                        string sql2 = "insert into azarov1.orders (clientid) values (" + lii_f + ");";
                        command = new MySqlCommand(sql2, conn);
                        command.ExecuteNonQueryAsync();

                        //select kinda last inserted index of the order by client id
                        command = new MySqlCommand("SELECT a.id from azarov1.orders a where clientid = " + lii_f + ";", conn);
                        lii = command.ExecuteScalar().ToString();
                    }

                    //having last inserted index, we update (insert) other stuff in the order
                    if (lii != null)
                    {
                        orderid = lii;

                        //inserting (by update) order created date
                        if (opendate != null)
                        {
                            try
                            {
                                string sql4 = "update azarov1.orders set created = '" + opendate + "' where id = " + orderid + ";";
                                command = new MySqlCommand(sql4, conn);
                                command.ExecuteNonQueryAsync();
                            }
                            catch (Exception ex1)
                            {
                                MessageBox.Show(ex1.Message);
                            }
                        }

                        //inserting (by update) order deadline
                        if (deadline != null)
                        {
                            try
                            {
                                string sql5 = "update azarov1.orders set deadline = '" + deadline + "' where id = " + orderid + ";";
                                command = new MySqlCommand(sql5, conn);
                                command.ExecuteNonQueryAsync();
                            }
                            catch (Exception ex2)
                            {
                                MessageBox.Show(ex2.Message);
                            }
                        }

                        //inserting (by update) order status
                        if (status != null && status != "")
                        {
                            string inds;

                            try
                            {
                                command = new MySqlCommand("select id from statustable where description = '" + status + "';", conn);
                                inds = command.ExecuteScalar().ToString();
                                command = new MySqlCommand("update azarov1.orders set orderstatus = " + inds.ToString() + "where id = " + orderid + " ;", conn);
                                command.ExecuteNonQueryAsync();
                            }

                            catch (Exception exs)
                            {
                                MessageBox.Show(exs.Message);
                            }

                        }

                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            LoadOrder(ref orderid, ref fio, ref status, ref opendate, ref deadline, ref closedate);
        }

        public void insert_service_itemtype(string descr)
        {

            MessageBoxResult result = MessageBox.Show("Добавить тип изделия '"+descr+"' ? ", "Добавление типа", MessageBoxButton.YesNo);

            switch(result)
            {
                case MessageBoxResult.Yes:

                    using (MySqlConnection connection = new MySqlConnection(MyConString))
                    {
                        try
                        {
                            connection.Open();
                            //check if already exists
                            MySqlCommand command = new MySqlCommand("select description from azarov1.itemtypes where description = '" + descr + "' ;", connection);
                            var result2 = command.ExecuteScalar();
                            if(result2 == null)
                            {
                                command = new MySqlCommand("insert into azarov1.itemtypes (description) values ('" + descr + "') ;", connection);
                                command.ExecuteNonQuery();
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    break;

                case MessageBoxResult.No:
                    break;

                default:
                    break;
            }

            
        }

        public void insert_service_materials(string descr,string price)
        {
            MessageBoxResult result = MessageBox.Show("Добавить материал '" + descr + "' по цене '"+price+"' ? ", "Добавление материала", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:

                    using (MySqlConnection connection = new MySqlConnection(MyConString))
                    {
                        try
                        {
                            connection.Open();
                            //check if already exists
                            MySqlCommand command = new MySqlCommand("select description from azarov1.materials where description = '" + descr + "' ;", connection);
                            if (command.ExecuteScalar() == null)
                            {
                                    command = new MySqlCommand("insert into azarov1.materials (description,price) values ('" + descr + "'," + price + ") ;", connection);
                                    command.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    break;

                case MessageBoxResult.No:
                    break;

                default:
                    break;
            }
        }

        public void insert_service_additions(string descr, string price)
        {
            MessageBoxResult result = MessageBox.Show("Добавить услугу '" + descr + "' по цене '" + price + "' ? ", "Добавление услуги", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:

                    using (MySqlConnection connection = new MySqlConnection(MyConString))
                    {
                        try
                        {
                            connection.Open();
                            //check if already exists
                            MySqlCommand command = new MySqlCommand("select description from azarov1.additionals where description = '" + descr + "' ;", connection);
                            if (command.ExecuteScalar() == null)
                            {
                                    command = new MySqlCommand("insert into azarov1.additionals (description,price) values ('" + descr + "'," + price + ") ;", connection);
                                    command.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    break;

                case MessageBoxResult.No:
                    break;

                default:
                    break;
            }
        }

        public void insert_service_item(string itemname,string itemtype, string material,string price, string sizing)
        {
            string typeid, matid;
            MessageBoxResult result = MessageBox.Show("Добавить изделие '" + itemname + "' по цене '" + price + "' ? ", "Добавление услуги", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    using (MySqlConnection connection = new MySqlConnection(MyConString))
                    {
                        try
                        {
                            connection.Open();
                            MySqlCommand command = new MySqlCommand("select id from azarov1.itemtypes where description = '" + itemtype + "' ;", connection);
                            typeid = command.ExecuteScalar().ToString();
                            command = new MySqlCommand("select id from azarov1.materials where description = '" + material + "' ;", connection);
                            matid = command.ExecuteScalar().ToString();
                            command = new MySqlCommand("insert into azarov1.items (itemtypeid,itemname,materialid,price,sizing) " +
                                "values ("+typeid+",'"+itemname+"',"+matid+","+price+",'"+sizing+"') ;", connection);
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }

        public string Insert_New_Item(string name, string type, string material, string price, string sizing)
        {
            string id = "0";
            MessageBoxResult result = MessageBox.Show("Добавить новое изделие ? ", "Добавление изделия", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:

                    using (MySqlConnection connection = new MySqlConnection(MyConString))
                    {
                        if(name != null && name != "")
                        {
                            try
                            {
                                connection.Open();
                                MySqlCommand command = new MySqlCommand
                                    ("insert into azarov1.items(itemname) values ('" +
                                    name + "');", connection);
                                command.ExecuteNonQueryAsync();

                                command = new MySqlCommand("select LAST_INSERT_ID()", connection);
                                id = command.ExecuteScalar().ToString();

                                if (type != null && type != "")
                                {
                                    string typeid;
                                    try
                                    {
                                        MySqlCommand commandsel = new MySqlCommand("select id from azarov1.itemtypes where description = '" + type + "' ;", connection);
                                        typeid = commandsel.ExecuteScalar().ToString();

                                        command = new MySqlCommand("update azarov1.items set itemtypeid = " + typeid + " where id = " + id + " ;", connection);
                                        command.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);

                                    }   
                                }

                                if (material != null && material != "")
                                {
                                    string materialid;
                                    try
                                    {
                                        MySqlCommand commandsel2 = new MySqlCommand("select id from azarov1.materials where description = '" + material + "' ;", connection);
                                        materialid = commandsel2.ExecuteScalar().ToString();
                                        command = new MySqlCommand("update azarov1.items set materialid = " + materialid + " where id = " + id + " ;", connection);
                                        command.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }

                                if (price != null && price != "")
                                {
                                    command = new MySqlCommand("update azarov1.items set price = " + price + " where id = " + id + " ;", connection);
                                    command.ExecuteNonQuery();

                                }

                                if (sizing != null && sizing != "")
                                {
                                    command = new MySqlCommand("update azarov1.items set sizing = '" + sizing + "' where id = " + id + " ;", connection);
                                    command.ExecuteNonQuery();
                                }



                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                               
                            }
                        }
                    }
                    break;

                case MessageBoxResult.No:
                    break;

                default:
                    break;
            }
            return id;

        }

        public void insert_orderitem(string orderid, string item_id)
        {
            MessageBoxResult result = MessageBox.Show("Добавить изделие в заказ? ", "Добавление изделия", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:

                    using (MySqlConnection connection = new MySqlConnection(MyConString))
                    {
                        if (orderid != null && item_id != null)
                        {
                            try
                            {
                                string msg_in = "call proc_add_orderitem(" + orderid + "," + item_id + ");";
                                connection.Open();
                                MySqlCommand command = new MySqlCommand(msg_in, connection);
                                // выполняем запрос
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("тут");
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Проблема с номером заказа, обратитесь в техподдержку.");
                        }
                    }
                    break;

                case MessageBoxResult.No:
                    break;

                default:
                    break;
            }

            
        }

        public void insert_oi_additional(string orderitem, string addid)
        {
            MessageBoxResult result = MessageBox.Show("Добавить услугу к изделию? ", "Добавление услуги", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:

                    using (MySqlConnection connection = new MySqlConnection(MyConString))
                    {
                        if (orderitem != null && addid != null)
                        {
                            try
                            {
                                string msg_in = "call proc_add_orderitem_additional(" + orderitem + "," + addid + ")";
                                connection.Open();
                                MySqlCommand command = new MySqlCommand(msg_in, connection);
                                // выполняем запрос
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Проблема с идентификаторами, обратитесь в техподдержку.");
                        }
                    }
                    break;

                case MessageBoxResult.No:
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Removing

        public void remove_orderitem(string id)
        {
            MessageBoxResult result = MessageBox.Show("Удалить изделие под номером " + id + " из заявки ? ", "Удаление из заявки", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:

                    using (MySqlConnection connection = new MySqlConnection(MyConString))
                    {
                        if (id != null)
                        {
                            try
                            {
                                string msg_in = "call proc_remove_orderitem(" + id + ")";
                                connection.Open();
                                MySqlCommand command = new MySqlCommand(msg_in, connection);
                                // выполняем запрос
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Проблема с идентификаторами, обратитесь в техподдержку.");
                        }
                    }
                    break;

                case MessageBoxResult.No:
                    break;

                default:
                    break;
            }
        }

        public void remove_oi_additional(string id)
        {
            MessageBoxResult result = MessageBox.Show("Удалить услугу под номером "+id +" ? ", "Удаление услуги", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:

                    using (MySqlConnection connection = new MySqlConnection(MyConString))
                    {
                        if (id != null)
                        {
                            try
                            {
                                string msg_in = "call proc_remove_orderitem_additional(" + id + ")";
                                connection.Open();
                                MySqlCommand command = new MySqlCommand(msg_in, connection);
                                // выполняем запрос
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Проблема с идентификаторами, обратитесь в техподдержку.");
                        }
                    }
                    break;

                case MessageBoxResult.No:
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region SearchFilter

       // private string _order;

        //поиск по номеру заказа
        public ObservableCollection<Orders> searchby_ordernumb(ObservableCollection<Orders> param, string order)
        {

            foreach (Orders data in param.ToList())
            {
                if(data != null)
                {
                    if (data.id != Convert.ToInt32(order))
                    {
                        param.Remove(data);
                    }
                }
                
            }
            return param;
        }

        //поиск по фио
        public ObservableCollection<Orders> searchby_name(ObservableCollection<Orders> param, string name)
        {
            StringComparison comp = StringComparison.OrdinalIgnoreCase;
            foreach (Orders data in param.ToList())
            {
                if (data.FIO.Contains(name, comp) == false)
                {
                    param.Remove(data);
                }
            }
            return param;
        }

        #endregion

        #region Updating Data

        //обновление статуса заказа
        public void StatusUpdate(string status, string order)
        {
            using (MySqlConnection connection = new MySqlConnection(MyConString))
            {
                try
                {
                    connection.Open();
                    string inds;
                    MySqlCommand command = new MySqlCommand("select id from statustable where description = '" + status + "';", connection);
                    inds = command.ExecuteScalar().ToString();
                    command = new MySqlCommand("update azarov1.orders set orderstatus = " + inds.ToString() + " where id = " + order + " ;", connection);
                    command.ExecuteNonQueryAsync();

                }
                catch
                {

                }
                finally
                {
                    connection.Close();
                }
            }

        }

        //обновление статуса изделия
        public void StatusItemUpdate(string status, string orderitem)
        {
            using (MySqlConnection connection = new MySqlConnection(MyConString))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("update azarov1.orderitems set statusitem = '" + status + "' where id = " + orderitem + " ;", connection);
                    command.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Что-то пошло не так.");
                }
                finally
                {
                    connection.Close();
                }
            }


        }

        //обновить заказ
        public void FullOrderUpdate(ref string orderid, ref string fio, ref string status, ref string opendate, ref string deadline, ref string closedate)
        {
            using (MySqlConnection conn = new MySqlConnection(MyConString))
            {
                try
                {
                    conn.Open();
                    if (orderid != null)
                    {
                        //string sql = "SELECT a.fio FROM azarov1.Clients a inner join azarov1.orders b on b.clientid = a.id where b.id =" + orderid + ";";
                        // объект для выполнения SQL-запроса
                        //MySqlCommand command = new MySqlCommand(sql, conn);
                        MySqlCommand command;
                        if(fio != null)
                        {
                            command = new MySqlCommand("select clientid from azarov1.orders where id = "+orderid+" ; ",conn);
                            string pr = command.ExecuteScalar().ToString();
                            if(pr != null)
                            {
                                command = new MySqlCommand("update azarov1.clients set fio = '" + fio + "' where id = " + pr + " ;",conn);
                                command.ExecuteNonQuery();
                            }
                            
                        }

                        if (opendate != null)
                        {
                            string sql1 = "update azarov1.orders set created = '" + opendate + "' where id = " + orderid + " ;";
                            command = new MySqlCommand(sql1, conn);
                            command.ExecuteNonQuery();
                        }
                        if (closedate != null)
                        {
                            //string sql2 = "update azarov1.orders set closed = '" + closedate + "' where id = " + orderid + " ;";
                            //command = new MySqlCommand(sql2, conn);
                            //command.ExecuteNonQuery();
                        }

                        if(deadline != null)
                        {
                            string sql3 = "update azarov1.orders set deadline = '" + deadline + "' where id = " + orderid + " ;";
                            command = new MySqlCommand(sql3, conn);
                            command.ExecuteNonQuery();
                        }

                        if (status != null)
                        {
                            string inds;
                            command = new MySqlCommand("select id from statustable where description = '" + status + "';", conn);
                            inds = command.ExecuteScalar().ToString();
                            command = new MySqlCommand("update azarov1.orders set orderstatus = " + inds.ToString() + " where id = " + orderid + " ;", conn);
                            command.ExecuteNonQueryAsync();
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message) ;
                }
                finally
                {
                    conn.Close();
                    LoadOrder(ref orderid, ref fio, ref status, ref opendate, ref deadline, ref closedate);
                }

            }
        }

        public void CloseOrder(ref string orderid, ref string closedate)
        {
            using (MySqlConnection connection = new MySqlConnection(MyConString))
            {
                try
                {
                    connection.Open();
                    MessageBoxResult result = MessageBox.Show("Закрыть заказ под номером " + orderid + " под текущей датой?", "Закрытие заказа", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if(result == MessageBoxResult.Yes)
                    {
                        //string date = DateTime.Now.ToString("")
                        string sql4 = "update azarov1.orders set closed = '" + DateTime.Today.ToString("yyyy-MM-dd") + "' where id = " + orderid + ";";
                        MySqlCommand command = new MySqlCommand(sql4, connection);
                        command.ExecuteNonQuery();

                        command = new MySqlCommand("select closed from orders where id = '" + orderid + "' ;", connection);
                        closedate = command.ExecuteScalar().ToString();
                        
                    }
                    else
                    {
                        closedate = "";
                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void update_orderitem_quantity(string oi_id, string quantity)
        {
            MessageBoxResult result = MessageBox.Show("Изменить количество на '" + quantity + "' ? ", "Изменение количества", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:

                    using (MySqlConnection connection = new MySqlConnection(MyConString))
                    {
                        try
                        {
                            connection.Open();
                            MySqlCommand command = new MySqlCommand("update azarov1.orderitems set quantity = " + quantity + " where id = " + oi_id + " ;", connection);
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    break;

                case MessageBoxResult.No:
                    break;

                default:
                    break;
            }
        }

        public void update_itemtype(string typeid, string descr)
        {
            MessageBoxResult result = MessageBox.Show("Изменить тип изделия на '" + descr + "' ? ", "Изменение типа", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:

                    using (MySqlConnection connection = new MySqlConnection(MyConString))
                    {
                        try
                        {
                            connection.Open();
                            MySqlCommand command = new MySqlCommand("select description from azarov1.itemtypes where description = '" + descr + "' ;", connection);
                            if (command.ExecuteScalar() == null)
                            {
                                command = new MySqlCommand("update azarov1.itemtypes set description = '"+descr+"' where id = "+typeid+" ;", connection);
                                command.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    break;

                case MessageBoxResult.No:
                    break;

                default:
                    break;
            }

        }

        public void update_material(string matid, string descr, string price)
        {
            MessageBoxResult result = MessageBox.Show("Изе материал '" + descr + "' по цене '" + price + "' ? ", "Добавление материала", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:

                    using (MySqlConnection connection = new MySqlConnection(MyConString))
                    {
                        try
                        {
                            connection.Open();
                            MySqlCommand command = new MySqlCommand("update azarov1.materials set description = '"+descr+"' where id = "+matid+" ;", connection);
                            command.ExecuteNonQuery();
                            command = new MySqlCommand("update azarov1.materials set price = " + price + " where id = " + matid + " ;", connection);
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    break;

                case MessageBoxResult.No:
                    break;

                default:
                    break;
            }
        }

        public void update_additional(string addid, string descr, string price)
        {
            MessageBoxResult result = MessageBox.Show("Изменить материал '" + descr + "' по цене '" + price + "' ? ", "Изменение материала", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:

                    using (MySqlConnection connection = new MySqlConnection(MyConString))
                    {
                        try
                        {
                            connection.Open();
                            MySqlCommand command = new MySqlCommand("update azarov1.additionals set description = '" + descr + "' where id = " + addid + " ;", connection);
                            command.ExecuteNonQuery();
                            command = new MySqlCommand("update azarov1.additionals set price = " + price + " where id = " + addid + " ;", connection);
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    break;

                case MessageBoxResult.No:
                    break;

                default:
                    break;
            }
        }

        public void update_item(string id, string name, string type, string material, string price, string sizing)
        {
            string typeid;
            string materialid;
            MessageBoxResult result = MessageBox.Show("Изменить выбранное изделие ? ", "Изменение изделия", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:

                    using (MySqlConnection connection = new MySqlConnection(MyConString))
                    {
                        try
                        {
                            connection.Open();

                            MySqlCommand command = new MySqlCommand("update azarov1.items set itemname = '" + name + "' where id = " + id + " ;", connection);
                            command.ExecuteNonQuery();

                            try
                            {
                                MySqlCommand commandsel = new MySqlCommand("select id from azarov1.itemtypes where description = '" + type + "' ;", connection);
                                typeid = commandsel.ExecuteScalar().ToString();

                                command = new MySqlCommand("update azarov1.items set itemtypeid = " + typeid + " where id = " + id + " ;", connection);
                                command.ExecuteNonQuery();
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.Message);

                            }

                            try
                            {
                                MySqlCommand commandsel2 = new MySqlCommand("select id from azarov1.materials where description = '" + material + "' ;", connection);
                                materialid = commandsel2.ExecuteScalar().ToString();
                                command = new MySqlCommand("update azarov1.items set materialid = " + materialid + " where id = " + id + " ;", connection);
                                command.ExecuteNonQuery();
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }   

                            command = new MySqlCommand("update azarov1.items set price = " + price + " where id = " + id + " ;", connection);
                            command.ExecuteNonQuery();

                            command = new MySqlCommand("update azarov1.items set sizing = '" + sizing + "' where id = " + id + " ;", connection);
                            command.ExecuteNonQuery();
                            MessageBox.Show("Операция выполнена успешно!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    break;

                case MessageBoxResult.No:
                    break;

                default:
                    break;
            }
        }

        #endregion

    }
}
