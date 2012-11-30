﻿<%@ Page Language="C#" Title="<%$ Resources: MC_CMSResources, test_metatitle %>" MasterPageFile="~/MPBase.master" Theme="Default" Inherits="BusinessLogic.BaseWebPage" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.IO" %>



<script runat="server">
	protected void Page_Load(object sender, EventArgs e)
	{
		// *** Set meta tags ***
        HtmlHead head = (HtmlHead)Page.Header;
		string metaTagKeywords = Convert.ToString(GetGlobalResourceObject("MC_CMSResources","test_keywords"));
		string metaTagDescription = Convert.ToString(GetGlobalResourceObject("MC_CMSResources","test_description"));
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