using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262115_PRG2Assignment
{
    class Order
    {
        public int Id {  get; set; }
        public DateTime TimeReceived { get; set; }
        public DateTime? TimeFufilled { get; set; }
        public List<IceCream> IceCreamList { get; set; } = new List<IceCream>();
        public Order() { }
        public Order(int id, DateTime timeReceived)
        {
            Id = id;
            TimeReceived = timeReceived;
        }
        public void ModifyIceCream(int id)
        {

        }
        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);
        }
        public void DeleteIceCream(int id)
        {
            IceCreamList.RemoveAt(id - 1);
        }
        public double CalculateTotal()
        {

        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
