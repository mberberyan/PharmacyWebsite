//Find a document element by tag type and id
//param:tagname - input or span
//param:realName - the id of the asp control
//Author: Slavena Savova
//Date: 12/11/2006
function getName(tagType, realName) {    
	inputArray = document.body.getElementsByTagName(tagType);
	for (i = 0; i < inputArray.length; i++) {
		if (inputArray[i].id.toString().indexOf(realName)!= -1) {
		  return inputArray[i].id;
		}
	}
}

//Find a document element in parent doucment by tag type and id
//param:tagname - input or span
//param:realName - the id of the asp control
//Author: Melon Team
//Date: 01/21/2010
function getNameInParent(tagType, realName) {
    inputArray = parent.document.body.getElementsByTagName(tagType);
    for (i = 0; i < inputArray.length; i++) {
        if (inputArray[i].id.toString().indexOf(realName) != -1) {
            return inputArray[i].id;
        }
    }
}

// Returns an array of elements from a certain Tag and Id
// Param: tagName - tag name of the element
// Param: elementId - Id of the element
// Author: Yavor Ivanov
// Date: 04/04/2008   
function getElementsByTagNameAndId(tagName, elementId) {
    var resArray = new Array();

    var elementArray = document.getElementsByTagName(tagName);
    for (var j = 0; j < elementArray.length; j++) {
        if (elementArray[j].id.indexOf(elementId) != -1)
            resArray.push(elementArray[j]);
    }

    return resArray;
}

//Fire event with respective arguments after page is loaded
//param:func - fnction name to be called after page is loaded
//param:arg1, arg2, arg3 - function 'RenderSelectedCategoryList' needs three arguments that are passed as
// arguments
//Author: Melon Team
//Date: 11/09/2009

function addLoadEvent(func, arg1, arg2, arg3) 
{   
    if (window.onload) 
    {
        var oldOnLoad = window.onload;   
           
        window.onload = function()
        {        
            oldOnLoad();
            if(arg1!=null && arg2!=null && arg3!=null)
            {
                func(arg1, arg2, arg3);
            }
            else
            {
                func();   
            }
        }
    }   
    else 
    {       
            if(arg1!=null && arg2!=null && arg3!=null)
            {
                window.onload = function() 
                {
                    func(arg1, arg2, arg3);    
                }                
            }
            else
            {
                window.onload=func;   
            }            
    }               
}

//Fire event with respective arguments after page is loaded
//param:func - fnction name to be called after page is loaded
//param:arg1, arg2, arg3, arg4 - function 'RenderSelectedCategoryList' and 'RenderSelectedCategoryListSearch' needs four arguments that are passed as
// arguments
//Author: Melon Team
//Date: 11/09/2009

function addLoadEvent(func, arg1, arg2, arg3, arg4) {
    if (window.onload) {
        var oldOnLoad = window.onload;

        window.onload = function() {
            oldOnLoad();
            if (arg1 != null && arg2 != null && arg3 != null && arg4 != null) {
                func(arg1, arg2, arg3, arg4);
            }
            else {
                func();
            }
        }
    }
    else {
        if (arg1 != null && arg2 != null && arg3 != null && arg4 != null) {
            window.onload = function() {
                func(arg1, arg2, arg3, arg4);
            }
        }
        else {
            window.onload = func;
        }
    }
}

/*
Author: Melon Team
Date: 09/25/2009
Description: Display confirm dialog to user whether to delete system objects.    
*/
function OnDeleteObjectClientClick(confirmMessage)
{    
    return confirm(confirmMessage);
}

