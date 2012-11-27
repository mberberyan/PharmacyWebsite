/*
Description: Sets the Default Submit Button for the whole page on Enter key press.
*/
function fnTrapKD(btnId, event){

 var btn = document.getElementById(btnId);
 if (document.all){
  if (event.keyCode == 13){
   event.returnValue=false;
   event.cancel = true;
//   btn.setAttribute('autocomplete','off'); 
//    btn.focus();
//     btn.setAttribute('autocomplete','on'); 
   btn.click();
  
  }
 }
 else if (document.getElementById){
  if (event.which == 13){
   event.returnValue=false;
   event.cancel = true;
//    btn.setAttribute('autocomplete','off'); 
//     btn.focus();
//      btn.setAttribute('autocomplete','on'); 
   btn.click();
  
  }
 }
 else if(document.layers){
  if(event.which == 13){
   event.returnValue=false;
   event.cancel = true;
//   btn.setAttribute('autocomplete','off'); 
//     btn.focus();
//     btn.setAttribute('autocomplete','on'); 
   btn.click();
  }
 }
}


/***********************************************************************/
/*** ForumAddEdit.ascx scripts *****************************************/
/***********************************************************************/

/*
Description: Collect all selected forum users from listbox element passed as paramenter and return them in array.
Params:  listbox - listbox from which we get selected users.
*/
function getSelectedUsers(listbox) 
{     
    var selectedUsers = new Array();
    var j = 0;
    for (i=0; i < document.getElementById(listbox).options.length; i++) 
    {
       if (document.getElementById(listbox).options[i].selected) 
       {
          selectedUsers[j] = document.getElementById(listbox).options[i];
          j++;
       }
    }
    return selectedUsers;
}

/*
Description: Include selected forum user from source ListBox into target ListBox.
Params:  lstSelectedUsersId - destination ListBox ClientId.
         lstOtherUsersId - source ListBox ClientId.
*/
function includeIntoSelectedUsers(lstSelectedUsersId, lstOtherUsersId) 
{     
    selectedUsers = getSelectedUsers(lstOtherUsersId);
    for (i=0; i<selectedUsers.length; i++)
    {
        document.getElementById(lstSelectedUsersId).insertBefore(selectedUsers[i],null);
    }  
}

/*
Description: Exclude selected forum user from source ListBox into target ListBox.
Params:  lstSelectedUsersId - destination ListBox ClientId.
         lstOtherUsersId - source ListBox ClientId.
*/
function excludeFromSelectedUsers(lstSelectedUsersId, lstOtherUsersId) 
{
    selectedUsers = getSelectedUsers(lstSelectedUsersId);
    for (i=0; i<selectedUsers.length; i++)
    {
        document.getElementById(lstOtherUsersId).insertBefore(selectedUsers[i],null);
    }  
}

/*
Description: Set as value of hidden field: comma separated string of user names of selected users from listbox.
Params:  ListBox
         HiddenField
*/
function selectAllIncludedOptions(ListBox, HiddenField)
{
    HiddenField.value="";
    if (ListBox != null) 
    {
                for(var i=0; i < ListBox.options.length; i++)
                {
                    ListBox.options[i].selected = true;
                    if(i != ListBox.options.length - 1)
                        HiddenField.value += ListBox.options[i].value + ",";  
                    else
                        HiddenField.value += ListBox.options[i].value;
                }
    }
}

/*
Description: Causes the focus to be gained by the element with the specified Id after 1/10 of a second.
Paramaters: targetElementId - the Id of the element to receive the focus.
*/
function reFocus(targetElementId, sourceElementId)
{
	document.getElementById(sourceElementId).onfocus = null;
	setTimeout("document.getElementById('" + targetElementId +"').focus();",100);
}   

/***********************************************************************/
/*** ForumSearchCriteria.ascx scripts **********************************/
/***********************************************************************/


