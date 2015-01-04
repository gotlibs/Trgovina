using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Trgovina.Objects;
using Trgovina.Opozorilo;
using Trgovina.UserControls;
using System.Data;
using System.Drawing.Printing;
using System.Drawing;
using System.Web.Mail;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Collections.Specialized;

namespace Trgovina
{
    public partial class Trgovina : System.Web.UI.Page
    {
        #region Public Properties
        #region GridViewSortDirection
        private string GridViewSortDirection
        {
            get
            {
                return ViewState["SortDirection"] as string ?? "asc";
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }
        #endregion

        #region GridViewSortExpression
        private string GridViewSortExpression
        {
            get
            {
                return ViewState["SortExpression"] as string ?? "izdelek_id";
            }
            set
            {
                ViewState["SortExpression"] = value;
            }
        }

        public string SmtpServer
        {
            get
            {
                string smtp_server = "";
                if (ConfigurationManager.AppSettings["smtpServer"] != null)
                    smtp_server = ConfigurationManager.AppSettings["smtpServer"].ToString();
                return smtp_server;
            }
        }

        public string EmailPosiljatelja
        {
            get
            {
                string emailPosiljatelja = "";
                if (ConfigurationManager.AppSettings["emailPosiljatelja"] != null)
                    emailPosiljatelja = ConfigurationManager.AppSettings["emailPosiljatelja"].ToString();
                return emailPosiljatelja;
            }
        }

        public string EmailUsername
        {
            get
            {
                string emailUsername = "";
                if (ConfigurationManager.AppSettings["emailUsername"] != null)
                    emailUsername = ConfigurationManager.AppSettings["emailUsername"].ToString();
                return emailUsername;
            }
        }

        public string EmailPassword
        {
            get
            {
                string emailPassword = "";
                if (ConfigurationManager.AppSettings["emailPassword"] != null)
                    emailPassword = ConfigurationManager.AppSettings["emailPassword"].ToString();
                return emailPassword;
            }
        }
        #endregion
        #endregion
        //end sorting

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                FillData();
        }

        public void FillData()
        {
            List<Kategorija> listKategorije = Kategorije.VrniKategorije()
                                                        .Where(k => k.KATEGORIJA_ID != Convert.ToInt32(Enums.KategorijaOstaliIzdelki.OstaliIzdelki))
                                                        .ToList();

            rpKategorije.DataSource = listKategorije;
            rpKategorije.DataBind();

            foreach (RepeaterItem item in rpKategorije.Items)
            {
                GridView gvPregledIzdelkov = item.FindControl("gvPregledIzdelkov") as GridView;
                HiddenField hdfKategorijaId = item.FindControl("hdfKategorijaId") as HiddenField;
                if (gvPregledIzdelkov == null || hdfKategorijaId == null)
                    return;

                List<Izdelek> listIzdelkiZaKategorijo = Izdelki.VrniIzdelke(Convert.ToInt32(hdfKategorijaId.Value));

                gvPregledIzdelkov.DataSource = listIzdelkiZaKategorijo;
                gvPregledIzdelkov.DataBind();
            }

            if (Session["IzbraniIzdelki"] != null)
                NastaviIzbraneIzdelke(Session["IzbraniIzdelki"] as List<Izdelek>);
        }

        protected void gvPregledIzdelkov_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            HideOpozorilo(ucOpozorilo1);

            ImageButton lb = e.CommandSource as ImageButton;
            if (lb == null)
                return;

            GridViewRow row = (lb).NamingContainer as GridViewRow;
            if (row == null)
                return;

