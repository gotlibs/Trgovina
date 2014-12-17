using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trgovina;

namespace Trgovina.Objects
{
    public class Kategorije
    {
        public static List<Kategorija> VrniKategorije()
        {
            List<Kategorija> listKategorije = new List<Kategorija>();

            DataSetBaza.KategorijaDataTable dtKategorija = KategorijaDB.KategorijaGet();

            foreach (DataSetBaza.KategorijaRow row in dtKategorija.Rows)
            {
                Kategorija kategorija = new Kategorija(row);
                listKategorije.Add(kategorija);
            }

            return listKategorije;
        }
    }

    public class Kategorija
    {
        private DataSetBaza.KategorijaRow row;

        public Kategorija()
        {
            DataSetBaza.KategorijaDataTable dt = new DataSetBaza.KategorijaDataTable();
            row = dt.NewKategorijaRow();
        }

        public Kategorija(DataSetBaza.KategorijaRow row)
        {
            this.row = row;
        }

        public Kategorija(int kategorija_id, string naziv, int zap_st = -1)
        {
            DataSetBaza.KategorijaDataTable dt = new DataSetBaza.KategorijaDataTable();
            row = dt.NewKategorijaRow();
            row.ZAP_ST_PRIKAZ = zap_st;
            row.KATEGORIJA_ID = kategorija_id;
            row.NAZIV = naziv;
            dt.AddKategorijaRow(row);
            dt.AcceptChanges();
        }

        #region Public properties
        public int KATEGORIJA_ID
        {
            get
            {
                return row.KATEGORIJA_ID;
            }
            set
            {
                row.KATEGORIJA_ID = value;
            }
        }

        public string NAZIV
        {
            get
            {
                if (row.IsNAZIVNull())
                    return String.Empty;
                else
                    return row.NAZIV;
            }
            set
            {
                row.NAZIV = value;
            }
        }

        public int ZAP_ST_PREGLED
        {
            get
            {
                return row.ZAP_ST_PRIKAZ;
            }
            set
            {
                row.ZAP_ST_PRIKAZ = value;
            }
        }

        public int ZAP_ST_IZPIS
        {
            get
            {
                return row.ZAP_ST_IZPIS;
            }
            set
            {
                row.ZAP_ST_IZPIS = value;
            }
        }
        #endregion
    }
}