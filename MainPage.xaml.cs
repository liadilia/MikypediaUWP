using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Windows.UI.Popups;
using System.Data.Common;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MikypediaUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Enum.TryParse(DBType.Text, out DbTypes type);

            try
            {

                DbConnection dbConn = new DatabaseConnectionBuilder()
                    .withType(type)
                    .setHost(host.Text)
                    .setdbName(name.Text)
                    .setUsername(username.Text)
                    .setPassword(password.Text)
                    .build();


                this.Hide();
                Editor = new MikyPediaSQLClient(dbConn);
                Editor.Show();
            }
            catch (Exception exception)
            {
                var dialog = new MessageDialog("Error when attempting to open the connection " + exception);
                
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DBType.Text == "MSSQL")
            {
                host.Text = "localhost";
                name.Text = "lab1";
                WAuth.IsChecked = true;
            }
            else if (DBType.Text == "MySQL")
            {
                WAuth.IsChecked = false;
                host.Text = "";
                name.Text = "";

            }
        }
    }
}
