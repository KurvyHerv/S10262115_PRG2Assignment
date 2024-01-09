using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262115_PRG2Assignment
{
    class Waffle : IceCream
    {
        public string WaffleFlavour { get; set; }
        public Waffle() { }
        public Waffle(string options, int scoops, List<Flavour> flavours, List<Topping> toppings, string waffleFlavour) : base(options, scoops, flavours, toppings)
        {
            WaffleFlavour = waffleFlavour;
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