            if (e.CommandName == "izbrisi")
            {
                Izdelek izdelek = CreateIzdelekObjFromGridRow(row);
                if (izdelek == null)
                    return;

                int st_affected_rows = Izdelki.IzbrisiIzdelek(izdelek);
                if (st_affected_rows <= 0)
                {
                    FillOpozorilo("Neuspešno brisanje izdelka.", 0, ucOpozorilo1);
                    return;
                }
                FillOpozorilo("Uspešno brisanje izdelka.", 1, ucOpozorilo1);

                List<Izdelek> izbraniIzdelki = IzbraniIzdelki();
                Session["IzbraniIzdelki"] = izbraniIzdelki;

                FillData();
            }
            else if (e.CommandName == "uredi")
            {
                Izdelek izdelek = CreateIzdelekObjFromGridRow(row);
                if (izdelek == null)
                    return;

                puLblNazivError.Visible = false;
                puTbNaziv.Text = izdelek.NAZIV;
                puDdlKategorija.SelectedValue = izdelek.KATEGORIJA_ID.ToString();
                puDdlPogostost.SelectedValue = izdelek.POGOSTOST.ToString();
                puHdfIzdelekId.Value = izdelek.IZDELEK_ID.ToString();
                puHdfPageMode.Value = Enums.PageMode.Edit.ToString();
                lblNaslovPopup.Text = "Vnos izdelka";
                mpuIzdelekPopup.Show();
            }
        }

        private Izdelek CreateIzdelekObjFromGridRow(GridViewRow row)
        {
            HiddenField gvHdfIzdelekId = row.FindControl("gvHdfIzdelekId") as HiddenField;
            if (gvHdfIzdelekId == null)
                return null;

            List<Izdelek> listIzdelki = Izdelki.VrniIzdelke();
            List<Izdelek> izbranIzdelek = listIzdelki.Where(i => i.IZDELEK_ID == Convert.ToInt32(gvHdfIzdelekId.Value)).ToList();
            if (!izbranIzdelek.Any())
                return null;

            return izbranIzdelek[0];
        }

        //protected void gvPregledIzdelkov_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    GridViewSortExpression = e.SortExpression;
        //    GridViewSortDirection = (GridViewSortDirection == "desc") ? "asc" : "desc";
        //    gvPregledIzdelkov.DataBind();
        //}

        protected void lbVnosNovegaIzdelka_Click(object sender, EventArgs e)
        {
            puLblNazivError.Visible = false;
            lblNaslovPopup.Text = "Vnos izdelka";
            puHdfPageMode.Value = Enums.PageMode.Insert.ToString();
            PocistiPopup();
            puHdfIzdelekId.Value = "-1";
            mpuIzdelekPopup.Show();
        }

        public void PocistiPopup()
        {
            puTbNaziv.Text = String.Empty;
            puDdlKategorija.SelectedIndex = puDdlPogostost.SelectedIndex = 0;
        }

        protected void rpKategorije_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void lbtnShrani_Command(object sender, CommandEventArgs e)
        {
            HideOpozorilo(ucOpozorilo1);
            HideOpozorilo(ucOpozoriloPU);
            puLblNazivError.Visible = false;
            int shranjevanjeUspesno = 0;
            switch (e.CommandArgument.ToString())
            {
                case "shrani":
                    shranjevanjeUspesno = LastnostiShrani();
                    break;
                case "preklici":
                    shranjevanjeUspesno = 1;
                    break;
            }

            //ni uspesno -> prikazemo popup
            if (shranjevanjeUspesno == Enums.RezultatShranjevanja.Napaka)
                mpuIzdelekPopup.Show();
            else if (shranjevanjeUspesno == Enums.RezultatShranjevanja.Uspesno)
                mpuIzdelekPopup.Hide();
            else
                mpuIzdelekPopup.Hide();
        }

        public int LastnostiShrani()
        {

            int shranjevanjeUspesno = IzdelekShrani();

            if (shranjevanjeUspesno != 1)
                return shranjevanjeUspesno;

            List<Izdelek> izbraniIzdelki = IzbraniIzdelki();
            Session["IzbraniIzdelki"] = izbraniIzdelki;

            FillData();

            return Enums.RezultatShranjevanja.Uspesno;
        }

        public int IzdelekShrani()
        {
            if (puHdfPageMode.Value == Enums.PageMode.Edit.ToString())
            {
                Izdelek izdelek = IzdelekCreate();
                if (izdelek == null)
                    return 2;

                int st_affected_rows = Izdelki.PosodobiIzdelek(izdelek);
                if (st_affected_rows <= 0)
                {
                    FillOpozorilo("Neuspešno urejanje izdelka.", 0, ucOpozorilo1);
                    return 2;
                }
                FillOpozorilo("Uspešno urejanje izdelka.", 1, ucOpozorilo1);
            }
            else
            {
                Izdelek izdelek = IzdelekCreate();
                if (izdelek == null)
                    return 2;

                int st_affected_rows = Izdelki.DodajIzdelek(izdelek);
                if (st_affected_rows <= 0)
                {
                    FillOpozorilo("Neuspešno dodano izdelek.", 0, ucOpozorilo1);
                    return 2;
                }
                FillOpozorilo("Uspešno dodan izdelek.", 1, ucOpozorilo1);
            }

            return 1;
        }

