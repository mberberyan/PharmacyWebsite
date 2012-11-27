<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DynamicPropDefinition.ascx.cs" Inherits="Melon.Components.ProductCatalog.UI.CodeBehind.CodeBehind_DynamicPropDefinition" %>

<div class="left"><asp:Localize ID="locName" runat="server" meta:resourcekey="locName" /></div>
<div class="left mc_pc_prop_def_wrapper">             
    <!--[if lte IE 6.5]><div class="mc_productcatalog_select_free" id="dd3"><div class="bd" ><![endif]-->
    <asp:TextBox ID="txtDynamicProp" runat="server" 
            onchange="DropDownIndexClear('ddlPropDef');" 
            class="mc_pc_prop_def_input"/>
    <!--[if lte IE 6.5]><iframe></iframe></div></div><![endif]-->
    <asp:DropDownList ID="ddlPropDef" runat="server" 
        DataTextField="propName" 
        DataValueField="propName" 
        onchange="DropDownTextToBox(this,'txtDynamicProp');" 
        class="mc_pc_prop_def_select"
    />                         
</div>   
<div class="mc_pc_prop_def_button">
    <asp:Button ID="btnAddDynamicProp" runat="server" CssClass="mc_pc_button mc_pc_btn_61" meta:resourcekey="btnAddDynamicProp" CausesValidation="true" ValidationGroup="AddDynamicProperty" />
</div>
<div class="clear"></div>
<div>
    <asp:RequiredFieldValidator ID="rfvDynamicProp" runat="server" meta:resourcekey="rfvDynamicProp" ControlToValidate="txtDynamicProp" Display="Dynamic" ValidationGroup="AddDynamicProperty" CssClass="mc_pc_validator" />
    <asp:RequiredFieldValidator ID="rfvDdlDynamicProp" runat="server" meta:resourcekey="rfvDdlDynamicProp" ControlToValidate="ddlPropDef" Display="Dynamic" ValidationGroup="AddDynamicProperty" CssClass="mc_pc_validator" />
</div>
<div>
    <asp:RegularExpressionValidator ID="revDynamicProp" runat="server" ControlToValidate="txtDynamicProp" Display="Dynamic" SetFocusOnError="true" ValidationGroup="AddDynamicProperty"
        CssClass="mc_pc_validator" meta:resourcekey="revDynamicProp" ValidationExpression='[^,]+' />
</div>
<div class="clear"></div>
<div class="mc_pc_props_table_wrapper">
    <asp:GridView ID="gvDynamicProps" runat="server"
        AutoGenerateColumns="false"
        ShowHeader="true"
        CellPadding="2"                                        
        DataKeyNames="propId"                                             
    >
        <Columns>
            <asp:TemplateField ItemStyle-Width="80">
                <HeaderTemplate><asp:Literal ID="litPropertyName" runat="server" meta:resourcekey="litPropertyName" /></HeaderTemplate>
                <ItemTemplate>
                    <asp:Label id="lblDynamicProp" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "propName") %>' />
                </ItemTemplate> 
                <EditItemTemplate>
                    <asp:TextBox ID="txtDynamicPropEdit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "propName") %>' CssClass="mc_pc_input_extra_short"/>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvDynamicPropEdit" runat="server" meta:resourcekey="rfvDynamicPropEdit" ControlToValidate="txtDynamicPropEdit" Display="Dynamic" ValidationGroup="EditDynamicProperty" />
                    </div>
                </EditItemTemplate>                   
            </asp:TemplateField>
            <asp:TemplateField Visible="false">
                <HeaderTemplate>
                    <asp:Literal ID="litPropertyInheritedFrom" runat="server" meta:resourcekey="litPropertyInheritedFrom" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label id="lblInheritedCategory" runat="server" />
                </ItemTemplate> 
            </asp:TemplateField>                                                
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Literal ID="litPropertyActions" runat="server" meta:resourcekey="litPropertyActions" />
                </HeaderTemplate>
                <ItemTemplate>                            
                    <asp:Button ID="btnEdit" runat="server" CssClass="mc_pc_button mc_pc_btn_61" meta:resourcekey="btnEdit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "propId") %>' />
                    <asp:Button ID="btnRemove" runat="server" CssClass="mc_pc_button mc_pc_btn_61" meta:resourcekey="btnRemove" CommandName="Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "propId") %>' OnClientClick='<%# "return OnDeleteObjectClientClick(\"" + this.GetLocalResourceObject("ConfirmMessageDeletePropertyDefinition").ToString() + "\");" %>' />                        
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Button ID="btnUpdate" runat="server" CssClass="mc_pc_button mc_pc_btn_61" meta:resourcekey="btnUpdate" CommandName="UpdatePropDef" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "propId") %>' ValidationGroup="EditDynamicProperty"/>
                    <asp:Button ID="btnCancel" runat="server" CssClass="mc_pc_button mc_pc_btn_61" meta:resourcekey="btnCancel" CommandName="CancelPropDef" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "propId") %>' />
                </EditItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>        
<script type="text/javascript" language="javascript">
    var txtDynamicProp = document.getElementById('<%= txtDynamicProp.ClientID %>');
    var ddlPropDef = document.getElementById('<%= ddlPropDef.ClientID %>');
</script>