//==========================================================
// Student Number : S10258053
// Student Name : Soong Chu Wen Rena
// Partner Number : S10262115
// Partner Name : Hervin Darmawan Sie
//==========================================================

using Microsoft.VisualBasic.FileIO;
using S10262115_PRG2Assignment;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
Dictionary<int, List<Order>> orderDict = new Dictionary<int, List<Order>>();
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
    foreach (Order order in orderList)
    {
        Console.WriteLine(order.ToString());
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
    List<string> fList = new List<string> { "Vanilla", "Chocolate", "Strawberry" };
    List<string> fPremiumList = new List<string> { "Durian", "Ube", "Sea Salt" };

    //list customers from customers.csv file
    ListAllCustomers(customersDict);

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

    //show flavour list
    Console.WriteLine("\nFlavour list:");
    for (int i = 0; i < fList.Count; i++)
    {
        Console.WriteLine(fList[i]);
    }

    //show premium flavour list
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
        if (fType == "vanilla" || fType == "chocolate" || fType == "strawberry" || fType == "durian" || fType == "ube" || fType == "sea salt")
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid ice cream flavour. Please enter a valid topping.");
        }

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
                Console.WriteLine("Invalid input. Please enter True or False.");
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
                    throw new ArgumentException("Enter a quantity greater than 0.");
                }
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
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
    Console.Write("Enter toppings [sprinkles/mochi/oreos/sago/nil]: ");
    string tType = Console.ReadLine().ToLower();

    while (tType != "nil")
    {
        Topping topping;

        if (tType == "sprinkles" || tType == "mochi" || tType == "oreos" || tType == "sago")
        {
            topping = new Topping(tType);
            toppings.Add(topping);
        }
        else
        {
            Console.WriteLine("Invalid topping. Please enter a valid topping.");
        }

        Console.Write("Enter toppings [sprinkles/mochi/oreos/sago/nil]: ");
        tType = Console.ReadLine();
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

    //prompt user if they want to add another icecream
    string addIceCream;
    do
    {
        Console.Write("Do you want to add another ice cream? [Y/N] :");
        addIceCream = Console.ReadLine().ToUpper();
    }
    while (addIceCream != "Y" && addIceCream != "N");

    while (addIceCream == "Y")
    {

        //prompt user
        string addOption;
        //option
        while (true)
        {
            Console.Write("Enter serving option [waffle/cup/cone] : ");
            addOption = Console.ReadLine().ToLower();

            if (addOption == "cup" || addOption == "waffle" || addOption == "cone")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid option. Please enter [waffle/cup/cone]. ");
            }
        }
        //scoops
        int addScoops;
        Console.Write("Enter number of scoops: ");
        while (true)
        {
            try
            {
                addScoops = Convert.ToInt32(Console.ReadLine());

                if (addScoops < 1 || addScoops > 3)
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
        List<Flavour> flavourList = new List<Flavour>();

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
        for (int i = 1; i <= addScoops; i++)
        {
            Console.Write("Enter ice cream flavour: ");
            string addfType = Console.ReadLine();

            bool addfPremium;
            int addfQuantity;

            Console.Write("Is it premium?[True/ False]: ");
            addfPremium = Convert.ToBoolean(Console.ReadLine());
            Console.Write("Enter Quantity: ");
            addfQuantity = Convert.ToInt32(Console.ReadLine());

            Flavour flavour = new Flavour(addfType, addfPremium, addfQuantity);
            flavourList.Add(flavour);
        }

        List<Topping> toppingList = new List<Topping>();
        Console.Write("Enter toppings: ");
        string addtType = Console.ReadLine();
        while (tType != "nil")
        {
            Topping topping = new Topping(addtType);
            toppingList.Add(topping);

            Console.Write("Enter toppings [nil to stop]: ");
            addtType = Console.ReadLine();
        }

        IceCream addiceCream = null;
        switch (option)
        {
            case "Cup":
                addiceCream = new Cup(addOption, addScoops, flavourList, toppingList);
                break;
            case "Cone":
                Console.Write("Is ice cream dipped: ");
                bool adddipped = Convert.ToBoolean(Console.ReadLine());
                addiceCream = new Cone(addOption, addScoops, flavourList, toppingList, adddipped);
                break;
            case "Waffle":
                Console.Write("Enter the waffle flavour: ");
                string addwFlavour = Console.ReadLine();
                addiceCream = new Waffle(addOption, addScoops, flavourList, toppingList, addwFlavour);
                break;
        }
        break;
    }
    order.AddIceCream(iceCream);

    customer.CurrentOrder = order;
    orderDict.Add(orderDict.Count + 1, orderList);
    if (customer.Rewards.Tier == "Gold")
    {
        goldQueue.Enqueue(order);
        Console.WriteLine("Order added to gold queue.");
    }
    else
    {
        orderQueue.Enqueue(order);
        Console.WriteLine("Order added to normal queue.");
    }
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

////6 - Modify order details (Hervin)
//ListAllCustomers(customersDict);
//try
//{
//    Console.Write("Input customer ID: ");
//    int customerID = Convert.ToInt32(Console.ReadLine());

//    Console.WriteLine("current order: ");
//    foreach (IceCream iceCream in customersDict[customerID].CurrentOrder.IceCreamList)
//    {
//        Console.WriteLine(iceCream);
//    }
//    Console.WriteLine("[1] Choose an existing ice cream to modify");
//    Console.WriteLine("[2] Add a new ice cream to the order");
//    Console.WriteLine("[3] Choose an existing ice cream to delete from the order");
//    Console.Write("Enter your option: ");
//    string option = Console.ReadLine();

//    switch (option)
//    {
//        case "1":
//            Console.Write("Which ice cream: ");
//            int iceCream = Convert.ToInt32(Console.ReadLine());
//            customersDict[customerID].ModifyIceCream(iceCream - 1);
//            break;
//        case "2":
//            List<Flavour> flavours = new List<Flavour>();
//            Console.WriteLine("Enter option: ");
//            string option = Console.ReadLine();

//            Console.WriteLine("Enter number of scoops: ");
//            int scoops = Convert.ToInt32(Console.ReadLine());
//            for (int i = 0; i <= scoops; i++)
//            {
//                Console.WriteLine("Enter flavour type: ");
//                string flavourtype = Console.ReadLine();

//                Console.WriteLine("Is it premium? (True/False): ");
//                bool flavourpremium = Convert.ToBoolean(Console.ReadLine());

//                Console.WriteLine("Enter new flavour quantity: ");
//                int flavourquantity = Convert.ToInt32(Console.ReadLine());

//                Flavour flavour = new Flavour(flavourtype, flavourpremium, flavourquantity);
//                flavours.Add(flavour);
//            }

//            Console.WriteLine("Enter new topping (or nil to stop adding): ");
//            string toppingType = Console.ReadLine();
//            Topping topping = new Topping(toppingtype);
//            IceCreamList[iceCream].Toppings.Add(topping);

//            while (toppingType != "nil")
//            {
//                Console.WriteLine("Enter new topping (or nil to stop adding): ");
//                toppingType = Console.ReadLine();
//                topping = new Topping(toppingType);
//                IceCreamList[iceCream].Toppings.Add(topping);
//            }
//            break;
//        case "3":
//            if (customersDict[customerID].CurrentOrder.IceCreamList.Count > 1)
//            {
//                Console.Write("Which ice cream: ");
//            }
//            else
//            {
//                Console.WriteLine("You cannot have zero ice creams in an order");
//            }
//            break;
//    }
//}
//catch (Exception e)
//{
//    Console.WriteLine(e.Message);
//}


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
            CreateOrder(customersDict, orderDict);
            break;
        case "5":
            Console.WriteLine("========== [5] Display order details of a customer. ==========");
            CustomerOrder();
            break;
        case "6":
            break;
        case "0":
            Console.WriteLine("Bye!");
            toggle = false;
            break;
    }

}
