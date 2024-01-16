//==========================================================
// Student Number : S10258053, 
// Student Name : Soong Chu Wen Rena
// Partner Name : 
//==========================================================

using S10262115_PRG2Assignment;

//Create Customer Dictionary
Dictionary<int, Customer> customersDict = new Dictionary<int, Customer>();
string[] customersFile = File.ReadAllLines("customers.csv");
for (int i = 1; i < customersFile.Length; i++)
{
    string[] line = customersFile[i].Split(",");
    PointCard pointCard = new PointCard(Convert.ToInt32(line[4]), Convert.ToInt32(line[5]));
    pointCard.Tier = line[3];

    Customer customer = new Customer(line[0], Convert.ToInt32(line[1]), DateTime.Parse(line[2]));
    customer.Rewards = pointCard;
    customersDict.Add(customer.MemberId, customer);
}

//Qn1 - List all customers
void ListAllCustomers(Dictionary<int, Customer> customersDict)
{
    Console.WriteLine("[1] Display the information of all customers: ");
    Console.WriteLine("{0, -15} {1, -15} {2, -15} {3, -15} {4, -15} {5, -15}",
        )
}
PrintCustomers(customersDict);

string[] orders = File.ReadAllLines("orders.csv");
List<Order> orderList = new List<Order>();
List<Flavour> flavourList = new List<Flavour>();
List<Topping> toppingList = new List<Topping>();
string[] premiumList = { "Durian", "Ube", "Sea Salt" };

for (int i = 1; i < orders.Length; i++)
{
    string[] line = orders[i].Split(",");
    orderList.Add(new(Convert.ToInt32(line[0]), Convert.ToDateTime(line[2])));
    for (int j = 8; j < j + Convert.ToInt32(line[5]) - 1; j++)
    {
        bool premium = premiumList.Contains(line[j]);
        flavourList.Add(new(line[j], premium, 1));
    }
    for (int j = 11; j <= 14; j++)
    {
        if (line[j] != "")
        {
            toppingList.Add(new(line[j]));
        }
    }

    foreach (Order order in orderList)
    {
        if (line[6] == "Cup")
        {
            order.IceCreamList.Add(new Cup(line[4], Convert.ToInt32(line[5]), flavourList, toppingList));
        }
        if (line[6] == "Cone")
        {
            order.IceCreamList.Add(new Cone());
        }
        if (line[6] == "Waffle")
        {
            order.IceCreamList.Add(new Waffle());
        }

    }
}

//Menu
void Menu()
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
