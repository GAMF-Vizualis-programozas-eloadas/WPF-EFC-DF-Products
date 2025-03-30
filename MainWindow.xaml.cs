using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Products;

namespace WPF_EFC_DF_Products
{
	public partial class MainWindow : Window
	{
		cnProducts cn = new cnProducts();
		public MainWindow()
		{
			InitializeComponent();
			try
			{
				cn.Products.Load();
				var p = cn.Products.Local.ToObservableCollection();
				grProduct.DataContext = p;
				cbProductname.SelectedIndex = 0;
			}
			catch (Exception exc)
			{
				MessageBox.Show("Data error\r\n" + exc.Message);
			}
		}

		private void miProducts_Click(object sender, RoutedEventArgs e)
		{
			grProduct.Visibility = Visibility.Visible;
		}


		private void miExit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private static void DetachDb(cnProducts cn)
		{
			cn.Database.OpenConnection();
			var connection = cn.Database.GetDbConnection();
			if (connection.State == System.Data.ConnectionState.Closed)
			{
				connection.Open();
			}
			string[] commands =
			{ "USE master",
				$"ALTER DATABASE [{connection.Database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE",
				$"ALTER DATABASE [{connection.Database}] SET OFFLINE WITH ROLLBACK IMMEDIATE",
				$"EXEC sp_detach_db '{connection.Database}'"
			};
			using (var sqlCommand = new SqlCommand())
			{
				sqlCommand.Connection = connection as SqlConnection;
				foreach (string command in commands)
				{
					sqlCommand.CommandText = command;
					sqlCommand.ExecuteNonQuery();
				}
			}
		}

		private void miDelete_Click(object sender, RoutedEventArgs e)
		{
			var x = cbProductname.SelectedItem as Products.Product;
			cn.Products.Remove(x);
			cn.SaveChanges();
			MessageBox.Show("Product removed");
		}

		private void miSaveProduct_Click(object sender, RoutedEventArgs e)
		{
			cn.SaveChanges();
			MessageBox.Show("Product saved");
		}

		private void wndClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			DetachDb(cn);
		}

		private void tbSearchProductName_TextChanged(object sender, TextChangedEventArgs e)
		{
			string _ProductName = tbSearchProductName.Text.ToLower();
			cbProductname.Items.Filter = item =>
				_ProductName.Length == 0 ? true :
					((Product)item).ProductName.ToLower().Contains(_ProductName);
			cbProductname.IsDropDownOpen = true; // Keep dropdown open during typing
		}
	}
}