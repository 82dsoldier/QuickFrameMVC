using QuickFrame.Data.Interfaces;

namespace QuickFrame.Mvc {

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