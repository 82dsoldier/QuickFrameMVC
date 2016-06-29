using QuickFrame.Data.Interfaces;

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