//This function sets the default button of the page,
//i.e. if the "Enter" is clicked which button to be clicked from the page.
//param: btn - object
//author: Slavena Savova
//date: 07/17/2007
function SetDefaultButton(btn,keyEvent)
{
    if(btn!= null) 
    {
     if (document.all)    
     {
      if (keyEvent.srcElement.tagName=='TEXTAREA')
        return;
      if (keyEvent.keyCode == 13)
      {
       keyEvent.returnValue=false;
       keyEvent.cancel = true;
       btn.click();
      }
     }
     else if (document.getElementById)
     {
      if (keyEvent.target.tagName=='TEXTAREA')
        return;
      if (keyEvent.which == 13)
      {
       keyEvent.returnValue=false;
       keyEvent.cancel = true;
       btn.click();
      }
    }
    else if(document.layers)
    {
      if (keyEvent.target.tagName=='TEXTAREA')
        return;
      if(keyEvent.which == 13)
      {
       keyEvent.returnValue=false;
       keyEvent.cancel = true;
       btn.click();
      }
    }
   }    
}
/*------------------------------------------------ CategoryExplorer.ascx javascript----------------------------------------------*/
/*
Author: Nevena Uzunova
Date: 27/06/2008
Description:  Attach to every expande/collapse link onclick event - OnCategoryExpandCollapse(link).
*/
function InitCategoriesTreeView() 
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
            anchorExpand.onclick = function(){OnCategoryExpandCollapse(this);if (!!clickExpandEventHandler)clickExpandEventHandler;};
        }     
    }
}

/*
Author: Nevena Uzunova
Date: 03/07/2008
Description: Update hidden field hfExpandedCategories where are stores ids of currently expanded categories.
*/
function OnCategoryExpandCollapse(link)
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
          
    var strExpandedNodeIds = hfExpandedCategories.value;
    if (strExpandedNodeIds.match(new RegExp(","+nodeId+","))!= null)
    {
        //The category is now collapsed = > so remove it from expanded nodes.
        hfExpandedCategories.value = strExpandedNodeIds.replace(strExpandedNodeIds.match(new RegExp(","+nodeId+",")),",");
    }
    else
    {
        //The node is now expanded => so add it to expanded categories.
        hfExpandedCategories.value += nodeId + ','; 
    }   
}

/*------------------------------------------------ MenuTabs.ascx javascript----------------------------------------------*/
/*
Author: Melon Team
Date: 09/7/2009
Description: Set selected div visibility
*/
function SelectMenuTab(selectedTab)
{       
    hfSelectedTab.value='div'+selectedTab;

    if(typeof(divGeneralInformation)!='undefined' && divGeneralInformation!=null)
    { 
        divGeneralInformation.style.display = selectedTab=='GeneralInformation' ? 'block' : 'none';
        tabGeneralInformation.className=selectedTab=='GeneralInformation' ? "mc_pc_tab_selected" : "mc_pc_tab_unselected";        
    }
    
    if(typeof(divImages)!='undefined' && divImages!=null)
    {
        divImages.style.display = selectedTab=='Images' ? 'block' : 'none'; 
        tabImages.className=selectedTab=='Images' ? "mc_pc_tab_selected" : "mc_pc_tab_unselected";
    
        //activate image scrolling  
        $(document).ready(function() {
            $('#my-list').hoverscroll();
        });     
    }
    
    if(typeof(divAudio)!='undefined' && divAudio!=null)
    {
        divAudio.style.display = selectedTab=='Audio' ? 'block' : 'none';        
        tabAudio.className=selectedTab=='Audio' ? "mc_pc_tab_selected" : "mc_pc_tab_unselected";    
        
        //activate audio files scrolling  
        $(document).ready(function() {
		    $('#audio-list').hoverscroll();
	    });    
    }
    
    if(typeof(divVideo)!='undefined' && divVideo!=null)
    {
        divVideo.style.display = selectedTab=='Video' ? 'block' : 'none';        
        tabVideo.className=selectedTab=='Video' ? "mc_pc_tab_selected" : "mc_pc_tab_unselected";
        
        //activate video files scrolling  
        $(document).ready(function() {
		    $('#video-list').hoverscroll();
	    });        
    }
    
    if(typeof(divRelatedProducts)!='undefined' && divRelatedProducts!=null)
    {
        divRelatedProducts.style.display = selectedTab=='RelatedProducts' ? 'block' : 'none';        
        tabRelatedProducts.className=selectedTab=='RelatedProducts' ? "mc_pc_tab_selected" : "mc_pc_tab_unselected";        
    }
    
    if(typeof(divStatistics)!='undefined' && divStatistics!=null)
    {
        divStatistics.style.display = selectedTab=='Statistics' ? 'block' : 'none';        
        tabStatistics.className=selectedTab=='Statistics' ? "mc_pc_tab_selected" : "mc_pc_tab_unselected";        
    }
    
    if(typeof(divDynamicProperties)!='undefined' && divDynamicProperties!=null)
    {
        divDynamicProperties.style.display = selectedTab=='DynamicProperties' ? 'block' : 'none';        
        tabDynamicProperties.className=selectedTab=='DynamicProperties' ? "mc_pc_tab_selected" : "mc_pc_tab_unselected";        
    }
    
    if(typeof(divProducts)!='undefined' && divProducts!=null)
    {
        divProducts.style.display = selectedTab=='Products' ? 'block' : 'none';                    
        tabProducts.className=selectedTab=='Products' ? "mc_pc_tab_selected" : "mc_pc_tab_unselected";
    }
}

