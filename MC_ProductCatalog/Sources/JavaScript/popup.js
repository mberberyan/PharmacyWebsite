/***************************/
//@Author: Adrian "yEnS" Mato Gondelle
//@website: www.yensdesign.com
//@email: yensamg@gmail.com
//@license: Feel free to use it, but keep this credits please!					
/***************************/

//SETTING UP OUR POPUP
//0 means disabled; 1 means enabled;
var popupImageStatus = 0;

//loading popup with jQuery!
function loadImagePopup(img)
{
	//loads popup only if it is disabled
	if(popupImageStatus==0)
	{
		$("#backgroundImagePopup").css({
			"opacity": "0.5"
		});
		$("#backgroundImagePopup").fadeIn("fast");
		$("#popupImageContact").fadeIn("fast");
		popupImageStatus = 1;
		
		if(img!='undefined' && img!=null)
		{
		    litAddImageHeader.style.display='none';		    
		    litEditImageHeader.style.display='';
		    hfImageId.value=img.name;
		    hfAddNewImage.value='False';
		    txtPopupImageAlt.value=img.alt;
		    imgPopupImage.src=img.src;
		    imgPopupImage.style.visibility='visible';		   
		    rowImageUpload.style.display='none';
		    btnAddImage.style.display='none';
		    btnEditImage.style.display = '';
		    lnkPreviewImage.style.display = '';
		    lnkPreviewImage.href = 'javascript:openImagePopup(\'' + img.src + '\')';
		    chkMainImage.checked = img.name==hfMainImageSrc.value;
		}
		else
		{
            litAddImageHeader.style.display='';		    
		    litEditImageHeader.style.display='none';		
		    hfAddNewImage.value='True';		    
		    txtPopupImageAlt.value="";
		    imgPopupImage.style.visibility='hidden';		    
		    rowImageUpload.style.display='';
		    btnAddImage.style.display='';
		    btnEditImage.style.display = 'none';
		    lnkPreviewImage.style.display = 'none';
		    chkMainImage.checked=false;	    
		}
	}
}

//loading image popup in Front-End with jQuery!
function loadFEImagePopup(imgSrc) 
{
    //loads popup only if it is disabled
    if (popupImageStatus == 0) {
        $("#backgroundImagePopup").css({
            "opacity": "0.5"
        });
        $("#backgroundImagePopup").fadeIn("fast");
        $("#popupImageContact").fadeIn("fast");
        popupImageStatus = 1;
        
        imgPopupImage.src = imgSrc;                           
    }
}

//disabling popup with jQuery!
function disableImagePopup()
{
	//disables popup only if it is enabled
	if(popupImageStatus==1)
	{
		$("#backgroundImagePopup").fadeOut("fast");
		$("#popupImageContact").fadeOut("fast");
		popupImageStatus = 0;
	}
}

//centering popup
function centerImagePopup()
{
	//request data for centering
	var windowWidth = document.documentElement.clientWidth;
	var windowHeight = document.documentElement.clientHeight;
	var popupHeight = $("#popupImageContact").height();
	var popupWidth = $("#popupImageContact").width();
	//centering
	$("#popupImageContact").css({
		"position": "absolute",
		"top": windowHeight/2-popupHeight/2,
		"left": windowWidth/2-popupWidth/2
	});
	//only need force for IE6
	
	$("#backgroundImagePopup").css({
		"height": windowHeight
	});	
}


//CONTROLLING EVENTS IN jQuery
$(document).ready(function()
{						
	//CLOSING POPUP
	//Click the x event!
	$("#popupImageContactClose").click(function(){
		disableImagePopup();
	});
	//Click out event!
	$("#backgroundImagePopup").click(function(){
		disableImagePopup();
	});
	//Press Escape event!
	$(document).keypress(function(e){
		if(e.keyCode==27 && popupImageStatus==1){
			disableImagePopup();
		}
	});
});


//SETTING UP OUR POPUP
//0 means disabled; 1 means enabled;
var popupAudioStatus = 0;
    
