using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using QuickFrame.Data.Interfaces;

namespace QuickFrame.Mvc {

	public abstract class ControllerLong<TEntity, TIndex, TEdit>
		: ControllerCore<long, TEntity, TIndex, TEdit>
		where TEntity : IDataModelLong
		where TIndex : IDataTransferObjectLong<TEntity, TIndex>
		where TEdit : IDataTransferObjectLong<TEntity, TEdit> {

		public ControllerLong(IDataServiceLong<TEntity> dataService)
			: base(dataService) {
		}
	}
}