/*------------------------------------------------ ProductList.ascx javascript----------------------------------------------*/
/*
Author: Melon Team
Date: 09/14/2009
Description: Reset search criteria as they were before any user input.
*/
function ResetSearchCriteria()
{
    //Clear input controls.
    if (typeof (txtAdvancedSearchKeywords) != 'undefined' && txtAdvancedSearchKeywords != null) {
        txtAdvancedSearchKeywords.value = "";
    }

    //Clear input controls.
    if (typeof (txtSimpleSearchKeywords) != 'undefined' && txtSimpleSearchKeywords != null) {
        txtSimpleSearchKeywords.value = "";
    }
    
    //Clear input controls. 
    if(typeof(txtKeywords)!='undefined' && txtKeywords!=null)
    {
	    txtKeywords.value="";
	}

	//Clear input controls.
	if (typeof (txtFirstName) != 'undefined' && txtFirstName != null) {
	    txtFirstName.value = "";
	}

	//Clear input controls.
	if (typeof (txtLastName) != 'undefined' && txtLastName != null) {
	    txtLastName.value = "";
	}

	//Clear input controls.
	if (typeof (txtUserName) != 'undefined' && txtUserName != null) {
	    txtUserName.value = "";
	}

	//Clear input controls.
	if (typeof (txtEmail) != 'undefined' && txtEmail != null) {
	    txtEmail.value = "";
	}

	if (typeof (chkAdminRole) != 'undefined' && chkAdminRole != null) {
	    chkAdminRole.checked = false;
	}	
	
	if (typeof (ddlCategoryList) != 'undefined' && ddlCategoryList != null) {
	    ddlCategoryList.selectedIndex = 0;
	}

	if (typeof (chkRecursiveCategory) != 'undefined' && chkRecursiveCategory != null) {
	    chkRecursiveCategory.checked = false;
	}
	
	if (typeof (txtAddedFrom) != 'undefined' && txtAddedFrom != null) {
	    txtAddedFrom.value = "";
	}

	if (typeof (txtAddedTo) != 'undefined' && txtAddedTo != null) {
	    txtAddedTo.value = "";
	}

	if (typeof (revAddedFrom) != 'undefined' && revAddedFrom != null) {
	    revAddedFrom.style.display = "none";
	}

	if (typeof (revAddedTo) != 'undefined' && revAddedTo != null) {
	    revAddedTo.style.display = "none";
	}

	if (typeof (cvValidPeriod) != 'undefined' && cvValidPeriod != null) {
	    cvValidPeriod.style.display = "none";
	}
	
	if(typeof(txtPriceFrom)!='undefined' && txtPriceFrom!=null)
  
    {
	    txtPriceFrom.value="";
	}
	
	if(typeof(txtPriceTo)!='undefined' && txtPriceTo!=null)
    {
        txtPriceTo.value="";	
	}

	if (typeof (cvPriceFrom) != 'undefined' && cvPriceFrom != null) {
	    cvPriceFrom.style.display = "none";
	}

	if (typeof (cvPriceTo) != 'undefined' && cvPriceTo != null) {
	    cvPriceTo.style.display = "none";
	}

	if (typeof (cvComparePrices) != 'undefined' && cvComparePrices != null) {
	    cvComparePrices.style.display = "none";
	}

	if (typeof (chkCategory) != 'undefined' && chkCategory != null) {
	    chkCategory.checked = true;
	}

	if (typeof (chkProduct) != 'undefined' && chkProduct != null) {
	    chkProduct.checked = true;
	}

	if (typeof (chkCatalog) != 'undefined' && chkCatalog != null) {
	    chkCatalog.checked = true;
	}

	if (typeof (chkBundle) != 'undefined' && chkBundle != null) {
	    chkBundle.checked = true;
	}

	if (typeof (chkCollection) != 'undefined' && chkCollection != null) {
	    chkCollection.checked = true;
	}

	if (typeof (chkDiscount) != 'undefined' && chkDiscount != null) {
	    chkDiscount.checked = true;
	}

	if (typeof (chkCode) != 'undefined' && chkCode != null)
    {
        chkCode.checked = true; 
	}
	
	if(typeof(chkName)!='undefined' && chkName!=null)
    {
	    chkName.checked = true; 
	}
	
	if(typeof(chkDescription)!='undefined' && chkDescription!=null)
    {
	    chkDescription.checked = true;
	}

	if (typeof (chkReview) != 'undefined' && chkReview != null) {
	    chkReview.checked = true;
	}
	
	if(typeof(chkTags)!='undefined' && chkTags!=null)
    {
	    chkTags.checked = true; 
	}


	if (typeof (ddlActive) != 'undefined' && ddlActive != null) {
	    ddlActive.value='';
	}

	if (typeof (ddlInStock) != 'undefined' && ddlInStock != null) {
	    ddlInStock.value = '';
	}

	if (typeof (ddlFeatured) != 'undefined' && ddlFeatured != null) {
	    ddlFeatured.value = '';
	}	

	if (typeof (chkAdminRole) != 'undefined' && chkAdminRole != null) {
	    chkAdminRole.checked = true;
	}

	if (typeof (chkNonadminRole) != 'undefined' && chkNonadminRole != null) {
	    chkNonadminRole.checked = false;
	}

    // clear selected category list
	if (typeof (divCategoryListSearch) != 'undefined' && divCategoryListSearch != null) {
	    divCategoryListSearch.innerHTML = '';	    
	    divCategoryListSearch.className = "mc_pc_table_category_list hidden";	    
	}

	if (typeof (hfProductSetCategoryList) != 'undefined' && hfProductSetCategoryList != null) {
	    hfProductSetCategoryList.value = '';
	}

	if (typeof (hfProductSetCategoryName) != 'undefined' && hfProductSetCategoryName != null) {
	    hfProductSetCategoryName.value = '';
	}

	if (typeof (hfAdvancedSearchCategoryList) != 'undefined' && hfAdvancedSearchCategoryList != null) {
	    hfAdvancedSearchCategoryList.value = '';
	}

	if (typeof (hfAdvancedSearchCategoryName) != 'undefined' && hfAdvancedSearchCategoryName != null) {
	    hfAdvancedSearchCategoryName.value = '';
	}
	// end clear selected category list
}


