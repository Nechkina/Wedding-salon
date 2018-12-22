using CourseWorkClassLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
using WPF_CourseWork.EditForms;

namespace WPF_CourseWork
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		const string SerializationFolder = @".\Data\";
		const string SerializationFile = @".\Data\data.bin";

		public MainWindow()
		{
			InitializeComponent();
			User.CurrentUser.CurrUser.PropertyChanged += UserChanged;
		}

		~MainWindow()
		{
			if (!Directory.Exists(SerializationFolder))
				Directory.CreateDirectory(SerializationFolder);
			using (Stream SerializationStream = File.Create(SerializationFile))
			{
				BinaryFormatter serializer = new BinaryFormatter();
				serializer.Serialize(SerializationStream, Employee.Employees);
				serializer.Serialize(SerializationStream, PlaceTrading.PlacesTrading);
				serializer.Serialize(SerializationStream, Order.Orders);
				serializer.Serialize(SerializationStream, Product.Products);
				serializer.Serialize(SerializationStream, User.Users);
				serializer.Serialize(SerializationStream, ProductInPlaceTrading.ProductInPlacesTrading);
			}
		}

		private void UserChanged(object sender, EventArgs e)
		{
			User.CurrentUser currUser = (User.CurrentUser)sender;
			if (currUser.User.Status != Status.NoLogin)
			{
				tabControl.IsEnabled = true;
				if (currUser.User.Status == Status.Admin)
				{
					miRemove.IsEnabled = true;
					miAdd.IsEnabled = true;
				}

				UpdateDataGrid();
			}
		}

		private void miLogin_Click(object sender, RoutedEventArgs e)
		{
			var loginF = new LoginWindow();
			loginF.Owner = this;
			loginF.ShowDialog();
		}

		private void miAddEmployee_Click(object sender, RoutedEventArgs e)
		{
			var editorF = new EditorFormEmployee();
			editorF.Owner = this;
			editorF.ShowDialog();
			this.Show();
		}

		private void miAddShop_Click(object sender, RoutedEventArgs e)
		{
			var editorF = new EditorFormShop();
			editorF.Owner = this;
			editorF.ShowDialog();
			this.Show();
		}

		private void miAddStorage_Click(object sender, RoutedEventArgs e)
		{
			var editorF = new EditorFormStorage();
			editorF.Owner = this;
			editorF.ShowDialog();
			this.Show();
		}

		private void miAddOrder_Click(object sender, RoutedEventArgs e)
		{
			var editorF = new EditorFormOrder();
			editorF.Owner = this;
			editorF.ShowDialog();
			this.Show();
		}

		private void miAddProduct_Click(object sender, RoutedEventArgs e)
		{
			var editorF = new EditorFormProduct();
			editorF.Owner = this;
			editorF.ShowDialog();
			this.Show();
		}

		private void miRemove_Click(object sender, RoutedEventArgs e)
		{
			switch (tabControl.SelectedIndex)
			{
				case 0:
					{
						foreach (var item in dgWorkers.SelectedItems)
							Employee.Employees.Remove((Employee)item);
						dgWorkers.Items.Refresh();
						dgShops.Items.Refresh();
						dgStorages.Items.Refresh();
						break;
					}
				case 1:
					{
						foreach (var item in dgShops.SelectedItems)
							Shop.PlacesTrading.Remove(((KeyValuePair<Guid, Shop>)item).Key);
						dgShops.ItemsSource = Shop.Shops;
						dgShops.Items.Refresh();
						break;
					}
				case 2:
					{
						foreach (var item in dgStorages.SelectedItems)
							Storage.PlacesTrading.Remove(((KeyValuePair<Guid, Storage>)item).Key);
						dgStorages.ItemsSource = Storage.Storages;
						dgStorages.Items.Refresh();
						break;
					}
				case 3:
					{
						foreach (var item in dgOrders.SelectedItems)
							Order.Orders.Remove((Order)item);
						dgOrders.Items.Refresh();
						break;
					}
				case 4:
					{
						foreach (var item in dgProducts.SelectedItems)
							((KeyValuePair<int, Product>)item).Value.Remove();
						dgProducts.ItemsSource = Product.Products;
						dgProducts.Items.Refresh();
						break;
					}
				default:
					break;
			}
		}

		private void MainForm_Loaded(object sender, RoutedEventArgs e)
		{
			if (File.Exists(SerializationFile))
				using (Stream DeserializationStream = File.OpenRead(SerializationFile))
				{
					BinaryFormatter deserializer = new BinaryFormatter();
					Employee.Employees = (List<Employee>)deserializer.Deserialize(DeserializationStream);
					PlaceTrading.PlacesTrading = (Dictionary<Guid, PlaceTrading>)deserializer.Deserialize(DeserializationStream);
					Order.Orders = (List<Order>)deserializer.Deserialize(DeserializationStream);
					Product.Products = (Dictionary<int, Product>)deserializer.Deserialize(DeserializationStream);
					User.Users = (List<User>)deserializer.Deserialize(DeserializationStream);
					ProductInPlaceTrading.ProductInPlacesTrading = (List<ProductInPlaceTrading>)deserializer.Deserialize(DeserializationStream);
				} else
			{
				MessageBox.Show("Попередніх даних не знайдено.");
			}

			var loginF = new LoginWindow();
			loginF.Owner = this;
			loginF.ShowDialog();
		}

		private void UpdateDataGrid()
		{
			dgWorkers.ItemsSource = Employee.Employees;
			dgShops.ItemsSource = Shop.Shops;
			dgStorages.ItemsSource = Storage.Storages;
			dgOrders.ItemsSource = Order.Orders;
			dgProducts.ItemsSource = Product.Products;

			dgWorkers.Items.Refresh();
			dgShops.Items.Refresh();
			dgStorages.Items.Refresh();
			dgOrders.Items.Refresh();
			dgProducts.Items.Refresh();
		}

		private void miProfile_Click(object sender, RoutedEventArgs e)
		{
			ProfileWindow profileW = new ProfileWindow();
			profileW.Owner = this;
			profileW.DataContext = miLogin.DataContext;
			profileW.ShowDialog();
		}

		private void miExit_Click(object sender, RoutedEventArgs e)
		{
			miLogin.Visibility = Visibility.Collapsed;
			miDoLogin.Visibility = Visibility.Visible;
			User.CurrentUser.CurrUser.Logout();
			miRemove.IsEnabled = false;
			miAdd.IsEnabled = false;
			tabControl.IsEnabled = false;
		}

		private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
		{
			DataGridRow row = sender as DataGridRow;
			if (row.DetailsVisibility == Visibility.Collapsed)
				row.DetailsVisibility = Visibility.Visible;
			else
				row.DetailsVisibility = Visibility.Collapsed;
		}

		private void EmployeeEdit_Click(object sender, RoutedEventArgs e)
		{
			var editorF = new EditorFormEmployee((Employee)((sender as MenuItem).DataContext));
			editorF.Owner = this;
			editorF.ShowDialog();
			this.Show();
		}

		private void ShopEdit_Click(object sender, RoutedEventArgs e)
		{
			var editorF = new EditorFormShop(((KeyValuePair<Guid, Shop>)((MenuItem)sender).DataContext).Value);
			editorF.Owner = this;
			editorF.ShowDialog();
			this.Show();
		}

		private void StorageEdit_Click(object sender, RoutedEventArgs e)
		{
			var editorF = new EditorFormStorage(((KeyValuePair<Guid, Storage>)((MenuItem)sender).DataContext).Value);
			editorF.Owner = this;
			editorF.ShowDialog();
			this.Show();
		}

		private void OrderEdit_Click(object sender, RoutedEventArgs e)
		{
			var editorF = new EditorFormOrder((Order)((MenuItem)sender).DataContext);
			editorF.Owner = this;
			editorF.ShowDialog();
			this.Show();
		}

		private void ProductEdit_Click(object sender, RoutedEventArgs e)
		{
			var editorF = new EditorFormProduct(((KeyValuePair<Int32, Product>)((MenuItem)sender).DataContext).Value);
			editorF.Owner = this;
			editorF.ShowDialog();
			this.Show();
		}

		private void btnComplete_Click(object sender, RoutedEventArgs e)
		{
			var order = (sender as Control).DataContext as Order;
			order.Status = Order.сomplete;
			dgOrders.Items.Refresh();
		}

		private void lvChoose_Loaded(object sender, RoutedEventArgs e)
		{
			var lvChoose = sender as ListView;
			lvChoose.ItemsSource = (lvChoose.DataContext as Order).PurchasedProducts;
		}

		private void lvProducts_Loaded(object sender, RoutedEventArgs e)
		{
			var comboBox = sender as ComboBox;
			comboBox.ItemsSource = Product.Products;
		}

		private void btnAddProduct_Click(object sender, RoutedEventArgs e)
		{
			var btnAddProduct = sender as Button;
			var dgProductsInPlaceTrading = btnAddProduct.DataContext as DataGrid;
			var placeTrading = (PlaceTrading)(dgProductsInPlaceTrading.DataContext);
			var product = btnAddProduct.Tag as Product;
			if (btnAddProduct.Tag is Product)
				if (!placeTrading.ProductsInPlacesTrading.Exists(pipt => pipt.Product.Barcode == product.Barcode))
				{
					new ProductInPlaceTrading(placeTrading, product);
					dgProductsInPlaceTrading.ItemsSource = placeTrading.ProductsInPlacesTrading;
					dgProductsInPlaceTrading.Items.Refresh();
				} else
					MessageBox.Show(this, "Товар з таким індексом вже існує.", "Увага!", MessageBoxButton.OK, MessageBoxImage.Warning);
		}
	}
}
