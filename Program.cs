//==========================================================
// Student Number : S10258053, 
// Student Name : Soong Chu Wen Rena
// Partner Name : 
//==========================================================

using S10262115_PRG2Assignment;

//Qn 1
void DisplayCustomer()
{
    string[] lines = File.ReadAllLines("customers.csv");
    for (int i = 0; i < lines.Length; i++)
    {
        string[] line = lines[i].Split(",");
        Console.WriteLine("{0, -15} {1, -15} {2, -15} {3, -20} {4, -20} {5, -15}",
            line[0], line[1], line[2], line[3], line[4], line[5]);
    }
}
DisplayCustomer();