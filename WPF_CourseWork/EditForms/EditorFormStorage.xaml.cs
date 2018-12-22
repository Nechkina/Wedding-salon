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

namespace WPF_CourseWork
{
	/// <summary>
	/// Логика взаимодействия для EditorFormStorage.xaml
	/// </summary>
	public partial class EditorFormStorage : Window
	{
		public EditorFormStorage()
		{
			InitializeComponent();

			btn.Click += new RoutedEventHandler(this.ButtonAdd_Click);
		}

		public EditorFormStorage(Storage shop)
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
				new Storage(tbName.Text, tbAdress.Text, int.Parse(tbArea.Text));
				tbErrors.Text = "Новий запис створено.";
			}
		}

		private void ButtonEdit_Click(object sender, RoutedEventArgs e)
		{
			tbErrors.Text = string.Empty;
			if (IsValid())
			{
				var storage = (Storage)DataContext;
				storage.Location = tbAdress.Text;
				storage.Name = tbName.Text;
				storage.Area = int.Parse(tbArea.Text);

				tbErrors.Text = "Запис змінено.";
			}
		}

		private bool IsValid()
		{
			bool isValid = true;
			StringBuilder errorMessadge = new StringBuilder("Поля або поле");
			TextBlock [] textBlocks = { tblName, tblAdress, tblArea };
			TextBox [] textBoxs = { tbName, tbAdress, tbArea };

			for (int i = 0; i < 3; i++)
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
			int area;
			if (!isValid)
			{
				errorMessadge.Append(" не можуть бути пустими!");
				if (!int.TryParse(tbArea.Text, out area) && tbArea.Text != string.Empty)
					errorMessadge.Append("Поле \"Площа\" не може містити літери!");
				else if (area < 0)
					errorMessadge.Append("Поле \"Площа\" не може мати значення менше нуля!");
				tbErrors.Text = errorMessadge.ToString();
			}

			//if (!int.TryParse(tbArea.Text, out area) && tbArea.Text != string.Empty)
			//	tbErrors.Text += ("Поле \"Площа\" не може містити літери!");
			//else if (area < 0)
			//	tbErrors.Text += ("Поле \"Площа\" не може мати значення менше нуля!");
				
			return isValid;
		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			DataGrid ownerDgStorages = (Owner as MainWindow).dgStorages;
			ownerDgStorages.ItemsSource = Storage.Storages;
			ownerDgStorages.Items.Refresh();
		}

		private void tbArea_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}
	}
}
