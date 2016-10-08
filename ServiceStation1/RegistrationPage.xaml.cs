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

namespace ServiceStation1
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader dr;
        public RegistrationPage()
        {
            InitializeComponent();
            this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "img.jpg")));
        }
        private void ButtonHomeClicked(object sender, RoutedEventArgs e)
        {
            NavigationService navService = NavigationService.GetNavigationService(this);
            MainPage mainPage = new MainPage();
            navService.Navigate(mainPage);
        }
        private void ButtonCheckIfRegisteredClicked(object sender, RoutedEventArgs e)
        {
            Boolean exists = false;
            con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Natallia\workspace\ServiceStation1\ServiceStation1\TblClientInfo.mdf;Integrated Security=True");
            cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from TableClientsInfo";
            dr = cmd.ExecuteReader();
            if(dr.HasRows)
            {
                while(dr.Read())
                {
                    if (dr[1].ToString().Equals(TxtFirstName.Text) && dr[2].ToString().Equals(TxtLastName.Text))
                    {
                        exists = true;
                    }
                }
            }
            con.Close();
            if (exists)
                MessageBox.Show("YES!This client's already been registered.");
            else
                MessageBox.Show("No!This client's not been registered.");
        }
        private void ButtonSubmitClicked(object sender, RoutedEventArgs e)
        {
            con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Natallia\workspace\ServiceStation1\ServiceStation1\TblClientInfo.mdf;Integrated Security=True");
            cmd = new SqlCommand("INSERT INTO TableClientsInfo (FirstName,LastName,DateofBirth,Adress,Phone,Email) VALUES (@FirstName,@LastName,@DateofBirth,@Adress,@Phone,@Email)", con);
            try
            {                
                con.Open();
                cmd.Parameters.AddWithValue("@FirstName", TxtFirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", TxtLastName.Text);
                cmd.Parameters.AddWithValue("@DateofBirth", TxtBirth.Text);
                cmd.Parameters.AddWithValue("@Adress", TxtAdress.Text);
                cmd.Parameters.AddWithValue("@Phone", TxtPhone.Text);
                cmd.Parameters.AddWithValue("@Email", TxtEmail.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Saved!");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Something is going wrong");
            }
        }
    }
}
