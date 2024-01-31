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
    class Topping
    {
        public string Type { get; set; }
        public Topping() { }
        public Topping(string type) 
        {
            Type = type;
        }
        public override string ToString()
        {
            return Type;
        }
    }
}
