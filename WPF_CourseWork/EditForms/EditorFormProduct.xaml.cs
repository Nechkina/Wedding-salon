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
	/// Логика взаимодействия для EditorFormProduct.xaml
	/// </summary>
	public partial class EditorFormProduct : Window
	{
		public EditorFormProduct()
		{
			InitializeComponent();
			btn.Click += new RoutedEventHandler(this.ButtonAdd_Click);
		}

		public EditorFormProduct(Product product)
		{
			InitializeComponent();

			this.DataContext = product;
			btn.Content = "Змінити запис";
			btn.Click += new RoutedEventHandler(this.ButtonEdit_Click);
			tbBarcode.Visibility = Visibility.Collapsed;
			tblBarcode.Visibility = Visibility.Collapsed;
		}

		private void ButtonEdit_Click(object sender, RoutedEventArgs e)
		{
			//tbErrors.Text = string.Empty;
			if (IsValid())
			{
				var product = DataContext as Product;
				product.Name = tbName.Text;
				product.Price = int.Parse(tbPrice.Text);
				//product.TegsString = tbTegs.Text;
				product.Manufacturer = tbManufacturer.Text;
				product.CountryManufacturer = tbCountry.Text;

				tbErrors.Text = "Запис змінено.";
			}
		}

		private void ButtonAdd_Click(object sender, RoutedEventArgs e)
		{
			tbErrors.Text = string.Empty;
			try
			{
				if (IsValid())
				{
					new Product(tbName.Text, int.Parse(tbPrice.Text), //tbTegs.Text,
                        tbManufacturer.Text, tbCountry.Text, int.Parse(tbBarcode.Text));
					tbErrors.Text = "Новий запис створено.";
				}
			}
			catch (ArgumentException)
			{
				if (MessageBox.Show(this, "Товар з таким індексом вже існує. Ви хочете замінити його?", "Увага!", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
				{
					Product.Products [int.Parse(tbBarcode.Text)].Remove();
					new Product(tbName.Text, int.Parse(tbPrice.Text), //tbTegs.Text, 
                        tbManufacturer.Text, tbCountry.Text, int.Parse(tbBarcode.Text));
				}
			}
		}

		/// <summary>
		/// Перевірка на правильніть введених даних.
		/// </summary>
		/// <returns>повертає true, якщо правильно або false, якщо ні.</returns>
		private bool IsValid()
		{
			bool isValid = true;
			StringBuilder errorMessadge = new StringBuilder("Поля або поле");
			TextBlock [] textBlocks = { tblName, tblPrice, tblManufacturer, tblCountry, tblBarcode, //tblTegs
            };
			TextBox [] textBoxs = { tbName, tbPrice, tbManufacturer, tbCountry, tbBarcode, //tbTegs
            };

			//for (int i = 0; i < 6; i++)
			//{
			//	if (textBoxs [i].Text == string.Empty)
			//	{
			//		if (isValid)
			//		{
			//			isValid = false;
			//			errorMessadge.Append("\"" + textBlocks [i].Text + "\"");
			//		} else
			//		{
			//			errorMessadge.Append(", \"" + textBlocks [i].Text + "\"");
			//		}
			//	}
			//}

			if (!isValid)
			{
				errorMessadge.Append(" не можуть бути пустими!");
				tbErrors.Text = errorMessadge.ToString();
			}

			//int price, barcode;
			//if (!int.TryParse(tbPrice.Text, out price) && tbPrice.Text != string.Empty)
			//{
			//	tbErrors.Text += ("Поле \"Ціна\" не може містити літери!");
			//	isValid = false;
			//} else if (price < 0)
			//{
			//	tbErrors.Text += ("Поле \"Ціна\" не може мати значення менше нуля!");
			//	isValid = false;
			//}
			//if (!int.TryParse(tbBarcode.Text, out barcode) && tbBarcode.Text != string.Empty)
			//{
			//	tbErrors.Text += ("Поле \"Штрих-код\" не може містити літери!");
			//	isValid = false;
			//} else if (barcode < 0) { 
			//	tbErrors.Text += ("Поле \"Штрих-код\" не може мати значення менше нуля!");
			//	isValid = false;
			//}
			return isValid;
		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			var owner = Owner as MainWindow;
			owner.dgProducts.ItemsSource = Product.Products;
			owner.dgProducts.Items.Refresh();
		}

		private void tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}
	}
}