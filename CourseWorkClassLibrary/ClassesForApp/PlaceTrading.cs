using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkClassLibrary
{
  [Serializable]
  public abstract class PlaceTrading
  {
    static public Dictionary<Guid, PlaceTrading> PlacesTrading = new Dictionary<Guid, PlaceTrading>();

    private Guid _id;

    public Guid Id
    {
      get { return _id; }
      set { _id = value; }
    }

    private string _name;

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    private string _location;

    public string Location
    {
      get { return _location; }
      set { _location = value; }
    }

    public List<ProductInPlaceTrading> ProductsInPlacesTrading
    {
      get { return ProductInPlaceTrading.ProductInPlacesTrading.Where(pipt => pipt.PlaceTrading == this).ToList(); }
    }

    public int CountEmployee
    {
      get { return Employee.Employees.Count(emp => emp.Job == this); }
    }

    public List<Employee> EmployeeList
    {
      get { return Employee.Employees.Where(emp => emp.Job == this).ToList(); }
    }

    public override string ToString()
    {
      return Name + " (" + Location + ")";
    }
  }
}
