using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace Trgovina.Objects
{
    public class Pogostost
    {
        public ListDictionary VrniPogostosti()
        {
            return Enums.Pogostost.AllValues;
        }

        public static string VrniPogostost(string pogostostId)
        {
            return Enums.Pogostost.AllValues[Convert.ToInt32(pogostostId)].ToString();
        }
    }
}