using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
        private void setResults(GridView view, string query)
        {

            query = query.Trim();
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            //
            logText.Text += ">" + query + Environment.NewLine;


            try
            {

                //resultGrid.ItemsSource

                int affectedRows = 0;
                if (query.ToLower().StartsWith("select"))
                {

                    DbDataReader reader = cmd.ExecuteReader();

                    while (reader.Read()) {
                        affectedRows++;
                    }

                    reader.Close();
                        // https://stackoverflow.com/questions/3488962/how-to-create-a-dbdataadapter-given-a-dbcommand-or-dbconnection
                    /*     adapter = DbProviderFactories.GetFactory(conn).CreateDataAdapter();

                    

                         adapter.SelectCommand = cmd;
                         table = new DataTable();
                         adapter.Fill(table);

                         BindingSource bSource = new BindingSource();
                         bSource.DataSource = table;
                         resultGrid.DataSource = bSource;
                         affectedRows = table.Rows.Count;*/

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

        /*  private void tables_CellDoubleClick(object sender, GridViewCellEventArgs e)
          {
              setResults(resultGrid, "SELECT * FROM " + ((GridView)sender).Rows[e.RowIndex].Cells[0].Value);

          }*/
    }
}