/*
Description: Attaches the onclick event to all checkboxes in the tree with ClientID: treeID and check all checkboxes
Params: treeID - ClientId of TreeView tvForums
*/
function AttachTreeCheckBoxEvent(treeID)
{
    var aInputs = document.getElementById(treeID).getElementsByTagName("INPUT");
    if (aInputs!=null)
    {
        for(var i=0; i< aInputs.length;i++)
        {
            if (aInputs[i].type=="checkbox") {
                aInputs[i].onclick = CheckBoxCheckEvent;
            }
        }
    }
}

/*
Description: Check all nodes in the TreeView with ClientID: treeID
Params: treeID - ClientId of TreeView tvForums
*/
function CheckAllTreeNodes(treeID)
{
    var aInputs = document.getElementById(treeID).getElementsByTagName("INPUT");
    if (aInputs!=null)
    {
        for(var i=0; i< aInputs.length;i++)
        {
            if (aInputs[i].type=="checkbox") {
                aInputs[i].checked = true;
            }
        }
    }
}

/*
Description: Check/Uncheck all children of the selected checkbox
*/
function CheckBoxCheckEvent()
{
    CheckUncheckChildren(this.parentNode.parentNode.parentNode.parentNode);
    CheckUncheckParents(this.parentNode.parentNode.parentNode.parentNode);
}

/*
Description:  Find all children of the node and check/uncheck them
Params: node - node from the tree which was clicked
*/
function CheckUncheckChildren(node)
{   
	var children = GetChildNodes(node);
	for(var i = 0; i < children.length; i++)
	{
		DoNodeCheck(children[i],IsNodeChecked(node));
	}
}

function GetChildNodes(node)
{
	var result = new Array();
	// Get the current count of div tags
	var divCount = node.getElementsByTagName("DIV").length;
	
	var tmpNode = node.nextSibling;
	
	var tmpNodeDivCount = (!!tmpNode && tmpNode.getElementsByTagName)?tmpNode.getElementsByTagName("DIV").length:-1;
	var i = 0;
	while(!!tmpNode && (tmpNodeDivCount > divCount))
	{
	
		result[i] = tmpNode;
		i++;
		tmpNode = tmpNode.nextSibling;
		tmpNodeDivCount = (!!tmpNode && tmpNode.getElementsByTagName)?tmpNode.getElementsByTagName("DIV").length:-1;
		
	}
	return result;
}

function GetDirectChildNodes(node)
{
	var result = new Array();
	// Get the current count of div tags
	var divCount = node.getElementsByTagName("DIV").length;
	var tmpNode = node.nextSibling;
	var tmpNodeDivCount = (!!tmpNode && tmpNode.getElementsByTagName)?tmpNode.getElementsByTagName("DIV").length:-1;
	var i = 0;
	while(!!tmpNode && (tmpNodeDivCount == (divCount + 1)))
	{
		result[i] = tmpNode;
		i++;
		tmpNode = tmpNode.nextSibling;
	    tmpNodeDivCount = (!!tmpNode && tmpNode.getElementsByTagName)?tmpNode.getElementsByTagName("DIV").length:-1;
	}
	return result;
}

function GetParentNode(node)
{
    if(!node.getElementsByTagName)
    {
        return null;
    }
	var result = null;
	var divCount = node.getElementsByTagName("DIV").length;
	var tmpNode = node.previousSibling;
	var tmpNodeDivCount = (!!tmpNode && tmpNode.getElementsByTagName)?tmpNode.getElementsByTagName("DIV").length:-1;
	
	while(!!tmpNode)
	{
		if(tmpNodeDivCount < divCount)
		{
			result = tmpNode;
			break;
		}
		tmpNode = tmpNode.previousSibling;
	    tmpNodeDivCount = (!!tmpNode && tmpNode.getElementsByTagName)?tmpNode.getElementsByTagName("DIV").length:-1;
	}
	return result;
}