        public Izdelek IzdelekCreate()
        {
            if (puDdlKategorija.SelectedValue == "-1" || puDdlPogostost.SelectedValue == "-1")
                return null;

            //return new PlaciloObj(Convert.ToInt32(hdfPlaciloId.Value), Convert.ToInt32(ddlPopupTip.SelectedValue), Convert.ToInt32(ddlPopupUporabnikVnos.SelectedValue), Convert.ToInt32(ddlPopupUporabnikPlacal.SelectedValue), DateTime.MinValue, Convert.ToDateTime(tbDatumPopup.Text.Trim()), ddlPopupTip.SelectedValue == EnumsDenar.PlaciloTip.PartnerjuVSkupno.ToString() ? Convert.ToDecimal(tbZnesek.Text.Trim()) * 2 : Convert.ToDecimal(tbZnesek.Text.Trim()), tbOpomba.Text.Trim());
            return new Izdelek(Convert.ToInt32(puHdfIzdelekId.Value), puTbNaziv.Text, Convert.ToInt32(puDdlKategorija.SelectedValue), Convert.ToInt32(puDdlPogostost.SelectedValue));
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            HideOpozorilo(ucOpozorilo1);
            HideOpozorilo(ucOpozoriloPU);
            HideOpozorilo(ucOpozoriloPopUpEmail);

            List<Izdelek> izbraniIzdelki = IzbraniIzdelki();

            if (!izbraniIzdelki.Any())
            {
                FillOpozorilo("Niste izbrali nobenega izdelka", 0, ucOpozorilo1);
                return;
            }

            string textToPrint = VsebinaZaIzpis(izbraniIzdelki);

            Page.ClientScript.RegisterStartupScript(this.GetType(), "id", String.Format("CallPrint('{0}')", textToPrint), true);
        }

        protected void btnEmail_Click(object sender, EventArgs e)
        {
            HideOpozorilo(ucOpozorilo1);
            HideOpozorilo(ucOpozoriloPU);
            HideOpozorilo(ucOpozoriloPopUpEmail);

            tbEmail.Text = string.Empty;

            List<Izdelek> izbraniIzdelki = IzbraniIzdelki();

            if (!izbraniIzdelki.Any())
            {
                FillOpozorilo("Niste izbrali nobenega izdelka", 0, ucOpozorilo1);
                return;
            }

            FillEmails();

            mpuEmail.Show();
        }

        private void FillEmails()
        {
            ListDictionary emails = Enums.UporabnikiEmail.AllValues;

            gvUporabnikiEmaili.DataSource = emails;
            gvUporabnikiEmaili.DataBind();
        }

        protected void lbtnEmailPoslji_Command(object sender, CommandEventArgs e)
        {
            int rezultatShranjevanja = Enums.RezultatShranjevanja.Uspesno;

            switch (e.CommandArgument.ToString())
            {
                case "poslji":
                    rezultatShranjevanja = PosiljanjeEmaila();
                    break;
                case "preklici":
                    break;
            }

            if (rezultatShranjevanja == Enums.RezultatShranjevanja.Uspesno)
            {
                mpuEmail.Hide();
            }
            else
            {
                mpuEmail.Show();
            }
        }