//loading popup with jQuery!
function loadAudioPopup(img)
{
	//loads popup only if it is disabled
	if(popupAudioStatus==0)
	{
		$("#backgroundAudioPopup").css({
			"opacity": "0.5"
		});
		$("#backgroundAudioPopup").fadeIn("fast");
		$("#popupAudioContact").fadeIn("fast");
		popupAudioStatus = 1;
		
		if(img!='undefined' && img!=null)
		{
		    litAddAudioHeader.style.display='none';		    
		    litEditAudioHeader.style.display='';
		    hfAudioId.value = img.name.split(";")[0];
		    hfCurrentAudioTitle.value = img.name.split(";")[1];
		    hfAddNewAudio.value='False';
		    txtPopupAudioTitle.value=img.alt;		    		    
		    rowAudioUpload.style.display='none';
		    btnAddAudio.style.display='none';
		    btnEditAudio.style.display = '';
		    lnkPreviewAudio.style.display = '';
		    chkMainAudio.checked = img.name.split(";")[0] == hfMainAudioSrc.value;  
		}
		else
		{
            litAddAudioHeader.style.display='';		    
		    litEditAudioHeader.style.display='none';		
		    hfAddNewAudio.value='True';		    
		    txtPopupAudioTitle.value="";		    
		    rfvAudioUpload.style.display='none';
		    revAudioUpload.style.display='none';
		    rowAudioUpload.style.display = '';
		    lnkPreviewAudio.style.display = 'none';
		    document.getElementById('divAudioUpload').innerHTML = document.getElementById('divAudioUpload').innerHTML;
		    
		    btnAddAudio.style.display='';
		    btnEditAudio.style.display='none';		    
		    chkMainAudio.checked=false;	    
		}
	}
}

//disabling popup with jQuery!
function disableAudioPopup()
{
	//disables popup only if it is enabled
	if(popupAudioStatus==1)
	{
		$("#backgroundAudioPopup").fadeOut("fast");
		$("#popupAudioContact").fadeOut("fast");
		popupAudioStatus = 0;
	}
}

//centering popup
function centerAudioPopup()
{
	//request data for centering
	var windowWidth = document.documentElement.clientWidth;
	var windowHeight = document.documentElement.clientHeight;
	var popupHeight = $("#popupAudioContact").height();
	var popupWidth = $("#popupAudioContact").width();
	//centering
	$("#popupAudioContact").css({
		"position": "absolute",
		"top": windowHeight/2-popupHeight/2,
		"left": windowWidth/2-popupWidth/2
	});
	//only need force for IE6
	
	$("#backgroundAudioPopup").css({
		"height": windowHeight
	});	
}


//CONTROLLING EVENTS IN jQuery
$(document).ready(function()
{						
	//CLOSING POPUP
	//Click the x event!
	$("#popupAudioContactClose").click(function(){
		disableAudioPopup();
	});
	//Click out event!
	$("#backgroundAudioPopup").click(function(){
		disableAudioPopup();
	});
	//Press Escape event!
	$(document).keypress(function(e){
		if(e.keyCode==27 && popupAudioStatus==1){
			disableAudioPopup();
		}
	});
});


//SETTING UP OUR POPUP
//0 means disabled; 1 means enabled;
var popupVideoStatus = 0;
    
//loading popup with jQuery!
function loadVideoPopup(img)
{
	//loads popup only if it is disabled
	if(popupVideoStatus==0)
	{
		$("#backgroundVideoPopup").css({
			"opacity": "0.5"
		});
		$("#backgroundVideoPopup").fadeIn("fast");
		$("#popupVideoContact").fadeIn("fast");
		popupVideoStatus = 1;
		
		if(img!='undefined' && img!=null)
		{
		    litAddVideoHeader.style.display='none';		    
		    litEditVideoHeader.style.display='';
		    hfVideoId.value = img.name.split(";")[0];
		    hfCurrentVideoTitle.value = img.name.split(";")[1];
		    hfAddNewVideo.value='False';
		    txtPopupVideoTitle.value=img.alt;		    		    
		    rowVideoUpload.style.display='none';
		    btnAddVideo.style.display='none';
		    btnEditVideo.style.display = '';
		    lnkPreviewVideo.style.display = '';
		    chkMainVideo.checked = img.name.split(";")[0] == hfMainVideoSrc.value;
		}
		else
		{
            litAddVideoHeader.style.display='';		    
		    litEditVideoHeader.style.display='none';		
		    hfAddNewVideo.value='True';		    
		    txtPopupVideoTitle.value="";		  
		    rfvVideoUpload.style.display='none';
		    revVideoUpload.style.display = 'none';
		    lnkPreviewVideo.style.display = 'none';
		    document.getElementById('divVideoUpload').innerHTML = document.getElementById('divVideoUpload').innerHTML;
		      
		    rowVideoUpload.style.display='';
		    btnAddVideo.style.display='';
		    btnEditVideo.style.display='none';		    
		    chkMainVideo.checked=false;	    
		}
	}
}

