using QuickFrame.Data.Interfaces;

namespace QuickFrame.Mvc {

	public abstract class ControllerCore<TDataType, TEntity, TIndex, TEdit>
		 : GenericControllerCore<TDataType, TEntity, TIndex, TEdit>
		 where TEntity : IDataModel<TDataType>
		 where TIndex : IDataTransferObject<TDataType, TEntity, TIndex>
		 where TEdit : IDataTransferObject<TDataType, TEntity, TEdit> {

		public ControllerCore(IDataService<TDataType, TEntity> dataService)
			: base(dataService) {
		}
	}

	public abstract class ControllerCore<TEntity, TIndex, TEdit>
		: ControllerInt<TEntity, TIndex, TEdit>
		where TEntity : IDataModelInt
		where TIndex : IDataTransferObjectInt<TEntity, TIndex>
		where TEdit : IDataTransferObjectInt<TEntity, TEdit> {

		public ControllerCore(IDataServiceInt<TEntity> dataService)
			: base(dataService) {
		}
	}
}