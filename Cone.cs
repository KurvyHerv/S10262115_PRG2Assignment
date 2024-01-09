using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            double price = 0.00;
            if (Scoops == 1)
            {
                price = 4.00;
            }
            else if (Scoops == 2)
            {
                price = 5.50;
            }
            else if (Scoops == 3)
            {
                price = 6.50;
            }
            price = price + Toppings.Count;
            foreach (Flavour flavour in Flavours)
            {
                if (flavour.Premium)
                {
                    price += 2;
                }
            }
            if (Dipped)
            {
                price *= 2;
            }
            return price;
        }
        public override string ToString()
        {
            return base.ToString() + Dipped;
        }
    }
}
