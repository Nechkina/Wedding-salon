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
using System.Windows.Shapes;
using CourseWorkClassLibrary;
using System.ComponentModel;

namespace WPF_CourseWork
{
	/// <summary>
	/// Логика взаимодействия для EditorFormShop.xaml
	/// </summary>
	public partial class EditorFormShop : Window
	{
		public EditorFormShop()
		{
			InitializeComponent();

			btn.Click += new RoutedEventHandler(this.ButtonAdd_Click);
		}

		public EditorFormShop(Shop shop)
		{
			InitializeComponent();

			this.DataContext = shop;
			btn.Content = "Змінити запис";
			btn.Click += new RoutedEventHandler(this.ButtonEdit_Click);
		}

		private void ButtonAdd_Click(object sender, RoutedEventArgs e)
		{
			tbErrors.Text = string.Empty;
			if (IsValid())
			{
				new Shop(tbName.Text, tbAdress.Text);
				tbErrors.Text = "Новий запис створено.";
			}
		}

		private void ButtonEdit_Click(object sender, RoutedEventArgs e)
		{
			tbErrors.Text = string.Empty;
			if (IsValid())
			{
				var shop = DataContext as Shop;				
				shop.Location = tbAdress.Text;
				shop.Name = tbName.Text;

				tbErrors.Text = "Запис змінено.";
			}
		}

		private bool IsValid()
		{
			bool isValid = true;
			StringBuilder errorMessadge = new StringBuilder("Поля або поле");
			TextBlock [] textBlocks = { tblName, tblAdress };
			TextBox [] textBoxs = { tbName, tbAdress };

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

			if (!isValid)
			{
				errorMessadge.Append(" не можуть бути пустими!");
				tbErrors.Text = errorMessadge.ToString();
			}

			return isValid;
		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			DataGrid ownerDgShops = (Owner as MainWindow).dgShops;
			ownerDgShops.ItemsSource = Shop.Shops;
			ownerDgShops.Items.Refresh();
		}
	}
}
