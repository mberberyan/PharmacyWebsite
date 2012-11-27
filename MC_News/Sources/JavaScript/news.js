/*
Description: Display alert with a confirm message.
If the user choose "Yes" => true is returned for submit.
If the user choose "No" => false is returned.
*/
function ConfirmAction(message)
{
    if (confirm(message))
    {
        return true;    
    }
    else
    {
        return false;
    }
}

/*
Description: Sets the Default Submit Button for the whole page on Enter key press.
*/
function fnTrapKD(btn, event){
 if (document.all){
  if (event.keyCode == 13){
   event.returnValue=false;
   event.cancel = true;
   btn.click();
  }
 }
 else if (document.getElementById){
  if (event.which == 13){
   event.returnValue=false;
   event.cancel = true;
   btn.click();
  }
 }
 else if(document.layers){
  if(event.which == 13){
   event.returnValue=false;
   event.cancel = true;
   btn.click();
  }
 }
}

/*--------------------------------------------- AdminNewsList.ascx javascript------------------------------------*/
/*
Description: Collapse or expand the news search area.
*/
function CollapseExpandNewsSearchArea()
{
    var hfAdvancedSearchStatus = document.getElementById(hfAdvancedSearchStatusID);
    if (hfAdvancedSearchStatus.value == "collapsed")
    {
        hfAdvancedSearchStatus.value  = "expanded";
        document.getElementById("lnkAdvancedSearch").className = "mc_news_lnk_collapse";
        document.getElementById("divAdvancedSearch").style.display="";
    }
    else
    {
        hfAdvancedSearchStatus.value = "collapsed"
        document.getElementById("lnkAdvancedSearch").className= "mc_news_lnk_expand";
        document.getElementById("divAdvancedSearch").style.display="none";
    }
}

/*
Description: Reset news search criteria as they were before any user input.
*/
function ResetNewsSearchCriteria()
{
   //Simple search controls
   document.getElementById(txtKeywordsID).value = "";
   document.getElementById(ddlCategoryID).selectedIndex = 0; //Select "All"
   document.getElementById(txtAuthorID).value = "";
   
   //Advanced search controls
   document.getElementById(chkTitleID).checked = true;
   document.getElementById(chkSubTitleID).checked = false;
   document.getElementById(chkTextID).checked = false;
   document.getElementById(ddlStatusID).selectedIndex = 0; //Select "All" 
   document.getElementById(txtDatePostedFromID).value = "";
   document.getElementById(txtDatePostedToID).value = "";
   document.getElementById(txtNewsIdID).value = "";
 
   document.getElementById(chkFeaturedOnlyID).checked = false;
   document.getElementById(chkTranslatedOnlyID).checked = false;
   
   //Hide validators
   document.getElementById(revDatePostedFromID).style.display = 'none';
   document.getElementById(revDatePostedToID).style.display = 'none';
   document.getElementById(revNewsIdID).style.display = 'none';
}

/*
Description: Check/Uncheck all checkboxes in the grid view with news.
Params: newState - "true" to check, "false" to uncheck. 
        gvNewsID - the client id of the grid view with news.
*/
function ChangeAllCheckBoxStates(newState, gvNewsID)
{
    inputArray = document.getElementById(gvNewsID).getElementsByTagName("input");		        	   
    for (var i=0; i< inputArray.length; i++) 
    {			       
       if((inputArray[i].type == 'checkbox'))
       {
           inputArray[i].checked = newState;
       }				    
    }	
}

/*
Description: When a checkbox in the grid view with news is checked/unchecked then is 
    called this function to check whether all chechboxes are checked/unchecked and to update
    the state of the checkbox in the header according this.
    If all checkboxes in the row are checked = > the checkbox in the header is checked too.
    If all checkboxes in the row are unchecked = > the checkbox in the header is inchecked too.
Params: chkAllID - the client id of the checkbox in the header of the grid.
        gvNewsID - the client id of the grid view with news.
*/
function ChangeCheckBoxState(chkAllID,gvNewsID)
{
    var allChecked = true;
    
    inputArray = document.getElementById(gvNewsID).getElementsByTagName("input");		        	   
    for (var i=0; i< inputArray.length; i++) 
    {			       
       if((inputArray[i].type == 'checkbox') && (inputArray[i].id != chkAllID))
       {
           if (!inputArray[i].checked)
           {
              allChecked = false;
              break;
           }
       }				    
    }	
    document.getElementById(chkAllID).checked = allChecked;
}

/*----------------------------------- AdminNewsAddEdit.ascx javascript------------------------------------------*/
/*
Description: Validates that the news has Text entered.
*/
function ValidateNewsText(source,args)
{
    var doc = FCKeditorAPI.GetInstance(editorID);
    if (doc != null) {
        if ((doc.EditorDocument != null) && (doc.EditorDocument.body.innerText.length > 0)) {
            args.IsValid = true;
            return;
        }
    }
    args.IsValid = false;            
}

/*
Description: Validates date and time posted.
*/
function ValidateNewsDateTimePosted(source,args)
{
    if (document.getElementById(txtDatePostedID).value == "")
    {
        //If date posted is not entered then time posted(hours/minutes) should not be selected.
        if ((document.getElementById(ddlHourPostedID).selectedIndex == 0)&&
            (document.getElementById(ddlMinutesPostedID).selectedIndex == 0))
        {
            args.IsValid = true;
        }
        else
        {
            args.IsValid = false;
        }
    }
    else
    {
        //If date posted is entered then either both hour and minutes should be selected 
        //either none of them should be selected.
        if (((document.getElementById(ddlHourPostedID).selectedIndex != 0)&&
             (document.getElementById(ddlMinutesPostedID).selectedIndex != 0))
            ||
            ((document.getElementById(ddlHourPostedID).selectedIndex == 0)&&
             (document.getElementById(ddlMinutesPostedID).selectedIndex == 0))
           )
        {
            args.IsValid = true;
        } 
        else 
        {
            args.IsValid = false;
        }
    }
}

/*--------------------------------------------- AdminCommentList.ascx javascript------------------------------------*/

/*
Description: Reset the comment search criteria as they were before any user input.
*/
function ResetCommentSearchCriteria()
{
   //Clear inputs
   document.getElementById(txtKeywordsID).value = "";
   document.getElementById(txtAuthorID).value = "";
   var ddl = document.getElementById(ddlStatusID);
   if (!!document.getElementById(ddlStatusID))
   {
        document.getElementById(ddlStatusID).selectedIndex = 0; //Select "All" 
   }
   document.getElementById(txtDatePostedFromID).value = "";
   document.getElementById(txtDatePostedToID).value = "";
   document.getElementById(txtNewsTitleID).value = "";
   document.getElementById(txtNewsIdID).value = "";
   
   //Hide validators
   document.getElementById(revDatePostedFromID).style.display = 'none';
   document.getElementById(revDatePostedToID).style.display = 'none';
   document.getElementById(revNewsIdID).style.display = 'none';
}

/*--------------------------------------------- AdminUserList.ascx javascript----------------------------------------*/

/*
Description: Reset the user search criteria as they were before any user input.
*/
function ResetUserSearchCriteria()
{
    //Clear input controls. 
	document.getElementById(txtUserFirstNameID).value="";
	document.getElementById(txtUserLastNameID).value="";
    document.getElementById(txtUserNameID).value="";
	document.getElementById(txtUserEmailID).value="";
	
	document.getElementById(chkAdministratorID).checked = true; //Administrator
	document.getElementById(chkWriterID).checked = true; //Writer
	document.getElementById(chkNoneID).checked = false; //None
}



