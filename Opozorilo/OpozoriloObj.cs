using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trgovina.Opozorilo;

namespace Trgovina.Opozorilo
{
    public class OpozoriloObj
    {
        DataSetOpozorilo.OpozoriloRow dataRow;

        #region Konstruktorji
        public OpozoriloObj()
        {
            DataSetOpozorilo.OpozoriloDataTable dt = new DataSetOpozorilo.OpozoriloDataTable();
            dataRow = dt.NewOpozoriloRow();
        }

        public OpozoriloObj(int vrsta, string vsebina)
        {
            DataSetOpozorilo.OpozoriloDataTable dt = new DataSetOpozorilo.OpozoriloDataTable();
            dataRow = dt.NewOpozoriloRow();
            this.VRSTA = vrsta;
            this.VSEBINA = vsebina;
        }

        //public OpozoriloObj(Opozorilo.OpozoriloRow dr)
        //{
        //    dataRow = dr;
        //}
        #endregion

        #region Public property
        public int VRSTA
        {
            get
            {
                if (dataRow.IsVRSTANull())
                    return -1;
                return dataRow.VRSTA;
            }
            set
            {
                if (value == -1)
                    dataRow.SetVRSTANull();
                else
                    dataRow.VRSTA = value;
            }
        }

        public string VSEBINA
        {
            get
            {
                if (dataRow.IsVSEBINANull())
                    return string.Empty;
                return dataRow.VSEBINA;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    dataRow.SetVSEBINANull();
                else
                    dataRow.VSEBINA = value;
            }
        }
        #endregion
    }
}