        private int PosiljanjeEmaila()
        {
            HideOpozorilo(ucOpozoriloPopUpEmail);

            //string emailPrejemnika = tbEmail.Text.Trim();
            string emailPosiljatelja = EmailPosiljatelja;
            string smtpServer = SmtpServer;
            string naslov = "Seznam izdelkov za nakup";
            string emailUsername = EmailUsername;
            string emailPassword = EmailPassword;

            List<Izdelek> izbraniIzdelki = IzbraniIzdelki();

            string textToPrint = VsebinaZaIzpis(izbraniIzdelki);

            try
            {
                string emailiZaPosiljanje = tbEmail.Text.Trim();

                string[] arStringiZaPosiljanje = emailiZaPosiljanje.Split(';');

                if (arStringiZaPosiljanje.Length == 0)
                {
                    FillOpozorilo("Izberite emaile.", 0, ucOpozoriloPopUpEmail);
                    return Enums.RezultatShranjevanja.Neuspesno;
                }

                foreach (string emailPrejemnika in arStringiZaPosiljanje)
                {
                    PosljiEmail(emailPrejemnika, emailPosiljatelja, naslov, textToPrint, smtpServer, emailUsername, emailPassword);
                }

                FillOpozorilo("Uspešno poslan seznam izdelkov za nakup na email/e.", 1, ucOpozorilo1);
            }
            catch (Exception ex)
            {
                FillOpozorilo(string.Format("Prišlo je do napake pri pošiljanju emaila: Vsebina napake: {0}", ex.Message), 0, ucOpozoriloPopUpEmail);
                return Enums.RezultatShranjevanja.Neuspesno;
            }

            return Enums.RezultatShranjevanja.Uspesno;
        }


        public void PosljiEmail(string emailPrejemnika, string emailPosiljatelja, string naslov, string vsebina, string smtpServer, string emailUsername, string emailPassword)
        {
            //System.Web.Mail.MailMessage msg = new System.Web.Mail.MailMessage();
            //msg.To = emailPrejemnika;
            //msg.From = emailPosiljatelja;
            //msg.Subject = naslov;
            //msg.Body = vsebina;
            //msg.BodyFormat = MailFormat.Html;
            //SmtpMail.SmtpServer = smtpServer;
            //SmtpMail.Send(msg);

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage(emailPosiljatelja, emailPrejemnika, naslov, vsebina);
            msg.IsBodyHtml = true;
            var smtpClient = new SmtpClient(smtpServer, 587);
            smtpClient.Credentials = new System.Net.NetworkCredential(emailUsername, emailPassword);
            smtpClient.Send(msg);
        }

        private string VsebinaZaIzpis(List<Izdelek> izbraniIzdelki)
        {
            string textToPrint = string.Empty;

            VrniIzdelkeString(izbraniIzdelki, ref textToPrint);

            VrniOstaleIzdelkeString(izbraniIzdelki, ref textToPrint);

            return textToPrint;
        }

        private void VrniIzdelkeString(List<Izdelek> izbraniIzdelki, ref string textToPrint)
        {
            var kategorije = Kategorije.VrniKategorije()
                                       .Where(k => k.KATEGORIJA_ID != Convert.ToInt32(Enums.KategorijaOstaliIzdelki.OstaliIzdelki))
                                       .OrderBy(k => k.ZAP_ST_IZPIS);

            foreach (Kategorija kategorija in kategorije)
            {
                List<Izdelek> listIzdelek = izbraniIzdelki
                                            .Where(i => i.KATEGORIJA_ID == kategorija.KATEGORIJA_ID)
                                            .ToList();

                if (!listIzdelek.Any())
                    continue;

                textToPrint = String.Format("{0}{1} <br/>", textToPrint, kategorija.NAZIV);
                foreach (Izdelek izdelek in listIzdelek)
                {
                    if (String.IsNullOrEmpty(izdelek.OPIS))
                        textToPrint = String.Format("{0}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{1} <br/>", textToPrint, izdelek.NAZIV);
                    else
                        textToPrint = String.Format("{0}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{1} -> {2} <br/>", textToPrint, izdelek.NAZIV, izdelek.OPIS);
                }
            }
        }

        private void VrniOstaleIzdelkeString(List<Izdelek> izbraniIzdelki, ref string textToPrint)
        {
            Izdelek ostaliIzdelki = izbraniIzdelki.Where(i => i.IZDELEK_ID == Convert.ToInt32(Enums.IzdelekOstaliIzdelki.OstaliIzdelki)).FirstOrDefault();

            if (ostaliIzdelki == null)
                return;

            string[] ostaliIzdelkiArray = ostaliIzdelki.OPIS.Split(new string[] { "\n" }, StringSplitOptions.None);

            textToPrint = String.Format("{0}{1} <br/>", textToPrint, lblOstaliIzdelki.Text);

            foreach (string ostaliIzdelek in ostaliIzdelkiArray)
            {
                ostaliIzdelek.Replace("\r", "").Replace("\n", "");

                if (string.IsNullOrEmpty(ostaliIzdelek))
                    continue;

                textToPrint = String.Format("{0}&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{1} <br/>", textToPrint, ostaliIzdelek);
            }
        }

