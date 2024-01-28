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
        public Order MakeOrder(Dictionary<int, Customer>customersDict, Dictionary<int, List<Order>>orderDict)
        {
            List<string> fList = new List<string> { "Vanilla", "Chocolate", "Strawberry" };
            List<string> fPremiumList = new List<string> { "Durian", "Ube", "Sea Salt" };

            //prompt user for customer 
            int id;
            Console.Write("Enter customer memberID: ");
            while (true)
            {
                try
                {
                    id = Convert.ToInt32(Console.ReadLine());

                    if (!customersDict.ContainsKey(id))
                    {
                        throw new ArgumentException("Member ID does not exist. Enter valid ID. ");
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Enter valid ID.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            //retrieve customer id
            Customer customer = customersDict[id];

            //create new order
            Order order = new Order();

            //prompt user
            string option;
            //option
            while (true)
            {
                Console.Write("Enter serving option [waffle/cup/cone] : ");
                option = Console.ReadLine().ToLower();

                if (option == "cup" || option == "waffle" || option == "cone")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please enter [waffle/cup/cone]. ");
                }
            }
            //scoops
            int scoops;
            Console.Write("Enter number of scoops: ");
            while (true)
            {
                try
                {
                    scoops = Convert.ToInt32(Console.ReadLine());

                    if (scoops < 1 || scoops > 3)
                    {
                        throw new ArgumentException("Enter valid number of scoops. [1 - 3 scoops]. ");
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter valid input. ");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Invalid input");
                }
            }
            //flavour
            List<Flavour> flavours = new List<Flavour>();

            // Print flavour list
            Console.WriteLine("\nFlavour list:");
            for (int i = 0; i < fList.Count; i++)
            {
                Console.WriteLine(fList[i]);
            }

            // Print premium flavour list
            Console.WriteLine("\nPremium flavour list:");
            for (int i = 0; i < fPremiumList.Count; i++)
            {
                Console.WriteLine(fPremiumList[i]);
            }
            //prompt for ice cream flavours
            for (int i = 1; i <= scoops; i++)
            {
                Console.Write("\nEnter ice cream flavour: ");
                string fType = Console.ReadLine();

                bool fPremium;
                int fQuantity;

                Console.Write("Is it premium?[True/ False]: ");
                while (true)
                {
                    try
                    {
                        fPremium = Convert.ToBoolean(Console.ReadLine());
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid input. Please try again. ");
                    }
                }
                while (true)
                {
                    try
                    {
                        Console.Write("Enter Quantity: ");
                        fQuantity = Convert.ToInt32(Console.ReadLine());

                        if (fQuantity < 1)
                        {
                            throw new ArgumentException("Invalid input. Please enter valid input. ");
                        }
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid input. Please try again. ");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                Flavour flavour = new Flavour(fType, fPremium, fQuantity);
                flavours.Add(flavour);
            }

            List<Topping> toppings = new List<Topping>();
            while (true)
            {
                Console.Write("Enter toppings [sprinkles/mochi/sago/oreos] [nil to stop]: ");
                string tType = Console.ReadLine().ToLower();

                if (tType == "sprinkles" || tType == "mochi" || tType == "oreos" || tType == "sago")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Enter valid toppings.");
                }

                while (tType != "nil")
                {
                    Topping topping = new Topping(tType);
                    toppings.Add(topping);

                    Console.Write("Enter toppings [nil to stop]: ");
                    tType = Console.ReadLine();
                }
            }


            IceCream iceCream = null;
            switch (option)
            {
                case "Cup":
                    iceCream = new Cup(option, scoops, flavours, toppings);
                    break;
                case "Cone":
                    Console.Write("Is ice cream dipped: ");
                    bool dipped = Convert.ToBoolean(Console.ReadLine());
                    iceCream = new Cone(option, scoops, flavours, toppings, dipped);
                    break;
                case "Waffle":
                    Console.Write("Enter the waffle flavour: ");
                    string wFlavour = Console.ReadLine();
                    iceCream = new Waffle(option, scoops, flavours, toppings, wFlavour);
                    break;
            }
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