/*------------------------------------------------ Product.ascx javascript----------------------------------------------*/

var selImgName="selCat";
Array.prototype.contains = function ( item ) {
   for (i in this) {
       if (this[i] == item) return true;
   }
   return false;
}


function AddCategoryToList(hfCatList, hfCatName, lbCatList, divCatList, activePage, basePath)
{    
  if(typeof(hfCatList)=='undefined' || hfCatList==null
    || typeof(hfCatName)=='undefined' || hfCatName==null
    || typeof(lbCatList)=='undefined' || lbCatList==null
    || typeof(divCatList)=='undefined' || divCatList==null
  )
  {
    return;
  }
  var catList=Array();   
  var catName=Array();
  
  if(hfCatList.value!="")
  {
    catList=hfCatList.value.split(',');
  }
  
  if(hfCatName.value!="")
  {
    catName=hfCatName.value.split(',');
  }
  
  var optsLength = lbCatList.options.length;
  
  for(var i=0;i<optsLength;i++)
  {
    if(lbCatList.options[i].selected)
    {
        if(!catList.contains(lbCatList.options[i].value.split(';')[0]))
        {
            catList.push(lbCatList.options[i].value.split(';')[0]);
            catName.push(lbCatList.options[i].value.split(';')[1]);
        }
    }        
  }  
  
  if(catList.length>0)
  {
    hfCatList.value=catList.toString();
  }
  
  if(catName.length>0)
  {
    hfCatName.value=catName.toString();
  }

  // display selected categories depending where it opens
  // both methods do the same but are loaded via window.onload
  // that`s why they are named with different names
  if (activePage == 'ucGeneralInformation') 
  {
    RenderSelectedCategoryList(hfCatList, hfCatName, divCatList, basePath);
}
else if (activePage == 'ucProductSet' || activePage == 'ucAdvancedSearch')
  {
    RenderSelectedCategoryListSearch(hfCatList, hfCatName, divCatList, basePath);
  }  
}

