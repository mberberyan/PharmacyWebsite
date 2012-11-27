<%@ Page Language="C#" Title="<%$ Resources: MC_CMSResources, Home_metatitle %>" MasterPageFile="~/MPDefault.master" Theme="Default" Inherits="BusinessLogic.BaseWebPage" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.IO" %>

<asp:Content ContentPlaceHolderID="cphPageContent" runat="server">
    <%
        string currentLanguage = Thread.CurrentThread.CurrentUICulture.Name;
        switch (currentLanguage)
        {
			case "en-US": Response.Write("<h1 class=\"page_title\">Welcome</h1><p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat</p><p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat</p><p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat</p><p>&nbsp;</p>");
break;

default:Response.Write("<h1 class=\"page_title\">Welcome</h1><p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat</p><p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat</p><p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat</p><p>&nbsp;</p>");
break;
        }
    %>    
</asp:Content>

<script runat="server">
	protected void Page_Load(object sender, EventArgs e)
	{
		// *** Set meta tags ***
        HtmlHead head = (HtmlHead)Page.Header;
		string metaTagKeywords = Convert.ToString(GetGlobalResourceObject("MC_CMSResources","Home_keywords"));
		string metaTagDescription = Convert.ToString(GetGlobalResourceObject("MC_CMSResources","Home_description"));
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