        private List<Izdelek> IzbraniIzdelki()
        {
            List<Izdelek> izbraniIzdelki = new List<Izdelek>();
            int? kategorijaId = null;
            string kategorijaNaziv = string.Empty;

            foreach (RepeaterItem item in rpKategorije.Items)
            {
                foreach (Control control in item.Controls)
                {
                    if (control is HiddenField)
                    {
                        HiddenField hdfKategorijaId = item.FindControl(control.ID) as HiddenField;
                        if (hdfKategorijaId != null)
                            kategorijaId = Convert.ToInt32(hdfKategorijaId.Value);
                    }

                    if (control is Label)
                    {
                        Label lblKategorijaNaziv = item.FindControl(control.ID) as Label;
                        if (lblKategorijaNaziv != null)
                            kategorijaNaziv = lblKategorijaNaziv.Text;
                    }

                    if (control is GridView)
                    {
                        GridView gvGridView = item.FindControl(control.ID) as GridView;
                        if (gvGridView == null)
                            continue;

                        foreach (GridViewRow row in gvGridView.Rows)
                        {
                            CheckBox gvCbIzdelek = row.FindControl("gvCbIzdelek") as CheckBox;
                            if (gvCbIzdelek == null)
                                continue;

                            if (!gvCbIzdelek.Checked)
                                continue;

                            HiddenField gvHdfIzdelekId = row.FindControl("gvHdfIzdelekId") as HiddenField;
                            Label gvLblNaziv = row.FindControl("gvLblNaziv") as Label;
                            TextBox gvTbOpis = row.FindControl("gvTbOpis") as TextBox;
                            if (gvHdfIzdelekId == null || gvLblNaziv == null || gvTbOpis == null)
                                continue;

                            Izdelek izbranIzdelek = new Izdelek();
                            izbranIzdelek.IZDELEK_ID = Convert.ToInt32(gvHdfIzdelekId.Value);
                            izbranIzdelek.NAZIV = gvLblNaziv.Text;

                            if (kategorijaId.HasValue)
                                izbranIzdelek.KATEGORIJA_ID = kategorijaId.Value;

                            if (!string.IsNullOrEmpty(kategorijaNaziv))
                                izbranIzdelek.KATEGORIJA_NAZIV = kategorijaNaziv;

                            if (!string.IsNullOrEmpty(gvTbOpis.Text))
                                izbranIzdelek.OPIS = gvTbOpis.Text;

                            izbraniIzdelki.Add(izbranIzdelek);
                        }

                        kategorijaId = null;
                        kategorijaNaziv = string.Empty;
                    }
                }
            }

            OstaliIzdelki(ref izbraniIzdelki);

            return izbraniIzdelki;
        }

        private void OstaliIzdelki(ref List<Izdelek> izbraniIzdelki)
        {
            if (string.IsNullOrEmpty(tbOstaliIzdelki.Text.Trim()))
                return;

            Izdelek izbranIzdelek = new Izdelek();
            izbranIzdelek.IZDELEK_ID = Convert.ToInt32(Enums.IzdelekOstaliIzdelki.OstaliIzdelki);
            izbranIzdelek.NAZIV = "Ostalo";
            izbranIzdelek.KATEGORIJA_ID = Convert.ToInt32(Enums.KategorijaOstaliIzdelki.OstaliIzdelki);
            izbranIzdelek.KATEGORIJA_NAZIV = "Ostalo";
            izbranIzdelek.OPIS = tbOstaliIzdelki.Text;
            izbraniIzdelki.Add(izbranIzdelek);
        }

