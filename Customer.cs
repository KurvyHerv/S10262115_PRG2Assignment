//==========================================================
// Student Number : S10258053
// Student Name : Soong Chu Wen Rena
// Partner Number : S10262115
// Partner Name : Hervin Darmawan Sie
//==========================================================

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
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
            Console.WriteLine("\n====Enter order details====");
            
            //prompt serving option
            string option;
            while (true)
            {
                //show list of serving options
                Console.WriteLine("\n====Available serving options: ====");
                Console.WriteLine("1. Cup");
                Console.WriteLine("2. Waffle");
                Console.WriteLine("3. Cone");

                //prompt for serving option
                Console.Write("\nEnter serving option: ");
                option = Console.ReadLine().ToLower();

                if (option == "cup" || option == "waffle" || option == "cone") //check for valid input
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
                    //show list of available waffle flavours
                    Console.WriteLine("\n====Available waffle flavour: ====");
                    Console.WriteLine("1. red velvet");
                    Console.WriteLine("2. charcoal ");
                    Console.WriteLine("3. pandan");
                    Console.WriteLine("4. orginal");

                    //prompt
                    Console.Write("\nEnter waffle flavour: ");
                    waffleFlavour = Console.ReadLine().ToLower();

                    if (waffleFlavour == "red velvet" ||  waffleFlavour == "charcoal" || waffleFlavour == "pandan" || waffleFlavour == "original")
                    {
                        break; //check for valid input
                    }
                    else { Console.WriteLine("Invalid waffle flavour. Please enter [red velvet/charcoal/pandan/original]. "); }
                }
                
            }
            else if (option == "cone")
            {
                string dippedStf;
                while (true)
                {
                    Console.Write("\nWould your like your cone to be dipped? (Y/N): ");
                    dippedStf = Console.ReadLine().ToUpper();

                    if (dippedStf != "Y" && dippedStf != "N") //check for valid input Y/N
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
                    dipped = true; //bool dipped is set to true
                }
            }

            //prompt ice cream scoops
            int scoops;
            while (true)
            {
                Console.Write("\nEnter number of ice cream scoops[1-3]: ");
                try
                {
                    scoops = Convert.ToInt32(Console.ReadLine());

                    if (scoops < 1 || scoops > 3) //checks is scoops is between 1 and 3
                    {
                        throw new ArgumentException("Enter valid number of scoops [1-3]");
                    }
                    break;
                }
                catch (FormatException) //check if input is int format
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }          

            //check if scoops quota is met
            for (int i = scoops; i > 0;)
            {
                Console.WriteLine($"\n===Enter icecream flavour | remaining: {i} ==== ");
                string flavourType;
                bool premium = false;
                while (true)
                {
                    Console.WriteLine("\n====Available flavours: ====");
                    Console.WriteLine("\nNormal flavours: ");
                    Console.WriteLine("1. Vanilla");
                    Console.WriteLine("2. Chocolate");
                    Console.WriteLine("3. Strawberry");
                    Console.WriteLine("\nPremium flavours");
                    Console.WriteLine("1. Durian");
                    Console.WriteLine("2. Ube");
                    Console.WriteLine("3. Sea Salt");

                    Console.Write("\nEnter ice cream flavour: ");
                    flavourType = Console.ReadLine().ToLower();
                    if (flavourType == "vanilla" || flavourType == "chocolate" || flavourType == "strawberry" || flavourType == "durian" || flavourType == "ube" || flavourType == "sea salt")
                    {
                        break; //check if input is valid
                    }
                    else { Console.WriteLine("Invalid input. Enter valid ice cream flavour. "); }
                }
                if (premiumList.Contains(flavourType)) //check if flavour is premium
                {
                        premium = true; 
                    }

                //prompt for flavour quantity
                int flavourQuantity;
                while (true)
                {
                    Console.Write("Enter flavour quantity: ");
                    try
                    {
                        flavourQuantity = Convert.ToInt32(Console.ReadLine());

                        if (flavourQuantity == 0 || flavourQuantity > i) //check if flavourquantity is between 1-scoops
                        {
                            Console.WriteLine("Invalid input. Please enter valid input. ");
                        }
                        else
                        {
                            i -= flavourQuantity; //loops if scoop quota not met
                            break;
                        }
                    }
                    catch (FormatException) //checks if input is int format
                    {
                        Console.WriteLine("Invalid input. Please try again. ");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                    }
                }                   
                Flavour flavour = new Flavour(flavourType, premium, flavourQuantity);
                flavours.Add(flavour);
            }

            //prompt for toppings
            while (true)
            {
                string toppingType;

                //display topping list
                Console.WriteLine("\n====Available toppings: ====");
                Console.WriteLine("1. Sprinkles");
                Console.WriteLine("2. Oreos");
                Console.WriteLine("3. Sago");
                Console.WriteLine("4. Mochi");

                Console.Write("\nEnter toppings [nil for no toppings]: ");
                toppingType = Console.ReadLine().ToLower();

                if (toppingType == "nil") //check if customer wants topping 
                {
                    break;
                }
                else if (toppingType == "nil" || toppingType == "sprinkles" || toppingType == "oreos" || toppingType == "sago" || toppingType == "mochi") //checks if topping is valid
                {
                    Topping topping = new Topping(toppingType);
                    toppings.Add(topping);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
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
