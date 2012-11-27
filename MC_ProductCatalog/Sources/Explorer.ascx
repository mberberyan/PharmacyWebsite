<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Explorer.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_Explorer" %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<div class="mc_pc_explorer">    
    <!-- Product Catalog Explorer Actions -->
    <div id="divActions" runat="server" class="mc_pc_explorer_actions left"> 
        <div class="left">
            <asp:ImageButton ID="btnCreateObject" runat="server" meta:resourcekey="btnCreateObject" CausesValidation="false" />            
            <asp:ImageButton ID="btnDeleteObject" runat="server" meta:resourcekey="btnDeleteObject" CausesValidation="false" OnClientClick='<%# "return OnDeleteObjectClientClick(\"" + String.Format(this.GetLocalResourceObject("ConfirmMessageDeleteCategory").ToString(), SelectedObjectType.ToString()) + "\");" %>'/>    
        </div>
        
        <!-- Actions Delimiter -->
        <div class="mc_pc_action_delimiter"></div>
        
        <!-- Move Actions, Delete Action -->
        <div  class="mc_pc_explorer_actions_move">
            <asp:ImageButton ID="btnMoveUp" runat="server" meta:resourcekey="btnMoveUp" CausesValidation="false" />
            <asp:ImageButton ID="btnMoveDown" runat="server" meta:resourcekey="btnMoveDown" CausesValidation="false" />            
        </div>        
        <div class="mc_pc_clear">&nbsp;</div>    
    </div>
    
    <!-- Product Catalog Explorer -->
    <div class="left">
        <asp:TreeView ID="tvObjectExplorer" runat="server" 
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