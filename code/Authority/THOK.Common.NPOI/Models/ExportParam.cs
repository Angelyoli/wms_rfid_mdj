using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Common.NPOI.Models
{
    public class ExportParam
    {
        #region Variable
        string bigHeadFont = "微软雅黑";
        short bigHeadSize = 20;
        bool bigHeadBorder = true;
        string colHeadFont = "Arial";
        short colHeadSize = 10;
        bool colHeadBorder = true;
        string[] headerFooder = {   
                                     "……"  //眉左
                                    ,"……"  //眉中
                                    ,"……"  //眉右
                                    ,"&D"    //脚左 Datetime
                                    ,"……"  //脚中
                                    ,"&P"    //脚右 Page
                                };
        string streamType = "application/ms-excel";
        //Color sample: NPOI.HSSF.Util.HSSFColor.BLACK.index;
        #endregion

        #region Method
        /// <summary>First excel table sheet1's title</summary>
        public string HeadTitle1 { get; set; }
        /// <summary>First excel table sheet2's title</summary>
        public string HeadTitle2 { get; set; }

        /// <summary>First DataTable</summary>
        public System.Data.DataTable DT1 { get; set; }
        /// <summary>Second DataTable，if you do not give null</summary>
        public System.Data.DataTable DT2 { get; set; }

        /// <summary>Big headline font</summary>
        public string BigHeadFont { get { return bigHeadFont; } set { bigHeadFont = value; } }
        /// <summary>Big headline font size</summary>
        public short BigHeadSize { get { return bigHeadSize; } set { bigHeadSize = value; } }
        /// <summary>Big headline font color</summary>
        public short BigHeadColor { get; set; }
        /// <summary>Big headline whether has border</summary>
        public bool BigHeadBorder { get { return bigHeadBorder; } set { bigHeadBorder = value; } }

        /// <summary>column headline and content font</summary>
        public string ColHeadFont { get { return colHeadFont; } set { colHeadFont = value; } }
        /// <summary>column headline and content font size</summary>
        public short ColHeadSize { get { return colHeadSize; } set { colHeadSize = value; } }
        /// <summary>column headline and content font color</summary>
        public short ColHeadColor { get; set; }
        /// <summary>column headline and content whether have border</summary>
        public bool ColHeadBorder { get { return colHeadBorder; } set { colHeadBorder = value; } }

        /// <summary>content font color</summary>
        public short ContentColor { get; set; }

        /// <summary>Special module content</summary>
        public string ContentModule { get; set; }
        /// <summary>Special module content color</summary>
        public short ContentModuleColor { get; set; }

        /// <summary>header footer:[0]Top left[1]Top middle[2]Top right[3]Bottom left[4]Bottom middle[5]Bottom right</summary>
        public string[] HeaderFooter { get { return headerFooder; } set { headerFooder = value; } }

        /// <summary>MemoryStream type</summary>
        public string StreamType { get { return streamType; } set { streamType = value; } } 
        #endregion
    }
}