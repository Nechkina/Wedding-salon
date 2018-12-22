using CourseWorkClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF_CourseWork.EditForms
{
	/// <summary>
	/// Логика взаимодействия для EditorFormOrder.xaml
	/// </summary>
	public partial class EditorFormOrder : Window
	{
		public EditorFormOrder()
		{
			InitializeComponent();

			lbProducts.ItemsSource = Product.Products.Values.ToList();
			cpPT.ItemsSource = PlaceTrading.PlacesTrading.Values.ToList();
			btn.Click += new RoutedEventHandler(this.ButtonAdd_Click);
			spPT.Visibility = Visibility.Visible;
		}

		public EditorFormOrder(Order order)
		{
			InitializeComponent();

			lbProducts.ItemsSource = Product.Products.Values.ToList();
			cpPT.ItemsSource = PlaceTrading.PlacesTrading.Values.ToList();
			this.DataContext = order;
			btn.Content = "Змінити запис";
			btn.Click += new RoutedEventHandler(this.ButtonEdit_Click);

			if (order.IsDelivery)
				spAdress.Visibility = Visibility.Visible;
			else
			{
				foreach (var item in cpPT.Items)
				{
					if (item.ToString() == order.Address)
					{
						cpPT.Items.MoveCurrentTo(item);
						cpPT.SelectedItem = item;
						break;
					}
				}
				spPT.Visibility = Visibility.Visible;
			}

			var purchasedProducts = order.PurchasedProducts;
			foreach (var item in lbProducts.Items)
			{
				if (purchasedProducts.Any(pp => ((Product)item).Barcode == pp.Key.Barcode))
					lbProducts.SelectedItems.Add(item);
			}
		}

		private void ButtonEdit_Click(object sender, RoutedEventArgs e)
		{
			tbErrors.Text = string.Empty;
			if (IsValid())
			{
				var order = DataContext as Order;

				var prodList = new Dictionary<Product, int>();
				foreach (var item in lvChoose.Items)
				{
					var tbCount = FindTextBox(item);

					prodList.Add((Product)item, int.Parse(tbCount.Text));
				}

				order.CustomerName = tbName.Text;
				order.PurchasedProducts = prodList;
				order.Phone = tbPhone.Text;
				order.E_mail = tbEmail.Text;

				if (rbDelivMethodSelf.IsChecked == true)
				{
					order.Address = cpPT.Text;
					order.IsDelivery = false;
				} else
				{
					order.Address = tbAdress.Text;
					order.IsDelivery = true;
				}

				tbErrors.Text = "Запис змінено.";
			}
		}

		private void ButtonAdd_Click(object sender, RoutedEventArgs e)
		{
			tbErrors.Text = string.Empty;
			if (IsValid())
			{
				var prodList = new Dictionary<Product, int>();
				foreach (var item in lvChoose.Items)
				{
					var tbCount = FindTextBox(item);

					prodList.Add((Product)item, int.Parse(tbCount.Text));
				}
				if (rbDelivMethodSelf.IsChecked == true)
					new Order(tbName.Text, cpPT.Text, tbPhone.Text, tbEmail.Text, prodList, false);
				else
					new Order(tbName.Text, tbAdress.Text, tbPhone.Text, tbEmail.Text, prodList, true);
				tbErrors.Text = "Новий запис створено.";
			}
		}

		/// <summary>
		/// get from https://msdn.microsoft.com/ru-ru/library/bb613579%28v=vs.110%29.aspx
		/// </summary>
		/// <typeparam name="childItem"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		private childItem FindVisualChild<childItem>(DependencyObject obj)
		where childItem : DependencyObject
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(obj, i);
				if (child != null && child is childItem)
					return (childItem)child;
				else
				{
					childItem childOfChild = FindVisualChild<childItem>(child);
					if (childOfChild != null)
						return childOfChild;
				}
			}
			return null;
		}

		const string tbC = "tbCount";
		private TextBox FindTextBox(object item)
		{
			// get from https://msdn.microsoft.com/ru-ru/library/bb613579%28v=vs.110%29.aspx
			ListViewItem lbProductsItem = (ListViewItem)(lvChoose.ItemContainerGenerator.ContainerFromItem(item));
			ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(lbProductsItem);
			DataTemplate myDataTemplate = contentPresenter.ContentTemplate;
			return (TextBox)myDataTemplate.FindName(tbC, contentPresenter);
		}

		private bool IsValid()
		{
			bool isValid = true;
			StringBuilder errorMessadge = new StringBuilder("Поля або поле ");
			TextBlock [] textBlocks = { tblName, tblPhone };
			TextBox [] textBoxs = { tbName, tbPhone };

			if (rbDelivMethod.IsChecked == true)
			{
				if (tbAdress.Text == string.Empty)
				{
					isValid = false;
					errorMessadge.Append("\"" + tblAdress.Text + "\"");
				}
			}

			if (rbDelivMethodSelf.IsChecked == true)
			{
				if (cpPT.Text == string.Empty)
				{
					isValid = false;
					errorMessadge.Append("\"" + tblPT.Text + "\"");
				}
			}

			for (int i = 0; i < 2; i++)
			{
				if (textBoxs [i].Text == string.Empty)
				{
					if (isValid)
					{
						isValid = false;
						errorMessadge.Append("\"" + textBlocks [i].Text + "\"");
					} else
					{
						errorMessadge.Append(", \"" + textBlocks [i].Text + "\"");
					}
				}
			}

			if (!lvChoose.HasItems)
			{
				if (isValid)
				{
					isValid = false;
					errorMessadge.Append("\"Вибрані товари\"");
				} else
					errorMessadge.Append(", \"Вибрані товари\"");

				errorMessadge.Append(" не можуть бути пустими! ");
			} else
			{
				errorMessadge.Append(" не можуть бути пустими! ");

				foreach (var item in lvChoose.Items)
				{
					var tbCount = FindTextBox(item);

					if (tbCount.Text == string.Empty)
					{
						errorMessadge.Append("Поле \"Кількість\" в \"Вибрані товари\" не може бути пустим! ");
						break;
					}
				}
			}

			if (rbDelivMethodSelf.IsChecked == false && rbDelivMethod.IsChecked == false)
			{
				if (isValid)
				{
					isValid = false;
					errorMessadge.Clear();
				}
				errorMessadge.Append("Виберіть спосіб доставки!");
			}

			if (!isValid)
			{
				tbErrors.Text = errorMessadge.ToString();
			}

			return isValid;
		}

		private void rbDelivMethodSelf_Checked(object sender, RoutedEventArgs e)
		{
			spPT.Visibility = Visibility.Hidden;
			spAdress.Visibility = Visibility.Visible;
		}

		private void rbDelivMethodAdress_Checked(object sender, RoutedEventArgs e)
		{
			spPT.Visibility = Visibility.Visible;
			spAdress.Visibility = Visibility.Hidden;
		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			DataGrid ownerDgOrders = (Owner as MainWindow).dgOrders;
			ownerDgOrders.ItemsSource = Order.Orders;
			ownerDgOrders.Items.Refresh();
		}

		private void tbCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		private void lvChoose_Loaded(object sender, RoutedEventArgs e)
		{
			if (DataContext is Order)
			{
				var purchasedProducts = (DataContext as Order).PurchasedProducts;
				foreach (var item in lvChoose.Items)
				{
					var tbCount = FindTextBox(item);

					tbCount.Text = purchasedProducts.First(pp => pp.Key.Barcode == (tbCount.DataContext as Product).Barcode).Value.ToString();
				}
			}
		}
	}
}
