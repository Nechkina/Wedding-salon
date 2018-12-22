using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkClassLibrary
{
  [Serializable]
  sealed public class Order
  {
    static public List<Order> Orders = new List<Order>();

    public Dictionary<Product, int> PurchasedProducts;

    private string _customerName;

    public string CustomerName
    {
      get { return _customerName; }
      set { _customerName = value; }
    }

    private string _address;

    public string Address
    {
      get { return _address; }
      set { _address = value; }
    }

    private string _phone;

    public string Phone
    {
      get { return _phone; }
      set { _phone = value; }
    }

    private string _e_mail;

    public string E_mail
    {
      get { return _e_mail; }
      set { _e_mail = value; }
    }

    private string _status;

    public const string сomplete = "Виконано";
    public const string noComplete = "Не виконано";

    public string Status
    {
      get { return _status; }
      set { _status = value; }
    }

    private bool delivery;

    private const string trDeliv = "Доставка по адресу";
    private const string fDeliv = "Самовивіз";

    public string Delivery
    {
      get
      {
        if (delivery)
          return trDeliv;
        return fDeliv;
      }
    }

    public bool IsDelivery
    {
      get { return delivery; }
      set { delivery = value; }
    }

    public Order(string name, string addr, string phone, string email, IDictionary<Product, int> products, bool deliv = false)
    {
      CustomerName = name;
      Address = addr;
      Phone = phone;
      E_mail = email;
      PurchasedProducts = new Dictionary<Product, int>(products);
      Status = noComplete;
      delivery = deliv;
      Orders.Add(this);
    }

    public Order(string name, string addr, string phone, IDictionary<Product, int> products, bool deliv = false)
    {
      CustomerName = name;
      Address = addr;
      Phone = phone;
      PurchasedProducts = new Dictionary<Product, int>(products);
      Status = noComplete;
      delivery = deliv;
      Orders.Add(this);
    }
  }
}
