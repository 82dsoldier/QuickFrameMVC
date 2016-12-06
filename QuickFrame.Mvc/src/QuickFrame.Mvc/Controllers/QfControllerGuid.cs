using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Services;
using QuickFrame.Security;
using System;

namespace QuickFrame.Mvc.Controllers {

	public class QfControllerGuid<TEntity, TIndex>
		: QfControllerCore<TEntity, Guid, TIndex>
		where TEntity : class, new()
		where TIndex : class, IDataTransferObject<Guid> {

		public QfControllerGuid(IDataServiceCore<TEntity, Guid> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}
	}

	public class QfControllerGuid<TEntity, TIndex, TEdit>
		: QfControllerCore<TEntity, Guid, TIndex, TEdit>
		where TEntity : class, new()
		where TIndex : class, IDataTransferObjectGuid
		where TEdit : class, IDataTransferObjectGuid {

		public QfControllerGuid(IDataServiceCore<TEntity, Guid> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}
	}
}