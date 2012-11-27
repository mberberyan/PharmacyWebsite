
/*------------------------------------------------ Explorer.ascx javascript----------------------------------------------*/

/*
Date: 04/07/2008
Description: Display confirm dialog to user if filter is selected
that nodes which are not visible could be published.   
*/
function OnRecursivePublishClientClick(warningMessage)
{
    if (ddlFilter!= null && ddlFilter.value != "")
    {
        return confirm(warningMessage);
    }
    else
    {
        return true;
    }
}
 
/*
Date: 02/07/2008
Description: Display confirm dialog to user whether to delete node.    
*/
function OnDeleteNodeClientClick(confirmMessage)
{
    return confirm(confirmMessage);
}

/*
Date: 03/07/2008
Description: Update hidden field hfExpandedNodes where are stores ids of currently expanded nodes.
*/
function OnNodeExpandCollapse(link)
{
    //Get id of node which is collapsed/expanded. 
    var nodeId;

    var linkId = link.id;
    var nodeLinkId = linkId.substring(0, linkId.lastIndexOf("n"))+ "t" + linkId.substring(linkId.lastIndexOf("n") + 1,linkId.length);
    var nodeLink = document.getElementById(nodeLinkId);
    
    var regexNodeValuePath= /javascript:__doPostBack\(\'(.*?)\',\'s\\\\(.*?)\'\)/i;
    var match = nodeLink.href.match(regexNodeValuePath);
    if (match != null)
    {
        var arrIds = match[2].split('\\\\');
        nodeId = arrIds[arrIds.length-1];
    }
    else
    {
       //node "Explorer" is expanded/collapsed;
       nodeId = 'null';
    }
          
    var strExpandedNodeIds = hfExpandedNodes.value;
    if (strExpandedNodeIds.match(new RegExp(","+nodeId+","))!= null)
    {
        //The node is now collapsed = > so remove it from expanded nodes.
        hfExpandedNodes.value = strExpandedNodeIds.replace(strExpandedNodeIds.match(new RegExp(","+nodeId+",")),",");
    }
    else
    {
        //The node is now expanded => so add it to expanded nodes.
        hfExpandedNodes.value += nodeId + ','; 
    }
   
}

/*
Date: 27/06/2008
Description:  Attach to every expande/collapse link onclick event - OnNodeExpandCollapse(link).
*/
function InitNodesTreeView() 
{
    var treeNodes = document.body.getElementsByTagName('a');
    for (i = 0; i < treeNodes.length; i++) 
    {
        //Search the node. 
        var searchExpandPosition = treeNodes[i].id.search(regexExpand);   
        
        //Attach event to expand/collapse link.
        if (searchExpandPosition >= 0) 
        {
            var anchorExpand = document.getElementById(treeNodes[i].id);
            var clickExpandEventHandler = anchorExpand.onclick;
            anchorExpand.onclick = function(){OnNodeExpandCollapse(this);if (!!clickExpandEventHandler)clickExpandEventHandler;};
        }     
    }
}

/*------------------------------------------------ WorkArea.ascx javascript----------------------------------------------*/

/*
Date: 27/06/2008
Description:  Set tab "Content" as selected tab.
*/
function SelectContentTab()
{
    tabSettings.className='mc_cms_tab_unselected';
    tabContent.className='mc_cms_tab_selected'; 
    hfSelectedTab.value = 'tabContent';

    divSettings.style.display='none';
    divContent.style.display='block';
} 

/*
Date: 27/06/2008
Description:  Set tab "Settings" as selected tab.
*/
function SelectSettingsTab()
{
     tabSettings.className='mc_cms_tab_selected';
     tabContent.className='mc_cms_tab_unselected'; 
     hfSelectedTab.value = 'tabSettings';

     divSettings.style.display='block';
     divContent.style.display='none';
} 

/*
Date: 27/06/2008
Description:  Check whether the url of the frame where content is is displayed is not changes.
If it is changed then submit in order to select the appropriate node int the CMS tree. 
*/
function CheckFrameUrl(pageParams)
{
    var newUrl = iFramePageContent.contentWindow.document.location;
    if ((newUrl != 'about:blank')&&((newUrl.search.indexOf(pageParams) == -1)||(newUrl.search.indexOf('error=pageNotFound'))!= -1))
    {
        clearTimeout(timer_CheckFrameUrl);
        hfNewFrameUrl.value = newUrl;
        btnLoadPage.click();
    }
    else
    {
        timer_CheckFrameUrl = setTimeout("CheckFrameUrl('" + pageParams + "')",100);
    }
}

/*
Date: 27/06/2008
Description:  Stores in hidden field hfPageContent content of cms page entered in all editors.
*/
function GetPageContent()
{        
    var ready = false;
    var frame = iFramePageContent.contentWindow;
    
    while(!ready)
    {
        if (typeof(frame.FCKeditorAPI )!= 'undefined')
        {
            ready = true;
            for (var i = 0; i < contentPlaceholders.length; i++)
            {
                var editorName = "editorID_" + contentPlaceholders[i];
                if (frame.FCKeditorAPI.GetInstance(editorName) != undefined)
                {
                    ready = ready && true;
                }
                else
                {
                    ready = ready && false;
                }
            }
        }
        else {
            return;
        }
    }
    var xml = GetContentPlaceholdersXML(); 
    hfPageContent.value = xml;
}

/*
Date: 27/06/2008
Description:  Returns xml string with content from all editors displayed in the template's placeholders of cms page.
*/
function GetContentPlaceholdersXML()
{
    //contentPlaceholers - this variable is initialized in user control "SettingsBoxTemplate.ascx".
    var frame = iFramePageContent.contentWindow;
    var pageContent = "&lt;PageContent&gt;";
    for (var i = 0; i < contentPlaceholders.length; i++)
    {
         pageContent += "&lt;ContentPlaceholder ID='" + contentPlaceholders[i] + "' InnerHtml='" + frame.FCKeditorAPI.GetInstance('editorID_' + contentPlaceholders[i]).GetHTML().replace(/&/g,"&amp;").replace(/</g,"&lt;").replace(/>/g,"&gt;").replace(/'/g,"&apos;") + "' &gt;&lt;/ContentPlaceholder&gt;";
    }
    pageContent += "&lt;/PageContent&gt;";
    return pageContent;
}

/*
Date: 27/06/2008
Description: Clears input from file upload control. 
*/  
function ClearFileUploadControl(validatorControl, fileUploadControl)
{
    if (!!validatorControl && !!fileUploadControl)
    {
        if(!validatorControl.isvalid)
        {
            try
            {
                var parentEl = fileUploadControl.parentNode;
                parentEl.removeChild(fileUploadControl);

                var el = document.createElement("input");
                el.setAttribute('name', 'tempImg');
                el.setAttribute('id', fileUploadControl.id);
                el.setAttribute('type', 'file');
                el.setAttribute('class', 'mc_cms_input_file');
                el.setAttribute('size', '65');
               
                parentEl.appendChild(el);
            }
            catch(e)
            {
            }
        }
    }
}

                                    
/*------------------------------------------------ SettingsBoxGeneral.ascx javascript----------------------------------------------*/

/*
Date: 11/03/2008
Description: In case target "Frame" is selected in RadioButtonList rdolTarget, textbox for entering frame name is enabled.
Params: rdolTargetID - ClientID of RadioButtonList rdolTarget
*/
function SelectTargetOption(rdolTargetID)
{  
    if (document.getElementById(rdolTargetID  + "_2").checked)
    {   
        //"(Frame)" is selected.
        try
        {
            txtFrameName.disabled = false;
            txtFrameName.focus();
        }
        catch (err)
        {
            //There is error when tab "Content" is visible and tab "Settings" is not visible.
            //Then the element is not visible and thus couldn't be focused.
        }
        ValidatorEnable(rfvFrameName, true);
        ValidatorEnable(revFrameName, true);
    }
    else
    {
        txtFrameName.value = "";
        txtFrameName.disabled = true;
        ValidatorEnable(rfvFrameName, false);
        ValidatorEnable(revFrameName, false);
    }
}

/*------------------------------------------------ SettingsBoxPermissions.ascx javascript------------------------------------------*/

/*
Date: 11/03/2008
Description: Stores in HiddenField hfAccessibleFor the selected accessibility permissions from DropDown lstAccessibleFor 
separated with commas.
*/
function GetAccessibilityPermissions()
{
    if ((typeof(lstAccessibleFor) != "undefined") && (typeof(hfAccessibleFor)!= "undefined"))
    {
        hfAccessibleFor.value='';
        if (lstAccessibleFor != null) 
        {
            for(var i=0; i < lstAccessibleFor.options.length; i++)
            {
                if (lstAccessibleFor.options[i].selected)
                {
                    if (hfAccessibleFor.value == '')
                    {
                        hfAccessibleFor.value += lstAccessibleFor.options[i].value;
                    } 
                    else
                    {
                        hfAccessibleFor.value += ',' + lstAccessibleFor.options[i].value;
                    }
                }
            }
        } 
    }
}

/*
Date: 20/03/2008
Description:  Validate the selections for visibility.
    Multiple selection is allowed only for roles.
    This function is used from custom validator: cvVisibleFor.
Params: source - Span element in which is rendered the custom control.
        args - validate arguments: Value and IsValid.
*/
function ValidateVisibilitySelections(source, args)
{
    if (!IsValidPermissionSelection(lstVisibleFor))
    {
         args.IsValid = false;
         return;
    }
    args.IsValid = true;
}

/*
Date: 20/03/2008
Description:  Validate the selections for accessibility.
    Multiple selection is allowed only for roles.
    This function is used from custom validator: cvAccessibleFor.
Params: source - Span element in which is rendered the custom control.
        args - validate arguments: Value and IsValid.
*/
function ValidateAccessibilitySelections(source, args)
{
    if (!IsValidPermissionSelection(lstAccessibleFor))
    {
         args.IsValid = false;
         return;
    }
    args.IsValid = true;
}

function IsValidPermissionSelection(listbox)
{
    var optionAll = FindOptionByValue(listbox,codeAll)
    if (optionAll != null && optionAll.selected)
    {
        if (IsAnotherOptionSelected(listbox, optionAll))
        { 
            return false;
        } 
    }

    var optionAnonymousUsersOnly = FindOptionByValue(listbox,codeAnonymousUsersOnly)
    if (optionAnonymousUsersOnly != null && optionAnonymousUsersOnly.selected)
    {
        if (IsAnotherOptionSelected(listbox, optionAnonymousUsersOnly))
        { 
            return false;
        } 
    }
    
    var optionLoggedUsersOnly = FindOptionByValue(listbox,codeLoggedUsersOnly)
    if (optionLoggedUsersOnly != null && optionLoggedUsersOnly.selected)
    {
        if (IsAnotherOptionSelected(listbox, optionLoggedUsersOnly))
        { 
            return false;
        } 
    }
 
    return true;
}

function FindOptionByValue(listbox,option)
{
    for (i=0; i<listbox.options.length; i++)
    {
        if ((listbox.options[i].value == option))
        {
             return listbox.options[i];
        }
    }
    return null;
}

function IsAnotherOptionSelected(listbox,selectedOption)
{
    for (var i=0; i<listbox.options.length; i++)
    {
        if ((listbox.options[i].selected) && (listbox.options[i]!= selectedOption))
        {
             return true;
        }
    }
    return false;
}

function OnSelectVisibilityOption(selectedOption)
{
    if (selectedOption.value != "")
    {
        if (lstAccessibleFor != null)
        {  
             LoadAccessibilityOptions(selectedOption.value);
             lstAccessibleFor.options[0].selected=true;
        }
    }
}

function LoadAccessibilityOptions(visibilityOption)
{
    //Clear everything from ListBox Accessible for.
    lstAccessibleFor.innerHTML="";
    
    //Init system options. 
    var optionAll = document.createElement("OPTION");
    optionAll.text = textAll;
    optionAll.value = codeAll;
    
    var optionAnonymousUsersOnly = document.createElement("OPTION");
    optionAnonymousUsersOnly.text = textAnonymousUsersOnly;
    optionAnonymousUsersOnly.value = codeAnonymousUsersOnly;
    
    var optionLoggedUsersOnly = document.createElement("OPTION");
    optionLoggedUsersOnly.text = textLoggedUsersOnly;
    optionLoggedUsersOnly.value = codeLoggedUsersOnly;

    switch (visibilityOption)
    {
        case codeAll:
            lstAccessibleFor.options.add(optionAll,0);
            lstAccessibleFor.options.add(optionLoggedUsersOnly,1);
            //Add roles
            AddRolesOptions(lstAccessibleFor,"all");
            break;
        case codeAnonymousUsersOnly:
            lstAccessibleFor.options.add(optionAnonymousUsersOnly,0);
            break;
        case codeLoggedUsersOnly:
            lstAccessibleFor.options.add(optionLoggedUsersOnly,1);
            break;   
        default:
            //Add roles selected in visibility listbox. 
            AddRolesOptions(lstAccessibleFor, "visibilityRoles");
            break;        
     } 
}

function AddRolesOptions(listbox,filter)
{
    var roles = GetAllRoles();
        
    //Insert roles in the listbox.   
    for (var i=0; i<roles.length; i++)
    {
        var roleOption = document.createElement("OPTION");
        roleOption.value = roles[i][0];  
        roleOption.text = roles[i][1];
       
        if (filter=="visibilityRoles")
        {
            //Filter roles.
            var visibilityOption = FindOptionByValue(lstVisibleFor,roleOption.value)
            if (visibilityOption != null && visibilityOption.selected)
            {
                listbox.options.add(roleOption,listbox.options.length);    
            }
        }
        else
        {
            listbox.options.add(roleOption,listbox.options.length);      
        }
    }
}

/*------------------------------------------------ SettingsBoxStaticPage.ascx javascript-----------------------------------*/

/*
Date: 04/07/2008
Description: Change styles of selected row in repeater with local pages. 
             The repeater is used to simulate listbox because of listbox problem with horizontal scroll.
*/
function SelectLocalPage(item)
{
    if (selectedLocalPageDiv != null)
        selectedLocalPageDiv.parentNode.className="mc_cms_listbox_item";
    item.parentNode.className = "mc_cms_listbox_selectedItem";
    selectedLocalPageDiv = item;
    txtLocalPage.value=item.id;
    ValidatorEnable(rfvLocalPage,false);
}        

/*
Date: 04/07/2008
Description: Change styles of selected row in repeater with menu pages. 
             The repeater is used to simulate listbox because of listbox problem with horizontal scroll.
*/    
function SelectMenuPage(item)
{
    if (selectedMenuPageDiv != null)
        selectedMenuPageDiv.parentNode.className="mc_cms_listbox_item";
    item.parentNode.className = "mc_cms_listbox_selectedItem";
    selectedMenuPageDiv = item;
    txtMenuPage.value=item.id;
    ValidatorEnable(rfvMenuPage,false);
} 

/*------------------------------------------------ UserList.ascx javascript--------------------------------------------------------*/

/*
Date: 20/02/2008
Description: Reset search criteria as they were before any user input.
*/
function ResetSearchCriteria()
{
    //Clear input controls. 
	txtUserFirstName.value="";
	txtUserLastName.value="";
    txtUserName.value="";
	txtUserEmail.value="";
	
	chkSuperAdministrator.checked = true; //Super Administrator 
	chkAdministrator.checked = true; //Administrator
	chkWriter.checked = true; //Writer
	chkNone.checked = false; //None
}

/*------------------------------------------------ TemplateList.ascx javascript----------------------------------------------------*/

/*
Date: 27/06/2008
Description: Function called on delete template button to clear template settings (because of dangerous input) from user control TemplateAddEdit.ascx
             (if it is loaded) and display confirm message.
*/
function OnTemplateDeleteClientClick(confirmMessage)
{
    return confirm(confirmMessage);
}

/*------------------------------------------------ TemplateAddEdit.ascx javascript-------------------------------------------------*/

/*
Date: 27/07/2008
Description: Change styles of selected row in repeater with master pages. 
            The repeater is used to simulate listbox because of listbox problem with horizontal scroll. 
*/
function SelectMasterPage(item)
{
    if (revName.isvalid)
    {
        if (selectedMasterPageDiv != null)
            selectedMasterPageDiv.parentNode.className="mc_cms_listbox_item";
        item.parentNode.className = "mc_cms_listbox_selectedItem";
        selectedMasterPageDiv = item;
        txtMasterPage.value=item.id;
        ValidatorEnable(rfvMasterPage,false);
        btnSelectMasterPage.click();
    }
}
