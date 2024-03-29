﻿//==========================================================
// Student Number : S10258053
// Student Name : Soong Chu Wen Rena
// Partner Number : S10262115
// Partner Name : Hervin Darmawan Sie
//==========================================================

using Microsoft.VisualBasic.FileIO;
using S10262115_PRG2Assignment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Transactions;

bool toggle = true;
//Create Customer Dictionary
Dictionary<int, Customer> customersDict = new Dictionary<int, Customer>();     

//Append data from customer.csv to customersDict
string[] customers = File.ReadAllLines("customers.csv");
for (int i = 1; i < customers.Length; i++)
{
    string[] line = customers[i].Split(",");
    PointCard pointCard = new PointCard(Convert.ToInt32(line[4]), Convert.ToInt32(line[5]));
    pointCard.Tier = line[3];

    Customer customer = new Customer(line[0], Convert.ToInt32(line[1]), DateTime.Parse(line[2]));
    customer.Rewards = pointCard;
    customersDict.Add(customer.MemberId, customer); 
}

//Populate Order
string[] orders = File.ReadAllLines("orders.csv");

List<Order> orderList = new List<Order>();
Dictionary<int, List<Order>> orderDict = new Dictionary<int, List<Order>>();
Dictionary<int, int> currentOrderDict = new Dictionary<int, int>();
List<string> ids = new List<string>();
string[] premiumList = { "Durian", "Ube", "Sea Salt" };
int k = 0;

for (int i = 1; i < orders.Length; i++)
{
    List<Flavour> flavourList = new List<Flavour>();
    List<Topping> toppingList = new List<Topping>();
    bool dipped = false;
    string[] line = orders[i].Split(",");
    

    if (ids.Contains(line[0]))
    {
    }
    else
    {
        //if order id is not in id list, create new order
        orderList.Add(new(Convert.ToInt32(line[0]), Convert.ToDateTime(line[2]))); 
    }

    //populate flavourlist
    for (int j = 8; j <= 10; j++)
    {
        if (line[j] != "")
        {
            bool premium = premiumList.Contains(line[j]);
            flavourList.Add(new(line[j], premium, line.Count(str => str.Contains(line[j]))));
        }
    }
    //populate topping list
    for (int j = 11; j <= 14; j++)
    {
        if (line[j] != "")
        {
            toppingList.Add(new(line[j]));
        }
    }
    //find if waffle is dipped
    if (line[6] == "TRUE")
    {
        dipped = true;
    }

    // if order id is in id list add to order
    if (ids.Contains(line[0]))
    {
        for (int j = 0; j < customersDict[Convert.ToInt32(line[1])].OrderHistory.Count; j++)
        {
            if (customersDict[Convert.ToInt32(line[1])].OrderHistory[j].Id == Convert.ToInt32(line[0]))
            {
                if (line[4] == "Cup")
                {
                    customersDict[Convert.ToInt32(line[1])].OrderHistory[j].IceCreamList.Add(new Cup(line[4], Convert.ToInt32(line[5]), flavourList, toppingList));
                }
                if (line[4] == "Cone")
                {
                    customersDict[Convert.ToInt32(line[1])].OrderHistory[j].IceCreamList.Add(new Cone(line[4], Convert.ToInt32(line[5]), flavourList, toppingList, dipped));
                }
                if (line[4] == "Waffle")
                {
                    customersDict[Convert.ToInt32(line[1])].OrderHistory[j].IceCreamList.Add(new Waffle(line[4], Convert.ToInt32(line[5]), flavourList, toppingList, line[7]));
                }
            }
        }
        continue;
    }
    else
    {
        // if order id is not in id list add to the new order obj
        if (line[4] == "Cup")
        {
            orderList[k].TimeFullfilled = Convert.ToDateTime(line[3]);
            orderList[k].IceCreamList.Add(new Cup(line[4], Convert.ToInt32(line[5]), flavourList, toppingList));
            ids.Add(line[0]);
        }
        if (line[4] == "Cone")
        {
            orderList[k].TimeFullfilled = Convert.ToDateTime(line[3]);
            orderList[k].IceCreamList.Add(new Cone(line[4], Convert.ToInt32(line[5]), flavourList, toppingList, dipped));
            ids.Add(line[0]);
        }
        if (line[4] == "Waffle")
        {
            orderList[k].TimeFullfilled = Convert.ToDateTime(line[3]);
            orderList[k].IceCreamList.Add(new Waffle(line[4], Convert.ToInt32(line[5]), flavourList, toppingList, line[7]));
            ids.Add(line[0]);
        }
    }
    int customerId = Convert.ToInt32(line[1]);
    //add order to dict
    customersDict[customerId].OrderHistory.Add(orderList[k]);
    k++;
}

