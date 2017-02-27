using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuickFrame.Data.Dtos;
using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Services;
using QuickFrame.Mvc.Configuration;
using QuickFrame.Security;
using System.Data.SqlClient;

namespace QuickFrame.Mvc.Controllers {

	/// <summary>
	/// Expands on the <see cref="QfControllerBase{TEntity, TIndex}"/> class to add delete and lookup table functionality. 
	/// </summary>
	/// <typeparam name="TEntity">The type of entity that will be used in this controller.</typeparam>
	/// <typeparam name="TIdType">The type of ID used by the entity.</typeparam>
	/// <typeparam name="TIndex">The type of class to which the entity will be mapped when returning the Index page.</typeparam>
	public class QfControllerCore<TEntity, TIdType, TIndex>
		: QfControllerBase<TEntity, TIndex>
		where TEntity : class
		where TIndex : class, IDataTransferObjectCore {

		/// <summary>
		/// The constructor for the QFControllerCore class.
		/// </summary>
		/// <param name="dataService">The data service used to return data to the views contained within this class.</param>
		/// <param name="securityManager">The <see cref="QuickFrame.Security.QuickFrameSecurityManager"/>  used to apply security to functions within this class.</param>
		public QfControllerCore(IDataServiceCore<TEntity, TIdType> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}

		public QfControllerCore(IDataServiceBase<TEntity> dataService, QuickFrameSecurityManager securityManager, IOptions<ViewOptions> viewOptions) 
			:base (dataService, securityManager, viewOptions) {

		}
		/// <summary>
		/// The default delete function
		/// </summary>
		/// <param name="id">The id of the object to delete</param>
		/// <returns>True if the operation succeeds.</returns>
		[HttpDelete]
		public IActionResult Delete(TIdType id) => Authorize(() => DeleteCore(id));

		/// <summary>
		/// Returns the id and name field of the specified entity in a format that can be used to fill a combo box on a web page.
		/// </summary>
		/// <returns>A list of <see cref="LookupTableDto{TSrc, TIdType}" objects wrapped in a JsonResult to be parsed by javascript in the page./> </returns>
		[HttpGet]
		public IActionResult GetLookupTableData() => Authorize(() => GetLookupTableDataCore());

		[HttpGet]
		public IActionResult GetLookupTableDataEx(int? id, string columnName) => Authorize(() => GetLookupTableDataExCore(id, columnName));
		/// <summary>
		/// The core delete function
		/// </summary>
		/// <param name="id">The id of the object to delete</param>
		/// <returns>True if the operation succeeds.</returns>
		/// <remarks>If overriding the delete functionality, override this call rather than <see cref="QfControllerCore{TEntity, TIdType, TIndex}.Delete(TIdType)"/> </remarks>
		protected virtual IActionResult DeleteCore(TIdType id) {
			(_dataService as IDataServiceCore<TEntity, TIdType>).Delete(id);
			return new JsonResult("OK");
		}

		/// <summary>
		/// Returns the id and name field of the specified entity in a format that can be used to fill a combo box on a web page.
		/// </summary>
		/// <returns>A list of <see cref="LookupTableDto{TSrc, TIdType}" objects wrapped in a JsonResult to be parsed by javascript in the page./> </returns>
		/// <remarks>If overriding the GetLookupTableData functionality, override this function rather than <see cref="QfControllerCore{TEntity, TIdType, TIndex}.GetLookupTableData"/> </remarks>
		protected virtual IActionResult GetLookupTableDataCore() {
			return new JsonResult(_dataService.GetList<LookupTableDtoCore<TIdType>>());
		}

		protected virtual IActionResult GetLookupTableDataExCore(int? id, string columnName) {
			return new JsonResult(_dataService.GetList<LookupTableDto>(id, columnName));
		}
	}

