using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CourseWorkClassLibrary
{
  [Serializable]
  sealed public class Employee
  {
    static public List<Employee> Employees = new List<Employee>();

    private Guid _jobId;

    private string _name;

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    public PlaceTrading Job
    {
      get
      {
        if (_jobId == Guid.Empty)
          return null;
        return PlaceTrading.PlacesTrading [_jobId];
      }
      set
      {
        if (value == null)
          _jobId = Guid.Empty;
        else
          _jobId = value.Id;
      }
    }

    private string _position;

    public string Position
    {
      get { return _position; }
      set { _position = value; }
    }

    private string _birthday;

    public string Birthday
    {
      get { return _birthday; }
      set { _birthday = value; }
    }

    private string _address;

    public string Address
    {
      get { return _address; }
      set { _address = value; }
    }

    private string _mobilePhone;

    public string MobilePhone
    {
      get { return _mobilePhone; }
      set { _mobilePhone = value; }
    }

    private string _homePhone;

    public string HomePhone
    {
      get { return _homePhone; }
      set { _homePhone = value; }
    }

    public Employee(PlaceTrading job, string name, string birthday, string address, string position, string mobilePhone = null, string homePhone = null)
    {
      Job = job;
      Name = name;
      Birthday = birthday;
      Address = address;
      Position = position;
      MobilePhone = mobilePhone;
      HomePhone = homePhone;
      Employees.Add(this);
    }

    Employee() { }

    public override string ToString()
    {
      return Name;
    }
  }
}
