<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FEDynamicCategoryExplorer.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_FEDynamicCategoryExplorer" %>

<!--------- Menu --------->
<div class="navigation">
    <asp:Menu ID="mCategoryMenu" runat="server" Orientation="Vertical" CssClass="sub_menu"
         StaticHoverStyle-CssClass = "selected"   
         StaticMenuItemStyle-CssClass="sub_menu_item"
         DynamicMenuItemStyle-BackColor = "#cccccc"
         DynamicHoverStyle-BackColor = "#8dc63f" 
         DynamicMenuItemStyle-ForeColor = "#000000"
         DynamicHoverStyle-CssClass = "subMenu-hover"
         DynamicMenuItemStyle-CssClass = "subMenu"
         StaticEnableDefaultPopOutImage = "false"      
         >
        <DataBindings>
            <asp:MenuItemBinding DataMember="category" />
        </DataBindings>                    
    </asp:Menu>
</div>    
