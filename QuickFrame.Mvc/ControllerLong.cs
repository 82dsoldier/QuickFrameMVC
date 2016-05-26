using QuickFrame.Data.Interfaces;

namespace QuickFrame.Mvc {

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