function GetSiblingNodes(node)
{
	var result = new Array();
	// Get the current count of div tags
	var divCount = node.getElementsByTagName("DIV").length;
	var tmpNode = node.nextSibling;
	var tmpNodeDivCount = (!!tmpNode && tmpNode.getElementsByTagName)?tmpNode.getElementsByTagName("DIV").length:-1;
	var i = 0;
	while(!!tmpNode)
	{
		if(tmpNodeDivCount == divCount)
		{
			result[i] = tmpNode;
			i++;
		}
		if(tmpNodeDivCount < divCount)
		{
			break;
		}
		tmpNode = tmpNode.nextSibling;
	    tmpNodeDivCount = (!!tmpNode && tmpNode.getElementsByTagName)?tmpNode.getElementsByTagName("DIV").length:-1;
	}
	
	var tmpNode = node.previousSibling;
	var tmpNodeDivCount = (!!tmpNode && tmpNode.getElementsByTagName)?tmpNode.getElementsByTagName("DIV").length:-1;
	while(!!tmpNode)
	{
		if(tmpNodeDivCount == divCount)
		{
			result[i] = tmpNode;
			i++;
		}
		if(tmpNodeDivCount < divCount)
		{
			break;
		}
		tmpNode = tmpNode.previousSibling;
	    tmpNodeDivCount = (!!tmpNode && tmpNode.getElementsByTagName)?tmpNode.getElementsByTagName("DIV").length:-1;
	}
	result[i] = node;
	
	return result;
}

function IsNodeChecked(node)
{
	var result = false;
	if(node.getElementsByTagName)
    {
	    var childInput = node.getElementsByTagName("INPUT");
	    for(var i = 0; i < childInput.length; i++) 
        {
		    if(childInput[i].type == "checkbox")
            {
			    result = childInput[i].checked;
			    break;
            }
        }
    }
    return result;
}

function DoNodeCheck(node,how)
{
    if(node.getElementsByTagName)
    {
	    var childInput = node.getElementsByTagName("INPUT");
	    for(var i = 0; i < childInput.length; i++) 
        {
		    if(childInput[i].type == "checkbox")
            {
			    childInput[i].checked = how;
			    break;
            }
        }
    }
}

function GetNodeCollapseExpandImage(node)
{
	var children = GetChildNodes(node);
	if(children.length > 0)
	{
		var imgs = node.getElementsByTagName("IMG");
		if(imgs.length > 0)
		{
			return imgs[0];
		}
	}
	return null;
}

function CheckUncheckParents(node) 
{ 
	var parentNode = GetParentNode(node);
	if(parentNode != null)
	{
		var diff = false;
		var siblings = GetSiblingNodes(node);
		for(var i=0; i < siblings.length; i++)
		{
			if(IsNodeChecked(siblings[i]) != IsNodeChecked(node))
			{
				diff = true;
				break;
			}
		}
		DoNodeCheck(parentNode,(!diff && IsNodeChecked(node)));
        CheckUncheckParents(parentNode);
	}
	else
	{
		return;
	}

}

function CollapseExpandChildren(treeID, nodeIndex)
{
	var node = document.getElementById(treeID).getElementsByTagName("TABLE")[nodeIndex];
	var children = GetChildNodes(node);
	var currentDisplay;
	if(children.length > 0)
	{
		if(children[0].style.display == "none")
		{
			currentDisplay = "none";
			var img = GetNodeCollapseExpandImage(node);
			if(!!img)
				img.src = treeNodeExpanded.src;
		}
		else
		{
			currentDisplay = "block";
			var img = GetNodeCollapseExpandImage(node);
			if(!!img)
				img.src = treeNodeCollapsed.src;
		}
	}

	for(var i = 0; i < children.length; i++)
	{
		if(currentDisplay == "none")
		{
			children[i].style.display = "block";
			var img = GetNodeCollapseExpandImage(children[i]);
			if(!!img)
				img.src = treeNodeExpanded.src;
		}
		else
		{
			children[i].style.display = "none";
			var img = GetNodeCollapseExpandImage(children[i]);
			if(!!img)
				img.src = treeNodeCollapsed.src;
		}
	}
}


