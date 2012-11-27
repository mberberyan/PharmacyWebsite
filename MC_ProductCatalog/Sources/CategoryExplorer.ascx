<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CategoryExplorer.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_CategoryExplorer"  %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<div class="mc_pc_explorer">
    <!-- Product Catalog Category Explorer Actions -->
    <div id="divActions" runat="server" class="mc_pc_explorer_actions left"> 
        <div class="left">
            <asp:ImageButton ID="btnCreateCategory" runat="server" meta:resourcekey="btnCreateCategory" CausesValidation="false" />
            <asp:ImageButton ID="btnCreateProduct" runat="server" meta:resourcekey="btnCreateProduct" CausesValidation="false" />
            <asp:ImageButton ID="btnListProducts" runat="server" meta:resourcekey="btnListProducts" CausesValidation="false" />
            <asp:ImageButton ID="btnDeleteCategory" runat="server" meta:resourcekey="btnDeleteCategory" CausesValidation="false" OnClientClick='<%# "return OnDeleteObjectClientClick(\"" + this.GetLocalResourceObject("ConfirmMessageDeleteCategory").ToString() + "\");" %>'/>    
        </div>
        
        <!-- Actions Delimiter -->
        <div class="mc_pc_action_delimiter"></div>
        
        <!-- Move Actions, Delete Action -->
        <div  class="mc_pc_explorer_actions_move">
            <asp:ImageButton ID="btnMoveUp" runat="server" CausesValidation="false" meta:resourcekey="btnMoveUp" />
            <asp:ImageButton ID="btnMoveDown" runat="server" CausesValidation="false" meta:resourcekey="btnMoveDown" />
            <asp:ImageButton ID="btnMoveLeft" runat="server" CausesValidation="false" meta:resourcekey="btnMoveLeft" OnClientClick='<%# "return OnDeleteObjectClientClick(\"" + this.GetLocalResourceObject("ConfirmMessageMoveCategory").ToString() + "\");" %>' />
            <asp:ImageButton ID="btnMoveRight" runat="server" CausesValidation="false"  meta:resourcekey="btnMoveRight" />
        </div>
        
        <div class="mc_pc_clear">&nbsp;</div>    
    </div>
    
    <!-- Product Catalog Explorer -->
    <div class="left" >
        <asp:TreeView ID="tvCategoryExplorer" runat="server" 
            ExpandDepth="1" 
            ShowLines="true" 
            SkipLinkText="" 
            CssClass="mc_pc_tree"
            NodeStyle-CssClass="mc_pc_node"
            SelectedNodeStyle-CssClass="mc_pc_selected_node"        
        />
    </div>    
    <div class="clear"></div>
</div>
<asp:HiddenField ID="hfExpandedCategories" runat="server"/> 
<script language="javascript" type="text/javascript">      
    var hfExpandedCategories = document.getElementById('<%= hfExpandedCategories.ClientID %>'); 
           
    //Create regular expression pattern for finding link for expand/collapse node.
    var regexExpand = /<%= tvCategoryExplorer.ClientID%>n/i;  
               
    addLoadEvent(InitCategoriesTreeView,null, null, null);
    
</script>   
