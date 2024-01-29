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
    class Customer : Order
    {
        public string Name { get; set; }
        public int MemberId { get; set; }
        public DateTime Dob { get; set; }
        public Order CurrentOrder { get; set; }
        public List<Order> OrderHistory { get; set; } = new List<Order>();
        public PointCard Rewards { get; set; }
        public Customer() { }
        public Customer(string name, int memberId, DateTime dob)
        {
            Name = name;
            MemberId = memberId;
            Dob = dob;
        }
        public Order MakeOrder()
        {
            List<Flavour> flavours = new List<Flavour>();
            List<Topping> toppings = new List<Topping>();
            string[] premiumList = { "Durian", "Ube", "Sea Salt" };

            string waffleFlavour = "";
            bool dipped = false;

            Order order = new Order();
            IceCream? iceCream = null;

            Console.WriteLine("====Enter order details====");
            Console.Write("Enter serving option");
            string option = Console.ReadLine().ToLower();

            if (option == "waffle")
            {
                Console.WriteLine("Enter waffle flavour: ");
                waffleFlavour = Console.ReadLine();
            }
            else if (option == "cone")
            {
                Console.WriteLine("Would your like your cone to be dipped? (Y/N): ");
                string dippedStf = Console.ReadLine().ToUpper();
                if (dippedStf == "Y")
                {
                    dipped = true;
                }
            }

            Console.Write("Enter numbr of ice cream scoops: ");
            int scoops = Convert.ToInt32(Console.ReadLine());

            for (int i = scoops; i > 0; i--)
            {
                Console.Write($"===Enter icecream flavour | remaining: {i} ==== ");
                string flavourType = Console.ReadLine();
                bool premium = premiumList.Contains(flavourType);

                Console.Write("Enter flavour quantity: ");
                int flavourQuantity = Convert.ToInt32(Console.ReadLine());

                Flavour flavour = new Flavour(flavourType, premium, flavourQuantity);
                flavours.Add(flavour);
            }
            Console.WriteLine("Enter toppings [nil for no toppings]: ");

            string toppingType = Console.ReadLine();
            if (toppingType != "nil")
            {
                Topping topping = new Topping(toppingType);
                iceCream.Toppings.Add(topping);
                toppings.Add(topping);
                while (toppingType == "nil")
                {
                    Console.Write("Enter topping (or nil to stop adding): ");
                    toppingType = Console.ReadLine();
                    if (toppingType == "nil")
                    {
                        break;
                    }
                    topping = new Topping(toppingType);
                    toppings.Add(topping);
                }
            }

            switch (option)
            {
                case "cup":
                    iceCream = new Cup(option, scoops, flavours, toppings);
                    break;
                case "cone":
                    iceCream = new Cone(option, scoops, flavours, toppings, dipped);
                    break;
                case "waffle":
                    iceCream = new Waffle(option, scoops, flavours, toppings, waffleFlavour);
                    break;
            }

            int id = 0;

            CurrentOrder = new Order(id, DateTime.Now);
            CurrentOrder.IceCreamList.Add(iceCream);
            return CurrentOrder;

        }
        public bool IsBirthday()
        {
            return DateTime.Today.Month == Dob.Month && DateTime.Today.Day == Dob.Day;
        }
        public override string ToString()
        {
            return base.ToString() + "\tName: " + Name + "\tMember ID: " + MemberId  + "\tDate of Birth: " + Dob; 
        }
    }
}
