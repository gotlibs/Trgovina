using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Trgovina.Objects
{
    public class Izdelki
    {
        public static List<Izdelek> VrniIzdelke()
        {
            List<Izdelek> listIzdelki = new List<Izdelek>();

            DataSetBaza.IzdelekDataTable dtIzdelki = IzdelekDB.IzdelekGet();

            foreach (DataSetBaza.IzdelekRow row in dtIzdelki.Rows)
            {
                Izdelek izdelek = new Izdelek(row);
                listIzdelki.Add(izdelek);
            }

            return listIzdelki;
        }

        public static List<Izdelek> VrniIzdelke(int kategorijaId)
        {
            List<Izdelek> listIzdelki = VrniIzdelke();

            List<Izdelek> listIzdelkiKategorija = listIzdelki
                                                    .Where(i => i.KATEGORIJA_ID == kategorijaId)
                                                    .OrderBy(i => i.POGOSTOST)
                                                    .ThenBy(i => i.NAZIV)
                                                    .ToList();

            int zapSt = 1;
            foreach (Izdelek izdelek in listIzdelkiKategorija)
            {
                izdelek.ZAP_ST = zapSt;
                zapSt++;
            }

            return listIzdelkiKategorija;
        }

        public static int IzbrisiIzdelek(Izdelek izdelek)
        {
            int st_affected_rows = IzdelekDB.IzdelekDelete(izdelek);

            return st_affected_rows;
        }

        public static int PosodobiIzdelek(Izdelek izdelek)
        {
            int st_affected_rows = IzdelekDB.IzdelekUpdate(izdelek);

            return st_affected_rows;
        }

        public static int DodajIzdelek(Izdelek izdelek)
        {
            int st_affected_rows = IzdelekDB.IzdelekInsert(izdelek);

            return st_affected_rows;
        }
    }

    public class Izdelek
    {
        private string opis;
        private DataSetBaza.IzdelekRow row;

        public Izdelek()
        {
            DataSetBaza.IzdelekDataTable dt = new DataSetBaza.IzdelekDataTable();
            row = dt.NewIzdelekRow();
        }

        public Izdelek(DataSetBaza.IzdelekRow row)
        {
            this.row = row;
        }

        public Izdelek(int izdelek_id, string naziv, int kategorija_id, int pogostost, int zap_st = -1, string opis = "")
        {
            DataSetBaza.IzdelekDataTable dt = new DataSetBaza.IzdelekDataTable();
            row = dt.NewIzdelekRow();
            row.ZAP_ST = zap_st;
            row.IZDELEK_ID = izdelek_id;
            row.NAZIV = naziv;
            row.KATEGORIJA_ID = kategorija_id;
            row.POGOSTOST = pogostost;
            this.OPIS = opis;
            dt.AddIzdelekRow(row);
            dt.AcceptChanges();
        }

        #region Public properties
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

        public int POGOSTOST
        {
            get
            {
                return row.POGOSTOST;
            }
            set
            {
                row.POGOSTOST = value;
            }
        }

        public DateTime DATUM_VNOSA
        {
            get
            {
                if (row.IsDATUM_VNOSANull())
                    return DateTime.MinValue;
                else
                    return row.DATUM_VNOSA;
            }
            set
            {
                row.DATUM_VNOSA = value;
            }
        }

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

        public string KATEGORIJA_NAZIV
        {
            get
            {
                if (row.IsKATEGORIJA_NAZIVNull())
                    return String.Empty;
                else
                    return row.KATEGORIJA_NAZIV;
            }
            set
            {
                row.KATEGORIJA_NAZIV = value;
            }
        }

        public int ZAP_ST
        {
            get
            {
                return row.ZAP_ST;
            }
            set
            {
                row.ZAP_ST = value;
            }
        }

        public string OPIS
        {
            get
            {
                return opis;
            }
            set
            {
                opis = value;
            }
        }
        #endregion
    }
}