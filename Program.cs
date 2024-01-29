﻿//==========================================================
// Student Number : S10258053
// Student Name : Soong Chu Wen Rena
// Partner Number : S10262115
// Partner Name : Hervin Darmawan Sie
//==========================================================

using Microsoft.VisualBasic.FileIO;
using S10262115_PRG2Assignment;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

//orders
Dictionary<int, int> orderIdCustomerId = new Dictionary<int, int>();

//Populate Order
string[] orders = File.ReadAllLines("orders.csv");
List<Order> orderList = new List<Order>();
Dictionary<Order, int> orderDict = new Dictionary<Order, int>();
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
    orderDict.Add(orderList[k], customerId);
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
    Console.WriteLine("==========Menu==========");
    Console.WriteLine("[1] Display the information of all customers.");
    Console.WriteLine("[2] Display all current orders.");
    Console.WriteLine("[3] Register a new customer.");
    Console.WriteLine("[4] Create a customer's order.");
    Console.WriteLine("[5] Display order details of a customer.");
    Console.WriteLine("[6] Modify order details.");
    Console.WriteLine("[7] Process current order in queue");
    Console.WriteLine("[0] Exit.");
    Console.Write("Enter your option: ");
}

//1 - List all customers (Rena))
void ListAllCustomers(Dictionary<int, Customer> customersDict)
{
    Console.WriteLine("{0, -10} {1, -15} {2, -15} {3, -20} {4, -20} {5, -15}",
        "Name", "MemberID", "DOB", "Membership status", "Membership points", "PunchCard");

    foreach (Customer customer in customersDict.Values)
    {
        Console.WriteLine("{0, -10} {1, -15} {2, -15:dd/MM/yyyy} {3, -20} {4, -20} {5, -15}",
            customer.Name, customer.MemberId, customer.Dob, customer.Rewards.Tier, customer.Rewards.Points, customer.Rewards.PunchCard);
    }
    Console.WriteLine("\n");
}

//2 - List all Current Orders (Hervin)
void ListAllOrders()
{
    foreach (int customerId in customersDict.Keys)
    {
        Console.WriteLine(customersDict[customerId].CurrentOrder);
    }
}

//3 - Register a new Customer (Rena)
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

        if (!string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter))
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

            if (customerid.Length != 6)
            {
                throw new ArgumentException("Customer ID must have 6 digits. ");
            }
            customerId = Convert.ToInt32(customerid);
            if (customersDict.ContainsKey(customerId))
            {
                throw new ArgumentException("Customer ID already exist. Enter valid customer ID.");
            }
            break;
        }
        catch (FormatException)
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
            dob = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
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

    Console.WriteLine("New Customer added. The following information is added: ");
    Console.WriteLine($"" +
            $"{newCustomer.Name}," +
            $"{newCustomer.MemberId}," +
            $"{newCustomer.Dob.ToString("dd/MM/yyyy")}," +
            $"{newCustomer.Rewards.Tier}," +
            $"{newCustomer.Rewards.Points}," +
            $"{newCustomer.Rewards.PunchCard}");
    Console.WriteLine("\n");
}

