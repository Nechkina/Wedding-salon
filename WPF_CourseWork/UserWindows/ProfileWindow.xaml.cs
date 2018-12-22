using CourseWorkClassLibrary;
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

namespace WPF_CourseWork
{
	/// <summary>
	/// Логика взаимодействия для ProfileWindow.xaml
	/// </summary>
	public partial class ProfileWindow : Window
	{
		public ProfileWindow()
		{
			InitializeComponent();
		}

		private void tbtnName_Checked(object sender, RoutedEventArgs e)
		{
			spName.Visibility = Visibility.Visible;
		}

		private void tbtnLogin_Checked(object sender, RoutedEventArgs e)
		{
			spLogin.Visibility = Visibility.Visible;
		}		
		
		private void tbtnPass_Checked(object sender, RoutedEventArgs e)
		{
			spPass.Visibility = Visibility.Visible;
		}

		private void tbtnName_Unchecked(object sender, RoutedEventArgs e)
		{
			spName.Visibility = Visibility.Collapsed;
		}

		private void tbtnLogin_Unchecked(object sender, RoutedEventArgs e)
		{
			spLogin.Visibility = Visibility.Collapsed;
		}

		private void tbtnPass_Unchecked(object sender, RoutedEventArgs e)
		{
			if (pbOldPass.Password == User.CurrentUser.CurrUser.User.Pass)
			{
				spPass.Visibility = Visibility.Collapsed;
				User.CurrentUser.CurrUser.User.Pass = pbNewPass.Password;
				tblOutputPass.Text = string.Empty;
				pbOldPass.Password = pbNewPass.Password;
				pbNewPass.Password = string.Empty;
			} else
			{
				tblOutputPass.Visibility = Visibility.Visible;
				tblOutputPass.Text = "Неправильний пароль.";
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			var user = User.CurrentUser.CurrUser.User;
			tiCurrentUser.DataContext = user;
			if (user.Status != Status.Admin)
				tiOther.IsEnabled = false;
			else
				cbAccounts.ItemsSource = User.Users;
		}

		private void btnRemove_Click(object sender, RoutedEventArgs e)
		{
			User selectUser = (User)cbAccounts.SelectedItem;
			if (selectUser == null)
				return;
			if (User.Users.Count(user => user.Status == Status.Admin) == 1
				&& User.Users.First(user => user.Status == Status.Admin) == selectUser)
			{
				tblOutputOther.Text = "Не можна видаляти останій обліковий запис з адмінськими правами!";
				tblOutputOther.Visibility = Visibility.Visible;
				return;
			}
			if (MessageBox.Show(this, "Ви впевнені, що хочете видалити обліковий запис?", "Увага!", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
				return;

			if (User.CurrentUser.CurrUser.User.Equals(selectUser))
			{
				var owner = Owner as MainWindow;
				owner.miLogin.Visibility = Visibility.Collapsed;
				owner.miDoLogin.Visibility = Visibility.Visible;
				User.CurrentUser.CurrUser.Logout();
				User.Users.Remove(selectUser);
				this.Close();
			} else
			{
				User.Users.Remove(selectUser);
			}
			tblOutputOther.Text = "Обліковий запис видалено.";
			tblOutputOther.Visibility = Visibility.Visible;
			cbAccounts.Items.Refresh();
		}

		private void btnChange_Click(object sender, RoutedEventArgs e)
		{
			User selectUser = (User)cbAccounts.SelectedItem;
			if (selectUser == null)
				return;
			if(User.Users.Count(user => user.Status == Status.Admin) == 1 
				&& User.Users.First(user => user.Status == Status.Admin) == selectUser)
			{
				tblOutputOther.Text = "Не можна змінювати права останього облікового запису з адмінськими правами!";
				tblOutputOther.Visibility = Visibility.Visible;
				return;
			}

			Status [] statuses = { Status.Admin, Status.User };
			selectUser.Status = statuses [cbChangeStatus.SelectedIndex];
			tblOutputOther.Text = "Обліковий запис змінено.";
			tblOutputOther.Visibility = Visibility.Visible;
		}

		private void btnCreate_Click(object sender, RoutedEventArgs e)
		{
			if (User.Users.Exists(user => user.Login.Trim() == tbLogin.Text.Trim()))
			{
				tblOutputOther.Text = "Обліковий запис з таким іменем вже існує.";
				tblOutputOther.Visibility = Visibility.Visible;
				return;
			}
			Status [] statuses = {Status.Admin, Status.User};
			new User(tbLogin.Text.Trim(), pbPass.Password, statuses [cbStatus.SelectedIndex]);
			tblOutputOther.Text = "Обліковий запис створено.";
			tblOutputOther.Visibility = Visibility.Visible;
		}
	}
}
