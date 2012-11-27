<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MeasurementUnit.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_MeasurementUnit" %>
<%@ Register TagPrefix="melon" TagName="Pager" Src="Pager.ascx" %>
<%@ Import Namespace="Melon.Components.ProductCatalog" %>
<%@ Import Namespace="Melon.Components.ProductCatalog.Configuration" %>
<div id="divMeasurementUnit">
    <asp:HiddenField ID="hfMeasurementUnitId" runat="server" />
        
    <table>
        <tr>
            <td>
                <!-- Pager for the grid view with product reviews-->
                <melon:Pager ID="TopPager" runat="server" CssClass="mc_pc_pager" ShowItemsDetails="false" />
            </td>
        </tr>
        <tr>
            <td>
                <!-- *** Grid with found from search users *** -->
                <asp:GridView ID="gvMeasurementUnit" runat="server" 
                    AutoGenerateColumns="False"
                    EmptyDataText='<%$Resources:NoMeasurementUnits %>'     
                    GridLines="None"                        
                    ShowHeader="true" 
                    CssClass="mc_pc_grid"            
                    HeaderStyle-CssClass="mc_pc_grid_header"
                    RowStyle-CssClass="mc_pc_grid_row" 
                    AlternatingRowStyle-CssClass="mc_pc_grid_alt_row"
                    AllowPaging="true" 
                    PagerSettings-Visible="false" 
                    EmptyDataRowStyle-BackColor="#f7f7f7"
                    AllowSorting="true"    
                    Width="350"                                                         
                    >
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="120" HeaderStyle-CssClass="mc_pc_grid_header_colFirst">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lbtnName" runat="server" Text="<%$ Resources:Name%>" CommandName="Sort" CommandArgument="Name" />
                                <asp:Image ID="imgSortDirectionAbbreviation" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"arrow_grid_up.gif"):Utilities.GetImageUrl(this.Page,"arrow_grid_down.gif") %>'
                                    ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="Name" %>' />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="120" HeaderStyle-CssClass="mc_pc_grid_header_colMiddle">
                            <HeaderTemplate>
                                <asp:LinkButton ID="lbtnSortByDescription" runat="server" Text="<%$ Resources:Description %>"
                                    CommandName="Sort" CommandArgument="Description" />            
                                <asp:Image ID="imgSortDirectionDescription" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%#(this.SortDirection == "ASC")?Utilities.GetImageUrl(this.Page,"arrow_grid_up.gif"):Utilities.GetImageUrl(this.Page,"arrow_grid_down.gif") %>'
                                    ToolTip='<%#GetLocalResourceObject(this.SortDirection)%>' Visible='<%# this.SortExpression=="Description" %>' />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'/>            
                            </ItemTemplate>
                        </asp:TemplateField>                                
                        <asp:TemplateField ItemStyle-Width="60" HeaderStyle-CssClass="mc_pc_grid_header_colLast">
                            <HeaderTemplate>
                                <asp:Localize ID="locNavigate" runat="server" meta:resourcekey="locNavigate" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" meta:resourcekey="btnEdit" CssClass="mc_pc_button mc_pc_btn_26"/>
                                <asp:Button ID="btnRemove" runat="server" meta:resourcekey="btnRemove" CssClass="mc_pc_button mc_pc_btn_26" CommandName="Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' OnClientClick='<%# "return OnDeleteObjectClientClick(\"" + this.GetLocalResourceObject("ConfirmMessageDeleteMeasurementUnit").ToString() + "\");" %>'/>                                                                
                            </ItemTemplate>                    
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>            
        </tr>
        <tr>
        <td align="right">
            <input id="btnAdd" runat="server" meta:resourcekey="btnAdd" class="mc_pc_button mc_pc_btn_61" type="button" onclick="javascript:centerMeasurementUnitPopup();loadMeasurementUnitPopup(null);" />
        </td>
    </tr>
    </table>        
</div>
<div id="popupMeasurementUnitContact">
    <a id="popupMeasurementUnitContactClose">x</a>		    
    <p id="contactMeasurementUnitArea">
        <center>
            <span id="litAddMeasurementUnitHeader" class="mc_pc_title_text"><asp:Localize ID="locAddMeasurementUnitHeader" runat="server" meta:resourcekey="locAddMeasurementUnitHeader" /></span>
            <span id="litEditMeasurementUnitHeader" class="mc_pc_title_text"><asp:Localize ID="locEditMeasurementUnitHeader"  runat="server" meta:resourcekey="locEditMeasurementUnitHeader" /></span>
        </center>
	    <table class="mc_pc_table_popup">
	        <tr>
	            <td><asp:Localize ID="locAbbreviation" runat="server" meta:resourcekey="locAbbreviation" /></td>
	            <td>
	                <asp:TextBox ID="txtPopupMeasurementUnitName" runat="server" CssClass="mc_pc_input_short"/>
	            </td>
	        </tr>
	        <tr>
	            <td></td>
	            <td>	                
	                <asp:RequiredFieldValidator ID="rfvMeasurementUnitName" runat="server" meta:resourcekey="rfvMeasurementUnitName" ControlToValidate="txtPopupMeasurementUnitName" Display="Dynamic" CssClass="mc_pc_validator" ValidationGroup="MeasurementUnit" />	                    	                
	            </td>	            
	        </tr>
	        <tr>
	            <td><asp:Localize ID="locDescription" runat="server" meta:resourcekey="locDescription" /></td>
	            <td><asp:TextBox ID="txtPopupMeasurementUnitDescription" runat="server" CssClass="mc_pc_input_short"/></td>	            
	        </tr>
	        <tr>
	            <td></td>
	            <td>
	                <asp:Button ID="btnEditMeasurementUnit" runat="server" meta:resourcekey="btnEditMeasurementUnit" CssClass="mc_pc_button mc_pc_btn_61" ValidationGroup="MeasurementUnit"/>
	                <asp:Button ID="btnAddMeasurementUnit" runat="server" meta:resourcekey="btnAddMeasurementUnit" CssClass="mc_pc_button mc_pc_btn_61" ValidationGroup="MeasurementUnit" />
	            </td>
	        </tr>	        	        	        
	    </table>
    </p>
</div>
<div id="backgroundMeasurementUnitPopup"></div>
<script type="text/javascript">
    var hfMeasurementUnitId = document.getElementById('<%= hfMeasurementUnitId.ClientID %>');
    var litAddMeasurementUnitHeader = document.getElementById('litAddMeasurementUnitHeader');
    var litEditMeasurementUnitHeader = document.getElementById('litEditMeasurementUnitHeader'); 
    var txtPopupMeasurementUnitName = document.getElementById('<%= txtPopupMeasurementUnitName.ClientID %>');
    var txtPopupMeasurementUnitDescription = document.getElementById('<%= txtPopupMeasurementUnitDescription.ClientID %>');
    var rfvMeasurementUnitName = document.getElementById('<%= rfvMeasurementUnitName.ClientID %>');
    var btnAddMeasurementUnit = document.getElementById('<%= btnAddMeasurementUnit.ClientID %>');
    var btnEditMeasurementUnit = document.getElementById('<%= btnEditMeasurementUnit.ClientID %>');    
</script>