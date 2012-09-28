$(document).keydown(function (e) {
    e = e ? e : window.event;
    var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
    if (keyCode == 13) {
        //alert('searchKey--' + searchKey + '\n' + 'addKey--' + addKey + '\n' + 'editKey--' + editKey + '\n' + 'deleteKey--' + deleteKey);
        if (searchKey == true) {
            searchKey = false;
            if (module == "organization" || module == "stockIntoInBill") {
                GetQueryParams();
            }
            if (module == "warehouse" || module == "cigarette" || module == "cigaretteUnitList" || module == "stockInto"
             || module == "stockMove" || module == "checkBill" || module == "checkBillCb" || module == "stockDiffer"
             || module == "warehouseM" || module == "sort" || module == "sortLl" || module == "research"
             || module == "researchPw") {
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
            if (module == "organization" || module == "warehouse" || module == "cigarette" || module == "cigaretteUnitList"
             || module == "stockInto" || module == "stockIntoInBill" || module == "stockMove" || module == "checkBill"
             || module == "stockDiffer" || module == "sort" || module == "sortLl" || module == "researchPw") {
                save();
            }
            if (module == "warehouseProductSet") {
                enterSave();
            }
        }
        if (downloadKey == true) {
            downloadKey = false;
            if (module == "sortLl") {
                generateCheckClick();
            }
        }
    }
    if (keyCode == 27) {
        //alert('searchKey--' + searchKey + '\n' + 'addKey--' + addKey + '\n' + 'editKey--' + editKey + '\n' + 'deleteKey--' + deleteKey);
        if (searchKey == true) {
            searchKey = false;
            if (module == "organization" || module == "stockIntoInBill") {
                searchDialog.dialog('close');
            }
            if (module == "warehouse" || module == "cigarette" || module == "cigaretteUnitList" || module == "stockInto"
             || module == "stockIntoInBill" || module == "stockMove" || module == "checkBill" || module == "checkBillCb"
             || module == "stockDiffer" || module == "warehouseM" || module == "sortLl" || module == "research" 
             || module == "researchPw") {
                $('#dlg-search').dialog('close');
            }
            if (module == "warehouseProductSet") {
                $('#dlgSearch').dialog('close');
            }
            if (module == "sort") {
                $('#searchdlg').dialog('close');
            }
        }
        if (addKey == true || editKey == true || deleteKey == true) {
            addKey = false;
            editKey = false;
            deleteKey = false;
            if (module == "organization") {
                addDialog.dialog('close');
            }
            if (module == "warehouse" || module == "warehouseProductSet" || module == "cigarette" || module == "stockInto"
             || module == "stockIntoInBill" || module == "stockMove" || module == "checkBill" || module == "stockDiffer"
             || module == "sort" || module == "sortLl" || module == "researchPw") {
                $('#dlg').dialog('close');
            }
            if (module == "cigaretteUnitList") {
                $('#dlg-add').dialog('close');
            }
            if (module == "checkBillCb") {
                $('#addCheck').dialog('close');
            }
        }
    }
});