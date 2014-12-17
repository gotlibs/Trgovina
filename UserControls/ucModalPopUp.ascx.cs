using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Trgovina.UserControls
{
    public partial class ucModalPopUp : System.Web.UI.UserControl
    {
        [TemplateContainer(typeof(ContentContainer))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate ContentTemplate { get; set; }

        public class ContentContainer : Control, INamingContainer { }

        private string divCssClass = "popupMain sirina400";

        #region Public Properties
        //public String Naslov
        //{
        //    set 
        //    {
        //        popupDrag.Visible = true;
        //        lblNaslov.Text = value; 
        //    }
        //}

        public string DivCssClass
        {
            get { return divCssClass; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    divCssClass = value;
                    divPopupMain.Attributes.Remove("class");
                    divPopupMain.Attributes.Add("class", divCssClass);
                }
            }
        }
        #endregion

        #region Page_Init
        protected void Page_Init()
        {
            // If a content template has been defined on the parent page then initialize the content template
            if (ContentTemplate != null)
            {
                // Instantiate and initialize the content container class
                ContentContainer container = new ContentContainer();
                ContentTemplate.InstantiateIn(container);

                // Add the content template to the placeholder so that we can see the user content
                phContentTemplate.Controls.Add(container);
            }
        }
        #endregion

        #region Show / Hide Metode
        public void Show()
        {
            mpePopUp.Show();
        }

        public void Hide()
        {
            mpePopUp.Hide();
        }
        #endregion
    }
}