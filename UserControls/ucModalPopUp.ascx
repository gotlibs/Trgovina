<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucModalPopUp.ascx.cs" Inherits="Trgovina.UserControls.ucModalPopUp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:HiddenField runat="server" ID="hdfDummyTargetControl" />
<asp:Panel ID="pnlPopUp" runat="server" Style="display: none">
    <div class="popupMain sirina500" runat="server" id="divPopupMain">
        <div class="popupBody">
            <asp:PlaceHolder ID="phContentTemplate" runat="server" />
        </div>
    </div>
</asp:Panel>
<ajax:ModalPopupExtender ID="mpePopUp" runat="server" TargetControlID="hdfDummyTargetControl"
    PopupControlID="pnlPopUp" BackgroundCssClass="modalBckg" CancelControlID="hdfDummyTargetControl">
</ajax:ModalPopupExtender>