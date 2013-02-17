using System;
using System.Collections.Generic;
using DataRabbit;

namespace ELDB2
{
	[Serializable]
	public partial class Sy_ShowInfo : IEntity<System.Int32>
	{
	    #region Force Static Check
		public const string TableName = "Sy_ShowInfo" ;
		public const string _id = "id" ;
		public const string _OrderMasterID = "OrderMasterID" ;
		public const string _OrderDetailID = "OrderDetailID" ;
		public const string _StorageID = "StorageID" ;
		public const string _OperateType = "OperateType" ;
		public const string _TobaccoName = "TobaccoName" ;
		public const string _OperatePiece = "OperatePiece" ;
		public const string _OperateItem = "OperateItem" ;
		public const string _ConfirmState = "ConfirmState" ;
		public const string _ReadState = "ReadState" ;
		public const string _HardwareReadState = "HardwareReadState" ;
		public const string _ManyLine = "ManyLine" ;
	    #endregion
	 
	    #region Property
	
		#region Id
		private System.Int32 m_Id = 0 ; 
		public System.Int32 id
		{
			get
			{
				return this.m_Id ;
			}
			set
			{
				this.m_Id = value ;
			}
		}
		#endregion
	
		#region OrderMasterID
		private System.String m_OrderMasterID = "" ; 
		public System.String OrderMasterID
		{
			get
			{
				return this.m_OrderMasterID ;
			}
			set
			{
				this.m_OrderMasterID = value ;
			}
		}
		#endregion
	
		#region OrderDetailID
		private System.String m_OrderDetailID = "" ; 
		public System.String OrderDetailID
		{
			get
			{
				return this.m_OrderDetailID ;
			}
			set
			{
				this.m_OrderDetailID = value ;
			}
		}
		#endregion
	
		#region StorageID
		private System.String m_StorageID = "" ; 
		public System.String StorageID
		{
			get
			{
				return this.m_StorageID ;
			}
			set
			{
				this.m_StorageID = value ;
			}
		}
		#endregion
	
		#region OperateType
		private System.Int32 m_OperateType = 0 ; 
		public System.Int32 OperateType
		{
			get
			{
				return this.m_OperateType ;
			}
			set
			{
				this.m_OperateType = value ;
			}
		}
		#endregion
	
		#region TobaccoName
		private System.String m_TobaccoName = "" ; 
		public System.String TobaccoName
		{
			get
			{
				return this.m_TobaccoName ;
			}
			set
			{
				this.m_TobaccoName = value ;
			}
		}
		#endregion
	
		#region OperatePiece
		private System.Int32 m_OperatePiece = 0 ; 
		public System.Int32 OperatePiece
		{
			get
			{
				return this.m_OperatePiece ;
			}
			set
			{
				this.m_OperatePiece = value ;
			}
		}
		#endregion
	
		#region OperateItem
		private System.Int32 m_OperateItem = 0 ; 
		public System.Int32 OperateItem
		{
			get
			{
				return this.m_OperateItem ;
			}
			set
			{
				this.m_OperateItem = value ;
			}
		}
		#endregion
	
		#region ConfirmState
		private System.Int32 m_ConfirmState = 0 ; 
		public System.Int32 ConfirmState
		{
			get
			{
				return this.m_ConfirmState ;
			}
			set
			{
				this.m_ConfirmState = value ;
			}
		}
		#endregion
	
		#region ReadState
		private System.Int32 m_ReadState = 0 ; 
		public System.Int32 ReadState
		{
			get
			{
				return this.m_ReadState ;
			}
			set
			{
				this.m_ReadState = value ;
			}
		}
		#endregion
	
		#region HardwareReadState
		private System.Int32 m_HardwareReadState = 0 ; 
		public System.Int32 HardwareReadState
		{
			get
			{
				return this.m_HardwareReadState ;
			}
			set
			{
				this.m_HardwareReadState = value ;
			}
		}
		#endregion
	
		#region ManyLine
		private System.String m_ManyLine = "" ; 
		public System.String ManyLine
		{
			get
			{
				return this.m_ManyLine ;
			}
			set
			{
				this.m_ManyLine = value ;
			}
		}
		#endregion
	    #endregion
	 
	    #region IEntity 成员
	    public System.Int32 GetPKeyValue()
	    {
	       return this.id;
	    }
	    #endregion
		
		#region ToString 
		public override string ToString()
		{
			return this.id.ToString()  + " " + this.TobaccoName.ToString() ;
		}
		#endregion
	}
}
