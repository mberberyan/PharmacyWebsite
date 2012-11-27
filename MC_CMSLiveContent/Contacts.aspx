<%@ Page Language="C#" Title="<%$ Resources: MC_CMSResources, Contacts_metatitle %>" MasterPageFile="~/MPInnerPage.master" Theme="Default" Inherits="BusinessLogic.BaseWebPage" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.IO" %>

<asp:Content ContentPlaceHolderID="cphPageContent" runat="server">
    <%
        string currentLanguage = Thread.CurrentThread.CurrentUICulture.Name;
        switch (currentLanguage)
        {
			case "en-US": Response.Write("<table><tbody><tr><td style=\"padding-right: 20px\" valign=\"top\" width=\"500\">Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat</td><td><h1 style=\"margin-top: 0px\" class=\"page_title\">Contacts</h1><p><b>Company Name</b></p><p>11 Harbour Str. <br />12345 London, UK</p><p>T/F: +44 123 456 777<br />T/F: +44 123 456 888</p><p>E: <a href=\"mailto:info@company.net\">info@company.net</a><br />W: <a href=\"http://www.company.net\">www.company.net</a></p></td></tr></tbody></table>");
break;

default:Response.Write("<table><tbody><tr><td style=\"padding-right: 20px\" valign=\"top\" width=\"500\">Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat</td><td><h1 style=\"margin-top: 0px\" class=\"page_title\">Contacts</h1><p><b>Company Name</b></p><p>11 Harbour Str. <br />12345 London, UK</p><p>T/F: +44 123 456 777<br />T/F: +44 123 456 888</p><p>E: <a href=\"mailto:info@company.net\">info@company.net</a><br />W: <a href=\"http://www.company.net\">www.company.net</a></p></td></tr></tbody></table>");
break;
        }
    %>    
</asp:Content>

<script runat="server">
	protected void Page_Load(object sender, EventArgs e)
	{
		// *** Set meta tags ***
        HtmlHead head = (HtmlHead)Page.Header;
		string metaTagKeywords = Convert.ToString(GetGlobalResourceObject("MC_CMSResources","Contacts_keywords"));
		string metaTagDescription = Convert.ToString(GetGlobalResourceObject("MC_CMSResources","Contacts_description"));
		string metaTagAuthor = "";
		
		if (!String.IsNullOrEmpty(metaTagKeywords))
        {
            HtmlMeta keywords = new HtmlMeta();
            keywords.Name = "keywords";
            keywords.Content = metaTagKeywords;
            head.Controls.Add(keywords);
        }

        if (!String.IsNullOrEmpty(metaTagDescription))
        {
            HtmlMeta description = new HtmlMeta();
            description.Name = "description";
            description.Content = metaTagDescription;
            head.Controls.Add(description);
        }

        if (!String.IsNullOrEmpty(metaTagAuthor))
        {
            HtmlMeta author = new HtmlMeta();
            author.Name = "author";
            author.Content = metaTagAuthor;
            head.Controls.Add(author);
        }
	}
</script>