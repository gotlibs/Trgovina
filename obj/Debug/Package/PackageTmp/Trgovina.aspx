<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Trgovina.aspx.cs" Inherits="Trgovina.Trgovina" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/Opozorilo/ucOpozorilo.ascx" TagName="ucOpozorilo" TagPrefix="uc20" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/ucModalPopup.ascx" TagName="ucModalPopUp" TagPrefix="ucpopup" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" type="image/ico" href="Images/shopping_cart.png" />
    <link href="~/Styles/dizajn.css" type="text/css" rel="stylesheet" />
    <script src="~/Scripts/jquery-1.6.4.min.js" type="text/javascript"></script>
    <title></title>
</head>
<body>
    <script language="javascript" type="text/javascript">
        function CallPrint(strToPrint) {
            /*var DocumentContainer = document.getElementById('hdfToPrint');*/
            //document.getElementById('lblNaslov').innerHTML = strToPrint;
            //alert(strToPrint);
            var WindowObject = window.open('', "TrackHistoryData", "width=740,height=325,top=200,left=250,toolbars=no,scrollbars=yes,status=no,resizable=no");
            WindowObject.document.writeln(strToPrint);
            WindowObject.document.close();
            WindowObject.focus();
            WindowObject.print();
        }

        function MsgBoxOpen() {
            if (confirm("Ali resnično želite izbrisati izdelek?") == true)
                return true;
            else
                return false;
        }

        function MsgBoxOpenSeznamPocisti() {
            if (confirm("Ali resnično želite počistiti trenutno izbrane izdelke?") == true)
                return true;
            else
                return false;
        }

        function MsgBoxOpenSeznamShrani() {
            if (confirm("Ali resnično želite shraniti trenutno izbrane izdelke?") == true)
                return true;
            else
                return false;
        }
    </script>
    <form id="form1" runat="server">
    <div style="width: 1540px; padding-left: 20px;">
        <div class="naslov">
            <asp:Label ID="lblNaslov" runat="server" Text="Seznam izdelkov za nakup" />
        </div>
        <asp:HiddenField runat="server" ID="hdfToPrint" />
        <uc20:ucOpozorilo ID="ucOpozorilo1" runat="server" />
        <div runat="server" id="divDodajIzdelek" style="margin-bottom: 50px; text-align: center;">
            <asp:ImageButton runat="server" ID="ibDodajIzdelek" OnClick="lbVnosNovegaIzdelka_Click"
                ImageUrl="~/Images/add.gif" Style="float: left;" />
            <asp:LinkButton runat="server" ID="lbDodajIzdelek" CssClass="textDecNone hover" Text="Dodaj nov izdelek"
                OnClick="lbVnosNovegaIzdelka_Click" Style="float: left;"></asp:LinkButton>
            <asp:ImageButton runat="server" ID="ibSeznamPrikazi" OnClick="lbSeznamPrikazi_Click"
                ImageUrl="~/Images/seznam_prikazi.gif" Style="width: 20px; height: 20px;" />
            <asp:LinkButton runat="server" ID="lbSeznamPrikazi" CssClass="textDecNone hover"
                Text="Prikaži seznam" OnClick="lbSeznamPrikazi_Click" />
            <asp:ImageButton runat="server" ID="ibSeznamPocisti" OnClick="lbSeznamPocisti_Click"
                OnClientClick="return MsgBoxOpenSeznamPocisti()" ImageUrl="~/Images/seznam_izbrisi.png"
                Style="width: 20px; height: 20px; padding-left: 30px;" />
            <asp:LinkButton runat="server" ID="lbSeznamPocisti" CssClass="textDecNone hover"
                Text="Počisti seznam" OnClick="lbSeznamPocisti_Click" OnClientClick="return MsgBoxOpenSeznamPocisti()" />
            <asp:ImageButton runat="server" ID="ibSeznamShrani" OnClick="lbSeznamShrani_Click"
                OnClientClick="return MsgBoxOpenSeznamShrani()" ImageUrl="~/Images/seznam_shrani.gif"
                Style="width: 20px; height: 20px; padding-left: 30px;" />
            <asp:LinkButton runat="server" ID="lbSeznamShrani" CssClass="textDecNone hover" Text="Shrani seznam"
                OnClick="lbSeznamShrani_Click" OnClientClick="return MsgBoxOpenSeznamShrani()" />
            <asp:ImageButton runat="server" ID="ibPrint" ToolTip="Natisnite izdelke" OnClick="btnPrint_Click"
                ImageUrl="~/Images/printer.png" Style="width: 35px; height: 35px; float: right;" />
            <asp:ImageButton runat="server" ID="ibEmail" ToolTip="Pošlji izdelke na email" OnClick="btnEmail_Click"
                ImageUrl="~/Images/email.png" Style="width: 35px; height: 35px; float: right;" />
        </div>
        <asp:UpdatePanel runat="server" ID="upTrgovina">
            <ContentTemplate>
                <asp:ScriptManager ID="scriptMgrMain" runat="server">
                </asp:ScriptManager>
                <div runat="server" id="divSeznamIzdelkov">
                    <%--            <table>
                <tr>--%>
                    <ul>
                        <div style="margin-bottom: 10px; padding-left: 10px;">
                            <asp:Label runat="server" Style="font-size: 14px; font-weight: bold;" ID="lblOstaliIzdelki"
                                Text="Ostali izdelki" /><br />
                            <asp:TextBox runat="server" ID="tbOstaliIzdelki" TextMode="MultiLine" Rows="3" Columns="60"></asp:TextBox>
                        </div>
                <asp:Repeater ID="rpKategorije" runat="server" OnItemCommand="rpKategorije_ItemCommand">
                    <ItemTemplate>
                        <%--                    <td>--%>
                        <li style="width: 500px; margin-bottom: 10px; padding-left: 10px; vertical-align: top;
                            display: inline-block;">
                            <div>
                                <asp:HiddenField runat="server" ID="hdfKategorijaId" Value='<%# Eval("KATEGORIJA_ID") %>' />
                                <div style="margin-bottom: 10px;">
                                    <asp:Label runat="server" Style="font-size: 14px; font-weight: bold;" ID="lblKategorijaNaziv"
                                        Text='<%# Eval("NAZIV") %>' />
                                </div>
                                <asp:GridView runat="server" ID="gvPregledIzdelkov" GridLines="None" AutoGenerateColumns="False"
                                    CssClass="grid" AllowSorting="True" CellPadding="3" OnRowCommand="gvPregledIzdelkov_RowCommand"
                                    DataKeyNames="IZDELEK_ID,NAZIV,POGOSTOST,DATUM_VNOSA,KATEGORIJA_ID,KATEGORIJA_NAZIV"
                                    PagerSettings-Position="Bottom" PagerStyle-CssClass="gridPager" AllowPaging="True"
                                    PageSize="15">
                                    <AlternatingRowStyle CssClass="gridViewAlternatingRowStyle"></AlternatingRowStyle>
                                    <HeaderStyle CssClass="gvHeader" />
                                    <EmptyDataTemplate>
                                        <asp:Label runat="server" ID="lblNiPodatkov" Text="Ni podatkov za prikaz." />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="gvCbAllIzdelektGlava" runat="server" AutoPostBack="true" OnCheckedChanged="gvCbAllIzdelektGlava_CheckedChanged"
                                                    Style="padding-left: 3px;" ToolTip="Izberi vse" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="gvHdfIzdelekId" Value='<%# Eval("IZDELEK_ID") %>' />
                                                <asp:CheckBox runat="server" ID="gvCbIzdelek" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="20" />
                                            <HeaderStyle HorizontalAlign="Center" Width="20" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="gvLbZapStGlava" runat="server" Text="Zap. st." CssClass="textDecNone colorBlack hover"></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="gvLblZapSt" runat="server" Style="text-decoration: none; color: Black;"
                                                    Text='<%# Eval("ZAP_ST") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="50" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="gvLbNazivGlava" runat="server" Text="Naziv" CssClass="textDecNone colorBlack hover"></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="gvLblNaziv" runat="server" Style="text-decoration: none; color: Black;"
                                                    Text='<%# Eval("NAZIV") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="100" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="gvLbPogostostGlava" runat="server" Text="Pogostost up." CssClass="textDecNone colorBlack hover"></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="gvLblPogostost" runat="server" Style="text-decoration: none; color: Black;"
                                                    Text='<%# Trgovina.Objects.Pogostost.VrniPogostost(Eval("POGOSTOST").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="80" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="gvLbOpisGlava" runat="server" Text="Opis" CssClass="textDecNone colorBlack hover"></asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="gvTbOpis" runat="server" Style="text-decoration: none; color: Black;"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibUredi" runat="server" CommandName="uredi" ToolTip="Uredi izdelek"
                                                    CommandArgument='<%# Eval("izdelek_id") %>' ImageUrl="~/Images/edit.gif" />
                                                <asp:ImageButton ID="ibIzbrisi" runat="server" OnClientClick="return MsgBoxOpen()"
                                                    CommandName="izbrisi" ToolTip="Izbriši izdelek" CommandArgument='<%# Eval("izdelek_id") %>'
                                                    ImageUrl="~/Images/delete.gif" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" Width="40px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </li>
                        <%--                            </td>--%>
                    </ItemTemplate>
                </asp:Repeater>
                </ul>
                <%--                </tr>
            </table>--%>
                </div>
                <div id="divPopup">
                    <ucpopup:ucModalPopUp ID="mpuIzdelekPopup" runat="server" DivCssClass="popupMain width920 maxHeight570">
                        <ContentTemplate>
                            <asp:HiddenField runat="server" ID="puHdfIzdelekId" />
                            <asp:HiddenField runat="server" ID="puHdfPageMode" />
                            <div class="naslovPopup">
                                <asp:Label ID="lblNaslovPopup" runat="server" />
                            </div>
                            <div>
                                <uc20:ucOpozorilo ID="ucOpozoriloPU" runat="server" />
                            </div>
                            <div style="margin-bottom: 30px;">
                                <asp:Label runat="server" ID="lblZvezdica" Style="font-style: italic;" Text="Zvezdica (*) označuje obvezna polja."></asp:Label></div>
                            <div style="margin-top: 10px;">
                                <table cellspacing="10">
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="puLblNaziv" Text="Naziv *" Style="float: right;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="puTbNaziv" CssClass="width200 margL10"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="puRfvNaziv" runat="server" ValidationGroup="popup"
                                                ForeColor="Red" ErrorMessage="*Naziv je obvezno polje." ControlToValidate="puTbNaziv"
                                                Display="Dynamic" />
                                            <%--                                            <asp:RegularExpressionValidator ID="revZnesek" runat="server" ErrorMessage="*Vnesti morate samo številke."
                                                ControlToValidate="tbZnesek" ValidationExpression="^[0-9]*$" ForeColor="Red"
                                                Display="Dynamic" ValidationGroup="popup" />--%>
                                            <asp:Label runat="server" ID="puLblNazivError" ForeColor="Red" Text="*Napačen vnos."></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="puLblKategorija" Text="Kategorija *" Style="float: right;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="puDdlKategorija" DataValueField="kategorija_id"
                                                DataTextField="naziv" DataSourceID="puOdsKategorija" CssClass="width200 margL10">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="puOdsKategorija" runat="server" SelectMethod="VrniKategorije"
                                                TypeName="Trgovina.Objects.Kategorije"></asp:ObjectDataSource>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="puLblPogostost" Text="Pogostost *" Style="float: right;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="puDdlPogostost" DataValueField="Key" DataTextField="Value"
                                                DataSourceID="puOdsKPogostost" CssClass="width200 margL10">
                                            </asp:DropDownList>
                                            <asp:ObjectDataSource ID="puOdsKPogostost" runat="server" SelectMethod="VrniPogostosti"
                                                TypeName="Trgovina.Objects.Pogostost"></asp:ObjectDataSource>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="popupToolbar" style="margin-top: 10px;">
                                <asp:LinkButton runat="server" ID="lbPotrdi" Text="Shrani" CssClass="btn btnSave"
                                    CommandArgument="shrani" OnCommand="lbtnShrani_Command" ValidationGroup="popup" />
                                <asp:LinkButton runat="server" ID="lbPreklici" Text="Prekliči" CssClass="btn btnCancel"
                                    CommandArgument="preklici" OnCommand="lbtnShrani_Command" />
                                <div class="clear">
                                </div>
                            </div>
                        </ContentTemplate>
                    </ucpopup:ucModalPopUp>
                </div>
                <div id="divEmail">
                    <ucpopup:ucModalPopUp ID="mpuEmail" runat="server" DivCssClass="popupMain width920 maxHeight570">
                        <ContentTemplate>
                            <div class="naslovPopup">
                                <asp:Label ID="lblEmailNaslov" Text="Pošiljanje elektronske pošte izdelkov za nakup"
                                    runat="server" />
                            </div>
                            <div>
                                <uc20:ucOpozorilo ID="ucOpozoriloPopUpEmail" runat="server" />
                            </div>
                            <div style="margin-bottom: 30px;">
                                <asp:Label runat="server" ID="lblZvezdicaEmail" Style="font-style: italic;" Text="Zvezdica (*) označuje obvezna polja."></asp:Label></div>
                            <div style="margin-top: 10px;">
                                <div>
                                    <asp:GridView runat="server" ID="gvUporabnikiEmaili" GridLines="None" AutoGenerateColumns="False"
                                        CssClass="grid" CellPadding="3" ShowHeader="false" PagerSettings-Position="Bottom"
                                        PagerStyle-CssClass="gridPager" AllowPaging="True" PageSize="15" Width="100px">
                                        <EmptyDataTemplate>
                                            <asp:Label runat="server" ID="lblNiPodatkov" Text="Ni podatkov za prikaz." />
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HiddenField runat="server" ID="cbHdfIzdelekEmail" Value='<%# Eval("Key") %>' />
                                                    <asp:CheckBox runat="server" ID="cbIzdelekEmail" AutoPostBack="true" OnCheckedChanged="cbIzdelekEmail_CheckedChanged"
                                                        ToolTip="Izberi email" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="20" />
                                                <HeaderStyle HorizontalAlign="Center" Width="20" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lbIzdelekEmail" Text='<%# Eval("Value") %>' Style="text-decoration: none;
                                                        color: Black; text-align: left;" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="80" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <table cellspacing="10">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblEmail" Text="Elektronska pošta *" Style="float: right;" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tbEmail" runat="server" Text='<%# Bind("email") %>' Style="width: 600px;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="*Vpišite email!"
                                                ControlToValidate="tbEmail" ValidationGroup="popupEmail" ForeColor="Red" Display="Dynamic" />
                                            <%--<asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="tbEmail"
                                                ValidationGroup="popupEmail" ErrorMessage="*Napačen email!" ForeColor="Red" Display="Dynamic"
                                                ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" />--%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="popupToolbar" style="margin-top: 10px;">
                                <asp:LinkButton runat="server" ID="LinkButton1" Text="Pošlji" CssClass="btn btnSave"
                                    CommandArgument="poslji" OnCommand="lbtnEmailPoslji_Command" ValidationGroup="popupEmail" />
                                <asp:LinkButton runat="server" ID="LinkButton2" Text="Prekliči" CssClass="btn btnCancel"
                                    CommandArgument="preklici" OnCommand="lbtnEmailPoslji_Command" />
                                <div class="clear">
                                </div>
                            </div>
                        </ContentTemplate>
                    </ucpopup:ucModalPopUp>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