        private void NastaviIzbraneIzdelke(List<Izdelek> izbraniIzdelki)
        {
            if (!izbraniIzdelki.Any())
            {
                Session["IzbraniIzdelki"] = null;
                return;
            }

            int stNastavljenih = 0;
            foreach (RepeaterItem item in rpKategorije.Items)
            {
                foreach (Control control in item.Controls)
                {
                    if (control is GridView)
                    {
                        GridView gvGridView = item.FindControl(control.ID) as GridView;
                        if (gvGridView == null)
                            continue;

                        foreach (GridViewRow row in gvGridView.Rows)
                        {
                            CheckBox gvCbIzdelek = row.FindControl("gvCbIzdelek") as CheckBox;
                            HiddenField gvHdfIzdelekId = row.FindControl("gvHdfIzdelekId") as HiddenField;
                            TextBox gvTbOpis = row.FindControl("gvTbOpis") as TextBox;
                            if (gvCbIzdelek == null || gvHdfIzdelekId == null || gvTbOpis == null)
                                continue;

                            List<Izdelek> izdelek = izbraniIzdelki.Where(i => i.IZDELEK_ID == Convert.ToInt32(gvHdfIzdelekId.Value)).ToList();
                            if (!izdelek.Any())
                                continue;

                            gvCbIzdelek.Checked = true;
                            gvTbOpis.Text = izdelek[0].OPIS;

                            stNastavljenih++;
                            if (stNastavljenih == izbraniIzdelki.Count)
                            {
                                Session["IzbraniIzdelki"] = null;
                                return;
                            }
                        }
                    }
                }
            }

            NastaviOstaleIzdelke(izbraniIzdelki);

            Session["IzbraniIzdelki"] = null;
        }

        private void NastaviOstaleIzdelke(List<Izdelek> izbraniIzdelki)
        {
            Izdelek ostaliIzdelek = izbraniIzdelki.Where(i => i.IZDELEK_ID == Convert.ToInt32(Enums.IzdelekOstaliIzdelki.OstaliIzdelki)).FirstOrDefault();

            if (ostaliIzdelek == null)
                return;

            tbOstaliIzdelki.Text = ostaliIzdelek.OPIS;
        }

        protected void gvCbAllIzdelektGlava_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox gvCbAllIzdelek = ((CheckBox)sender) as CheckBox;
            GridView gridView = ((((CheckBox)sender).NamingContainer).NamingContainer) as GridView;
            if (gvCbAllIzdelek == null || gridView == null)
                return;

            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox gvCbIzdelek = row.FindControl("gvCbIzdelek") as CheckBox;
                if (gvCbIzdelek == null)
                    return;

