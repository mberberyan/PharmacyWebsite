using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Melon.Components.CMS;
using Melon.Components.CMS.ComponentEngine;


/// <summary>
/// Web page where are compared current node settings and settings from live version. If there is 
/// difference between the current and live version they are marked with different color. 
/// The page is displayed in pop-up window. 
/// </summary>
/// <remarks>
/// </remarks>
public partial class Compare : CMSControl
{

    /// <summary>
    /// Initialize the user control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    /// <summary>
    /// Display current and live node settings and if there are differences 
    /// the row for the setting is colored.
    /// </summary>
    /// <author>Mario Berberyan</author>
    /// <date>07/10/2009</date>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void LoadComparePages()
    {
        if (Session["mc_cms_current_node"] != null && Session["mc_cms_live_node"] != null)
        {
            Node currentNode = (Node)Session["mc_cms_current_node"];
            Node liveNode = (Node)Session["mc_cms_live_node"];

            lblCompareScreenTitle.Text = String.Format(Convert.ToString(GetLocalResourceObject("CompareScreenTitle")), currentNode.Code);

            #region General Settings

            //Alias
            lblCurrentAlias.Text = currentNode.Alias;
            lblLiveAlias.Text = liveNode.Alias;
            if (currentNode.Alias != liveNode.Alias)
            {
                trAlias.CssClass = "mc_cms_difference";
            }

            //Title
            lblCurrentTitle.Text = currentNode.Title;
            lblLiveTitle.Text = liveNode.Title;
            if (currentNode.Title != liveNode.Title)
            {
                trTitle.CssClass = "mc_cms_difference";
            }

            //Image
            if (String.IsNullOrEmpty(currentNode.ImagePath))
            {
                imgCurrentImage.Visible = false;
            }
            else
            {
                imgCurrentImage.ImageUrl = currentNode.ImagePath;
            }
            if (String.IsNullOrEmpty(liveNode.ImagePath))
            {
                imgLiveImage.Visible = false;
            }
            else
            {
                imgLiveImage.ImageUrl = liveNode.ImagePath;
            }

            //Hover Image
            if (String.IsNullOrEmpty(currentNode.HoverImagePath))
            {
                imgCurrentHoverImage.Visible = false;
            }
            else
            {
                imgCurrentHoverImage.ImageUrl = currentNode.HoverImagePath;
            }
            if (String.IsNullOrEmpty(liveNode.HoverImagePath))
            {
                imgLiveHoverImage.Visible = false;
            }
            else
            {
                imgLiveHoverImage.ImageUrl = liveNode.HoverImagePath;
            }

            //Selected Image
            if (currentNode.Type != NodeType.Folder)
            {
                if (String.IsNullOrEmpty(currentNode.SelectedImagePath))
                {
                    imgCurrentSelectedImage.Visible = false;
                }
                else
                {
                    imgCurrentSelectedImage.ImageUrl = currentNode.SelectedImagePath;
                }
                if (String.IsNullOrEmpty(liveNode.SelectedImagePath))
                {
                    imgLiveSelectedImage.Visible = false;
                }
                else
                {
                    imgLiveSelectedImage.ImageUrl = liveNode.SelectedImagePath;
                }
            }
            else
            {
                trSelectedImage.Visible = false;
            }

            //Hide in navigation
            if (currentNode.IsHiddenInNavigation.Value)
            {
                lblCurrentHideInNavigation.Text = Convert.ToString(GetLocalResourceObject("Yes"));
            }
            else
            {
                lblCurrentHideInNavigation.Text = Convert.ToString(GetLocalResourceObject("No"));
            }
            if (liveNode.IsHiddenInNavigation.Value)
            {
                lblLiveHideInNavigation.Text = Convert.ToString(GetLocalResourceObject("Yes"));
            }
            else
            {
                lblLiveHideInNavigation.Text = Convert.ToString(GetLocalResourceObject("No"));
            }
            if (currentNode.IsHiddenInNavigation != liveNode.IsHiddenInNavigation)
            {
                trHideInNavigation.CssClass = "mc_cms_difference";
            }

            //Target
            if (currentNode.Type != NodeType.Folder)
            {
                //Targets:
                //"_blank": Load the linked document into a new blank window. This window is not named. 
                //"_self": Default. Load the linked document into the window in which the link was clicked (the active window). 
                //"frame": In this case name of frame should be specified where the document to be loaded.
                if (!String.IsNullOrEmpty(currentNode.Target))
                {
                    if (currentNode.Target == "_self")
                    {
                        lblCurrentTarget.Text = Convert.ToString(GetLocalResourceObject("TargetSelf"));
                    }
                    else if (currentNode.Target == "_blank")
                    {
                        lblCurrentTarget.Text = Convert.ToString(GetLocalResourceObject("TargetBlank"));
                    }
                    else
                    {
                        lblCurrentTarget.Text = String.Format(Convert.ToString(GetLocalResourceObject("TargetFrame")), currentNode.Target);
                    }
                }

                if (!String.IsNullOrEmpty(liveNode.Target))
                {
                    if (liveNode.Target == "_self")
                    {
                        lblLiveTarget.Text = Convert.ToString(GetLocalResourceObject("TargetSelf"));
                    }
                    else if (liveNode.Target == "_blank")
                    {
                        lblLiveTarget.Text = Convert.ToString(GetLocalResourceObject("TargetBlank"));
                    }
                    else
                    {
                        lblLiveTarget.Text = String.Format(Convert.ToString(GetLocalResourceObject("TargetFrame")), liveNode.Target);
                    }
                }
                if (currentNode.Target != liveNode.Target)
                {
                    trTarget.CssClass = "mc_cms_difference";
                }
            }
            else
            {
                trTarget.Visible = false;
            }

            #endregion

            #region Permissions Settings

            if (currentNode.Type == NodeType.StaticMenuPage)
            {
                trPermissionsTitle.Visible = false;
                trVisibilityPermissions.Visible = false;
                trAccessibilityPermissions.Visible = false;
            }
            else
            {
                SortedList<string, string> lstRoles = Role.ListAsKeyList();

                //Visibility permissions
                string[] currentVisibilityPermissions = new string[currentNode.VisibleFor.Count];
                for (int i = 0; i < currentNode.VisibleFor.Count; i++)
                {
                    switch (currentNode.VisibleFor[i].Type)
                    {
                        case PermissionOption.All:
                            currentVisibilityPermissions[i] = Convert.ToString(GetLocalResourceObject("All"));
                            break;
                        case PermissionOption.AnonymousUsersOnly:
                            currentVisibilityPermissions[i] = Convert.ToString(GetLocalResourceObject("AnonymousUsersOnly"));
                            break;
                        case PermissionOption.LoggedUsersOnly:
                            currentVisibilityPermissions[i] = Convert.ToString(GetLocalResourceObject("LoggedUsersOnly"));
                            break;
                        case PermissionOption.UsersFromRole:
                            currentVisibilityPermissions[i] = String.Format(Convert.ToString(GetLocalResourceObject("UsersFromRole")), lstRoles[currentNode.VisibleFor[i].Name]);
                            break;
                    }
                }
                lblCurrentVisibleFor.Text = String.Join(",", currentVisibilityPermissions);

                string[] liveVisibilityPermissions = new string[liveNode.VisibleFor.Count];
                for (int i = 0; i < liveNode.VisibleFor.Count; i++)
                {
                    switch (liveNode.VisibleFor[i].Type)
                    {
                        case PermissionOption.All:
                            liveVisibilityPermissions[i] = Convert.ToString(GetLocalResourceObject("All"));
                            break;
                        case PermissionOption.AnonymousUsersOnly:
                            liveVisibilityPermissions[i] = Convert.ToString(GetLocalResourceObject("AnonymousUsersOnly"));
                            break;
                        case PermissionOption.LoggedUsersOnly:
                            liveVisibilityPermissions[i] = Convert.ToString(GetLocalResourceObject("LoggedUsersOnly"));
                            break;
                        case PermissionOption.UsersFromRole:
                            liveVisibilityPermissions[i] = String.Format(Convert.ToString(GetLocalResourceObject("UsersFromRole")), lstRoles[liveNode.VisibleFor[i].Name]);
                            break;
                    }
                }
                lblLiveVisibleFor.Text = String.Join(",", liveVisibilityPermissions);

                if (lblCurrentVisibleFor.Text != lblLiveVisibleFor.Text)
                {
                    trVisibilityPermissions.CssClass = "mc_cms_difference";
                }

                //Accessibility permissions
                if (currentNode.Type == NodeType.Folder || currentNode.Type == NodeType.StaticExternalPage)
                {
                    trAccessibilityPermissions.Visible = false;
                }
                else
                {
                    string[] currentAccessibilityPermissions = new string[currentNode.AccessibleFor.Count];
                    for (int i = 0; i < currentNode.AccessibleFor.Count; i++)
                    {
                        switch (currentNode.AccessibleFor[i].Type)
                        {
                            case PermissionOption.All:
                                currentAccessibilityPermissions[i] = Convert.ToString(GetLocalResourceObject("All"));
                                break;
                            case PermissionOption.AnonymousUsersOnly:
                                currentAccessibilityPermissions[i] = Convert.ToString(GetLocalResourceObject("AnonymousUsersOnly"));
                                break;
                            case PermissionOption.LoggedUsersOnly:
                                currentAccessibilityPermissions[i] = Convert.ToString(GetLocalResourceObject("LoggedUsersOnly"));
                                break;
                            case PermissionOption.UsersFromRole:
                                currentAccessibilityPermissions[i] = lstRoles[currentNode.AccessibleFor[i].Name];
                                break;
                        }
                    }
                    lblCurrentAccessibleFor.Text = String.Join(",", currentAccessibilityPermissions);

                    string[] liveAccessibilityPermissions = new string[liveNode.AccessibleFor.Count];
                    for (int i = 0; i < liveNode.AccessibleFor.Count; i++)
                    {
                        switch (liveNode.AccessibleFor[i].Type)
                        {
                            case PermissionOption.All:
                                liveAccessibilityPermissions[i] = Convert.ToString(GetLocalResourceObject("All"));
                                break;
                            case PermissionOption.AnonymousUsersOnly:
                                liveAccessibilityPermissions[i] = Convert.ToString(GetLocalResourceObject("AnonymousUsersOnly"));
                                break;
                            case PermissionOption.LoggedUsersOnly:
                                liveAccessibilityPermissions[i] = Convert.ToString(GetLocalResourceObject("LoggedUsersOnly"));
                                break;
                            case PermissionOption.UsersFromRole:
                                liveAccessibilityPermissions[i] = lstRoles[liveNode.AccessibleFor[i].Name];
                                break;
                        }
                    }
                    lblLiveAccessibleFor.Text = String.Join(",", liveAccessibilityPermissions);

                    if (lblCurrentAccessibleFor.Text != lblLiveAccessibleFor.Text)
                    {
                        trAccessibilityPermissions.CssClass = "mc_cms_difference";
                    }
                }
            }

            #endregion

            #region Static Page Settings

            if (currentNode.Type == NodeType.StaticLocalPage
                || currentNode.Type == NodeType.StaticExternalPage
                || currentNode.Type == NodeType.StaticMenuPage)
            {
                //Display setting section "Static Page"
                if (currentNode.Type == NodeType.StaticLocalPage)
                {
                    lblUrl.Text = Convert.ToString(GetLocalResourceObject("VirtualPath"));
                    lblCurrentUrl.Text = currentNode.LocalPagePath;
                    lblLiveUrl.Text = liveNode.LocalPagePath;
                    if (currentNode.LocalPagePath != liveNode.LocalPagePath)
                    {
                        trStaticPageUrl.CssClass = "mc_cms_difference";
                    }
                }
                else if (currentNode.Type == NodeType.StaticExternalPage)
                {
                    lblUrl.Text = Convert.ToString(GetLocalResourceObject("Url"));
                    lblCurrentUrl.Text = currentNode.ExternalPageURL;
                    lblLiveUrl.Text = liveNode.ExternalPageURL;
                    if (currentNode.ExternalPageURL != liveNode.ExternalPageURL)
                    {
                        trStaticPageUrl.CssClass = "mc_cms_difference";
                    }
                }
                else
                {
                    lblUrl.Text = Convert.ToString(GetLocalResourceObject("ReferredPage"));
                    SortedList<int, string> listPages = Node.ListCandidateMenuPagesAsKeyList();
                    lblCurrentUrl.Text = Convert.ToString(GetLocalResourceObject("Explorer")) + " / " + listPages[currentNode.ReferredNodeId.Value];
                    lblLiveUrl.Text = Convert.ToString(GetLocalResourceObject("Explorer")) + " / " + listPages[liveNode.ReferredNodeId.Value];
                    if (currentNode.ReferredNodeId != liveNode.ReferredNodeId)
                    {
                        trStaticPageUrl.CssClass = "mc_cms_difference";
                    }
                }
            }
            else
            {
                trStaticPage.Visible = false;
                trStaticPageUrl.Visible = false;
            }

            #endregion

            #region Content Manageable Page Settings

            if (currentNode.Type == NodeType.ContentManageablePage)
            {
                //Template
                lblCurrentTemplateName.Text = currentNode.TemplateName;
                lblLiveTemplateName.Text = liveNode.TemplateName;
                if (currentNode.TemplateName != liveNode.TemplateName)
                {
                    trTemplateName.CssClass = "mc_cms_difference";
                }

                //Meta tags
                lblCurrentMetaTagAuthor.Text = currentNode.Author;
                lblLiveMetaTagAuthor.Text = liveNode.Author;
                if (currentNode.Author != liveNode.Author)
                {
                    trMetaTagAuthor.CssClass = "mc_cms_difference";
                }

                lblCurrentMetaTagKeywords.Text = currentNode.Keywords;
                lblLiveMetaTagKeywords.Text = liveNode.Keywords;
                if (currentNode.Keywords != liveNode.Keywords)
                {
                    trMetaTagKeywords.CssClass = "mc_cms_difference";
                }

                lblCurrentMetaTagDescription.Text = currentNode.Description;
                lblLiveMetaTagDescription.Text = liveNode.Description;
                if (currentNode.Description != liveNode.Description)
                {
                    trMetaTagDescription.CssClass = "mc_cms_difference";
                }
            }
            else
            {
                trTemplate.Visible = false;
                trTemplateName.Visible = false;
                trMetaTags.Visible = false;
                trMetaTagAuthor.Visible = false;
                trMetaTagKeywords.Visible = false;
                trMetaTagDescription.Visible = false;
            }

            #endregion

            #region Content Manageable Page Content

            if (currentNode.Type == NodeType.ContentManageablePage)
            {
                string currentContentXml = Convert.ToString(Session["mc_cms_current_content"]);
                NodeContent currentContent = new NodeContent(currentContentXml);
                NodeContent liveContent = NodeContent.LoadContent(currentNode.Id.Value, NodeVersion.Live, currentNode.LanguageCulture);

                //Get all placeholder's ids from current and live version.
                List<string> placeholders = new List<string>();
                placeholders.AddRange(currentContent.Content.Keys);
                foreach (string key in liveContent.Content.Keys)
                {
                    if (!currentContent.Content.ContainsKey(key))
                    {
                        placeholders.Add(key);
                    }
                }
                placeholders.Sort();

                foreach (string placeholder in placeholders)
                {
                    TableRow row = new TableRow();

                    TableCell cellPlaceholder = new TableCell();
                    cellPlaceholder.Text = placeholder;
                    cellPlaceholder.CssClass = "mc_cms_td_delimiter";
                    row.Cells.Add(cellPlaceholder);

                    TableCell cellCurrentContent = new TableCell();
                    if (currentContent.Content.ContainsKey(placeholder))
                    {
                        cellCurrentContent.Text = currentContent.Content[placeholder];
                    }
                    cellCurrentContent.CssClass = "mc_cms_td_delimiter";
                    row.Cells.Add(cellCurrentContent);

                    TableCell cellLiveContent = new TableCell();
                    if (liveContent.Content.ContainsKey(placeholder))
                    {
                        cellLiveContent.Text = liveContent.Content[placeholder];
                    }
                    row.Cells.Add(cellLiveContent);

                    tblContent.Rows.Add(row);
                    if (cellCurrentContent.Text != cellLiveContent.Text)
                    {
                        row.CssClass = "mc_cms_difference";
                    }
                }

            }
            else
            {
                tblContent.Visible = false;
            }

            #endregion

        }
    }
}