function RemoveCategoryFromList(imgName, _hfCatList, _hfCatName, _divCatList, activePage, basePath)
{
    var hfCatList=document.getElementById(_hfCatList);
    var hfCatName=document.getElementById(_hfCatName);
    var divCatList=document.getElementById(_divCatList);
    
    if(typeof(hfCatList)=='undefined' || hfCatList==null
        || typeof(hfCatName)=='undefined' || hfCatName==null       
        || typeof(divCatList)=='undefined' || divCatList==null
      )
      {
        return;
      }
      
    var str = imgName.substring(selImgName.length,imgName.length); // find category id to remove
    var catList=Array();
    var catName=Array();
    
    if(hfCatList.value!="")
    {
        catList=hfCatList.value.split(',');
    }
    
    if(hfCatName.value!="")
    {
        catName=hfCatName.value.split(',');
    }
  
    // find the index of element to remove from the 'catList' array 
    var selCatId=-1;
    for(var y=0;y<catList.length;y++)
    {
        if(catList[y]==str)
        {
            selCatId=y;
        }    
    }               
    
    if(selCatId!=-1)
    {
        // remove category id from 'catList' array
        catList.splice(selCatId,1);
        
        // remove category name from the 'catName' array by its index
        catName.splice(selCatId,1);
    }
    
    hfCatList.value=catList.toString();
    hfCatName.value=catName.toString();    
    
    // display selected categories depending where it opens
    // both methods do the same but are loaded via window.onload
    // that`s why they are named with different names
    if (activePage == 'ucGeneralInformation')
    {
      RenderSelectedCategoryList(hfCatList, hfCatName, divCatList, basePath);
    }
    else if (activePage == 'ucProductSet' || activePage == 'ucAdvancedSearch')
    {
      RenderSelectedCategoryListSearch(hfCatList, hfCatName, divCatList, basePath);
    }
  }

function RenderSelectedCategoryList(hfCatList, hfCatName, divCatList, basePath)
{
    if(typeof(hfCatList)=='undefined' || hfCatList==null
        || typeof(hfCatName)=='undefined' || hfCatName==null        
        || typeof(divCatList)=='undefined' || divCatList==null
      )
      {
        return;
      }
      
    var catList=Array();
    var catName=Array();
    
    if(hfCatList.value!="")
    {
        catList=hfCatList.value.split(',');
    }
    
    if(hfCatName.value!="")
    {
        catName=hfCatName.value.split(',');
    }
    
    if(divCatList!=null)
    {
        divCatList.innerHTML="";
        if (catList.length > 0 && catName.length > 0) 
        {
            divCatList.className = "mc_pc_table_category_list";
            divCatList.innerHTML += "Added categories<br/>";
            for (var x = 0; x < catList.length; x++) {
                divCatList.innerHTML += "<div id='divCat" + catList[x] + "'><img name='" + selImgName + catList[x] + "' src='MC_ProductCatalog/Sources/Styles/Images/btn_close.gif' title='Remove category' onclick=\"javascript:RemoveCategoryFromList(this.name, '" + hfCatList.id + "', '" + hfCatName.id + "','" + divCatList.id + "','ucGeneralInformation','" + basePath + "')\" />  " + "<span id='catValue'" + catList[x] + ">" + catName[x] + "</span></div>";
            }
        }
        else 
        {
            divCatList.className = "mc_pc_table_category_list hidden";
        }
    }
}