                gvCbIzdelek.Checked = gvCbAllIzdelek.Checked;
            }
        }

        protected void lbSeznamPrikazi_Click(object sender, EventArgs e)
        {
            PocistiIzbraneIzdelke();

            List<SeznamIzdelek> listSeznamIzdelek = SeznamIzdelkov.VrniSeznamIzdelkov();

            List<Izdelek> listIzdelek = SeznamIzdelkov.VrniIzdelkeSeznamaIzdelkov(listSeznamIzdelek);

            NastaviIzbraneIzdelke(listIzdelek);

        }

        protected void lbSeznamPocisti_Click(object sender, EventArgs e)
        {
            PocistiIzbraneIzdelke();
        }

        protected void lbSeznamShrani_Click(object sender, EventArgs e)
        {
            SeznamIzdelkov.IzbrisiSeznamIzdelkov();

            List<Izdelek> listIzdelek = IzbraniIzdelki();

            SeznamIzdelkov.ShraniSeznamIzdelkov(listIzdelek);
        }

        private void PocistiIzbraneIzdelke()
        {
            foreach (RepeaterItem item in rpKategorije.Items)
            {
                foreach (Control control in item.Controls)
                {
                    if (control is GridView)
                    {
                        GridView gvGridView = item.FindControl(control.ID) as GridView;
                        if (gvGridView == null)
                            continue;

                        PocistiIzbraneIzdelkeHeader(gvGridView.HeaderRow);

                        PocistiIzbraneIzdelkeBody(gvGridView);

                        PocistiIzbraneOstaleIzdelke();
                    }
                }
            }
        }

        private void PocistiIzbraneIzdelkeHeader(GridViewRow row)
        {
            CheckBox gvCbAllIzdelektGlava = row.FindControl("gvCbAllIzdelektGlava") as CheckBox;
            if (gvCbAllIzdelektGlava == null)
                return;

            if (gvCbAllIzdelektGlava.Checked)
                gvCbAllIzdelektGlava.Checked = false;
        }

        private void PocistiIzbraneIzdelkeBody(GridView gvGridView)
        {
            foreach (GridViewRow row in gvGridView.Rows)
            {
                CheckBox gvCbIzdelek = row.FindControl("gvCbIzdelek") as CheckBox;
                TextBox gvTbOpis = row.FindControl("gvTbOpis") as TextBox;
                if (gvCbIzdelek == null || gvTbOpis == null)
                    continue;

                if (gvCbIzdelek.Checked)
                    gvCbIzdelek.Checked = false;

                if (!string.IsNullOrEmpty(gvTbOpis.Text))
                    gvTbOpis.Text = string.Empty;
            }
        }

        private void PocistiIzbraneOstaleIzdelke()
        {
            tbOstaliIzdelki.Text = string.Empty;
        }

        protected void cbIzdelekEmail_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbIzdelekEmail = ((CheckBox)sender) as CheckBox;
            GridViewRow gridViewRow = (((CheckBox)sender).NamingContainer) as GridViewRow;
            if (cbIzdelekEmail == null || gridViewRow == null)
                return;

            Label lbIzdelekEmail = gridViewRow.FindControl("lbIzdelekEmail") as Label;
            if (lbIzdelekEmail == null)
                return;

            string emailiZaPosiljanje = tbEmail.Text.Trim();

            if (cbIzdelekEmail.Checked)
            {
                if (string.IsNullOrEmpty(emailiZaPosiljanje))
                {
                    emailiZaPosiljanje = lbIzdelekEmail.Text.Trim();
                }
                else
                {
                    emailiZaPosiljanje = string.Format("{0};{1}", emailiZaPosiljanje, lbIzdelekEmail.Text.Trim());
                }
            }
            else
            {
                string emailPodpicje = string.Format(";{0}", lbIzdelekEmail.Text.Trim());

                if (emailiZaPosiljanje.Contains(string.Format(";{0}", lbIzdelekEmail.Text.Trim())))
                {
                    emailiZaPosiljanje = emailiZaPosiljanje.Replace(string.Format(";{0}", lbIzdelekEmail.Text.Trim()), string.Empty);
                }
                else if (emailiZaPosiljanje.Contains(string.Format("{0};", lbIzdelekEmail.Text.Trim())))
                {
                    emailiZaPosiljanje = emailiZaPosiljanje.Replace(string.Format("{0};", lbIzdelekEmail.Text.Trim()), string.Empty);
                }
                else
                {
                    emailiZaPosiljanje = emailiZaPosiljanje.Replace(lbIzdelekEmail.Text.Trim(), string.Empty);
                }
            }

            tbEmail.Text = emailiZaPosiljanje;

            mpuEmail.Show();
        }

        /// <summary>
        /// Metoda napolne objekt z parametri.
        /// </summary>
        /// <param name="vsebina"></param>
        /// <param name="vrsta">0-error, 1-opozorilo</param>
        #region FillOpozorilo
        public void FillOpozorilo(string vsebina, int vrsta, ucOpozorilo ucOpozorilo)
        {
            OpozoriloObj opozorilo = new OpozoriloObj();
            opozorilo.VSEBINA = vsebina;
            opozorilo.VRSTA = vrsta;
            ShowOpozorilo(opozorilo, ucOpozorilo);
        }
        #endregion
        /// <summary>
        /// Metoda prikaze opozorilo.
        /// </summary>
        /// <param name="opozoriloVsebina"></param>
        #region ShowOpozorilo
        public void ShowOpozorilo(OpozoriloObj opozorilo, ucOpozorilo ucOpozorilo)
        {
            ucOpozorilo.Visible = true;
            ucOpozorilo.Text = opozorilo.VSEBINA.ToString();
            ucOpozorilo.Vrsta = opozorilo.VRSTA;
        }
        #endregion

        /// <summary>
        /// Metoda skrije opozorilo.
        /// </summary>
        #region HideOpozorilo
        public void HideOpozorilo(ucOpozorilo ucOpozorilo)
        {
            ucOpozorilo.Visible = false;
            ucOpozorilo.Text = String.Empty;
        }
        #endregion
    }
}