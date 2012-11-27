<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FEProductFilter.ascx.cs" Inherits="FEProductFilter" %>
<table class="gv_productFilter">
    <tr>
        <td>
            <span class="mc_pc_bold_text"><asp:Localize ID="locSizePrefix" runat="server" meta:resourcekey="locSizePrefix" /></span><asp:DropDownList ID="ddlSize" runat="server" CssClass="mc_pc_dropdown" OnSelectedIndexChanged="ddlSize_SelectedIndexChanged" AutoPostBack="true"/><span class="mc_pc_bold_text"><asp:Localize ID="locSizeSuffix" runat="server" meta:resourcekey="locSizeSuffix" /></span>
        </td>
        <td>
            <span class="mc_pc_bold_text"><asp:Localize ID="locDisplay" runat="server" meta:resourcekey="locDisplay" /></span><asp:DropDownList ID="ddlDisplay" runat="server" CssClass="mc_pc_dropdown" OnSelectedIndexChanged="ddlDisplay_SelectedIndexChanged" AutoPostBack="true"/>
        </td>
        <td>
            <span class="mc_pc_bold_text"><asp:Localize ID="locSort" runat="server" meta:resourcekey="locSort" /></span><asp:DropDownList ID="ddlSort" runat="server" CssClass="mc_pc_dropdown" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged" AutoPostBack="true"/>
        </td>
    </tr>
</table>