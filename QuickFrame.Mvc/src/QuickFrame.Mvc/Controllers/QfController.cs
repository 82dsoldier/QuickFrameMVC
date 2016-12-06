using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Services;
using QuickFrame.Security;

namespace QuickFrame.Mvc.Controllers {

	public class QfController<TEntity, TIndex>
		: QfControllerInt<TEntity, TIndex>
		where TEntity : class, new()
		where TIndex : class, IDataTransferObjectInt {

		public QfController(IDataServiceCore<TEntity, int> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}
	}

	public class QfController<TEntity, TIndex, TEdit>
		: QfControllerInt<TEntity, TIndex, TEdit>
		where TEntity : class, new()
		where TIndex : class, IDataTransferObject<int>
		where TEdit : class, IDataTransferObject<int> {

		public QfController(IDataServiceCore<TEntity, int> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}
	}
}