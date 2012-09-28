$(document).keydown(function (e) {
    e = e ? e : window.event;
    var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
    if (keyCode == 13) {
//        alert('searchKey----' + searchKey
//            + '\n' + 'addKey-------' + addKey
//            + '\n' + 'editKey------' + editKey
//            + '\n' + 'deleteKey----' + deleteKey);
        if (searchKey == true) {
            searchKey = false;
            GetQueryParams();
        }
        if (addKey == true || editKey == true || deleteKey == true) {
            addKey = false;
            editKey = false;
            deleteKey = false;
            save();
        }
    }
    if (keyCode == 27) {
//        alert('searchKey----' + searchKey
//            + '\n' + 'addKey-------' + addKey
//            + '\n' + 'editKey------' + editKey
//            + '\n' + 'deleteKey----' + deleteKey);
        if (searchKey == true) {
            searchKey = false;
            searchDialog.dialog('close');
        }
        if (addKey == true || editKey == true || deleteKey == true) {
            addKey = false;
            editKey = false;
            deleteKey = false;
            addDialog.dialog('close');
        }
    }
});