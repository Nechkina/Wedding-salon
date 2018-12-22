using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkClassLibrary
{
	[Serializable]
	public class User
	{
		public static List<User> Users = new List<User>();

		private string _name;

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		private string _login;

		public string Login
		{
			get { return _login; }
			set { _login = value; }
		}

		private string _pass;

		public string Pass
		{
			get { return _pass; }
			set { _pass = value; }
		}

		private Status _status;

		public Status Status
		{
			get { return _status; }
			set { _status = value; }
		}

		public User(string name, string login, string pass, Status status)
		{
			_name = name;
			_login = login;
			_pass = pass;
			_status = status;
			User.Users.Add(this);
		}

		public User(string login, string pass, Status status)
		{
			_name = string.Empty;
			_login = login;
			_pass = pass;
			_status = status;
			User.Users.Add(this);
		}

		private User() { }

		public override string ToString()
		{
			return string.Format("{0}({1})", Login, Status);
		}

		public class CurrentUser : INotifyPropertyChanged
		{
			public User User;

			private static CurrentUser instance;

			public event PropertyChangedEventHandler PropertyChanged;

			public static CurrentUser CurrUser
			{
				get
				{
					if (instance == null)
					{
						instance = new CurrentUser();
						instance.User = new User
						{
							Name = string.Empty,
							Login = string.Empty,
							Pass = string.Empty,
							Status = Status.NoLogin
						};
					}
					return instance;
				}
			}

			public void DoLogin(User user)
			{
				var lastStat = User.Status;
				instance.User = user;
				if (PropertyChanged != null && lastStat != User._status)
					PropertyChanged(this, new PropertyChangedEventArgs("Status"));
			}

			public void Logout()
			{
				var lastStat = User.Status;
				instance.User = new User
				{
					Name = string.Empty,
					Login = string.Empty,
					Pass = string.Empty,
					Status = Status.NoLogin
				};
				if (PropertyChanged != null && lastStat != Status.NoLogin)
					PropertyChanged(this, new PropertyChangedEventArgs("Status"));
			}

			public override string ToString()
			{
				return User._login;
			}
		}
	}
}
