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
        public List<Order> OrderHistory { get; set; }
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
            Console.WriteLine("Enter the order id: ");
            int id = Convert.ToInt32(Console.ReadLine());
            DateTime timereceived = DateTime.Now;
            return new Order(id, timereceived);
        }
        public bool IsBirthday()
        {
            return DateTime.Today.Month == Dob.Month && DateTime.Today.Day == Dob.Day;
        }
        public override string ToString()
        {
            return base.ToString() + Name + MemberId + Dob;
        }
    }
}
