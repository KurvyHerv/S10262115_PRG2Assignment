//==========================================================
// Student Number : S10258053, 
// Student Name : Soong Chu Wen Rena
// Partner Name : 
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262115_PRG2Assignment
{
    class PointCard
    {
        public int Points { get; set; }
        public int PunchCard { get; set; }
        public string Tier { get; set; }
        public PointCard() { }
        public PointCard(int points, int punchCard)
        {
            Points = points;
            PunchCard = punchCard;
        }
        public void AddPoints(int points)
        {
            Points += points;
        }
        public void RedeemPoints(int points)
        {
            Points -= points;
        }
        public void Punch()
        {
            if (PunchCard < 11)
            {
                PunchCard++;
            }
            else
            {
                PunchCard = 0;
            }
        }
        public override string ToString()
        {
            return Points + PunchCard + Tier;
        }
    }
}