//Queue
Queue<Order> orderQueue = new Queue<Order>();
Queue<Order> goldQueue = new Queue<Order>();



//====Basic Features====
//Menu
void Menu()
{
    Console.WriteLine("\n==========Menu==========");
    Console.WriteLine("[1] Display the information of all customers.");
    Console.WriteLine("[2] Display all current orders.");
    Console.WriteLine("[3] Register a new customer.");
    Console.WriteLine("[4] Create a customer's order.");
    Console.WriteLine("[5] Display order details of a customer.");
    Console.WriteLine("[6] Modify order details.");
    Console.WriteLine("[7] Process current order in queue");
    Console.WriteLine("[8] Monthly charged amounts breakdown");
    Console.WriteLine("[0] Exit.");
    Console.Write("Enter your option: ");
}

//1 - List all customers 
void ListAllCustomers(Dictionary<int, Customer> customersDict)
{
    Console.WriteLine("\n{0, -10} {1, -15} {2, -15} {3, -20} {4, -20} {5, -15}",
        "Name", "MemberID", "DOB", "Membership status", "Membership points", "PunchCard");

    foreach (Customer customer in customersDict.Values)
    {
        Console.WriteLine("{0, -10} {1, -15} {2, -15:dd/MM/yyyy} {3, -20} {4, -20} {5, -15}",
            customer.Name, customer.MemberId, customer.Dob, customer.Rewards.Tier, customer.Rewards.Points, customer.Rewards.PunchCard);
    }
}

//2 - List all Current Orders 
void ListAllOrders(Dictionary<int, Customer> customersDict)
{
    bool toggle = false;
    foreach (int customerId in customersDict.Keys)
    {
        if (customersDict[customerId].CurrentOrder != null)
        {
            Console.WriteLine(customersDict[customerId].CurrentOrder);
            toggle = true;
        }
    }
    if (toggle == false) { Console.WriteLine("No current orders"); }
}

//3 - Register a new Customer 
void RegisterNewCustomer(Dictionary<int, Customer> customersDict)
{
    //default points & punchcard
    int defaultPoints = 0;
    int defaultPunchCard = 0;
    string defaultTier = "Ordinary";

    //Prompt for customer name
    string name;
    
    while (true)
    {
        Console.Write("Enter the customer's name: ");
        name = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter)) //check if customer name is blank/integar
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid name. "); 
        }
    }

    //Prompt for customer id
    int customerId;
    while (true)
    {
        try
        {
            Console.Write("Enter the customer's id number: ");
            string customerid = Console.ReadLine();

            if (customerid.Length != 6) //check if customerID has 6 digits. If not invalid input
            {
                throw new ArgumentException("Customer ID must have 6 digits. ");
            }

            customerId = Convert.ToInt32(customerid);
            if (customersDict.ContainsKey(customerId)) //check if customerID exists already. If not invalid input
            {
                throw new ArgumentException("Customer ID already exist. Enter valid customer ID.");
            }
            break;
        }
        catch (FormatException) //checks if customerID has alphebets. if not invalid input
        {
            Console.WriteLine("Invalid customer ID. Please enter valid ID. ");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    //Prompt for customer dob
    DateTime dob;
    while (true)
    {
        try
        {
            Console.Write("Enter the customer's date of birth (dd/mm/yyyy): ");
            dob = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture); //checks if datetime is valid format
            break;
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input. Please enter valid data in format dd/MM/yyyy");
        }
    }
    
    //Create Customer Object
    Customer newCustomer = new Customer(name, customerId, dob);

    //Create Pointcard Object
    PointCard defaultPointCard = new PointCard(defaultPoints, defaultPunchCard);

    //Assign PointCard obj to customer
    newCustomer.Rewards = defaultPointCard;
    newCustomer.Rewards.Tier = defaultTier;
    customersDict.Add(newCustomer.Id, newCustomer);

    //append customer information to csv file
    string newCustomerInfo = $"" +
            $"{newCustomer.Name}," +
            $"{newCustomer.MemberId}," +
            $"{newCustomer.Dob.ToString("dd/MM/yyyy")}," +
            $"{newCustomer.Rewards.Tier}," +
            $"{newCustomer.Rewards.Points}," +
            $"{newCustomer.Rewards.PunchCard}";
    File.AppendAllText("customers.csv", newCustomerInfo);

    //display new customer
    Console.WriteLine("New Customer added. The following information is added: ");
    Console.WriteLine($"" +
            $"{newCustomer.Name}," +
            $"{newCustomer.MemberId}," +
            $"{newCustomer.Dob.ToString("dd/MM/yyyy")}," +
            $"{newCustomer.Rewards.Tier}," +
            $"{newCustomer.Rewards.Points}," +
            $"{newCustomer.Rewards.PunchCard}");
}

