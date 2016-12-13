using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Services;
using QuickFrame.Security;

namespace QuickFrame.Mvc.Controllers {

	/// <summary>
	/// An alias for QfControllerInt.
	/// </summary>
	/// <typeparam name="TEntity">The type of entity used by this controller.</typeparam>
	/// <typeparam name="TIndex">The type of class used as a model for the index page.</typeparam>
	public class QfController<TEntity, TIndex>
		: QfControllerInt<TEntity, TIndex>
		where TEntity : class, new()
		where TIndex : class, IDataTransferObjectInt {

		/// <summary>
		/// The constructor for the QfController class.
		/// </summary>
		/// <param name="dataService">The data service used to return data to the views contained within this class.</param>
		/// <param name="securityManager">The <see cref="QuickFrame.Security.QuickFrameSecurityManager"/>  used to apply security to functions within this class.</param>
		public QfController(IDataServiceCore<TEntity, int> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}
	}
	/// <summary>
	/// An alias for QfControllerInt.
	/// </summary>
	/// <typeparam name="TEntity">The type of entity used by this controller.</typeparam>
	/// <typeparam name="TIndex">The type of class used as a model for the index page.</typeparam>
	/// <typeparam name="TEdit">The type of class used as a model for the create/edit page.</typeparam>
	public class QfController<TEntity, TIndex, TEdit>
		: QfControllerInt<TEntity, TIndex, TEdit>
		where TEntity : class, new()
		where TIndex : class, IDataTransferObject<int>
		where TEdit : class, IDataTransferObject<int> {

		/// <summary>
		/// The constructor for the QfController class.
		/// </summary>
		/// <param name="dataService">The data service used to return data to the views contained within this class.</param>
		/// <param name="securityManager">The <see cref="QuickFrame.Security.QuickFrameSecurityManager"/>  used to apply security to functions within this class.</param>
		public QfController(IDataServiceCore<TEntity, int> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}
	}
}