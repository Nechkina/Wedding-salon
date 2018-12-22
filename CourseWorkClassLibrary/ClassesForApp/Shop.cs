using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace CourseWorkClassLibrary
{
  [Serializable]
  sealed public class Shop : PlaceTrading
  {
    static public Dictionary<Guid, Shop> Shops
    {
      get { return PlacesTrading.Where(pt => pt.Value is Shop).ToDictionary(pt => pt.Key, pt => (Shop)pt.Value); }
    }

    public Shop(string name, string loc)
    {
      Name = name;
      Location = loc;
      Id = Guid.NewGuid();
      //Shops.Add(Id, this);
      PlacesTrading.Add(Id, this);
    }

    public void Remove()
    {
      //Shops.Remove(Id);
      PlacesTrading.Remove(Id);
    }

    public override string ToString()
    {
      return "Магазин " + base.ToString();
    }
  }
}
