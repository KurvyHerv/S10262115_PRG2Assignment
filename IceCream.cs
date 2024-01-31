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
    abstract class IceCream
    {
        public string Option { get; set; }
        public int Scoops { get; set; }
        public List<Flavour> Flavours { get; set; } = new List<Flavour>(new Flavour[3]);
        public List<Topping> Toppings { get; set; } = new List<Topping>(new Topping[4]);
        public IceCream() { }
        public IceCream(string option, int scoops, List<Flavour> flavours, List<Topping> toppings)
        { 
            Option = option;
            Scoops = scoops;
            Flavours = flavours;
            Toppings = toppings;
        }
        public abstract double CalculatePrice();
        public override string ToString()
        {
            List<string> flavourList = new List<string>();
            List<string> topppingList = new List<string>();
            foreach (Flavour flavour in Flavours)
            {
                flavourList.Add(flavour.Type);
            }
            foreach (Topping topping in Toppings)
            {
                topppingList.Add(topping.Type);
            }
            return $"\nOption: {Option}"+
                $"\nScoops: {Scoops}"+
                $"\nFlavour(s) {string.Join(", ", flavourList)}"+
                $"\nTopping(s) {string.Join(", ", topppingList)}";
        }

    }
}
