using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Trgovina.UserControls
{
    public partial class ucOpozorilo : System.Web.UI.UserControl
    {
        #region Public properties
        public int Vrsta
        {
            set
            {
                //error
                if (value == 0)
                {
                    lblOpozorilo.CssClass = "errorLabel";
                    divOpozorilo.Attributes.Add("class", "errorDiv");
                }
                else//opozorilo
                {
                    lblOpozorilo.CssClass = "opozoriloLabel";
                    divOpozorilo.Attributes.Add("class", "opozoriloDiv");
                }
            }
        }

        #region Label properties
        public string Text
        {
            get { return lblOpozorilo.Text; }
            set { lblOpozorilo.Text = value; }
        }

        public bool LabelVisible
        {
            get { return lblOpozorilo.Visible; }
            set { lblOpozorilo.Visible = value; }
        }

        public string LabelClass
        {
            get { return lblOpozorilo.CssClass; }
            set { lblOpozorilo.CssClass = value; }
        }
        #endregion
        #region Div properties
        public bool DivVisible
        {
            get { return divOpozorilo.Visible; }
            set { divOpozorilo.Visible = value; }
        }

        public string DivClass
        {
            get { return divOpozorilo.Attributes["class"]; }
            set { divOpozorilo.Attributes.Add("class", value); }
        }
        #endregion
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion
    }
}