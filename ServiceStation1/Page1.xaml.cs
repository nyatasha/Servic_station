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

namespace ServiceStation1
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {           
            InitializeComponent();
            this.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "img.jpg")));
        }
        private void BtnLogInClicked(object sender, RoutedEventArgs e)
        {   
            NavigationService navService = NavigationService.GetNavigationService(this);
            MainPage mainPage = new MainPage();

            if ((TxtName.Text.Equals("Administrator") || TxtName.Text.Equals("administrator")) 
                && !TxtName.Text.Equals(""))
            {
                navService.Navigate(mainPage);
            }
            else
                MessageBox.Show("Only the administrator can log in!");            
        }
    }
}
