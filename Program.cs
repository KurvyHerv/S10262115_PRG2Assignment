//==========================================================
// Student Number : S10258053, 
// Student Name : Soong Chu Wen Rena
// Partner Name : 
//==========================================================

using S10262115_PRG2Assignment;

//Create Customer Dictionary
Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
string[] customersFile = File.ReadAllLines("customers.csv");
for (int i = 1; i < customersFile.Length; i++)
{
    string[] line = customersFile[i].Split(",");
    PointCard pointCard = new PointCard(Convert.ToInt32(line[4]), Convert.ToInt32(line[5]));
    pointCard.Tier = line[3];

    Customer customer = new Customer(line[0], Convert.ToInt32(line[1]), DateTime.Parse(line[2]));
    customer.Rewards = pointCard;
    customers.Add(customer.MemberId, customer);
}