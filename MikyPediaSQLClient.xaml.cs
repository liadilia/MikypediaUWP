using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MikypediaUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MikyPediaSQLClient : Page
    {
        DbConnection conn = null;
        string dbType = null;

        public MikyPediaSQLClient()
        {
            this.InitializeComponent();



            //this.conn = conn;
            //this.DBUrl.Text = conn.DataSource;
            //this.DBName.Text = conn.Database;




            /*    tables.Columns.Add("table", "Table");
                DataTable schema = conn.GetSchema("Tables");
                foreach (DataRow row in schema.Rows)
                {

                    tables.Rows.Add(row[2].ToString());
                }*/
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            this.conn = e.Parameter as DbConnection;

            this.DBUrl.Text = conn.DataSource;
            this.DBName.Text = conn.Database;
            base.OnNavigatedTo(e);
        }

        private DataTable table;
        private DbDataAdapter adapter;
        private void setResults(DataGrid view, string query)
        {

            query = query.Trim();
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            //
            logText.Text += ">" + query + Environment.NewLine;


            try
            {

               

                int affectedRows = 0;
                if (query.ToLower().StartsWith("select"))
                {

                    DbDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    System.Diagnostics.Debug.WriteLine(dt.ToString());
                    FillDataGrid(dt, resultGrid);
                    reader.Close();
                  
                  

                }
                else
                {
                    affectedRows = cmd.ExecuteNonQuery();
                }
                //prevText = logText.Text;
                logText.Text  += "  affected rows " + affectedRows + Environment.NewLine;


            }
            catch (Exception exception)
            {
                var dialog = new MessageDialog("Query invalid " + exception);
                logText.Text += "!!! " + exception.Message + Environment.NewLine;

            }

        }


        public static void FillDataGrid(DataTable table, DataGrid grid)
        {
            grid.Columns.Clear();
            grid.AutoGenerateColumns = false;
            for (int i = 0; i < table.Columns.Count; i++)
            {
                grid.Columns.Add(new DataGridTextColumn()
                {
                    Header = table.Columns[i].ColumnName,
                    Binding = new Binding { Path = new PropertyPath("[" + i.ToString() + "]") }
                });
            }

            var collection = new ObservableCollection<object>();
            foreach (DataRow row in table.Rows)
            {
                collection.Add(row.ItemArray);
            }

            grid.ItemsSource = collection;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String query = Query.Text;
            setResults(resultGrid, query);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.conn.Close();
            /*    this.Hide();
                DBConnectForm connectionForm = new DBConnectForm();*/
            //   connectionForm.Show();
        }

        private void tables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void resultGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /*  private void tables_CellDoubleClick(object sender, GridViewCellEventArgs e)
          {
              setResults(resultGrid, "SELECT * FROM " + ((GridView)sender).Rows[e.RowIndex].Cells[0].Value);

          }*/
    }
}