function AttachCollapse(treeID)
{
	var aNodes = document.getElementById(treeID).getElementsByTagName("TABLE");
    if (aNodes!=null)
    {
        for(var i=0; i< aNodes.length;i++)
        {
            var anchor = aNodes[i].getElementsByTagName("A");
            for(var j=0; j < anchor.length; j++)
            {
				anchor[j].href = "javascript:CollapseExpandChildren('" + treeID + "'," + i + ");";
            }
        }
    }
}

/*
Description: Based in the selected value in DropDown ddlDateInterval shows the needed input fields for dates
Params: dropDownID - ClientId of DropDown ddlDateInterval
*/
function DisplayDateCriteria(dropDownID)
{

    //"(All)" is selected
    if (document.getElementById(dropDownID).options[0].selected)
    {   
        document.getElementById("spanDateFrom").style.visibility='hidden';
        document.getElementById("spanDateTo").style.visibility='hidden';
        return;
    }
    
    //"On" is selected
    if (document.getElementById(dropDownID).options[1].selected)
    {   
        document.getElementById("spanDateFrom").style.visibility='visible';
        document.getElementById("spanDateTo").style.visibility='hidden';
        return;
    }
    
    //"Between" is selected
    if (document.getElementById(dropDownID).options[2].selected)
    {   
        document.getElementById("spanDateFrom").style.visibility='visible';
        document.getElementById("spanDateTo").style.visibility='visible';
        return;
    }
}

function CheckIsSearchLocationSpecified(treeID, lblErrorMessage)
{
    var aInputs = document.getElementById(treeID).getElementsByTagName("INPUT");
    if (aInputs!=null)
    {
        for(var i=0; i< aInputs.length;i++)
        {
            if ((aInputs[i].type=="checkbox")&& (aInputs[i].checked)) {
                document.getElementById(lblErrorMessage).style.display="none";
                return true;
            }
        }
    }
    document.getElementById(lblErrorMessage).style.display="block";
    return false;
}

/*
Description: Clears the controls in the search form
Params: The IDs of all controls to be cleared.
*/
function ClearControls(txbSearchForID,txbAuthorID,treeID,ddlDateCriteriaID,txbDateFromID,txbDateToID,ddlSortFieldID,cvDateFirstID,cvDateFormatFirstID,cvDateSecondID,cvDateFormatSecondID)
{
	var txbSearchFor = document.getElementById(txbSearchForID);
	var txbAuthor = document.getElementById(txbAuthorID);
	var ddlDateCriteria = document.getElementById(ddlDateCriteriaID);
	var txbDateFrom = document.getElementById(txbDateFromID);
	var txbDateTo = document.getElementById(txbDateToID);
	var ddlSortField = document.getElementById(ddlSortFieldID);
	var cvDateFirst = document.getElementById(cvDateFirstID);
	var cvDateFormatFirst = document.getElementById(cvDateFormatFirstID);
	var cvDateSecond = document.getElementById(cvDateSecondID);
	var cvDateFormatSecond = document.getElementById(cvDateFormatSecondID);
	var sDateFrom = document.getElementById("spanDateFrom");
	var sDateTo = document.getElementById("spanDateTo");
	
	txbSearchFor.value = "";
	txbAuthor.value = "";
	CheckAllTreeNodes(treeID);
	ddlDateCriteria.selectedIndex = 0;
	txbDateFrom.value = "";
	txbDateTo.value = "";
	sDateFrom.style.visibility = "hidden";
	sDateTo.style.visibility = "hidden";
	ddlSortField.selectedIndex = 0;
	cvDateFirst.style.display = "none";
	cvDateFormatFirst.style.display = "none";
	cvDateSecond.style.display = "none";
	cvDateFormatSecond.style.display = "none";
}

/***********************************************************************/
/*** ForumUserProfileEdit.ascx scripts **********************************/
/***********************************************************************/

//Class used to raize pop-up modal window display from page in iframe.
function UserProfileError (errorMessage) 
{
    this.errorMessage = errorMessage;
    this.show = UserProfileErrorShow;
}