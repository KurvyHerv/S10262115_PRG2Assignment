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


            string option;
            while (true)
            {
                //show list of serving options
                Console.WriteLine("\n====Available serving options: ====");
                Console.WriteLine("1. Cup");
                Console.WriteLine("2. Waffle");
                Console.WriteLine("3. Cone");

                //prompt
                Console.Write("\nEnter new serving option: ");
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
                    //show list of available waffle flavours
                    Console.WriteLine("\n====Available waffle flavour: ====");
                    Console.WriteLine("1. Red velvet");
                    Console.WriteLine("2. Charcoal ");
                    Console.WriteLine("3. Pandan");
                    Console.WriteLine("4. Original");

                    //prompt
                    Console.Write("\nEnter new waffle flavour: ");
                    waffleFlavour = Console.ReadLine().ToLower();

                    if (waffleFlavour == "red velvet" || waffleFlavour == "charcoal" || waffleFlavour == "pandan" || waffleFlavour == "original")
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
                    Console.Write("\nWould your like your cone to be dipped? (Y/N): ");
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
                Console.Write("\nEnter number of ice cream scoops[1-3]: ");
                try
                {
                    scoops = Convert.ToInt32(Console.ReadLine());

                    if (scoops < 1 || scoops > 3)
                    {
                        throw new ArgumentException("Enter valid number of scoops [1-3]");
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            for (int i = scoops; i > 0;)
            {
                Console.WriteLine($"\n===Enter new icecream flavour | remaining: {i} ==== ");
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

                    Console.Write("\nEnter new ice cream flavour: ");
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


                int flavourQuantity;
                while (true)
                {
                    Console.Write("Enter flavour quantity: ");
                    try
                    {
                        flavourQuantity = Convert.ToInt32(Console.ReadLine());

                        if (flavourQuantity == 0 || flavourQuantity > i)
                        {
                            Console.WriteLine("Invalid input. Please enter valid input. ");
                        }
                        else
                        {
                            i -= flavourQuantity;
                            break;
                        }
                        break;
                    }
                    catch (FormatException)
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

            while (true)
            {
                string toppingType;
                Console.WriteLine("\n====Available toppings: ====");
                Console.WriteLine("1. Sprinkles");
                Console.WriteLine("2. Oreos");
                Console.WriteLine("3. Sago");
                Console.WriteLine("4. Mochi");

                Console.Write("\nEnter new toppings [nil for no toppings]: ");
                toppingType = Console.ReadLine().ToLower();

                if (toppingType == "nil")
                {
                    break;
                }
                else if (toppingType == "nil" || toppingType == "sprinkles" || toppingType == "oreos" || toppingType == "sago" || toppingType == "mochi")
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
                case "waffle":
                    IceCreamList[index] = new Waffle(option, scoops, flavours, toppings, waffleFlavour);
                    Console.WriteLine("Added new ice cream Successfully");

                    Console.WriteLine("\n====Ice cream summary: ====");
                    Console.WriteLine(IceCreamList[index]);
                    break;
                case "cone":
                    IceCreamList[index] = new Cone(option, scoops, flavours, toppings, dipped);
                    Console.WriteLine("Added new ice cream Successfully");

                    Console.WriteLine("\n====Ice cream summary: ====");
                    Console.WriteLine(IceCreamList[index]);
                    break;
                case "cup":
                    IceCreamList[index] = new Cup(option, scoops, flavours, toppings);
                    Console.WriteLine("Added new ice cream Successfully");

                    Console.WriteLine("\n====Ice cream summary: ====");
                    Console.WriteLine(IceCreamList[index]);
                    break;
            }
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
            return $"Order ID: {Id}" + 
                $"\nTime Received: {TimeReceived}" + 
                $"\nTime Fulfilled: {TimeFullfilled}" +
                $"\n====IceCream details: ==={iceCreams}"; 
        }
    }
}