function RenderSelectedCategoryListSearch(hfCatList, hfCatName, divCatList, basePath) {
    if (typeof (hfCatList) == 'undefined' || hfCatList == null
        || typeof (hfCatName) == 'undefined' || hfCatName == null
        || typeof (divCatList) == 'undefined' || divCatList == null
      ) {
        return;
    }

    var catList = Array();
    var catName = Array();

    if (hfCatList.value != "") {
        catList = hfCatList.value.split(',');
    }

    if (hfCatName.value != "") {
        catName = hfCatName.value.split(',');
    }

    if (divCatList != null) {
        divCatList.innerHTML = "";
        if (catList.length > 0 && catName.length > 0) 
        {
            divCatList.className = "mc_pc_table_category_list";
            divCatList.innerHTML += "Added categories<br/>";
            for (var x = 0; x < catList.length; x++) {
                divCatList.innerHTML += "<div id='divCat" + catList[x] + "'><img name='" + selImgName + catList[x] + "' src='MC_ProductCatalog/Sources/Styles/Images/btn_close.gif' title='Remove category' onclick=\"javascript:RemoveCategoryFromList(this.name, '" + hfCatList.id + "','" + hfCatName.id + "', '" + divCatList.id + "','ucProductSet','" + basePath + "')\" />  " + "<span id='catValue'" + catList[x] + ">" + catName[x] + "</span></div>";
            }
        }
        else 
        {
            divCatList.className = "mc_pc_table_category_list hidden";
        }
    }
}

function ValidateDynPropEntries(source, args) 
{
    var tblDynPropArray = getElementsByTagNameAndId("INPUT", "txtEdit");    

    for (var x = 0; x < tblDynPropArray.length; x++) {
        if (tblDynPropArray[x].value != "" 
            && ((document.all && tblDynPropArray[x].parentElement.style.display != "none")
                || (document.getElementById && tblDynPropArray[x].parentNode.style.display != "none"))
           )
        {
            args.IsValid = true;
            return;
        }
    }
    args.IsValid = false;                     
}
/*------------------------------------------------ DynamicPropDefinition.ascx javascript----------------------------------------------*/
/*
Author: Melon Team
Date: 10/08/2009
Description: Set dropdown value in textbox
*/
function DropDownTextToBox(objDropdown, strTextboxId) 
{ 
    // if this control is used in Product, then txtDynamicProp textbox control is not used
    if (txtDynamicProp.style.display=='none')
    {
        return;
    }
        
    txtDynamicProp.value = objDropdown.options[objDropdown.selectedIndex].value; 
    DropDownIndexClear(objDropdown.value); 
    txtDynamicProp.focus(); 
} 

/*
Author: Melon Team 
Date: 10/082009
Description: Clear dropdown selection when textbox value is changed
*/
function DropDownIndexClear(strDropdownId) 
{ 
    if (ddlPropDef != null) 
    { 
        ddlPropDef.selectedIndex = -1; 
    }
}

/*------------------------------------------------ Export.ascx javascript----------------------------------------------*/
/*
Author: Melon Team 
Date: 01/04/2010
Description: Check or uncheck or object details checkboxes
*/
function SelectAllDetails(objSelectAll,objSelectDetails) 
{
    var j = 0;
    var chk = document.getElementById(getName('table',objSelectDetails)).getElementsByTagName('input');
    
    for (i = 0; i < chk.length; i++) 
    {
        chk[i].checked = objSelectAll.checked;
    }
}

