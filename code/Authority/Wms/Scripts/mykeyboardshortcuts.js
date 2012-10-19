$(document).keydown(function (e) {
    e = e ? e : window.event;
    var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
    if (keyCode == 13) {
        /*----------------- enter ����ҳ -------------------*/
        if (searchKey == true) {
            searchKey = false;
            switch (module) {
                //��֯�ṹ����
                case "Company":
                case "Department":
                case "Employee":
                case "Job":
                    GetQueryParams();
                    break;
                //�ֿ���Ϣ����
                case "Warehouse2":
                case "DefaultProductSet":
                    select();
                    break;
                //������Ϣ����
                case "Product":
                case "Supplier":
                case "Brand":
                case "UnitList":
                case "Unit":
                //��ⵥ�ݹ���
                case "StockInBillType":
                    select();
                    break;
                case "StockInBill":
                    GetQueryParams()
                    break;
                //���ⵥ�ݹ���
                case "StockOutBillType":
                case "StockOutBill":
                //�ƿⵥ�ݹ���
                case "StockMoveBillType":
                case "StockMoveBill":
                //�̵㵥�ݹ���
                case "CheckBillType":
                case "CheckBill":
                //���浥�ݹ���
                case "ProfitLossBillType":
                case "ProfitLossBill":
                //�����Ϣ����
                case "DailyBalance":
                case "CurrentStock":
                case "Distribution":
                case "Cargospace":
                case "StockLedger":
                case "HistoricalDetail":
                case "CellHistorical":
                //�ּ���Ϣ����
                case "SortingLine":
                case "SortingLowerLimit":
                case "SortingOrder":
                case "SortOrderDispatch":
                case "SortWorkDispatch":
                //�ۺ����ݲ�ѯ
                case "StockIntoSearch":
                case "StockOutSearch":
                case "StockMoveSearch":
                case "StockCheckSearch":
                case "StockDifferSearch":
                case "SortOrderSearch":
                //��Ʒ��������
                case "ProductWarning":
                case "QuantityLimits":
                case "ProductTimeOut":
                    select();
                    break;
            }
        }
        /*----------------- enter ����ɾ���� ҳ -------------------*/
        if (addKey == true || editKey == true || deleteKey == true) {
            deleteKey = false;
            switch (module) {
                //��֯�ṹ����
                case "Company":
                case "Department":
                case "Employee":
                case "Job":
                //�ֿ���Ϣ����
                case "Warehouse2":
                    save();
                    break;
                case "DefaultProductSet":
                    enterSave();
                    break;
                //������Ϣ����
                case "Product":
                case "Supplier":
                case "Brand":
                case "UnitList":
                case "Unit":
                //��ⵥ�ݹ���
                case "StockInBillType":
                case "StockInBill":
                //���ⵥ�ݹ���
                case "StockOutBillType":
                case "StockOutBill":
                //�ƿⵥ�ݹ���
                case "StockMoveBillType":
                case "StockMoveBill":
                //�̵㵥�ݹ���
                case "CheckBillType":
                case "CheckBill":
                //���浥�ݹ���
                case "ProfitLossBillType":
                case "ProfitLossBill":
                //�����Ϣ����
                case "DailyBalance":
                case "CurrentStock":
                case "Distribution":
                case "Cargospace":
                case "StockLedger":
                case "HistoricalDetail":
                case "CellHistorical":
                //�ּ���Ϣ����
                case "SortingLine":
                case "SortingLowerLimit":
                case "SortingOrder":
                case "SortOrderDispatch":
                case "SortWorkDispatch":
                //�ۺ����ݲ�ѯ
                case "StockIntoSearch":
                case "StockOutSearch":
                case "StockMoveSearch":
                case "StockCheckSearch":
                case "StockDifferSearch":
                case "SortOrderSearch":
                //��Ʒ��������
                case "ProductWarning":
                case "QuantityLimits":
                case "ProductTimeOut":
                    save();
                    break;
            }
        }
        if (productKey = true) {
            switch (module) {
                case "SortingLowerLimit":
                    ProductQueryClick();
                    break;
            }
        }
    }
    if (keyCode == 27) {
        /*------------------ esc ����ҳ -------------------*/
        if (searchKey == true) {
            searchKey = false;
            switch (module) {
                //��֯�ṹ����
                case "Company":
                case "Department":
                case "Employee":
                case "Job":
                    searchDialog.dialog('close');
                    break;
                //�ֿ���Ϣ����
                case "Warehouse2":
                case "DefaultProductSet":
                    $('#dlgSearch').dialog('close')
                    break;
                //������Ϣ����
                case "Product":
                case "Supplier":
                case "Brand":
                case "UnitList":
                case "Unit":
                //��ⵥ�ݹ���
                case "StockInBillType":
                    $('#dlg-search').dialog('close');
                    break;
                case "StockInBill":
                    searchDialog.dialog('close')
                    break;
                //���ⵥ�ݹ���
                case "StockOutBillType":
                case "StockOutBill":
                //�ƿⵥ�ݹ���
                case "StockMoveBillType":
                case "StockMoveBill":
                //�̵㵥�ݹ���
                case "CheckBillType":
                case "CheckBill":
                //���浥�ݹ���
                case "ProfitLossBillType":
                case "ProfitLossBill":
                //�����Ϣ����
                case "DailyBalance":
                case "CurrentStock":
                case "Distribution":
                case "Cargospace":
                case "StockLedger":
                case "HistoricalDetail":
                case "CellHistorical":
                    $('#dlg-search').dialog('close');
                    break;
                //�ּ���Ϣ����
                case "SortingLine":
                    $('#searchdlg').dialog('close');
                    break;
                case "SortingLowerLimit":
                case "SortingOrder":
                    $('#dlg-search').dialog('close');
                    break;
                case "SortOrderDispatch":
                case "SortWorkDispatch":
                    $('#searchdlg').dialog('close');
                    break;
                //�ۺ����ݲ�ѯ
                case "StockIntoSearch":
                    $('#dlg-search').dialog('close');
                    break;
                case "StockOutSearch":
                case "StockMoveSearch":
                case "StockCheckSearch":
                case "StockDifferSearch":
                case "SortOrderSearch":
                //��Ʒ��������
                case "ProductWarning":
                case "QuantityLimits":
                case "ProductTimeOut":
                    $('#dlg-search').dialog('close');
                    break;
            }
        }
        /*----------------- esc ����ɾ���� ҳ -------------------*/
        if (addKey == true || editKey == true || deleteKey == true) {
            addKey = false;
            editKey = false;
            deleteKey = false;
            switch (module) {
                //��֯�ṹ����
                case "Company":
                case "Department":
                case "Employee":
                case "Job":
                    addDialog.dialog('close');
                    break;
                //�ֿ���Ϣ����
                case "Warehouse2":
                case "DefaultProductSet":
                //������Ϣ����
                case "Product":
                case "Supplier":
                case "Brand":
                    $('#dlg').dialog('close');
                    break;
                case "UnitList":
                    $('#dlg-add').dialog('close');
                    break;
                case "Unit":
                //��ⵥ�ݹ���
                case "StockInBillType":
                case "StockInBill":
                //���ⵥ�ݹ���
                case "StockOutBillType":
                case "StockOutBill":
                //�ƿⵥ�ݹ���
                case "StockMoveBillType":
                case "StockMoveBill":
                //�̵㵥�ݹ���
                case "CheckBillType":
                    $('#dlg').dialog('close');
                    break;
                case "CheckBill":
                    $('#addCheck').dialog('close');
                    break;
                //���浥�ݹ���
                case "ProfitLossBillType":
                case "ProfitLossBill":
                //�����Ϣ����
                case "DailyBalance":
                case "CurrentStock":
                case "Distribution":
                case "Cargospace":
                case "StockLedger":
                case "HistoricalDetail":
                case "CellHistorical":
                //�ּ���Ϣ����
                case "SortingLine":
                case "SortingLowerLimit":
                case "SortingOrder":
                case "SortOrderDispatch":
                case "SortWorkDispatch":
                //�ۺ����ݲ�ѯ
                case "StockIntoSearch":
                case "StockOutSearch":
                case "StockMoveSearch":
                case "StockCheckSearch":
                case "StockDifferSearch":
                case "SortOrderSearch":
                //��Ʒ��������
                case "ProductWarning":
                case "QuantityLimits":
                case "ProductTimeOut":
                    $('#dlg').dialog('close');
                    break;
            }
        }
    }
});