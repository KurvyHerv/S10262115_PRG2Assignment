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
            string[] premiumList = { "durian", "ube", "sea salt" };
            string[] normalList = { "vanilla", "chocolate", "strawberry" };

            string waffleFlavour = "";
            bool dipped = false;

            Order order = new Order();
            IceCream? iceCream = null;

            //prompt user for order details
            Console.WriteLine("====Enter order details====");
            
            //prompt serving option
            string option;
            while (true)
            {
                Console.Write("Enter serving option: ");
                option = Console.ReadLine().ToLower();

                if (option == "cup" || option == "waffle" || option == "cone")
                {
                    break;
                }
                else { Console.WriteLine("Invalid option. Please enter [waffle/cone/cup]. "); }
            }

            //prompt (waffle-flavour)/(cone-dipped)
            if (option == "waffle")
            {
                while (true)
                {
                    Console.Write("Enter waffle flavour: ");
                    waffleFlavour = Console.ReadLine().ToLower();

                    if (waffleFlavour == "red velvet" ||  waffleFlavour == "charcoal" || waffleFlavour == "pandan")
                    {
                        break;
                    }
                    else { Console.WriteLine("Invalid waffle flavour. Please enter [red velvet/charcoal/pandan]. "); }
                }
                
            }
            else if (option == "cone")
            {
                string dippedStf;
                while (true)
                {
                    Console.Write("Would your like your cone to be dipped? (Y/N): ");
                    dippedStf = Console.ReadLine().ToUpper();

                    if (dippedStf != "Y" && dippedStf != "N")
                    {
                        Console.WriteLine("Invalid input. Please enter [Y/N]. ");
                    }
                    else
                    {
                        break;
                    }
                }
                if (dippedStf == "Y")
                {
                    dipped = true;
                }
            }

            //prompt ice cream scoops
            int scoops;
            while (true)
            {
                Console.Write("Enter number of ice cream scoops: ");
                scoops = Convert.ToInt32(Console.ReadLine());   

                if (scoops > 0 && scoops < 4)
                {
                    break;
                }
                else { Console.WriteLine("Enter valid number of scoops [1-3]"); }
            }          

            for (int i = scoops; i > 0; i--)
            {
                Console.WriteLine($"===Enter icecream flavour | remaining: {i} ==== ");
                string flavourType;
                bool premium = false;
                while (true)
                {
                    Console.Write("Enter ice cream flavour: ");
                    flavourType = Console.ReadLine();
                    if (flavourType == "vanilla" || flavourType == "chocolate" || flavourType == "strawberry" || flavourType == "durian" || flavourType == "ube" || flavourType == "sea salt")
                    {
                        break;
                    }
                    else { Console.WriteLine("Enter valid ice cream flavour. "); }
                }
                if (premiumList.Contains(flavourType))
                    {
                        premium = true;
                    }

                
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
