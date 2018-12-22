using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkClassLibrary
{
  [Serializable]
  sealed public class Storage : PlaceTrading
  {
    static public Dictionary<Guid, Storage> Storages
    {
      get { return PlacesTrading.Where(pt => pt.Value is Storage).ToDictionary(pt => pt.Key, pt => (Storage)pt.Value); }
    }

    private int _area;

    public int Area
    {
      get { return _area; }
      set { _area = value; }
    }

    private int Occupancy = 0;

    public Storage(string name, string loc, int area)
    {
      Name = name;
      Location = loc;
      Area = area;
      Id = Guid.NewGuid();
      PlacesTrading.Add(Id, this);
    }

    public void Remove()
    {
      PlacesTrading.Remove(Id);
    }

    public string PercentOccupancy
    {
      get { return (Occupancy / Area) * 100 + "%"; }
    }

    public override string ToString()
    {
      return "Склад " + base.ToString();
    }
  }
}