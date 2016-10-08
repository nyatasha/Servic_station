using System;
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
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace ServiceStation1
{
    /// <summary>
    /// Логика взаимодействия для ClientsInfo.xaml
    /// </summary>
    public partial class ClientsInfo : Page
    {
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader dr;
        public ClientsInfo()
        {
            InitializeComponent();
            FillLisfOfClient();
            this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "img.jpg")));
        }  
        public void FillLisfOfClient()
        {
            con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Natallia\workspace\ServiceStation1\ServiceStation1\TblClientInfo.mdf;Integrated Security=True");
            cmd = new SqlCommand();
            cmd.Connection = con;
            try 
            { 
                con.Open();
                cmd.CommandText = "select * from TableClientsInfo";
                dr = cmd.ExecuteReader();
                int count = 1;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {                       
                        ListOfClients.Items.Add(count + ". " + dr[1].ToString() + " " + dr[2].ToString());
                        count++;
                    }
                }
                con.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Something is going wrong");
            }
        }
        private void ClientsIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            ListMake.Items.Clear();
            ListModel.Items.Clear();
            ListYear.Items.Clear();
            ListVin.Items.Clear();
            int index = ListOfClients.SelectedIndex;
            if (index != -1)
            {
                con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Natallia\workspace\ServiceStation1\ServiceStation1\TblClientInfo.mdf;Integrated Security=True");
                cmd = new SqlCommand("SELECT Make,Model,Year,VIN FROM TableCars WHERE TableCars.ClientID='" + index + "'", con);
                try
                {
                    con.Open();
                    dr = cmd.ExecuteReader();
                    int count = 1;
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ListMake.Items.Add(count + ". " + dr[0].ToString());
                            ListModel.Items.Add(dr[1].ToString());
                            ListYear.Items.Add(dr[2].ToString());
                            ListVin.Items.Add(dr[3].ToString());
                            count++;
                        }
                    }
                    con.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Something is going wrong");
                }
            }
        }

        private void CarsIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox l = sender as ListBox;
            if (l.SelectedIndex != -1)
            {
                ListMake.SelectedIndex = l.SelectedIndex;
                ListModel.SelectedIndex = l.SelectedIndex;
                ListYear.SelectedIndex = l.SelectedIndex;
                ListVin.SelectedIndex = l.SelectedIndex;
            }
            int index = ListMake.SelectedIndex;
            if (index != -1)
            {
                String model = ListModel.SelectedItem.ToString();
                String year = ListYear.SelectedItem.ToString();
                String id = GetCarId(model, year);
                ListDate.Items.Clear();
                ListOrderAmount.Items.Clear();
                ListOrderStatus.Items.Clear();
                cmd = new SqlCommand("SELECT Date,OrderAmount,OrderStatus FROM TableOrders WHERE CarId='" + id + "'", con);
                try
                {
                    con.Open();
                    dr = cmd.ExecuteReader();
                    int count = 1;
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ListDate.Items.Add(count + ". " + dr[0].ToString());
                            ListOrderAmount.Items.Add(dr[1].ToString());
                            ListOrderStatus.Items.Add(dr[2].ToString());
                            count++;
                        }
                    }                    
                    con.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Something is going wrong");
                }
            }
        }
        private void SelectedOrderChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox l = sender as ListBox;
            if (l.SelectedIndex != -1)
            {
                ListMake.SelectedIndex = l.SelectedIndex;
                ListModel.SelectedIndex = l.SelectedIndex;
                ListYear.SelectedIndex = l.SelectedIndex;
                ListVin.SelectedIndex = l.SelectedIndex;
            }
        }
        private String GetCarId(String model, String year)
        {
            String id = "";
            con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Natallia\workspace\ServiceStation1\ServiceStation1\TblClientInfo.mdf;Integrated Security=True");
            cmd = new SqlCommand("SELECT CarId FROM TableCars WHERE Model='" + model + "'AND Year='" + year + "'", con);
            try
            {
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        id = dr[0].ToString();
                    }
                }
                cmd.Clone();
                con.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Something is going wrong");
            }
            return id;
        }
        private void BtnDeleteCarsClicked(object sender, RoutedEventArgs e)
        {
            int index = ListMake.SelectedIndex;
            String model = ListModel.SelectedItem.ToString();
            String year = ListYear.SelectedItem.ToString();
            String id = GetCarId(model, year);
            if (index != -1)
            {                
                con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Natallia\workspace\ServiceStation1\ServiceStation1\TblClientInfo.mdf;Integrated Security=True");
                cmd = new SqlCommand("delete from TableCars where CarId='" + id + "'", con);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    //cmd.Clone();
                    con.Close();
                    MessageBox.Show("Deleted!");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Something is going wrong");
                }
            }
        }
        private void BtnAddCarsClicked(object sender, RoutedEventArgs e)
        {
            TxtMake.Visibility = System.Windows.Visibility.Visible;
            TxtModel.Visibility = System.Windows.Visibility.Visible;
            TxtYear.Visibility = System.Windows.Visibility.Visible;
            TxtVin.Visibility = System.Windows.Visibility.Visible;
            LblEnter.Visibility = System.Windows.Visibility.Visible;
            BtnSubmit.Visibility = System.Windows.Visibility.Visible;
        }
        private void BtnSubmitCarClicked(object sender, RoutedEventArgs e)
        {
            cmd = new SqlCommand("INSERT INTO TableCars (ClientId,Make,Model,Year,VIN) VALUES (@ClientId,@Make,@Model,@Year,@VIN)", con);
            try
            {
                con.Open();     
                cmd.Parameters.AddWithValue("@ClientId", ListOfClients.SelectedIndex.ToString());
                cmd.Parameters.AddWithValue("@Make", TxtMake.Text);
                cmd.Parameters.AddWithValue("@Model", TxtModel.Text);
                cmd.Parameters.AddWithValue("@Year", TxtYear.Text);
                cmd.Parameters.AddWithValue("@VIN", TxtVin.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Saved!");
                TxtMake.Visibility = System.Windows.Visibility.Hidden;
                TxtModel.Visibility = System.Windows.Visibility.Hidden;
                TxtYear.Visibility = System.Windows.Visibility.Hidden;
                TxtVin.Visibility = System.Windows.Visibility.Hidden;
                LblEnter.Visibility = System.Windows.Visibility.Hidden;
                BtnSubmit.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Something is going wrong");
            }
        }
        private void BtnAddOrderClicked(object sender, RoutedEventArgs e)
        {
            TxtDate.Visibility = System.Windows.Visibility.Visible;
            TxtOrderAmount.Visibility = System.Windows.Visibility.Visible;
            LblEnter1.Visibility = System.Windows.Visibility.Visible;
            BtnSubmit1.Visibility = System.Windows.Visibility.Visible;
        }
        private void BtnSubmitOrderClicked(object sender, RoutedEventArgs e)
        {
            cmd = new SqlCommand("INSERT INTO TableOrders (CarId,Date,OrderAmount,OrderStatus) VALUES (@CarId,@Date,@OrderAmount,@OrderStatus)", con);
            try
            {
                String model = ListModel.SelectedItem.ToString();
                String year = ListYear.SelectedItem.ToString();
                String id = GetCarId(model, year);
                con.Open();
                cmd.Parameters.AddWithValue("@CarId", id);
                cmd.Parameters.AddWithValue("@Date", TxtDate.Text);
                cmd.Parameters.AddWithValue("@OrderAmount", TxtOrderAmount.Text);
                cmd.Parameters.AddWithValue("@OrderStatus", "In progress");
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Saved!");
                TxtDate.Visibility = System.Windows.Visibility.Hidden;
                TxtOrderAmount.Visibility = System.Windows.Visibility.Hidden;
                LblEnter1.Visibility = System.Windows.Visibility.Hidden;
                BtnSubmit1.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Something is going wrong");
            }
        }
        private void BtnDeleteOrderClicked(object sender, RoutedEventArgs e)
        {
            int index = ListDate.SelectedIndex;
            String date = ListDate.SelectedItem.ToString();
            String orderAmount = ListOrderAmount.SelectedItem.ToString();
            String id = GetOrderId(date, orderAmount);
            if (index != -1)
            {
                con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Natallia\workspace\ServiceStation1\ServiceStation1\TblClientInfo.mdf;Integrated Security=True");
                cmd = new SqlCommand("delete from TableOrders where OrderId='" + id + "'", con);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show(id);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Something is going wrong");
                }
            }
        }
        private String GetOrderId(String date, String orderAmount)
        {
            String id = "";
            String model = ListModel.SelectedItem.ToString();
            String year = ListYear.SelectedItem.ToString();
            String id_car = GetCarId(model, year);
            con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Natallia\workspace\ServiceStation1\ServiceStation1\TblClientInfo.mdf;Integrated Security=True");
            cmd = new SqlCommand("SELECT OrderId FROM TableOrder WHERE CarId='" + id_car + "'AND Date='" + date + "'AND OrderAmount='" + orderAmount + "'", con);
            try
            {
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        id = dr[0].ToString();
                    }
                }
                cmd.Clone();
                con.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Something is going wrong");
            }
            return id;
        }
        private void BtnHomeClicked(object sender, RoutedEventArgs e)
        {
            NavigationService navService = NavigationService.GetNavigationService(this);
            MainPage mainPage = new MainPage();
            navService.Navigate(mainPage);
        }
    }
}
