$(document).keydown(function (e) {
    e = e ? e : window.event;
    var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
    if (keyCode == 13) {
        //alert('searchKey--' + searchKey + '\n' + 'addKey--' + addKey + '\n' + 'editKey--' + editKey + '\n' + 'deleteKey--' + deleteKey);
        if (searchKey == true) {
            searchKey = false;
            if (module == "organization") {
                GetQueryParams();
            }
            if (module == "warehouse" || module == "cigarette" || module == "cigaretteUnitList") {
                select();
            }
            if (module == "warehouseProductSet") {
                searchResult();
            }
        }
        if (addKey == true || editKey == true || deleteKey == true) {
            addKey = false;
            editKey = false;
            deleteKey = false;
            if (module == "organization" || module == "warehouse" || module == "cigarette" || module == "cigaretteUnitList") {
                save();
            }
            if (module == "warehouseProductSet") {
                enterSave();
            }
        }
    }
    if (keyCode == 27) {
        //alert('searchKey--' + searchKey + '\n' + 'addKey--' + addKey + '\n' + 'editKey--' + editKey + '\n' + 'deleteKey--' + deleteKey);
        if (searchKey == true) {
            searchKey = false;
            if (module == "organization") {
                searchDialog.dialog('close');
            }
            if (module == "warehouse" || module == "cigarette" || module == "cigaretteUnitList") {
                $('#dlg-search').dialog('close');
            }
            if (module == "warehouseProductSet") {
                $('#dlgSearch').dialog('close');
            }
        }
        if (addKey == true || editKey == true || deleteKey == true) {
            addKey = false;
            editKey = false;
            deleteKey = false;
            if (module == "organization") {
                addDialog.dialog('close');
            }
            if (module == "warehouse" || module == "warehouseProductSet" || module == "cigarette") {
                $('#dlg').dialog('close');
            }
            if (module == "cigaretteUnitList") {
                $('#dlg-add').dialog('close');
            }
        }
    }
});