	/// <summary>
	/// Expands the QfControllerCore class by adding create and edit functionality
	/// </summary>
	/// <typeparam name="TEntity">The type of entity that will be used in this controller.</typeparam>
	/// <typeparam name="TIdType">The type of ID used by the entity.</typeparam>
	/// <typeparam name="TIndex">The type of class to which the entity will be mapped when returning the Index page.</typeparam>
	/// <typeparam name="TEdit">The type of class to which the entity will be mapped when returning the create or edit functionality.</typeparam>
	public class QfControllerCore<TEntity, TIdType, TIndex, TEdit>
		: QfControllerCore<TEntity, TIdType, TIndex>
		where TEntity : class, new()
		where TIndex : class, IDataTransferObjectCore
		where TEdit : class, IDataTransferObjectCore, new() {
		protected string EditPage = "CreateOrEdit";
		protected string CreatePage = "CreateOrEdit";

		/// <summary>
		/// The constructor for the QFControllerCore class.
		/// </summary>
		/// <param name="dataService">The data service used to return data to the views contained within this class.</param>
		/// <param name="securityManager">The <see cref="QuickFrame.Security.QuickFrameSecurityManager"/>  used to apply security to functions within this class.</param>
		public QfControllerCore(IDataServiceCore<TEntity, TIdType> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}

		public QfControllerCore(IDataServiceBase<TEntity> dataService, QuickFrameSecurityManager securityManager, IOptions<ViewOptions> viewOptions)
			: base(dataService, securityManager, viewOptions) {

		}
		/// <summary>
		/// Provides the default create view loaded with an empty model.
		/// </summary>
		/// <param name="closeOnSubmit">True to close the create view when data is successfully submitted and saved to the database.</param>
		/// <returns>An IActionResult representing the view used to create a new entity.</returns>
		[HttpGet]
		public IActionResult Create(bool closeOnSubmit = true) => Authorize(() => CreateCore(closeOnSubmit));

		/// <summary>
		/// Upon submission, saves the newly created model to the database.
		/// </summary>
		/// <param name="model">The model to save.</param>
		/// <returns>An IActionResult representing the view used to create a new entity or an IActionResult that will close the current fancybox depending on the value of closeOnSubmit.</returns>
		[HttpPost]
		public IActionResult Create(TEdit model) => Authorize(() => CreateCore(model));

		/// <summary>
		/// Provides the default edit view loaded with a model representing the record indicated by the id parameter.
		/// </summary>
		/// <param name="id">The unique identifier of the model to edit.</param>
		/// <param name="closeOnSubmit">True to close the edit view when data is successfully submitted and saved to the database.</param>
		/// <returns>An IActionResult representing the view used to edit an entity.</returns>
		[HttpGet]
		public IActionResult Edit(TIdType id, bool closeOnSubmit = true) => Authorize(() => EditCore(id, closeOnSubmit));

		/// <summary>
		/// Upon submission, saves the edited model to the database.
		/// </summary>
		/// <param name="model">The model to save.</param>
		/// <returns>An IActionResult representing the view used to edit an entity or an IActionResult that will close the current fancybox depending on the value of closeOnSubmit.</returns>
		[HttpPost]
		public IActionResult Edit(TEdit model) => Authorize(() => EditCore(model));

		/// <summary>
		/// The core create functionality.
		/// </summary>
		/// <param name="closeOnSubmit">True to close the create view when data is successfully submitted and saved to the database.</param>
		/// <returns>An IActionResult representing the view used to create a new entity.</returns>
		/// <remarks>If overriding the create functionality, override this function rather than <see cref="QfControllerCore{TEntity, TIdType, TIndex, TEdit}.Create(bool)"/>.</remarks>
		public virtual IActionResult CreateCore(bool closeOnSubmit) {
			HttpContext.Session.SetBoolean(CurrentAction, closeOnSubmit);
			return View(CreatePage, new TEdit());
		}

		/// <summary>
		/// The core create funcionality
		/// </summary>
		/// <param name="model">The model to save.</param>
		/// <returns>An IActionResult representing the view used to create a new entity or an IActionResult that will close the current fancybox depending on the value of closeOnSubmit.</returns>
		/// <remarks>If overriding the create functionality, override this function rather than <see cref="QfControllerCore{TEntity, TIdType, TIndex, TEdit}.Create(TEdit)"/>.</remarks>
		protected virtual IActionResult CreateCore<TModel>(TModel model) where TModel : IDataTransferObjectCore {
			if(ModelState.IsValid) {
				_dataService.Create(model);
				var closeOnSubmit = (bool)HttpContext.Session.GetBoolean(CurrentAction, true);
				HttpContext.Session.Remove(CurrentAction);
				if(closeOnSubmit)
					return View("CloseCurrentView");
			}
			return View(CreatePage, model);
		}

		/// <summary>
		/// The core edit functionality.
		/// </summary>
		/// <param name="id">The unique identifier of the model to edit.</param>
		/// <param name="closeOnSubmit">True to close the edit view when data is successfully submitted and saved to the database.</param>
		/// <returns>An IActionResult representing the view used to edit an entity.</returns>
		/// <remarks>If overriding the edit functionality, override this function rahter than <see cref="QfControllerCore{TEntity, TIdType, TIndex, TEdit}.Create(bool)"/> </remarks>
		protected virtual IActionResult EditCore(TIdType id, bool closeOnSubmit) {
			HttpContext.Session.SetBoolean(CurrentAction, closeOnSubmit);
			return View(EditPage, (_dataService as IDataServiceCore<TEntity, TIdType>).Get<TEdit>(id));
		}

		/// <summary>
		/// The core edit functionality.
		/// </summary>
		/// <param name="model">The model to save.</param>
		/// <returns>An IActionResult representing the view used to edit an entity or an IActionResult that will close the current fancybox depending on the value of closeOnSubmit.</returns>
		/// <remarks>If overriding the edit functionality, override this function rahter than <see cref="QfControllerCore{TEntity, TIdType, TIndex, TEdit}.Create(bool)"/> </remarks>
		protected virtual IActionResult EditCore(TEdit model) {
			if(ModelState.IsValid) {
				_dataService.Save(model);
				var closeOnSubmit = HttpContext.Session.GetBoolean(CurrentAction);
				HttpContext.Session.Remove(CurrentAction);
				if(closeOnSubmit == true)
					return View("CloseCurrentView");
			}
			return View(EditPage, model);
		}
	}
}