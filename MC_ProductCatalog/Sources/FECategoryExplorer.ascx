<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FECategoryExplorer.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_FECategoryExplorer" %>
<%@ Import Namespace="Melon.Components.ProductCatalog.Configuration" %>
<script src='<%=ResolveUrl(ProductCatalogSettings.BasePath)+ "Sources/JavaScript/ProductCatalog.js"%>' type="text/javascript"></script>
<asp:HiddenField ID="hfExpandedCategories" runat="server"/> 
<div style="padding-top:20px;">
    <asp:Label ID="lblSearchByCategory" runat="server" meta:resourcekey="lblSearchByCategory" CssClass="mc_pc_title_text" />
    <asp:TreeView ID="tvCategoryExplorer" runat="server" 
        ExpandDepth="1" 
        ShowLines="true" 
        SkipLinkText="" 
        CssClass="mc_pc_feTree"
        NodeStyle-CssClass="mc_pc_node"
        SelectedNodeStyle-CssClass="mc_pc_selected_node"        
    />
</div>       
<script language="javascript" type="text/javascript">      
    var hfExpandedCategories = document.getElementById('<%= hfExpandedCategories.ClientID %>'); 
           
    //Create regular expression pattern for finding link for expand/collapse node.
    var regexExpand = /<%= tvCategoryExplorer.ClientID%>n/i;  
               
    addLoadEvent(InitCategoriesTreeView,null, null, null);
    
</script>   
