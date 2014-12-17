using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trgovina.Objects
{
    public class SeznamIzdelkov
    {
        public static List<SeznamIzdelek> VrniSeznamIzdelkov()
        {
            List<SeznamIzdelek> listSeznamIzdelkov = new List<SeznamIzdelek>();

            DataSetBaza.SeznamIzdelekDataTable dtSeznamIzdelek = SeznamIzdelekDB.SeznamIzdelekGet();

            foreach (DataSetBaza.SeznamIzdelekRow row in dtSeznamIzdelek.Rows)
            {
                SeznamIzdelek seznamIzdelek = new SeznamIzdelek(row);
                listSeznamIzdelkov.Add(seznamIzdelek);
            }

            return listSeznamIzdelkov;
        }

        public static List<Izdelek> VrniIzdelkeSeznamaIzdelkov(List<SeznamIzdelek> listSeznamIzdelkov)
        {
            List<Izdelek> listIzdelek = new List<Izdelek>();

            foreach (SeznamIzdelek seznamIzdelek in listSeznamIzdelkov)
            {
                Izdelek izdelek = new Izdelek();
                izdelek.IZDELEK_ID = seznamIzdelek.IZDELEK_ID;
                izdelek.OPIS = seznamIzdelek.OPIS;
                listIzdelek.Add(izdelek);
            }

            return listIzdelek;
        }

        public static List<SeznamIzdelek> VrniSeznamIzdelkovIzdelkov(List<Izdelek> listIzdelek)
        {
            List<SeznamIzdelek> listSeznamIzdelek = new List<SeznamIzdelek>();

            foreach (Izdelek izdelek in listIzdelek)
            {
                SeznamIzdelek seznamIzdelek = new SeznamIzdelek();
                seznamIzdelek.IZDELEK_ID = izdelek.IZDELEK_ID;
                seznamIzdelek.OPIS = izdelek.OPIS;
                listSeznamIzdelek.Add(seznamIzdelek);
            }

            return listSeznamIzdelek;
        }

        public static void IzbrisiSeznamIzdelkov()
        {
            SeznamIzdelekDB.SeznamIzdelekDelete();
        }

        public static void ShraniSeznamIzdelkov(List<Izdelek> listIzdelek)
        {
            List<SeznamIzdelek> listSeznamIzdelek = VrniSeznamIzdelkovIzdelkov(listIzdelek);

            foreach (SeznamIzdelek seznamIzdelek in listSeznamIzdelek)
            {
                int st_affected_rows = SeznamIzdelekDB.SeznamIzdelekInsert(seznamIzdelek);
            }
        }
    }

    public class SeznamIzdelek
    {
        private DataSetBaza.SeznamIzdelekRow row;
        private string opis;

        public SeznamIzdelek()
        {
            DataSetBaza.SeznamIzdelekDataTable dt = new DataSetBaza.SeznamIzdelekDataTable();
            row = dt.NewSeznamIzdelekRow();
        }

        public SeznamIzdelek(DataSetBaza.SeznamIzdelekRow row)
        {
            this.row = row;
        }

        public SeznamIzdelek(int izdelekId)
        {
            DataSetBaza.SeznamIzdelekDataTable dt = new DataSetBaza.SeznamIzdelekDataTable();
            row = dt.NewSeznamIzdelekRow();
            row.IZDELEK_ID = izdelekId;
            dt.AddSeznamIzdelekRow(row);
            dt.AcceptChanges();
        }

        #region Public properties
        public int SEZNAM_IZDELEK_ID
        {
            get
            {
                return row.SEZNAM_IZDELEK_ID;
            }
            set
            {
                row.SEZNAM_IZDELEK_ID = value;
            }
        }

        public int IZDELEK_ID
        {
            get
            {
                return row.IZDELEK_ID;
            }
            set
            {
                row.IZDELEK_ID = value;
            }
        }

        public string OPIS
        {
            get
            {
                if (row.IsOPISNull())
                    return String.Empty;
                else
                    return row.OPIS;
            }
            set
            {
                row.OPIS = value;
            }
        }
        #endregion
    }
}