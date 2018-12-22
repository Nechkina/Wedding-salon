using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkClassLibrary
{
  [Serializable]
  public class ProductInPlaceTrading
  {
    static public List<ProductInPlaceTrading> ProductInPlacesTrading = new List<ProductInPlaceTrading>();

    private int _productBarcode;
    public Product Product
    {
      get { return Product.Products [_productBarcode]; }
      set { _productBarcode = value.Barcode; }
    }

    private Guid _placeTradingId;
    public PlaceTrading PlaceTrading
    {
      get { return PlaceTrading.PlacesTrading [_placeTradingId]; }
      set { _placeTradingId = value.Id; }
    }

    private int _count;

    public int Count
    {
      get { return _count; }
      set { _count = value; }
    }

    public ProductInPlaceTrading(PlaceTrading placeTrading, Product product)
    {
      PlaceTrading = placeTrading;
      Product = product;
      ProductInPlacesTrading.Add(this);
    }
  }
}
