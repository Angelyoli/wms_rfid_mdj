using System;
using System.Collections.Generic;
using System.Collections;
using DataRabbit;

namespace DataRabbit.HashOrm
{
	[Serializable]
	public partial class Storages : IEntity<System.String>,IHashEntity
	{
	    #region Force Static Check
		public const string TableName = "Storages" ;
		public const string _StorageID = "StorageID" ;
		public const string _StorageName = "StorageName" ;
		public const string _Row = "Row" ;
		public const string _Address = "Address" ;
		public const string _Port = "Port" ;
		public const string _Act = "Act" ;
		public const string _ProductName = "ProductName" ;
		public const string _Contents = "Contents" ;
		public const string _Sign = "Sign" ;
		public const string _Err = "Err" ;
	    #endregion
	 
	    #region Property
	
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
	
		#region StorageName
		private System.String m_StorageName = "" ; 
		public System.String StorageName
		{
			get
			{
				if (this._PropertySet.ContainsKey(_StorageName))
				{
					return (System.String)this._PropertySet[_StorageName];
				}
				return this.m_StorageName;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_StorageName))
				{
					this._PropertySet.Remove(_StorageName);
				}
				this._PropertySet.Add(_StorageName, value);
				this.m_StorageName = value ;
			}
		}
		#endregion
	
		#region Row
		private System.String m_Row = "" ; 
		public System.String Row
		{
			get
			{
				if (this._PropertySet.ContainsKey(_Row))
				{
					return (System.String)this._PropertySet[_Row];
				}
				return this.m_Row;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_Row))
				{
					this._PropertySet.Remove(_Row);
				}
				this._PropertySet.Add(_Row, value);
				this.m_Row = value ;
			}
		}
		#endregion
	
		#region Address
		private System.String m_Address = "" ; 
		public System.String Address
		{
			get
			{
				if (this._PropertySet.ContainsKey(_Address))
				{
					return (System.String)this._PropertySet[_Address];
				}
				return this.m_Address;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_Address))
				{
					this._PropertySet.Remove(_Address);
				}
				this._PropertySet.Add(_Address, value);
				this.m_Address = value ;
			}
		}
		#endregion
	
		#region Port
		private System.String m_Port = "" ; 
		public System.String Port
		{
			get
			{
				if (this._PropertySet.ContainsKey(_Port))
				{
					return (System.String)this._PropertySet[_Port];
				}
				return this.m_Port;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_Port))
				{
					this._PropertySet.Remove(_Port);
				}
				this._PropertySet.Add(_Port, value);
				this.m_Port = value ;
			}
		}
		#endregion
	
		#region Act
		private System.String m_Act = "" ; 
		public System.String Act
		{
			get
			{
				if (this._PropertySet.ContainsKey(_Act))
				{
					return (System.String)this._PropertySet[_Act];
				}
				return this.m_Act;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_Act))
				{
					this._PropertySet.Remove(_Act);
				}
				this._PropertySet.Add(_Act, value);
				this.m_Act = value ;
			}
		}
		#endregion
	
		#region ProductName
		private System.String m_ProductName = "" ; 
		public System.String ProductName
		{
			get
			{
				if (this._PropertySet.ContainsKey(_ProductName))
				{
					return (System.String)this._PropertySet[_ProductName];
				}
				return this.m_ProductName;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_ProductName))
				{
					this._PropertySet.Remove(_ProductName);
				}
				this._PropertySet.Add(_ProductName, value);
				this.m_ProductName = value ;
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
	
		#region Sign
		private System.Int32 m_Sign = 0 ; 
		public System.Int32 Sign
		{
			get
			{
				if (this._PropertySet.ContainsKey(_Sign))
				{
					return (System.Int32)this._PropertySet[_Sign];
				}
				return this.m_Sign;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_Sign))
				{
					this._PropertySet.Remove(_Sign);
				}
				this._PropertySet.Add(_Sign, value);
				this.m_Sign = value ;
			}
		}
		#endregion
	
		#region Err
		private System.Int32 m_Err = 0 ; 
		public System.Int32 Err
		{
			get
			{
				if (this._PropertySet.ContainsKey(_Err))
				{
					return (System.Int32)this._PropertySet[_Err];
				}
				return this.m_Err;
			}
			set
			{
				if (this._PropertySet.ContainsKey(_Err))
				{
					this._PropertySet.Remove(_Err);
				}
				this._PropertySet.Add(_Err, value);
				this.m_Err = value ;
			}
		}
		#endregion
	    #endregion
	 
	    #region IEntity 成员
	    public System.String GetPKeyValue()
	    {
	       return this.StorageID;
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
			get { return _StorageID;}
		}
		public string[] ColNames
		{
			get
			{
				string[] strs = {_StorageName,_Row,_Address,_Port,_Act,_ProductName,_Contents,_Sign,_Err};
				return strs;
			}
		}
		#endregion
	}
}
