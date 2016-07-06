using Microsoft.AspNetCore.Mvc;
using QuickFrame.Data.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuickFrame.Mvc {

	public abstract class ControllerInt<TEntity, TIndex, TEdit>
			: ControllerCore<int, TEntity, TIndex, TEdit>
			where TEntity : IDataModelInt
			where TIndex : IDataTransferObjectInt<TEntity, TIndex>
			where TEdit : IDataTransferObjectInt<TEntity, TEdit> {

		public ControllerInt(IDataServiceInt<TEntity> dataService)
			: base(dataService) {
		}
	}
}
