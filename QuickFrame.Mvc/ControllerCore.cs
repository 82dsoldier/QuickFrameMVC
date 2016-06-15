using Microsoft.AspNetCore.Mvc;
using QuickFrame.Data.Interfaces;
using QuickFrame.Security.Attributes;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using static QuickFrame.Security.AuthorizationExtensions;

namespace QuickFrame.Mvc {
	[Roles(new[] { "SiteUsers"})]
	public class ControllerCore<TDataType, TEntity, TIndex, TEdit>
		 : GenericControllerCore<TDataType, TEntity, TIndex, TEdit>
		 where TEntity : IDataModelCore<TDataType>
		 where TIndex : IDataTransferObjectCore<TDataType, TEntity, TIndex>
		 where TEdit : IDataTransferObjectCore<TDataType, TEntity, TEdit> {

		public ControllerCore(IDataServiceCore<TDataType, TEntity> dataService) 
			: base(dataService) {
		}
	}
}