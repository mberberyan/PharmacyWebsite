/*
Author: Melon Team 
Date: 01/04/2010
Description: Unselect all dynamic properties values
*/
function ClearAllFeatureValues(objFeatureTable) 
{
    var j = 0;
    var chk = document.getElementById(getName('table', objFeatureTable)).getElementsByTagName('select');
    
    for (i = 0; i < chk.length; i++) 
    {
        chk[i].selectedIndex = 0;
    }
}

/*
Author: Melon Team 
Date: 03/25/2010
Description: Set max length to textarea control
Remarks: User cannot enter more then 'MaxLen' chars in textarea control 'Object'
*/
function imposeMaxLength(Object, MaxLen)
{
  return (Object.value.length <= MaxLen);
}
