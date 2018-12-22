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
	/// Логика взаимодействия для EditorFormEmpoyee.xaml
	/// </summary>
	public partial class EditorFormEmployee : Window
	{
		public EditorFormEmployee()
		{
			InitializeComponent();

			btn.Click += new RoutedEventHandler(this.ButtonAdd_Click);
			cbPlaceWork.ItemsSource = PlaceTrading.PlacesTrading;
		}

		public EditorFormEmployee(Employee employee)
		{
			InitializeComponent();

			this.DataContext = employee;
			btn.Content = "Змінити запис";

			btn.Click += new RoutedEventHandler(this.ButtonEdit_Click);

			cbPlaceWork.ItemsSource = PlaceTrading.PlacesTrading;
			foreach (var item in cbPlaceWork.Items)
			{
				if (((KeyValuePair<Guid, PlaceTrading>)item).Value == employee.Job)
				{
					cbPlaceWork.Items.MoveCurrentTo(item);
					cbPlaceWork.SelectedItem = item;
					break;
				}
			}
		}

		private void ButtonAdd_Click(object sender, RoutedEventArgs e)
		{
			tbErrors.Text = string.Empty;
			if (IsValid())
			{
				new Employee(((KeyValuePair<Guid, PlaceTrading>)cbPlaceWork.SelectedItem).Value, tbName.Text,
								dpBithday.Text, tbAdress.Text, tbPosition.Text, tbMobPhone.Text, tbHomephone.Text);
				tbErrors.Text = "Новий запис створено.";
			}
		}

		private void ButtonEdit_Click(object sender, RoutedEventArgs e)
		{
			tbErrors.Text = string.Empty;
			if (IsValid())
			{
				var employee = DataContext as Employee;
				employee.Job = ((KeyValuePair<Guid, PlaceTrading>)cbPlaceWork.SelectedItem).Value;
				employee.Name = tbName.Text;
				employee.Birthday = dpBithday.Text;
				employee.Address = tbAdress.Text;
				employee.Position = tbPosition.Text;
				employee.MobilePhone = tbMobPhone.Text;
				employee.HomePhone = tbHomephone.Text;

				tbErrors.Text = "Запис змінено.";
			}
		}

		private bool IsValid()
		{
			bool isValid = true;
			StringBuilder errorMessadge = new StringBuilder("Поля або поле");
			TextBlock [] textBlocks = { tblName, tblPosition, tblMobPhone, tblHomephone, tblAdress };
			TextBox [] textBoxs = { tbName, tbPosition, tbMobPhone, tbHomephone, tbAdress };

			for (int i = 0; i < 5; i++)
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

			if (cbPlaceWork.Text == string.Empty)
			{
				if (isValid)
				{
					isValid = false;
					errorMessadge.Append("\"" + tblPlaceWork.Text + "\"");
				} else
				{
					errorMessadge.Append(", \"" + tblPlaceWork.Text + "\"");
				}
			}

			if (dpBithday.Text == string.Empty)
			{
				if (isValid)
				{
					isValid = false;
					errorMessadge.Append("\"" + tblBithday.Text + "\"");
				} else
				{
					errorMessadge.Append(", \"" + tblBithday.Text + "\"");
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
			var owner = Owner as MainWindow;
			owner.dgWorkers.ItemsSource = Employee.Employees;
			owner.dgWorkers.Items.Refresh();
			owner.dgShops.ItemsSource = Shop.Shops;
			owner.dgShops.Items.Refresh();
			owner.dgStorages.ItemsSource = Storage.Storages;
			owner.dgStorages.Items.Refresh();
		}
	}
}
