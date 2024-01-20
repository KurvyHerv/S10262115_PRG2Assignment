//==========================================================
// Student Number : S10258053
// Student Name : Soong Chu Wen Rena
// Partner Number : S10262115
// Partner Name : Hervin Darmawan Sie
//==========================================================

using S10262115_PRG2Assignment;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

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
List<Order> orderDictList = new List<Order>();
Dictionary<int, List<Order>> orderDict = new Dictionary<int, List<Order>>();
string[] premiumList = { "Durian", "Ube", "Sea Salt" };
int k = 0;

for (int i = 1; i < orders.Length; i++)
{
    List<Flavour> flavourList = new List<Flavour>();
    List<Topping> toppingList = new List<Topping>();
    bool dipped = false;
    string[] line = orders[i].Split(",");
    orderList.Add(new(Convert.ToInt32(line[0]), Convert.ToDateTime(line[2])));

    for (int j = 8; j < 10; j++)
    {
        if (line[j] != "")
        {
            bool premium = premiumList.Contains(line[j]);
            flavourList.Add(new(line[j], premium, line.Count(str => str.Contains(line[j]))));
        }
    }

    for (int j = 11; j <= 14; j++)
    {
        if (line[j] != "")
        {
            toppingList.Add(new(line[j]));
        }
    }

    if (line[6] == "TRUE")
    {
        dipped = true;
    }

    if (line[4] == "Cup")
    {
        orderList[k].TimeFullfilled = Convert.ToDateTime(line[3]);
        orderList[k].IceCreamList.Add(new Cup(line[4], Convert.ToInt32(line[5]), flavourList, toppingList));
    }
    if (line[4] == "Cone")
    {
        orderList[k].TimeFullfilled = Convert.ToDateTime(line[3]);
        orderList[k].IceCreamList.Add(new Cone(line[4], Convert.ToInt32(line[5]), flavourList, toppingList, dipped));
    }
    if (line[4] == "Waffle")
    {
        orderList[k].TimeFullfilled = Convert.ToDateTime(line[3]);
        orderList[k].IceCreamList.Add(new Waffle(line[4], Convert.ToInt32(line[5]), flavourList, toppingList, line[7]));
    }
    int customerId = Convert.ToInt32(line[1]);
    if (orderDict.ContainsKey(customerId))
    {
        orderDict[customerId].Add(orderList[k]);
    }
    else
    {
        orderDict.Add(customerId, new List<Order> { orderList[k] });
    }
    k++;
}


//====Basic Features====
//Menu
static void Menu()
{
    Console.WriteLine("----------------Menu----------------");
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
static void ListAllCustomers(Dictionary<int, Customer> customersDict)
{
    Console.WriteLine("\n");
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
    Console.WriteLine("\n");
    foreach (Order order in orderList)
    {
        Console.WriteLine(order.ToString());
    }
}

//3 - Register a new Customer (Rena)
static void RegisterNewCustomer(Dictionary<int, Customer> customersDict)
{
    Console.WriteLine("\n");

    //default points& punchcard
    int defaultPoints = 0;
    int defaultPunchCard = 0;
    string defaultTier = "Ordinary";

    //Prompt for customer name
    Console.Write("Enter the customer's name: ");
    string name = Console.ReadLine();

    //Prompt for customer id
    Console.Write("Enter the customer's id number: ");
    int customerId = Convert.ToInt32(Console.ReadLine());

    //Prompt for customer dob
    Console.Write("Enter the customer's date of birth (dd/mm/yyyy): ");
    DateTime dob = DateTime.Parse(Console.ReadLine());

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
            $"{newCustomer.Dob.ToString("dd/mm/yyyy")}," +
            $"{newCustomer.Rewards.Tier}," +
            $"{newCustomer.Rewards.Points}," +
            $"{newCustomer.Rewards.PunchCard}";
    File.AppendAllText("customers.csv", newCustomerInfo);

    Console.WriteLine("New Customer added. The following information is added: ");
    Console.WriteLine($"" +
            $"{newCustomer.Name}," +
            $"{newCustomer.MemberId}," +
            $"{newCustomer.Dob.ToString("dd/mm/yyyy")}," +
            $"{newCustomer.Rewards.Tier}," +
            $"{newCustomer.Rewards.Points}," +
            $"{newCustomer.Rewards.PunchCard}");
    Console.WriteLine("\n");
}

//4 - Create a Customer's Order (Rena)
static void CreateOrder(Dictionary<int, Customer> customersDict)
{
    //list customers from customers.csv file
    ListAllCustomers(customersDict);

    //promt user to select customer
    Console.Write("Input customer ID: ");
    int customerID = Convert.ToInt32(Console.ReadLine());
}

//5 - Display order details of a customer (Hervin)
void CustomerOrder()
{
    ListAllCustomers(customersDict);
    try
    {
        Console.Write("Input customer ID: ");
        int customerID = Convert.ToInt32(Console.ReadLine());

        foreach (Order order in orderDict[customerID])
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


//Initialise methods
while (toggle)
{
    Menu();
    string option = Console.ReadLine();
    switch (option)
    {
        case "1":
            ListAllCustomers(customersDict);
            break;
        case "2":
            ListAllOrders(); 
            break;
        case "3":
            RegisterNewCustomer(customersDict);
            break;
        case "4":
            break;
        case "5":
            CustomerOrder();
            break;
        case "6":
            break;
        case "0":
            toggle = false;
            break;
    }

}
