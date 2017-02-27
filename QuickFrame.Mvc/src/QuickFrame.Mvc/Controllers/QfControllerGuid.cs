using Microsoft.Extensions.Options;
using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Services;
using QuickFrame.Mvc.Configuration;
using QuickFrame.Security;
using System;

namespace QuickFrame.Mvc.Controllers {

	/// <summary>
	/// A controller base class for a entity with an id of type <see cref="System.Guid"/> 
	/// </summary>
	/// <typeparam name="TEntity">The type of entity that will be used in this controller.</typeparam>
	/// <typeparam name="TIndex">The type of class to which the entity will be mapped when returning the Index page.</typeparam>
	public class QfControllerGuid<TEntity, TIndex>
		: QfControllerCore<TEntity, Guid, TIndex>
		where TEntity : class, new()
		where TIndex : class, IDataTransferObject<Guid> {

		/// <summary>
		/// The constructor for the QFControllerCore class.
		/// </summary>
		/// <param name="dataService">The data service used to return data to the views contained within this class.</param>
		/// <param name="securityManager">The <see cref="QuickFrame.Security.QuickFrameSecurityManager"/>  used to apply security to functions within this class.</param>
		public QfControllerGuid(IDataServiceCore<TEntity, Guid> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}

		public QfControllerGuid(IDataServiceCore<TEntity, Guid> dataService, QuickFrameSecurityManager securityManager, IOptions<ViewOptions> viewOptions) 
			:base (dataService, securityManager, viewOptions) {

		}

	}

	/// <summary>
	/// A controller base class for a entity with an id of type <see cref="System.Guid"/> 
	/// </summary>
	/// <typeparam name="TEntity">The type of entity that will be used in this controller.</typeparam>
	/// <typeparam name="TIndex">The type of class to which the entity will be mapped when returning the Index page.</typeparam>
	/// <typeparam name="TEdit">The type of class to which the entity will be mapped when returning the create or edit functionality.</typeparam>
	public class QfControllerGuid<TEntity, TIndex, TEdit>
		: QfControllerCore<TEntity, Guid, TIndex, TEdit>
		where TEntity : class, new()
		where TIndex : class, IDataTransferObjectGuid
		where TEdit : class, IDataTransferObjectGuid, new() {

		/// <summary>
		/// The constructor for the QFControllerCore class.
		/// </summary>
		/// <param name="dataService">The data service used to return data to the views contained within this class.</param>
		/// <param name="securityManager">The <see cref="QuickFrame.Security.QuickFrameSecurityManager"/>  used to apply security to functions within this class.</param>
		public QfControllerGuid(IDataServiceCore<TEntity, Guid> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}

		public QfControllerGuid(IDataServiceCore<TEntity, Guid> dataService, QuickFrameSecurityManager securityManager, IOptions<ViewOptions> viewOptions)
			: base(dataService, securityManager, viewOptions) {

		}
	}
}