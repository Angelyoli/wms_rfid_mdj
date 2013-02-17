using System;
using System.Collections.Generic;
using System.Collections;
using DataRabbit;

namespace DataRabbit.HashOrm
{
	[Serializable]
	public partial class Sy_ShowInfo : IEntity<System.Int32>,IHashEntity
	{
	    #region Force Static Check
		public const string TableName = "Sy_ShowInfo" ;
		public const string _Id = "id" ;
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
		public const string _Contents = "Contents" ;
	    #endregion
	 
	    #region Property
	
		#region Id
		private System.Int32 m_Id = 0 ; 
		public System.Int32 Id
		{
			get
			{
				if (this._PropertySet.ContainsKey(_Id))
				{
					return (System.Int32)this._PropertySet[_Id];
				}
				return this.m_Id;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_Id))
				{
					this._PropertySet.Remove(_Id);
				}
				this._PropertySet.Add(_Id, value);
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
				if (this._PropertySet.ContainsKey(_OrderMasterID))
				{
					return (System.String)this._PropertySet[_OrderMasterID];
				}
				return this.m_OrderMasterID;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_OrderMasterID))
				{
					this._PropertySet.Remove(_OrderMasterID);
				}
				this._PropertySet.Add(_OrderMasterID, value);
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
				if (this._PropertySet.ContainsKey(_OrderDetailID))
				{
					return (System.String)this._PropertySet[_OrderDetailID];
				}
				return this.m_OrderDetailID;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_OrderDetailID))
				{
					this._PropertySet.Remove(_OrderDetailID);
				}
				this._PropertySet.Add(_OrderDetailID, value);
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
				if (this._PropertySet.ContainsKey(_StorageID))
				{
					return (System.String)this._PropertySet[_StorageID];
				}
				return this.m_StorageID;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_StorageID))
				{
					this._PropertySet.Remove(_StorageID);
				}
				this._PropertySet.Add(_StorageID, value);
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
				if (this._PropertySet.ContainsKey(_OperateType))
				{
					return (System.Int32)this._PropertySet[_OperateType];
				}
				return this.m_OperateType;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_OperateType))
				{
					this._PropertySet.Remove(_OperateType);
				}
				this._PropertySet.Add(_OperateType, value);
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
				if (this._PropertySet.ContainsKey(_TobaccoName))
				{
					return (System.String)this._PropertySet[_TobaccoName];
				}
				return this.m_TobaccoName;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_TobaccoName))
				{
					this._PropertySet.Remove(_TobaccoName);
				}
				this._PropertySet.Add(_TobaccoName, value);
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
				if (this._PropertySet.ContainsKey(_OperatePiece))
				{
					return (System.Int32)this._PropertySet[_OperatePiece];
				}
				return this.m_OperatePiece;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_OperatePiece))
				{
					this._PropertySet.Remove(_OperatePiece);
				}
				this._PropertySet.Add(_OperatePiece, value);
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
				if (this._PropertySet.ContainsKey(_OperateItem))
				{
					return (System.Int32)this._PropertySet[_OperateItem];
				}
				return this.m_OperateItem;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_OperateItem))
				{
					this._PropertySet.Remove(_OperateItem);
				}
				this._PropertySet.Add(_OperateItem, value);
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
				if (this._PropertySet.ContainsKey(_ConfirmState))
				{
					return (System.Int32)this._PropertySet[_ConfirmState];
				}
				return this.m_ConfirmState;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_ConfirmState))
				{
					this._PropertySet.Remove(_ConfirmState);
				}
				this._PropertySet.Add(_ConfirmState, value);
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
				if (this._PropertySet.ContainsKey(_ReadState))
				{
					return (System.Int32)this._PropertySet[_ReadState];
				}
				return this.m_ReadState;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_ReadState))
				{
					this._PropertySet.Remove(_ReadState);
				}
				this._PropertySet.Add(_ReadState, value);
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
				if (this._PropertySet.ContainsKey(_HardwareReadState))
				{
					return (System.Int32)this._PropertySet[_HardwareReadState];
				}
				return this.m_HardwareReadState;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_HardwareReadState))
				{
					this._PropertySet.Remove(_HardwareReadState);
				}
				this._PropertySet.Add(_HardwareReadState, value);
				this.m_HardwareReadState = value ;
			}
		}
		#endregion
	
		#region Contents
		private System.String m_Contents = "" ; 
		public System.String Contents
		{
			get
			{
				if (this._PropertySet.ContainsKey(_Contents))
				{
					return (System.String)this._PropertySet[_Contents];
				}
				return this.m_Contents;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_Contents))
				{
					this._PropertySet.Remove(_Contents);
				}
				this._PropertySet.Add(_Contents, value);
				this.m_Contents = value ;
			}
		}
		#endregion
	    #endregion
	 
	    #region IEntity 成员
	    public System.Int32 GetPKeyValue()
	    {
	       return this.Id;
	    }
	    #endregion
	
		#region IHashEntity 成员
	    private Hashtable _PropertySet = new Hashtable();
	    public Hashtable PropertySet
	    {
	    	get { return _PropertySet; }
	    	set { PropertySet = value; }
	    }
		string IHashEntity.TableName
		{
			get
			{
				return TableName;
			}
		}
		public string PKeyName
		{
			get { return _Id;}
		}
		public string[] ColNames
		{
			get
			{
				string[] strs = {_Id,_OrderMasterID,_OrderDetailID,_StorageID,_OperateType,_TobaccoName,_OperatePiece,_OperateItem,_ConfirmState,_ReadState,_HardwareReadState,_Contents};
				return strs;
			}
		}
		#endregion
	}
}
