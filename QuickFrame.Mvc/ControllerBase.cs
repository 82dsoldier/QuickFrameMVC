using Microsoft.AspNetCore.Mvc;
using QuickFrame.Data.Interfaces;
using System.Data.SqlClient;

namespace QuickFrame.Mvc {

	public abstract class ControllerBase<TEntity, TIndex, TEdit>
	: ControllerInt<TEntity, TIndex, TEdit>
	where TEntity : IDataModelInt
	where TIndex : IDataTransferObjectInt<TEntity, TIndex>
	where TEdit : IDataTransferObjectInt<TEntity, TEdit> {

		public ControllerBase(IDataServiceInt<TEntity> dataService)
			: base(dataService) {
		}
	}
}