//4 - Create a Customer's Order (Rena)
void CreateOrder(Dictionary<int, Customer> customersDict, Dictionary<int, List<Order>> orderDict)
{
    //this is for the repeat
    List<Flavour> flavours = new List<Flavour>();
    List<Topping> toppings = new List<Topping>();
    string[] premiumList = { "Durian", "Ube", "Sea Salt" };

    string waffleFlavour = "";
    bool dipped = false;


    ListAllCustomers(customersDict);

    int customerID;
    while (true)
    {
        Console.Write("Enter customer ID: ");
        try
        {
            customerID = Convert.ToInt32(Console.ReadLine());

            if (!customersDict.ContainsKey(customerID))
            {
                throw new ArgumentException("Customer ID does not exist. Enter valid ID.");
            }
            break;
        }
        catch(FormatException)
        {
            Console.WriteLine("Invalid input. Enter valid ID.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    Customer customer = customersDict[customerID];

    int orderId = 0;
    Order order = customer.MakeOrder();
    
    foreach (Order order1 in orderList)
    {
        if (order1.Id > orderId)
        {
            orderId = order1.Id;
        }
    }

    order.Id = orderId + 1;

    string add;
    do
    {
        Console.Write("Do you want to add another ice cream[Y/N]: ");
        add = Console.ReadLine().ToUpper();
    }
    while (add != "Y" &&  add != "N");

    while (add == "Y")
    {
        List<Flavour> flavours1 = new List<Flavour>();
        List<Topping> toppings1 = new List<Topping>();
        string waffleFlavour1 = "";
        bool dipped1 = false;
        Console.Write("Enter serving option: ");
        string type = Console.ReadLine().ToLower();
        if (type == "waffle")
        {
            Console.Write("Enter waffle flavour: ");
            waffleFlavour1 = Console.ReadLine();
        }
        else if (type == "cone")
        {
            Console.Write("Would you like your cone to be dipped? (Y/N): ");
            string dippedStr = Console.ReadLine().ToUpper();
            if (dippedStr == "Y")
            {
                dipped1 = true;
            }
            else { dipped1 = false; }
        }
        Console.Write("Enter number of scoops: ");
        int scoops = Convert.ToInt32(Console.ReadLine());
        for (int i = scoops; i > 0; i--)
        {
            Console.WriteLine($"Enter new flavour | remaining: {i}: ");
            string flavourtype = Console.ReadLine();
            bool premium = premiumList.Contains(flavourtype);
            Console.WriteLine("Enter new flavour quantity: ");
            int flavourquantity = Convert.ToInt32(Console.ReadLine());
            Flavour flavour = new Flavour(flavourtype, premium, flavourquantity);
            flavours1.Add(flavour);
        }

        Console.WriteLine("Enter new topping (or nil for no toppings): ");
        string toppingType = Console.ReadLine();
        Topping topping = new Topping(toppingType);
        toppings1.Add(topping);

        while (toppingType != "nil")
        {
            Console.WriteLine("Enter new topping (or nil to stop adding): ");
            toppingType = Console.ReadLine();
            topping = new Topping(toppingType);
            toppings1.Add(topping);
        }
        if (type == "waffle")
        {
            customersDict[customerID].CurrentOrder.AddIceCream(new Waffle(type, scoops, flavours, toppings1, waffleFlavour1));
        }
        else if (type == "cone")
        {
            customersDict[customerID].CurrentOrder.AddIceCream(new Cone(type, scoops, flavours, toppings1, dipped1));
        }
        else
        {
            customersDict[customerID].CurrentOrder.AddIceCream(new Cup(type, scoops, flavours, toppings));
        }
        Console.Write("Do you want to add another ice cream[Y/N]: ");
        add = Console.ReadLine().ToUpper();
    }
    

    if (customer.Rewards.Tier == "Gold")
    {
        goldQueue.Enqueue(order);
        Console.WriteLine("Order added to gold queue. ");
    }
    else
    {
        orderQueue.Enqueue(order);
        Console.WriteLine("Order added to queue. ");
    }
    Console.WriteLine("Order added successfully. ");
    Console.WriteLine("Order summary: ");
    Console.WriteLine(order.ToString());
}

//5 - Display order details of a customer (Hervin)
void CustomerOrder()
{
    ListAllCustomers(customersDict);
    try
    {
        Console.Write("Input customer ID: ");
        int customerID = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("current order: ");
        Console.WriteLine(customersDict[customerID].CurrentOrder);

        Console.WriteLine("past orders: ");
        foreach (Order order in customersDict[customerID].OrderHistory)
        {
            Console.WriteLine(order);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine (e.Message);
    }
}

//6 - Modify order details (Hervin)
void modifyOrder()
{
    ListAllCustomers(customersDict);

    Console.Write("Input customer ID: ");
    int customerID = Convert.ToInt32(Console.ReadLine());
    customersDict[customerID].CurrentOrder = new Order(9999999, orderList[0].TimeReceived);
    customersDict[customerID].CurrentOrder.AddIceCream(orderList[0].IceCreamList[0]);
    customersDict[customerID].CurrentOrder.AddIceCream(orderList[1].IceCreamList[0]);
    Console.WriteLine("current order: ");
    for (int i = 0; i < customersDict[customerID].CurrentOrder.IceCreamList.Count; i++)
    {
        Console.WriteLine($"{i+1}. {customersDict[customerID].CurrentOrder.IceCreamList[i]}");
    }
    Console.WriteLine("[1] Choose an existing ice cream to modify");
    Console.WriteLine("[2] Add a new ice cream to the order");
    Console.WriteLine("[3] Choose an existing ice cream to delete from the order");
    Console.Write("Enter your option: ");
    string option = Console.ReadLine();

    switch (option)
    {
        case "1":
            Console.Write("Which ice cream: ");
            int iceCream = Convert.ToInt32(Console.ReadLine());
            customersDict[customerID].CurrentOrder.ModifyIceCream(iceCream - 1);
            break;
        case "2":
            List<Flavour> flavours = new List<Flavour>();
            List<Topping> toppings = new List<Topping>();
            string waffleFlavour = "";
            bool dipped = false;
            Console.Write("Enter ice cream type: ");
            string type = Console.ReadLine().ToLower();
            if (type == "waffle")
            {
                Console.Write("Enter waffle flavour: ");
                waffleFlavour = Console.ReadLine();
            }
            else if (type == "cone")
            {
                Console.Write("Would you like your cone to be dipped? (Y/N): ");
                string dippedStr = Console.ReadLine().ToUpper();
                if (dippedStr == "Y")
                {
                    dipped = true;
                }
                else { dipped = false; }
            }
            Console.Write("Enter number of scoops: ");
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

            Console.WriteLine("Enter new topping (or nil for no toppings): ");
            string toppingType = Console.ReadLine();
            Topping topping = new Topping(toppingType);
            toppings.Add(topping);

            while (toppingType != "nil")
            {
                Console.WriteLine("Enter new topping (or nil to stop adding): ");
                toppingType = Console.ReadLine();
                topping = new Topping(toppingType);
                toppings.Add(topping);
            }
            if (type == "waffle")
            {
                customersDict[customerID].CurrentOrder.AddIceCream(new Waffle(type, scoops, flavours, toppings, waffleFlavour));
            }
            else if (type == "cone")
            {
                customersDict[customerID].CurrentOrder.AddIceCream(new Cone(type, scoops, flavours, toppings, dipped));
            }
            else
            {
                customersDict[customerID].CurrentOrder.AddIceCream(new Cup(type, scoops, flavours, toppings));
            }
            break;
        case "3":
            if (customersDict[customerID].CurrentOrder.IceCreamList.Count >= 1)
            {
                Console.Write("Which ice cream: ");
                customersDict[customerID].CurrentOrder.DeleteIceCream(Convert.ToInt32(Console.ReadLine()));
            }
            else
            {
                Console.WriteLine("You cannot have zero ice creams in an order");
            }
            break;
    }

}

// Process order and checkout
void processOrder()
{
    if (goldQueue.Count !> 0)
    {
        Order currentOrder = orderQueue.Dequeue();
        if (currentOrder != null)
        {
            for (int i = 0; i < currentOrder.IceCreamList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {currentOrder.IceCreamList[i]}");
            }
            Console.WriteLine($"Total price: {currentOrder.CalculateTotal()}");
            Console.WriteLine($"Membership status: {customersDict[orderDict[currentOrder]].Rewards.Tier}");
        }
    }
    else
    {
        Console.WriteLine("Gold queue exist");
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
            Console.WriteLine("========== [1] Display the information of all customers. ==========");
            ListAllCustomers(customersDict);
            break;
        case "2":
            Console.WriteLine("========== [2] Display all current orders. ==========");
            ListAllOrders(); 
            break;
        case "3":
            Console.WriteLine("========== [3] Register a new customer. ==========");
            RegisterNewCustomer(customersDict);
            break;
        case "4":
            Console.WriteLine("========== [4] Create a customer's order. ==========");
            //CreateOrder(customersDict, orderDict);
            break;
        case "5":
            Console.WriteLine("========== [5] Display order details of a customer. ==========");
            CustomerOrder();
            break;
        case "6":
            modifyOrder();
            break;
        case "7":
            processOrder();
            break;
        case "0":
            Console.WriteLine("Bye!");
            toggle = false;
            break;
    }

}