/*
Author: Melon Team 
Date: 01/04/2010
Description: Check or uncheck 'Select All' checkbox depending on object details checkboxes
*/
function CheckSelectAll(objSelectDetails, chklSelectAllObjName) 
{
    var j = 0;
    var isChecked = true;
    var chk = document.getElementById(getName('table', objSelectDetails.id)).getElementsByTagName('input');
    var chkSelectAll = document.getElementById(getName('input', chklSelectAllObjName));

    for (i = 0; i < chk.length; i++) 
    {
        if (!chk[i].checked) {
            isChecked = false;
        }
    }
    
    chkSelectAll.checked = isChecked;
}

/*
Author: Melon Team
Date: 02/23/2010
Description: Check whether object columns are selected - if not raise alert message.    
*/
function OnExportObjects(categoryCheckBox, productCheckBox, bundleCheckBox, catalogCheckBox, collCheckBox, discountCheckBox, confirmMessage) {

    var isChecked = false;
    
    var chkCategory = document.getElementById(getName('table', categoryCheckBox)).getElementsByTagName('input');
    for (i = 0; i < chkCategory.length; i++) {
        if (chkCategory[i].checked) {
            isChecked = true;
        }
    }

    var chkProduct = document.getElementById(getName('table', productCheckBox)).getElementsByTagName('input');
    for (i = 0; i < chkProduct.length; i++) {
        if (chkProduct[i].checked) {
            isChecked = true;
        }
    }

    var chkBundle = document.getElementById(getName('table', bundleCheckBox)).getElementsByTagName('input');
    for (i = 0; i < chkBundle.length; i++) {
        if (chkBundle[i].checked) {
            isChecked = true;
        }
    }

    var chkCatalog = document.getElementById(getName('table', catalogCheckBox)).getElementsByTagName('input');
    for (i = 0; i < chkCatalog.length; i++) {
        if (chkCatalog[i].checked) {
            isChecked = true;
        }
    }

    var chkCollection = document.getElementById(getName('table', collCheckBox)).getElementsByTagName('input');
    for (i = 0; i < chkCollection.length; i++) {
        if (chkCollection[i].checked) {
            isChecked = true;
        }
    }

    var chkDiscount = document.getElementById(getName('table', discountCheckBox)).getElementsByTagName('input');
    for (i = 0; i < chkDiscount.length; i++) {
        if (chkDiscount[i].checked) {
            isChecked = true;
        }
    }
    

    if (!isChecked) {
        alert(confirmMessage);
        return false;
    }
}

/*------------------------------------------------ DiscountInformation.ascx javascript----------------------------------------------*/
function SetDiscountTypeLabel(rblDiscountIn) 
{
    lblDiscountType.innerText = rblDiscountIn.value;
}

function CheckDiscountValue(source, args) {
    var txtDiscountValue = document.getElementById(getName("INPUT", "txtDiscountValue"));
    var lblDiscountType = document.getElementById(getName("SPAN", "lblDiscountType"));

    args.IsValid = !(lblDiscountType.innerHTML=='%' && txtDiscountValue.value >= 100);
}

/*------------------------------------------------ Search.ascx javascript----------------------------------------------*/
function CheckPriceRange(source, args) {
    var txtPriceFrom = document.getElementById(getName("INPUT", "txtPriceFrom"));
    var txtPriceTo = document.getElementById(getName("INPUT", "txtPriceTo"));   

    args.IsValid = !(txtPriceFrom.value != '' && txtPriceTo.value != '' && parseInt(txtPriceFrom.value) >= parseInt(txtPriceTo.value));
}

/*------------------------------------------------ ProductSet.ascx javascript----------------------------------------------*/
/*
Author: Melon Team
Date: 24/02/2010
Description: Collapse or expand the product set search area.
*/
function CollapseExpandProductSetSearchArea()
{    
    if (hfProductSetSearchStatus.value == "collapsed")
    {
        hfProductSetSearchStatus.value = "expanded";
        document.getElementById("lnkProductSetSearch").className = "mc_pc_lnk_collapse";
        document.getElementById("tabProductSetSearchResult").style.display = "";
    }
    else
    {
        hfProductSetSearchStatus.value = "collapsed";
        document.getElementById("lnkProductSetSearch").className = "mc_pc_lnk_expand";
        document.getElementById("tabProductSetSearchResult").style.display = "none";
    }
}
