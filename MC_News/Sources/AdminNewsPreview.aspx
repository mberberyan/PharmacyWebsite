<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminNewsPreview.aspx.cs" Inherits="Melon.Components.News.UI.CodeBehind.AdminNewsPreview" %>
<%@ Register TagPrefix="melon" TagName="NewsPreview" Src="NewsDetails.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link  type="text/css" rel="stylesheet" rev="stylesheet" href="NewsStyles/FrontEndStyles.css" />
</head>
<body >
    <form id="form1" runat="server">
    <div>
        <melon:NewsPreview ID="cntrlNewsPreview" runat="server"/>
    </div>
    </form>
</body>
</html>