//disabling popup with jQuery!
function disableVideoPopup()
{
	//disables popup only if it is enabled
	if(popupVideoStatus==1)
	{
		$("#backgroundVideoPopup").fadeOut("fast");
		$("#popupVideoContact").fadeOut("fast");
		popupVideoStatus = 0;
	}
}

//centering popup
function centerVideoPopup()
{
	//request data for centering
	var windowWidth = document.documentElement.clientWidth;
	var windowHeight = document.documentElement.clientHeight;
	var popupHeight = $("#popupVideoContact").height();
	var popupWidth = $("#popupVideoContact").width();
	//centering
	$("#popupVideoContact").css({
		"position": "absolute",
		"top": windowHeight/2-popupHeight/2,
		"left": windowWidth/2-popupWidth/2
	});
	//only need force for IE6
	
	$("#backgroundVideoPopup").css({
		"height": windowHeight
	});	
}


//CONTROLLING EVENTS IN jQuery
$(document).ready(function()
{						
	//CLOSING POPUP
	//Click the x event!
	$("#popupVideoContactClose").click(function(){
		disableVideoPopup();
	});
	//Click out event!
	$("#backgroundVideoPopup").click(function(){
		disableVideoPopup();
	});
	//Press Escape event!
	$(document).keypress(function(e){
		if(e.keyCode==27 && popupVideoStatus==1){
			disableVideoPopup();
		}
	});
});


//SETTING UP OUR POPUP
//0 means disabled; 1 means enabled;
var popupMeasurementUnitStatus = 0;

//loading popup with jQuery!
function loadMeasurementUnitPopup(unitId, name, description) {
    //loads popup only if it is disabled
    if (popupMeasurementUnitStatus == 0) {
        $("#backgroundMeasurementUnitPopup").css({
            "opacity": "0.5"
        });
        $("#backgroundMeasurementUnitPopup").fadeIn("fast");
        $("#popupMeasurementUnitContact").fadeIn("fast");
        popupMeasurementUnitStatus = 1;

        if (unitId != 'undefined' && unitId != null) {
            litAddMeasurementUnitHeader.style.display = 'none';
            litEditMeasurementUnitHeader.style.display = '';
            hfMeasurementUnitId.value = unitId;

            txtPopupMeasurementUnitName.value = name;
            txtPopupMeasurementUnitDescription.value = description;
            rfvMeasurementUnitName.style.display = 'none';
            
            btnAddMeasurementUnit.style.display = 'none';
            btnEditMeasurementUnit.style.display = 'inline';            
        }
        else {
            litAddMeasurementUnitHeader.style.display = '';
            litEditMeasurementUnitHeader.style.display = 'none';
            hfMeasurementUnitId.value = '';

            txtPopupMeasurementUnitName.value = '';
            txtPopupMeasurementUnitDescription.value = '';
            rfvMeasurementUnitName.style.display = 'none';

            btnAddMeasurementUnit.style.display = 'inline';
            btnEditMeasurementUnit.style.display = 'none';
        }        
    }
}

//disabling popup with jQuery!
function disableMeasurementUnitPopup() {
    //disables popup only if it is enabled
    if (popupMeasurementUnitStatus == 1) {
        $("#backgroundMeasurementUnitPopup").fadeOut("fast");
        $("#popupMeasurementUnitContact").fadeOut("fast");
        popupMeasurementUnitStatus = 0;
    }
}

//centering popup
function centerMeasurementUnitPopup() {
    //request data for centering
    var windowWidth = document.documentElement.clientWidth;
    var windowHeight = document.documentElement.clientHeight;
    var popupHeight = $("#popupMeasurementUnitContact").height();
    var popupWidth = $("#popupMeasurementUnitContact").width();
    //centering
    $("#popupMeasurementUnitContact").css({
        "position": "absolute",
        "top": windowHeight / 2 - popupHeight / 2,
        "left": windowWidth / 2 - popupWidth / 2
    });
    //only need force for IE6

    $("#backgroundMeasurementUnitPopup").css({
        "height": windowHeight
    });
}


//CONTROLLING EVENTS IN jQuery
$(document).ready(function() {
    //CLOSING POPUP
    //Click the x event!
$("#popupMeasurementUnitContactClose").click(function() {
disableMeasurementUnitPopup();
    });
    //Click out event!
    $("#backgroundMeasurementUnitPopup").click(function() {
    disableMeasurementUnitPopup();
    });
    //Press Escape event!
    $(document).keypress(function(e) {
    if (e.keyCode == 27 && popupMeasurementUnitStatus == 1) {
            disableMeasurementUnitPopup();
        }
    });
});
