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

namespace WPF_CourseWork
{
	/// <summary>
	/// Логика взаимодействия для LoginForm.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		public LoginWindow()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			User user = User.Users.FirstOrDefault(u => u.Login == tbLogin.Text && u.Pass == pbPass.Password);
			if (user != default(User))
				Login(user);
			else
			{
				tblOutput.Text = "Неправильне ім'я користувача або пароль.";
				tblOutput.Visibility = Visibility.Visible;
			}
		}

		private void Login(User user)
		{
			User.CurrentUser.CurrUser.DoLogin(user);
			MainWindow owner = Owner as MainWindow;
			owner.miDoLogin.Visibility = Visibility.Collapsed;
			MenuItem miLogin = owner.miLogin;
			miLogin.DataContext = user;
			miLogin.Visibility = Visibility.Visible;
			this.Close();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (User.Users.Count == 0)
			{
				Login(new User(String.Empty, "admin", String.Empty, Status.Admin));
				return;
			}
			if (User.Users.Count == 1 && User.Users.Exists(user => user.Pass == string.Empty))
				Login(User.Users [0]);
		}
	}
}
