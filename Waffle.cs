//==========================================================
// Student Number : S10258053
// Student Name : Soong Chu Wen Rena
// Partner Number : S10262115
// Partner Name : Hervin Darmawan Sie
//==========================================================

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
            double price = 0.00;
            if (Scoops == 1)
            {
                price = 7.00;
            }
            else if (Scoops == 2)
            {
                price = 8.50;
            }
            else if (Scoops == 3)
            {
                price = 9.50;
            }

            foreach (Flavour flavour in Flavours)
            {
                if (flavour.Premium)
                {
                    price += 2;
                }
            }
            price = price + Toppings.Count;

            if (WaffleFlavour == "Red Velvet" || WaffleFlavour == "Charcoal" || WaffleFlavour == "Pandan")
            {
                price += 3.00;
            }
            return price;
        }
        public override string ToString()
        {
            return base.ToString() + " Waffle Flavour: " + WaffleFlavour;
        }
    }
}
