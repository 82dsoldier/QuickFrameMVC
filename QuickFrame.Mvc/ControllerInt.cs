using QuickFrame.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Mvc
{
	public class ControllerInt<TEntity, TIndex, TEdit>
			: ControllerCore<int, TEntity, TIndex, TEdit>
			where TEntity : IDataModelInt
			where TIndex : IDataTransferObjectInt<TEntity, TIndex>
			where TEdit : IDataTransferObjectInt<TEntity, TEdit> {

		public ControllerInt(IDataServiceInt<TEntity> dataService)
			: base(dataService) {
		}
	}
}
