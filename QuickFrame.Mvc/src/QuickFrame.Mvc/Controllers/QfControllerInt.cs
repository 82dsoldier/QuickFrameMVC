using Microsoft.Extensions.Options;
using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Services;
using QuickFrame.Mvc.Configuration;
using QuickFrame.Security;

namespace QuickFrame.Mvc.Controllers {

	/// <summary>
	/// A controller base class for a entity with an id of type <see cref="System.Int32"/> 
	/// </summary>
	/// <typeparam name="TEntity">The type of entity that will be used in this controller.</typeparam>
	/// <typeparam name="TIndex">The type of class to which the entity will be mapped when returning the Index page.</typeparam>
	public class QfControllerInt<TEntity, TIndex>
		: QfControllerCore<TEntity, int, TIndex>
		where TEntity : class, new()
		where TIndex : class, IDataTransferObject<int> {

		/// <summary>
		/// The constructor for the QfControllerInt class.
		/// </summary>
		/// <param name="dataService">The data service used to return data to the views contained within this class.</param>
		/// <param name="securityManager">The <see cref="QuickFrame.Security.QuickFrameSecurityManager"/>  used to apply security to functions within this class.</param>
		public QfControllerInt(IDataServiceCore<TEntity, int> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}

		public QfControllerInt(IDataServiceCore<TEntity, int> dataService, QuickFrameSecurityManager securityManager, IOptions<ViewOptions> viewOptions) 
			:base (dataService, securityManager, viewOptions) {

		}
	}


	/// <summary>
	/// A controller base class for a entity with an id of type <see cref="System.Int32"/> 
	/// </summary>
	/// <typeparam name="TEntity">The type of entity that will be used in this controller.</typeparam>
	/// <typeparam name="TIndex">The type of class to which the entity will be mapped when returning the Index page.</typeparam>
	/// <typeparam name="TEdit">The type of class to which the entity will be mapped when returning the create or edit functionality.</typeparam>
	public class QfControllerInt<TEntity, TIndex, TEdit>
		: QfControllerCore<TEntity, int, TIndex, TEdit>
		where TEntity : class, new()
		where TIndex : class, IDataTransferObject<int>
		where TEdit : class, IDataTransferObject<int>, new() {

		/// <summary>
		/// The constructor for the QfControllerInt class.
		/// </summary>
		/// <param name="dataService">The data service used to return data to the views contained within this class.</param>
		/// <param name="securityManager">The <see cref="QuickFrame.Security.QuickFrameSecurityManager"/>  used to apply security to functions within this class.</param>
		public QfControllerInt(IDataServiceCore<TEntity, int> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}
		public QfControllerInt(IDataServiceCore<TEntity, int> dataService, QuickFrameSecurityManager securityManager, IOptions<ViewOptions> viewOptions)
			: base(dataService, securityManager, viewOptions) {

		}
	}
}