using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkClassLibrary
{
  [Serializable]
  public class Product
  {
    static public Dictionary<int, Product> Products = new Dictionary<int, Product>();

    private string _name;

    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    private int _price;

    public int Price
    {
      get { return _price; }
      set { _price = value; }
    }

   // public List<string> Tegs = new List<string>();

   // public string TegsString
   // {
   //   get
   //   {
   //     StringBuilder tegsString = new StringBuilder();
   //     Tegs.ForEach(t => tegsString.Append(t + ", "));
   //     return tegsString.ToString();
   //   }
   //   set { Tegs = value.Split(',').Select(el => el.Trim()).ToList(); }
   // }

    private string _manufacturer;

    public string Manufacturer
    {
      get { return _manufacturer; }
      set { _manufacturer = value; }
    }

    private string _countryManufacturer;

    public string CountryManufacturer
    {
      get { return _countryManufacturer; }
      set { _countryManufacturer = value; }
    }

    private int _barcode;

    public int Barcode
    {
      get { return _barcode; }
      protected set { _barcode = value; }
    }

    public Product(string name, int price, //string tegs,
        string manufacturer, string country, int barcode)
    {
      Name = name;
      Price = price;
      //TegsString = tegs;
      Manufacturer = manufacturer;
      CountryManufacturer = country;
      Barcode = barcode;
      Products.Add(barcode, this);
    }

    public int CountInShops
    {
      get
      {
        return ProductInPlaceTrading.ProductInPlacesTrading.Where(pint => pint.Product == this && pint.PlaceTrading is Shop).Sum(pint => pint.Count);
      }
    }

    public int CountInStorages
    {
      get
      {
        return ProductInPlaceTrading.ProductInPlacesTrading.Where(pint => pint.Product == this && pint.PlaceTrading is Storage).Sum(pint => pint.Count);
      }
    }

    public List<ProductInPlaceTrading> ShopsList
    {
      get
      {
        return ProductInPlaceTrading.ProductInPlacesTrading.Where(pint => pint.Product == this && pint.PlaceTrading is Shop).ToList();
      }
    }

    public List<ProductInPlaceTrading> StoragesList
    {
      get
      {
        return ProductInPlaceTrading.ProductInPlacesTrading.Where(pint => pint.Product == this && pint.PlaceTrading is Storage).ToList();
      }
    }

    public bool Remove()
    {
      ProductInPlaceTrading.ProductInPlacesTrading.RemoveAll(pint => pint.Product == this);
      return Products.Remove(Barcode);
    }

    public override string ToString()
    {
      return string.Format("{0} - {1}грн", Name, Price);
    }
  }
}
