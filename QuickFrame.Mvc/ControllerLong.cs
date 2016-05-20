using QuickFrame.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Mvc
{
	public class ControllerLong<TEntity, TIndex, TEdit>
		: ControllerCore<long, TEntity, TIndex, TEdit>
		where TEntity : IDataModelLong
		where TIndex : IDataTransferObjectLong<TEntity, TIndex>
		where TEdit : IDataTransferObjectLong<TEntity, TEdit> {

		public ControllerLong(IDataServiceLong<TEntity> dataService)
			: base(dataService) {
		}
	}
}
