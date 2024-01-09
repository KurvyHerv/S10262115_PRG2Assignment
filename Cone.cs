using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262115_PRG2Assignment
{
    class Cone : IceCream
    {
        public bool Dipped { get; set; }
        public Cone() { }
        public Cone(string options, int scoops, List<Flavour> flavours, List<Topping> toppings, bool dipped) : base(options, scoops, flavours, toppings)
        {
            Dipped = dipped;
        }
        public override double CalculatePrice()
        {

        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
