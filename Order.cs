//==========================================================
// Student Number : S10258053, 
// Student Name : Soong Chu Wen Rena
// Partner Name : 
//==========================================================

using System;
using System.Collections.Generic;
using System.Data;
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
        public void ModifyIceCream(int index)
        {
            List<Flavour> flavours = new List<Flavour>();
            List<Topping> toppings = new List<Topping>();
            string[] premiumList = { "Durian", "Ube", "Sea Salt" };
            IceCream iceCream = IceCreamList[index];
            string waffleFlavour = "";
            bool dipped = false;


            Console.WriteLine("Enter new option: ");
            string option = Console.ReadLine().ToLower();

            if (option == "waffle")
            {
                Console.Write("Enter waffle flavour: ");
                waffleFlavour = Console.ReadLine();
            }
            else if (option == "cone")
            {
                Console.Write("Would you like your cone to be dipped? (Y/N): ");
                string dippedStr = Console.ReadLine().ToUpper();
                if (dippedStr == "Y")
                {
                    dipped = true;
                }
            }

            Console.WriteLine("Enter new number of scoops: ");
            int scoops = Convert.ToInt32(Console.ReadLine());

            for (int i = scoops; i > 0; i--)
            {
                Console.WriteLine($"Enter new flavour | remaining: {i}: ");
                string flavourtype = Console.ReadLine();
                bool premium = premiumList.Contains(flavourtype);
                Console.WriteLine("Enter new flavour quantity: ");
                int flavourquantity = Convert.ToInt32(Console.ReadLine());
                Flavour flavour = new Flavour(flavourtype, premium, flavourquantity);
                flavours.Add(flavour);
            }
            Console.WriteLine("Enter new topping (or nil for no topping): ");

            string toppingType = Console.ReadLine();
            if (toppingType != "nil")
            {
                Topping topping = new Topping(toppingType);
                iceCream.Toppings.Add(topping);
                toppings.Add(topping);
                while (toppingType != "nil")
                {
                    Console.Write("Enter new topping (or nil to stop adding): ");
                    toppingType = Console.ReadLine();
                    if ( toppingType == "nil")
                    {
                        break;
                    }
                    topping = new Topping(toppingType);
                    toppings.Add(topping);
                }
            }

            if (option == "waffle")
            {
                IceCreamList[index] = new Waffle(option, scoops, flavours, toppings, waffleFlavour);
                Console.WriteLine("Added Successfully");
            }
            else if (option == "cone")
            {
                IceCreamList[index] = new Cone(option, scoops, flavours, toppings, dipped);
                Console.WriteLine("Added Successfully");
            }
            else
            {
                IceCreamList[index] = new Cup(option, scoops, flavours, toppings);
                Console.WriteLine("Added Successfully");

            }
            Console.WriteLine("Failed to add ice cream"); 
        }
        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);
            Console.WriteLine("Added successfully.");
        }
        public void DeleteIceCream(int index)
        {
            IceCreamList.RemoveAt(index);
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
