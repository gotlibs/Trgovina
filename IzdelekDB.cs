using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trgovina.Objects;

namespace Trgovina
{
    public class IzdelekDB
    {
        #region Izdelek
        public static DataSetBaza.IzdelekDataTable IzdelekGet(int izdelek_id = -1)
        {
            DataSetBaza ds = new DataSetBaza();
            DataSetBazaTableAdapters.IzdelekTableAdapter ta = new DataSetBazaTableAdapters.IzdelekTableAdapter();
            ta.Fill(ds.Izdelek, izdelek_id);
            return ds.Izdelek;
        }

        public static int IzdelekInsert(Izdelek izdelek)
        {
            int? st_affected_rows_out = 0;
            DataSetBazaTableAdapters.IzdelekTableAdapter ta = new DataSetBazaTableAdapters.IzdelekTableAdapter();
            ta.Insert(izdelek.NAZIV, izdelek.POGOSTOST, izdelek.KATEGORIJA_ID, ref st_affected_rows_out);
            return Convert.ToInt32(st_affected_rows_out);
        }

        public static int IzdelekUpdate(Izdelek izdelek)
        {
            int? st_affected_rows_out = 0;
            DataSetBazaTableAdapters.IzdelekTableAdapter ta = new DataSetBazaTableAdapters.IzdelekTableAdapter();
            ta.Update(izdelek.IZDELEK_ID, izdelek.NAZIV, izdelek.POGOSTOST, izdelek.KATEGORIJA_ID, ref st_affected_rows_out);
            return Convert.ToInt32(st_affected_rows_out);
        }

        public static int IzdelekDelete(Izdelek izdelek)
        {
            int? st_affected_rows_out = 0;
            DataSetBazaTableAdapters.IzdelekTableAdapter ta = new DataSetBazaTableAdapters.IzdelekTableAdapter();
            ta.Delete(izdelek.IZDELEK_ID, ref st_affected_rows_out);
            return Convert.ToInt32(st_affected_rows_out);
        }
        #endregion
    }
}