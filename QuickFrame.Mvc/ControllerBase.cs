using Microsoft.AspNet.Mvc;
using QuickFrame.Data.Interfaces;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using static QuickFrame.Security.AuthorizationExtensions;

namespace QuickFrame.Mvc {
	public class ControllerBase<TEntity, TIndex, TEdit>
	: ControllerInt<TEntity, TIndex, TEdit>
	where TEntity : IDataModelInt
	where TIndex : IDataTransferObjectInt<TEntity, TIndex>
	where TEdit : IDataTransferObjectInt<TEntity, TEdit> {

		public ControllerBase(IDataServiceInt<TEntity> dataService)
			: base(dataService) {
		}
	}
}