using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace Trgovina
{
    public class Enums
    {
        public sealed class Pogostost
        {
            private Pogostost()
            {

            }

            public static ListDictionary AllValues
            {
                get
                {
                    ListDictionary ar = new ListDictionary();
                    ar.Add(1, "Zelo pogosto");
                    ar.Add(2, "Pogosto");
                    ar.Add(3, "Redko");
                    return (ar);
                }
            }
            public static int ZeloPogosto { get { return 1; } }
            public static int Pogosto { get { return 2; } }
            public static int Redko { get { return 3; } }
        }

        #region PageMode
        public enum PageMode
        {
            ReadOnly = 0,
            Insert = 1,
            Edit = 2
        }
        #endregion

        #region RezultatShranjevanja
        public sealed class RezultatShranjevanja
        {
            private RezultatShranjevanja()
            {

            }

            public static ListDictionary AllValues
            {
                get
                {
                    ListDictionary ar = new ListDictionary();
                    ar.Add(0, "Napaka");
                    ar.Add(1, "Uspesno");
                    ar.Add(2, "Neuspesno");
                    return (ar);
                }
            }
            public static int Napaka { get { return 0; } }
            public static int Uspesno { get { return 1; } }
            public static int Neuspesno { get { return 2; } }
        }
        #endregion

        public sealed class UporabnikiEmail
        {
            private UporabnikiEmail()
            {

            }

            public static ListDictionary AllValues
            {
                get
                {
                    ListDictionary ar = new ListDictionary();
                    ar.Add(1, "anitafranko@gmail.com");
                    ar.Add(2, "simon.gotlib@gmail.com");
                    return (ar);
                }
            }
            public static int AnitaFranko { get { return 1; } }
            public static int SimonGotlib { get { return 2; } }
        }
    }
}