//4 - Create a Customer's Order 
void CreateOrder(Dictionary<int, Customer> customersDict, Dictionary<int, List<Order>> orderDict)
{
    List<Flavour> flavours = new List<Flavour>();
    List<Topping> toppings = new List<Topping>();
    string[] premiumList = { "Durian", "Ube", "Sea Salt" };

    string waffleFlavour = "";
    bool dipped = false;


    ListAllCustomers(customersDict);

    //prompt user for customer id
    int customerID;
    while (true)
    {
        Console.Write("Enter customer ID: ");
        try
        {
            customerID = Convert.ToInt32(Console.ReadLine());

            if (!customersDict.ContainsKey(customerID)) //check if customer exists
            {
                throw new ArgumentException("Customer ID does not exist. Enter valid ID.");
            }
            break;
        }
        catch(FormatException) //check if customerID is int
        {
            Console.WriteLine("Invalid input. Enter valid ID.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    //retrieve customer
    Customer customer = customersDict[customerID];

    //prompt for new order details
    int orderId = 0;
    Order order = customer.MakeOrder();
    
    foreach (Order order1 in orderList)
    {
        if (order1.Id > orderId)
        {
            orderId = order1.Id; //create new order ID
        }
    }
    foreach (int customerId in customersDict.Keys)
    {
        if (customersDict[customerId].CurrentOrder != null) //checks if customerID exists in current order
        {
            int id = customersDict[customerId].CurrentOrder.Id;
            if (id > orderId) 
            {
                orderId = id; //links new order with current order
            }
        }
    }

    //create order id
    order.Id = orderId + 1;

    //Add order to current order dict
    currentOrderDict.Add(orderId + 1, customerID);

    //prompt user if customer wants another ice cream
    string add;
    while (true)
    {
        Console.Write("Do you want to add another ice cream[Y/N]: ");
        add = Console.ReadLine().ToUpper();

        if (add != "Y" && add != "N") //checks if valid input Y/N
        {
            Console.WriteLine("Invalid input. Please enter Y/N.");
        }
        else if (add == "Y") //if customer wants another ice cream, prompt for order details
        {
            List<Flavour> flavours1 = new List<Flavour>();
            List<Topping> toppings1 = new List<Topping>();

            string waffleFlavour1 = "";
            bool dipped1 = false;

            string option;
            while (true)
            {
                //show list of serving options
                Console.WriteLine("\n====Available serving options: ====");
                Console.WriteLine("1. Cup");
                Console.WriteLine("2. Waffle");
                Console.WriteLine("3. Cone");

                //prompt
                Console.Write("\nEnter serving option: ");
                option = Console.ReadLine().ToLower();

                if (option == "cup" || option == "waffle" || option == "cone") //check if option is valid
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
                    Console.Write("\nEnter waffle flavour: ");
                    waffleFlavour = Console.ReadLine().ToLower();

                    if (waffleFlavour == "red velvet" || waffleFlavour == "charcoal" || waffleFlavour == "pandan" || waffleFlavour == "original")
                    {
                        break; //check if waffle flavour is valid
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

                    if (dippedStf != "Y" && dippedStf != "N") //check if input is valid Y/N
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
                    dipped = true; //bool dipped = true when cone is to be dipped
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

                    if (scoops < 1 || scoops > 3) //checks if scoops is between 1 and 3
                    {
                        throw new ArgumentException("Enter valid number of scoops [1-3]");
                    }
                    break;
                }
                catch (FormatException) //check if input is int
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            for (int i = scoops; i > 0;) //when quota of ice cream scoops has not been met, for loops
            {
                Console.WriteLine($"\n===Enter icecream flavour | remaining: {i} ==== ");
                string flavourType;
                bool premium = false;
                while (true)
                {
                    //list available flavours
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
                    flavourType = Console.ReadLine();
                    if (flavourType == "vanilla" || flavourType == "chocolate" || flavourType == "strawberry" || flavourType == "durian" || flavourType == "ube" || flavourType == "sea salt")
                    {
                        break; //check if flavour type is valid
                    }
                    else { Console.WriteLine("Invalid input. Please enter valid ice cream flavour. "); }
                }
                if (premiumList.Contains(flavourType))
                {
                    premium = true; //checks if flavour type is premium or normal
                }

                //prompt for number of scoops for chosen flavour
                int flavourQuantity;
                while (true)
                {
                    Console.Write("Enter flavour quantity: ");
                    try
                    {
                        flavourQuantity = Convert.ToInt32(Console.ReadLine());

                        if (flavourQuantity == 0 || flavourQuantity > i) //checks is scoops for chosen is flavour between 1 and scoops chosen
                        {
                            Console.WriteLine("Invalid input. Please enter valid input. ");
                        }
                        else
                        {
                            i -= flavourQuantity; // scoops - flavourQuantity to check if quota of scoops has been met. If not loops again
                            break;
                        }
                    }
                    catch (FormatException) //checks if input is int
                    {
                        Console.WriteLine("Invalid input. Please try again. ");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                    }
                }
                Flavour flavour = new Flavour(flavourType, premium, flavourQuantity);
                flavours1.Add(flavour);
            }

            //prompt for toppings
            while (true)
            {

                //list toppings list
                string toppingType;
                Console.WriteLine("\n====Available toppings: ====");
                Console.WriteLine("1. Sprinkles");
                Console.WriteLine("2. Oreos");
                Console.WriteLine("3. Sago");
                Console.WriteLine("4. Mochi");

                Console.Write("\nEnter toppings [nil for no toppings]: ");
                toppingType = Console.ReadLine().ToLower();

                if (toppingType == "nil")
                {
                    break; //checks if user wants topping (if not no topping added)
                }
                else if (toppingType == "nil" || toppingType == "sprinkles" || toppingType == "oreos" || toppingType == "sago" || toppingType == "mochi") //check if input is valid
                {
                    Topping topping = new Topping(toppingType);
                    toppings1.Add(topping);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }

            switch (option)
            {
                case "waffle":
                    customersDict[customerID].CurrentOrder.AddIceCream(new Waffle(option, scoops, flavours, toppings1, waffleFlavour1));
                    break;
                case "cone":
                    customersDict[customerID].CurrentOrder.AddIceCream(new Cone(option, scoops, flavours, toppings1, dipped1));
                    break;
                case "cup":
                    customersDict[customerID].CurrentOrder.AddIceCream(new Cup(option, scoops, flavours, toppings));
                    break;
            }
        }
        else { break; }
    }

    if (customer.Rewards.Tier == "Gold") //if the customer has a gold-tier Pointcard, append their order to the back of the gold members order queue
    {
        goldQueue.Enqueue(order);
        Console.WriteLine("\n====Order added to gold queue.==== ");
    }
    else
    {
        orderQueue.Enqueue(order);
        Console.WriteLine("\n====Order added to normal queue.==== ");
    }
    
    orderList.Add(order);
    Console.WriteLine("\n====Order summary: ====");
    Console.WriteLine(order.ToString());
}

//5 - Display order details of a customer 
void CustomerOrder()
{
    ListAllCustomers(customersDict);

    //prompt user for customer id
    int customerID;
    while (true)
    {
        Console.Write("Enter customer ID: ");
        try
        {
            customerID = Convert.ToInt32(Console.ReadLine()); 

            if (!customersDict.ContainsKey(customerID)) //checks if customerid exists
            {
                throw new ArgumentException("Customer ID does not exist. Enter valid ID.");
            }
            break;
        }
        catch (FormatException) //checks if input is int
        {
            Console.WriteLine("Invalid input. Enter valid ID.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    Console.WriteLine("====Current order: ====");
    if (customersDict[customerID].CurrentOrder == null) //checks if there are any current order
    {
        Console.WriteLine("No current orders");
    }
    else
    {
        Console.WriteLine(customersDict[customerID].CurrentOrder); //displays order
    }

    Console.WriteLine("\n====Past orders: ====");
    if (customersDict[customerID].OrderHistory == null) //checks if there is any past orders
    {
        Console.WriteLine("No past order. ");
    }
    else
    {
        foreach (Order order in customersDict[customerID].OrderHistory) //displays order
        {
            Console.WriteLine($"\n{order}");
        }
    }
}

//6 - Modify order details
void modifyOrder()
{
    //list customers
    Console.WriteLine("\n====Customer List: ====");
    ListAllCustomers(customersDict);

    //prompt user for customer id
    int customerID;
    while (true)
    {
        Console.Write("Select customer ID to view current order: ");
        try
        {
            customerID = Convert.ToInt32(Console.ReadLine());

            if (!customersDict.ContainsKey(customerID)) //checks if customer id exists
            {
                throw new ArgumentException("Customer ID does not exist. Enter valid ID.");
            }
            break;
        }
        catch (FormatException) //checks if input is int
        {
            Console.WriteLine("Invalid input. Enter valid ID.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    customersDict[customerID].CurrentOrder = new Order(9999999, orderList[0].TimeReceived);
    customersDict[customerID].CurrentOrder.AddIceCream(orderList[0].IceCreamList[0]);
    customersDict[customerID].CurrentOrder.AddIceCream(orderList[1].IceCreamList[0]);

    Console.WriteLine("\nCurrent order: ");
    for (int i = 0; i < customersDict[customerID].CurrentOrder.IceCreamList.Count; i++)
    {
        Console.WriteLine($"{i+1}. {customersDict[customerID].CurrentOrder.IceCreamList[i]}");
    }

    //prompt the user to either [1] choose an existing ice cream object to modify, [2] add an entirely new ice cream object to the order, or[3] choose an existing ice cream object to delete from the order
    Console.WriteLine("\n====Select an option: ====");
    Console.WriteLine("[1] Choose an existing ice cream to modify");
    Console.WriteLine("[2] Add a new ice cream to the order");
    Console.WriteLine("[3] Choose an existing ice cream to delete from the order");
    Console.Write("Enter your option: ");
    string option = Console.ReadLine();

    switch (option)
    {
        case "1":
            int iceCream;
            while (true)
            {
                Console.Write("Which ice cream: ");
                try
                {
                    iceCream = Convert.ToInt32(Console.ReadLine());

                    if (iceCream > customersDict[customerID].CurrentOrder.IceCreamList.Count || iceCream == 0) //checks if ice cream exists in list
                    {
                        throw new ArgumentException("Ice cream does not exist. Enter valid input.");
                    }
                    break;
                }
                catch (FormatException) //checks if input is int
                {
                    Console.WriteLine("Invalid input. Enter valid ID.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            customersDict[customerID].CurrentOrder.ModifyIceCream(iceCream-1);
            break;

        case "2":
            List<Flavour> flavours = new List<Flavour>();
            List<Topping> toppings = new List<Topping>();
            string waffleFlavour = "";
            bool dipped = false;

            string type;
            while (true)
            {
                //show list of serving options
                Console.WriteLine("\n====Available serving options: ====");
                Console.WriteLine("1. Cup");
                Console.WriteLine("2. Waffle");
                Console.WriteLine("3. Cone");

                //prompt for serving options
                Console.Write("\nEnter new serving option: ");
                type = Console.ReadLine().ToLower();

                if (type == "cup" || type == "waffle" || type == "cone") //check if input is valid
                {
                    break;
                }
                else { Console.WriteLine("Invalid option. Please enter [waffle/cone/cup]. "); }
            }

            //prompt (waffle-flavour)/(cone-dipped)
            if (type == "waffle")
            {
                while (true)
                {
                    //show list of available waffle flavours
                    Console.WriteLine("\n====Available waffle flavour: ====");
                    Console.WriteLine("1. red velvet");
                    Console.WriteLine("2. charcoal ");
                    Console.WriteLine("3. pandan");
                    Console.WriteLine("4. original");

                    //prompt
                    Console.Write("\nEnter new waffle flavour: ");
                    waffleFlavour = Console.ReadLine().ToLower();

                    if (waffleFlavour == "red velvet" || waffleFlavour == "charcoal" || waffleFlavour == "pandan" || waffleFlavour == "original")
                    {
                        break; //checks if input is valid
                    }
                    else { Console.WriteLine("Invalid waffle flavour. Please enter [red velvet/charcoal/pandan/original]. "); }
                }

            }

            //prompt (cone-dipped)
            else if (type == "cone")
            {
                string dippedStf;
                while (true)
                {
                    Console.Write("\nWould your like your cone to be dipped? (Y/N): ");
                    dippedStf = Console.ReadLine().ToUpper();

                    if (dippedStf != "Y" && dippedStf != "N") //checks if input is valid
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

                    if (scoops < 1 || scoops > 3) //check if scoops is between 1-3
                    {
                        throw new ArgumentException("Enter valid number of scoops [1-3]");
                    }
                    break;
                }
                catch (FormatException) //check if input is int
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            for (int i = scoops; i > 0;) //when quota of ice cream scoops has not been met, for loops
            {
                Console.WriteLine($"\n===Enter icecream flavour | remaining: {i} ==== ");
                string flavourType;
                bool premium = false;
                while (true)
                {
                    //display available flavours
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
                        break; //checks if flavour type is valid
                    }
                    else { Console.WriteLine("Invalid flavour input. Enter valid ice cream flavour. "); }
                }
                if (premiumList.Contains(flavourType))
                {
                    premium = true; //checks if flavour is premium or normal
                }

                int flavourQuantity;
                while (true)
                {
                    //prompts for number of scoops for chosen flavour
                    Console.Write("Enter flavour quantity: "); 
                    try
                    {
                        flavourQuantity = Convert.ToInt32(Console.ReadLine());

                        if (flavourQuantity == 0 || flavourQuantity > i) //checks if input is between 1-i
                        {
                            Console.WriteLine("Invalid input. Please enter valid input. ");
                        }
                        else
                        {
                            i -= flavourQuantity; //if scoops quota is not met, loops 
                            break;
                        }
                    }
                    catch (FormatException) //check if input in int
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
                //display list of toppings
                string toppingType;
                Console.WriteLine("\n====Available toppings: ====");
                Console.WriteLine("1. Sprinkles");
                Console.WriteLine("2. Oreos");
                Console.WriteLine("3. Sago");
                Console.WriteLine("4. Mochi");

                Console.Write("\nEnter toppings [nil for no toppings]: ");
                toppingType = Console.ReadLine().ToLower();

                if (toppingType == "nil")
                {
                    break; //check if user wants toppings
                }
                else if (toppingType == "nil" || toppingType == "sprinkles" || toppingType == "oreos" || toppingType == "sago" || toppingType == "mochi")
                {
                    Topping topping = new Topping(toppingType); //checks if topping is valid
                    toppings.Add(topping);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }

            switch (type)
            {
                case "waffle":
                    customersDict[customerID].CurrentOrder.AddIceCream(new Waffle(type, scoops, flavours, toppings, waffleFlavour));
                    Console.WriteLine("Added new ice cream Successfully");

                    Console.WriteLine("\n====Order summary: ====");
                    for (int i = 0; i < customersDict[customerID].CurrentOrder.IceCreamList.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {customersDict[customerID].CurrentOrder.IceCreamList[i]}");
                    }
                    break;
                case "cone":
                    customersDict[customerID].CurrentOrder.AddIceCream(new Cone(type, scoops, flavours, toppings, dipped));
                    Console.WriteLine("Added new ice cream Successfully");

                    Console.WriteLine("\n====Order summary: ====");
                    for (int i = 0; i < customersDict[customerID].CurrentOrder.IceCreamList.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {customersDict[customerID].CurrentOrder.IceCreamList[i]}");
                    }
                    break;
                case "cup":
                    customersDict[customerID].CurrentOrder.AddIceCream(new Cup(option, scoops, flavours, toppings));
                    Console.WriteLine("Added new ice cream Successfully");

                    Console.WriteLine("\n====Order summary: ====");
                    for (int i = 0; i < customersDict[customerID].CurrentOrder.IceCreamList.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {customersDict[customerID].CurrentOrder.IceCreamList[i]}");
                    }
                    break;
            }
            break;

        case "3":
            if (customersDict[customerID].CurrentOrder.IceCreamList.Count >= 1)
            {
                //prompt for ice cream to delete
                int icecream;
                while (true)
                {
                    Console.Write("Which ice cream: ");
                    try
                    {
                        icecream = Convert.ToInt32(Console.ReadLine());

                        if (icecream > customersDict[customerID].CurrentOrder.IceCreamList.Count || icecream == 0) //checks if ice cream exists in list
                        {
                            throw new ArgumentException("Ice cream does not exist. Enter valid input.");
                        }
                        break;
                    }
                    catch (FormatException) //checks if input is int
                    {
                        Console.WriteLine("Invalid input. Enter valid ID.");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                customersDict[customerID].CurrentOrder.DeleteIceCream(icecream);

                Console.WriteLine("====Deleted ice cream====");
                Console.WriteLine($"{customersDict[customerID].CurrentOrder}");
            }
            else
            {
                Console.WriteLine("You cannot have zero ice creams in an order");
            }
            break;
    }
}

// Advanced A) Process order and checkout
void processOrder()
{
    Order currentOrder = null;

    if (goldQueue.Count != 0)
    {
        currentOrder = goldQueue.Dequeue(); //dequeue first order in gold queue
    }
    else if (orderQueue.Count != 0)
    {
        currentOrder = orderQueue.Dequeue(); //dequeue first order in normal queue
    }
    else
    {
        Console.WriteLine("No current orders"); //checks if there is any current order
    }

    //if there is available current order
    if (currentOrder != null)
    {
        Console.WriteLine("\n====Order details====");
        for (int i = 0; i < currentOrder.IceCreamList.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {currentOrder.IceCreamList[i]}");
        }
        Customer customer = customersDict[currentOrderDict[currentOrder.Id]]; //retrieve order of chosen customer
        double totalPrice = currentOrder.CalculateTotal(); //calculate total order of customer

        //display total
        Console.WriteLine("\n====Checkout====");
        Console.WriteLine($"Total price: ${totalPrice:0.00}");

        //display membership status and points
        Console.WriteLine("\n====Display membership status and points====");
        Console.WriteLine($"Membership status: {customer.Rewards.Tier}");
        Console.WriteLine($"Available Points: {customer.Rewards.Points}");

        //checks for customer birthday
        if (customer.IsBirthday())
        {
            double mostExpensive = 0;
            Console.WriteLine("\n====Birthday Special!====");
            Console.WriteLine("It's your birthday! Special discount available");

            for (int i = 0; i < currentOrder.IceCreamList.Count; i++)
            {
                if (currentOrder.IceCreamList[i].CalculatePrice() > mostExpensive) //if customer birthday, most expensive order = 0
                {
                    mostExpensive = currentOrder.IceCreamList[i].CalculatePrice();
                }
            }
            totalPrice -= mostExpensive; //recalculate total
            Console.WriteLine($"\nNew total: ${totalPrice:0.00}"); //display new total
        }

        if (customer.Rewards.PunchCard == 10) //check if customer has completed the punch card
        {
            totalPrice -= currentOrder.IceCreamList[0].CalculatePrice(); //first order is removed from total
            customer.Rewards.PunchCard = 0; //reset punch card

            Console.WriteLine("\n====Punch card completed! ====");
            Console.WriteLine("\nPunch Card Redeemed");
        }
        else //continues
        {
            if (customer.Rewards.Tier == "Silver" || customer.Rewards.Tier == "Gold") //checks if customer is silver or gold for more benefits
            {
                Console.WriteLine($"\n===={customer.Rewards.Tier} benefits====");

                //prompt user if customer wants to redeem points
                int points;
                while (true)
                {
                    Console.WriteLine($"Total Payable: {totalPrice}");
                    Console.Write($"You have {customer.Rewards.Points} points. How much would you like to redeem?: ");
                    try
                    {
                        points = Convert.ToInt32(Console.ReadLine());

                        if (points > customer.Rewards.Points) //checks if points requested to be redeemed exceeds points available
                        {
                            throw new ArgumentException("Input exceeded available points. Please enter valid input. ");
                        }
                        break;
                    }
                    catch(FormatException) //checks if input is int
                    {
                        Console.WriteLine("Invalid input. Please try again.");
                    }
                    catch(ArgumentException ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                    }
                }
                Console.WriteLine($"{points} redeemed");

                if ((totalPrice - points * 0.04) <= 0) //checks if points redeem covers total price
                {
                    points -= Convert.ToInt32(Math.Floor(totalPrice * 25)); //deducts points redeemed
                    totalPrice = 0;
     
                    Console.WriteLine($"{points} remaaining. "); //display points remaining
                }
                else if ((totalPrice - points * 0.04) > 0) //if points does not cover total price
                {
                    customer.Rewards.RedeemPoints(points); 
                    totalPrice -= points * 0.04; //total price recalculated
                }
            }
        }

        //display final bill
        Console.WriteLine("\n====Final bill====");
        Console.WriteLine($"Final bill amount: ${totalPrice:0.00}");
        Console.Write("Press any key to make payment");
        Console.ReadLine();

        //increment punch card for every ice cream in order
        customer.Rewards.PunchCard += currentOrder.IceCreamList.Count;
        Console.WriteLine("\n====Member rewards claim====");
        Console.WriteLine("10 points added to PunchCard");
        if (customer.Rewards.PunchCard > 10) //if goes above 10, goes back to 10
        {
            customer.Rewards.PunchCard = 10;
        }

        customer.Rewards.AddPoints(Convert.ToInt32(Math.Floor(totalPrice * 0.72))); //adds points
        if (customer.Rewards.Points >= 50 && customer.Rewards.Points < 100) //upgrade member points accordingly
        {
            customer.Rewards.Tier = "Silver";
            Console.WriteLine("Member upgraded to silver. "); //upgrade to silver
        }
        else if (customer.Rewards.Points >= 100)
        {
            customer.Rewards.Tier = "Gold";
            Console.WriteLine("Member upgraded to gold. ");//upgrade to gold
        }
        currentOrder.TimeFullfilled = DateTime.Now; //create time fullfilled
        customer.OrderHistory.Add(currentOrder); //add to order history
        customer.CurrentOrder = null;
    }
}

//Advanced B) Display monthly charged amounts breakdown & total charged amounts for the year
void displayMonthlyCharges()
{
    double total = 0;
    int inputYear;
    Dictionary<int, double> monthlyDict = new Dictionary<int, double>();

    // Check if orderList is null
    if (orderList == null || orderList.Count == 0)
    {
        Console.WriteLine("No orders available.");
        return;
    }


    while (true)
    {
        // prompt user for year
        Console.Write("Enter the year: ");

        try
        {
            // Validate user input
            if (!int.TryParse(Console.ReadLine(), out inputYear))
            {
                throw new FormatException(); //check input format
            }

            if (inputYear < 2023 || inputYear > 2024)
            {
                throw new ArgumentException("Invalid input. Enter valid year [2023-2024]");
            }
            else
            {
                foreach (Order order in orderList)
                {
                    if (order.TimeFullfilled.HasValue && order.TimeFullfilled.Value.Year == inputYear)
                    {
                        double totalPrice = order.IceCreamList.Sum(iceCream => iceCream.CalculatePrice()); //calculate total price in the month
                        total += totalPrice;

                        if (monthlyDict.ContainsKey(order.TimeFullfilled.Value.Month))
                        {
                            monthlyDict[order.TimeFullfilled.Value.Month] += totalPrice;
                        }
                        else
                        {
                            monthlyDict.Add(order.TimeFullfilled.Value.Month, totalPrice);
                        }
                    }
                }

                Console.WriteLine("");

                for (int month = 1; month <= 12; month++) //display every month
                {
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month); 
                    double monthlyTotal = monthlyDict.ContainsKey(month) ? monthlyDict[month] : 0.0;
                    Console.WriteLine($"{monthName} {inputYear}: {monthlyTotal,-15:C2}");
                }

                Console.WriteLine($"\nTotal: {total:C2}"); //display total
                break;
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input. Enter valid year [2023-2024]");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

//Initialise methods
while (toggle)
{
    Menu();
    string option = Console.ReadLine();
    switch (option)
    {
        case "1":
            Console.WriteLine("\n========== [1] Display the information of all customers. ==========");
            ListAllCustomers(customersDict);
            break;
        case "2":
            Console.WriteLine("\n========== [2] Display all current orders. ==========");
            ListAllOrders(customersDict); 
            break;
        case "3":
            Console.WriteLine("\n========== [3] Register a new customer. ==========");
            RegisterNewCustomer(customersDict);
            break;
        case "4":
            Console.WriteLine("\n========== [4] Create a customer's order. ==========");
            CreateOrder(customersDict, orderDict);
            break;
        case "5":
            Console.WriteLine("\n========== [5] Display order details of a customer. ==========");
            CustomerOrder();
            break;
        case "6":
            Console.WriteLine("\n========== [6] Modify order details. ==========");
            modifyOrder();
            break;
        case "7":
            Console.WriteLine("\n========== [7] Process an order an checkout. ==========");
            processOrder();
            break;
        case "8": 
            Console.WriteLine("\n========== [8] Display monthly charged amounts breakdown & total charged amounts for the year. ==========");
            displayMonthlyCharges();
            break;
        case "0":
            Console.WriteLine("Bye!");
            toggle = false;
            break;
        default:
            Console.WriteLine("Invalid input. Please try again.");
            break;
    }
}
