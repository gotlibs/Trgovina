using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trgovina
{
    public class KategorijaDB
    {
        #region Kategorija
        public static DataSetBaza.KategorijaDataTable KategorijaGet(int kategorija_id = -1)
        {
            DataSetBaza ds = new DataSetBaza();
            DataSetBazaTableAdapters.KategorijaTableAdapter ta = new DataSetBazaTableAdapters.KategorijaTableAdapter();
            ta.Fill(ds.Kategorija, kategorija_id);
            return ds.Kategorija;
        }
        #endregion
    }
}