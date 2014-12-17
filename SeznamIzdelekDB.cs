using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trgovina.Objects;

namespace Trgovina
{
    public class SeznamIzdelekDB
    {
        #region SeznamIzdelek
        public static DataSetBaza.SeznamIzdelekDataTable SeznamIzdelekGet()
        {
            DataSetBaza ds = new DataSetBaza();
            DataSetBazaTableAdapters.SeznamIzdelekTableAdapter ta = new DataSetBazaTableAdapters.SeznamIzdelekTableAdapter();
            ta.Fill(ds.SeznamIzdelek);
            return ds.SeznamIzdelek;
        }

        public static int SeznamIzdelekInsert(SeznamIzdelek seznamIzdelek)
        {
            int? st_affected_rows_out = 0;
            DataSetBazaTableAdapters.SeznamIzdelekTableAdapter ta = new DataSetBazaTableAdapters.SeznamIzdelekTableAdapter();
            ta.Insert(seznamIzdelek.IZDELEK_ID, seznamIzdelek.OPIS, ref st_affected_rows_out);
            return Convert.ToInt32(st_affected_rows_out);
        }

        public static int SeznamIzdelekDelete()
        {
            int? st_affected_rows_out = 0;
            DataSetBazaTableAdapters.SeznamIzdelekTableAdapter ta = new DataSetBazaTableAdapters.SeznamIzdelekTableAdapter();
            ta.Delete(ref st_affected_rows_out);
            return Convert.ToInt32(st_affected_rows_out);
        }
        #endregion
    }
}