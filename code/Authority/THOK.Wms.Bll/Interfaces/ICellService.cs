﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ICellService : IService<Cell>
    {
        object GetDetails(int page, int rows, string cellCode);

        object GetDetails(int page, int rows, string queryString, string value);

        object  GetDetail(int page, int rows, string type, string id);

        bool Add(Cell cell, out string errorInfo);

        bool Delete(string cellCode);

        bool Save(Cell cell);

        object GetWareCheck(string shelfCode);

        object GetSearch(string wareCode);

        object FindCell(string parameter);

        object GetCell(string shelfCode);

        object GetCellCode(string shelfCode);

        object GetMoveCellDetails(string shelfCode, string inOrOut, string productCode);

        bool SaveCell(string wareCodes, string areaCodes, string shelfCodes, string cellCodes, string defaultProductCode, string editType);

        object GetCellInfo(int page,int rows);

        object GetCellInfo(string productCode);

        object GetSortCell(string areaType);

        object GetCellCheck(string productCode);

        bool DeleteCell(string productCodes);

        object GetCellBy(int page, int rows, string QueryString, string Value);

        System.Data.DataTable GetProductCell(string queryString, string value);

        System.Data.DataTable GetCell(int page, int rows, string type, string id);

        System.Data.DataTable GetCellByE(int page, int rows, string QueryString, string Value);

        bool uploadCell();

        object GetCellDetail(string shelfCode);

        object GetSplitPalletCell(int page, int rows, string productCode,string shelfType);
        bool SaveSplitPalletCell(Cell cell, out string strResult);
    }
}
