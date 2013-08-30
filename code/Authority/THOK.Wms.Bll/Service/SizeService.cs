using System;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;
using System.Linq;

namespace THOK.Wms.Bll.Service
{
    public class SizeService : ServiceBase<Size>, ISizeService
    {
        [Dependency]
        public ISizeRepository SizeRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, Size size)
        {
            IQueryable<Size> sizeQuery = SizeRepository.GetQueryable();
            var SizeDetail = sizeQuery.Where(s => s.SizeName.Contains(size.SizeName)).OrderBy(s => s.ID);

            var SizeDetail1 = SizeDetail;
            if (size.SizeNo != null && size.SizeNo != 0)
            {
                SizeDetail1 = SizeDetail.Where(s => s.SizeNo == size.SizeNo).OrderBy(s => s.ID);
            }

            int total = SizeDetail.Count();
            var sRMDetails = SizeDetail1.Skip((page - 1) * rows).Take(rows);
            var sRM_Detail = sRMDetails.ToArray().Select(s => new
            {
                s.ID,
                s.SizeName,
                s.Length,
                s.SizeNo,
                s.Width,
                s.Height
            });
            return new { total, rows = sRM_Detail.ToArray() };
        }
           
        public bool Add(Size size)
        {
            var s = new Size();
            s.ID = size.ID;
            s.Height = size.Height;
            s.Length = size.Length;
            s.SizeName = size.SizeName;
            s.SizeNo = size.SizeNo;
            s.Width = size.Width;
            SizeRepository.Add(s);
            SizeRepository.SaveChanges();
            return true;
        }

        public bool Save(Size size)
        {
            var si = SizeRepository.GetQueryable().FirstOrDefault(s => s.ID == size.ID);
            si.ID = size.ID;
            si.Height = size.Height;
            si.Length = size.Length;
            si.Width = size.Width;
            si.SizeNo = size.SizeNo;
            si.SizeName = size.SizeName;
            SizeRepository.SaveChanges();
            return true;
        }

        public bool Delete(int sizeId)
        {
            var size= SizeRepository.GetQueryable().FirstOrDefault(s => s.ID == sizeId);
            if (size != null)
            {
                SizeRepository.Delete(size);
                SizeRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public object GetSize(int page, int rows, string queryString, string value)
        {
            string id = "", sizeName = "";

            if (queryString == "id")
            {
                id = value;
            }
            else
            {
                sizeName = value;
            }
            IQueryable<Size> sizeQuery = SizeRepository.GetQueryable();
            //int Id = Convert.ToInt32(id);
            var size = sizeQuery.Where(si => si.SizeName.Contains(sizeName))
                .OrderBy(si => si.ID).AsEnumerable().
                Select(si => new
                {
                    si.ID,
                    si.SizeName,
                    si.SizeNo,
                    si.Length,
                    si.Width,
                    si.Height
                });
            int total = size.Count();
            size = size.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = size.ToArray() };
        }
        public System.Data.DataTable GetSize(int page, int rows, Size size)
        {
            IQueryable<Size> sizeQuery = SizeRepository.GetQueryable();
            var sizeDetail = sizeQuery.Where(s =>
            s.SizeName.Contains(size.SizeName))
            .OrderBy(ul => ul.SizeName);
            var size_Detail = sizeDetail.ToArray().Select(s => new
            {
                s.ID,
                s.SizeName,
                s.SizeNo,
                s.Length,
                s.Width,
                s.Height
            });

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("尺寸ID", typeof(string));
            dt.Columns.Add("尺寸名称", typeof(string));
            dt.Columns.Add("尺寸编号", typeof(string));
            dt.Columns.Add("长度", typeof(string));
            dt.Columns.Add("宽度", typeof(string));
            dt.Columns.Add("高度", typeof(string));
            foreach (var item in size_Detail)
            {
                dt.Rows.Add
                    (
                        item.ID,
                        item.SizeName,
                        item.SizeNo,
                        item.Length,
                        item.Width,
                        item.Height
                    );
            }
            return dt;
        }
    }
}

