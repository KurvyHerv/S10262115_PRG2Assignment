//==========================================================
// Student Number : S10258053, 
// Student Name : Soong Chu Wen Rena
// Partner Name : 
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262115_PRG2Assignment
{
    class Order
    {
        public int Id { get; set; }
        public DateTime TimeReceived { get; set; }
        public DateTime? TimeFullfilled { get; set; }
        public List<IceCream> IceCreamList { get; set; } = new List<IceCream>();
        public Order() { }
        public Order(int id, DateTime timereceived)
        {
            Id = id;
            TimeReceived = timereceived;
        }
        public void ModifyIceCream(int iceCream)
        {
            Console.WriteLine("Enter new option: ");
            IceCreamList[iceCream].Option = Console.ReadLine();

            Console.WriteLine("Enter new number of scoops: ");
            IceCreamList[iceCream].Scoops = Convert.ToInt32(Console.ReadLine());
            IceCreamList[iceCream].Flavours.Clear();
            Console.WriteLine("Enter new flavour type (or nil to stop adding): ");
            string flavourtype = Console.ReadLine();
            Console.WriteLine("Is it premium? (True/False): ");
            bool flavourpremium = Convert.ToBoolean(Console.ReadLine());
            Console.WriteLine("Enter new flavour quantity: ");
            int flavourquantity = Convert.ToInt32(Console.ReadLine());
            Flavour flavour = new Flavour(flavourtype, flavourpremium, flavourquantity);
            IceCreamList[iceCream].Flavours.Add(flavour);
            while (flavourtype != "nil")
            {
                Console.WriteLine("Enter new flavour (or nil to stop adding): ");
                flavourtype = Console.ReadLine();
                Console.WriteLine("Is it premium? (True/False): ");
                flavourpremium = Convert.ToBoolean(Console.ReadLine());
                Console.WriteLine("Enter new flavour quantity: ");
                flavourquantity = Convert.ToInt32(Console.ReadLine());
                flavour = new Flavour(flavourtype, flavourpremium, flavourquantity);
                IceCreamList[iceCream].Flavours.Add(flavour);
            }
            IceCreamList[iceCream].Toppings.Clear();
            Console.WriteLine("Enter new topping (or nil to stop adding): ");
            string toppingtype = Console.ReadLine();
            Topping topping = new Topping(toppingtype);
            IceCreamList[iceCream].Toppings.Add(topping);
            while (toppingtype != "nil")
            {
                Console.WriteLine("Enter new topping (or nil to stop adding): ");
                toppingtype = Console.ReadLine();
                topping = new Topping(toppingtype);
                IceCreamList[iceCream].Toppings.Add(topping);
            }
        }
        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);
            Console.WriteLine("Added successfully.");
        }
        public void DeleteIceCream(int num)
        {
            IceCreamList.RemoveAt(num);
            Console.WriteLine("Deleted successfully.");
        }
        public double CalculateTotal()
        {
            double total = 0;
            foreach (IceCream icecream in IceCreamList)
            {
                total += icecream.CalculatePrice();
            }
            return total;
        }
        public override string ToString()
        {


            string iceCreams = "";
            for (int i = 0; i < IceCreamList.Count; i++)
            {
                iceCreams += $"{IceCreamList[i].ToString()} | ";
            }
            return "ID: " + Id + "\tTime Received: " + TimeReceived + "\tTime Fullfilled: " + TimeFullfilled + "\tIceCreamList: " + iceCreams;
        }
    }
}
