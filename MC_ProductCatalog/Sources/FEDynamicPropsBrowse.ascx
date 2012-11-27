<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FEDynamicPropsBrowse.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_FEDynamicPropsBrowse" %>
<div class="filter">
<h3><asp:Label ID="lblSearchByDynamicProps" runat="server" meta:resourcekey="lblSearchByDynamicProps" /></h3>
<table width="100%">
    <tr>
        <td>
            <asp:GridView ID="gvBrowseByFeature" runat="server"            
                GridLines="None"
                AutoGenerateColumns="False" 
                EmptyDataText='<%$Resources:NoFeaturesErrorMessage %>' 
                ShowHeader="false"                 
                AllowPaging="false" 
                PagerSettings-Visible="false"                 
                AllowSorting="false"                                                             
            >
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="lblDynPropName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "propName") %>' />
                    </ItemTemplate>                     
                </asp:TemplateField>            
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlValues" runat="server" />                        
                    </ItemTemplate>                     
                </asp:TemplateField>                
            </Columns>
        </asp:GridView>
        </td>
    </tr>
    <tr>
        <td align="right">
            
            <asp:Button id="btnSearch" runat="server" CssClass="shortButton" meta:resourcekey="btnSearch" />
        </td>
    </tr>
</table>
</div>    
