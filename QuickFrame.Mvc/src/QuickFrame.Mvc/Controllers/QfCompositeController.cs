using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Services;
using QuickFrame.Mvc.Configuration;
using QuickFrame.Security;

namespace QuickFrame.Mvc.Controllers {

	/// <summary>
	/// A controller base class for entities with a composite key.
	/// </summary>
	/// <typeparam name="TEntity">The type of entity used by the controller.</typeparam>
	/// <typeparam name="TFirst">The type of the first key in the entity.</typeparam>
	/// <typeparam name="TSecond">The type of the second key in the entity.</typeparam>
	/// <typeparam name="TIndex">The class to use as a model for the index page.</typeparam>
	/// <typeparam name="TEdit">The class to use as a model for the create/edit page.</typeparam>
	public class QfCompositeController<TEntity, TFirst, TSecond, TIndex>
		: QfControllerBase<TEntity, TIndex>
		where TEntity : class
		where TIndex : class, IDataTransferObjectCore {

		/// <summary>
		/// The constructor for the QfCompositeController class.
		/// </summary>
		/// <param name="dataService">The data service used to return data to the views contained within this class.</param>
		/// <param name="securityManager">The <see cref="QuickFrame.Security.QuickFrameSecurityManager"/>  used to apply security to functions within this class.</param>
		public QfCompositeController(IDataServiceComposite<TEntity, TFirst, TSecond> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}
		public QfCompositeController(IDataServiceComposite<TEntity, TFirst, TSecond> dataService, QuickFrameSecurityManager securityManager, IOptions<ViewOptions> viewOptions)
			: base(dataService, securityManager, viewOptions) {
		}
	}

	/// <summary>
	/// A controller base class for entities with a composite key.
	/// </summary>
	/// <typeparam name="TEntity">The type of entity used by the controller.</typeparam>
	/// <typeparam name="TFirst">The type of the first key in the entity.</typeparam>
	/// <typeparam name="TSecond">The type of the second key in the entity.</typeparam>
	/// <typeparam name="TIndex">The class to use as a model for the index page.</typeparam>
	/// <typeparam name="TEdit">The class to use as a model for the create/edit page.</typeparam>
	public class QfCompositeController<TEntity, TFirst, TSecond, TIndex, TEdit>
		: QfControllerBase<TEntity, TIndex>
		where TEntity : class
		where TIndex : class, IDataTransferObjectCore
		where TEdit : class, IDataTransferObjectCore, new() {
		protected string EditPage = "EditOrCreate";
		protected string CreatePage = "CreateOrEdit";

		/// <summary>
		/// The constructor for the QfCompositeController class.
		/// </summary>
		/// <param name="dataService">The data service used to return data to the views contained within this class.</param>
		/// <param name="securityManager">The <see cref="QuickFrame.Security.QuickFrameSecurityManager"/>  used to apply security to functions within this class.</param>
		public QfCompositeController(IDataServiceComposite<TEntity, TFirst, TSecond> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}
		public QfCompositeController(IDataServiceComposite<TEntity, TFirst, TSecond> dataService, QuickFrameSecurityManager securityManager, IOptions<ViewOptions> viewOptions)
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
		/// The delete function
		/// </summary>
		/// <param name="first">The first key value.</param>
		/// <param name="second">The second key value.</param>
		/// <returns>An IActionResult encapsulating a boolean value to indicate success or failure of the call.</returns>
		[HttpDelete]
		public IActionResult Delete(TFirst first, TSecond second) => DeleteCore(first, second);

		/// <summary>
		/// The edit function
		/// </summary>
		/// <param name="first">The first key value.</param>
		/// <param name="second">The second key value.</param>
		/// <param name="closeOnSubmit">Set to true to indicate the editing window should close when the model is successfully submitted.</param>
		/// <returns>An IActionResult representing the edit view.</returns>
		[HttpGet]
		public IActionResult Edit(TFirst first, TSecond second, bool closeOnSubmit = true) => EditCore(first, second, closeOnSubmit);

		/// <summary>
		/// Processes the given model when submitted.
		/// </summary>
		/// <param name="model">The model to edit.</param>
		/// <returns>If closeOnSubmit is true, returns a view that will close the current fancybox.  Otherwise returns an IActionResult representing the edit view.</returns>
		[HttpPost]
		public IActionResult Edit(TEdit model) => EditCore(model);

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
		/// <param name="first">The first key value.</param>
		/// <param name="second">The second key value.</param>
		/// <param name="closeOnSubmit">Set to true to indicate the editing window should close when the model is successfully submitted.</param>
		/// <returns>An IActionResult representing the edit view.</returns>
		/// <remarks>In an inherited class, overload this function rather than <see cref="QfCompositeController{TEntity, TFirst, TSecond, TIndex, TEdit}.Edit(TFirst, TSecond, bool)"/> </remarks>
		protected virtual IActionResult EditCore(TFirst first, TSecond second, bool closeOnSubmit) {
			HttpContext.Session.SetBoolean("closeOnSubmit", closeOnSubmit);
			return View(EditPage, (_dataService as IDataServiceComposite<TEntity, TFirst, TSecond>).Get<TEdit>(first, second));
		}

		/// <summary>
		/// The core edit functionality.
		/// </summary>
		/// <param name="model">The model to edit.</param>
		/// <returns>If closeOnSubmit is true, returns a view that will close the current fancybox.  Otherwise returns an IActionResult representing the edit view.</returns>
		/// <remarks>In an inherited class, overload this function rather than <see cref="QfCompositeController{TEntity, TFirst, TSecond, TIndex, TEdit}.Edit(TEdit)/> </remarks>
		protected virtual IActionResult EditCore(TEdit model) {
			_dataService.Save(model);
			var closeOnSubmit = HttpContext.Session.GetBoolean("closeOnSubmit");
			if(closeOnSubmit == true)
				return View("CloseCurrentView");
			return View(EditPage, model);
		}

		/// <summary>
		/// The core delete functionality
		/// </summary>
		/// <param name="first">The first key value.</param>
		/// <param name="second">The second key value.</param>
		/// <returns>An IActionResult encapsulating a boolean value to indicate success or failure of the call.</returns>
		///<remarks>In an inherited class, overload this function rather than <see cref="QfCompositeController{TEntity, TFirst, TSecond, TIndex, TEdit}.Delete(TFirst, TSecond)"/> </remarks>
		protected virtual IActionResult DeleteCore(TFirst first, TSecond second) {
			(_dataService as IDataServiceComposite<TEntity, TFirst, TSecond>).Delete(first, second);
			return new JsonResult("OK");
		}
	}

	/// <summary>
	/// A controller base class for entities with a composite key.
	/// </summary>
	/// <typeparam name="TEntity">The type of entity used by the controller.</typeparam>
	/// <typeparam name="TFirst">The type of the first key in the entity.</typeparam>
	/// <typeparam name="TSecond">The type of the second key in the entity.</typeparam>
	/// <typeparam name="TThird">The type of the third key in the entity.</typeparam>
	/// <typeparam name="TIndex">The class to use as a model for the index page.</typeparam>
	/// <typeparam name="TEdit">The class to use as a model for the create/edit page.</typeparam>
	public class QfCompositeController<TEntity, TFirst, TSecond, TThird, TIndex, TEdit>
			: QfControllerBase<TEntity, TIndex>
			where TEntity : class
			where TIndex : class, IDataTransferObjectCore
			where TEdit : class, IDataTransferObjectCore {
				protected string EditPage = "EditOrCreate";

		/// <summary>
		/// The constructor for the QfCompositeController class.
		/// </summary>
		/// <param name="dataService">The data service used to return data to the views contained within this class.</param>
		/// <param name="securityManager">The <see cref="QuickFrame.Security.QuickFrameSecurityManager"/>  used to apply security to functions within this class.</param>
		public QfCompositeController(IDataServiceComposite<TEntity, TFirst, TSecond, TThird> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}

		/// <summary>
		/// The constructor for the QfCompositeController class.
		/// </summary>
		/// <param name="dataService">The data service used to return data to the views contained within this class.</param>
		/// <param name="securityManager">The <see cref="QuickFrame.Security.QuickFrameSecurityManager"/>  used to apply security to functions within this class.</param>
		public QfCompositeController(IDataServiceComposite<TEntity, TFirst, TSecond, TThird> dataService, QuickFrameSecurityManager securityManager, IOptions<ViewOptions> viewOptions)
			: base(dataService, securityManager, viewOptions) {
		}

		/// <summary>
		/// The delete function
		/// </summary>
		/// <param name="first">The first key value.</param>
		/// <param name="second">The second key value.</param>
		/// <param name="third">The third key value.</param>
		/// <returns>An IActionResult encapsulating a boolean value to indicate success or failure of the call.</returns>
		[HttpDelete]
		public IActionResult Delete(TFirst first, TSecond second, TThird third) => DeleteCore(first, second, third);

		/// <summary>
		/// The edit function
		/// </summary>
		/// <param name="first">The first key value.</param>
		/// <param name="second">The second key value.</param>
		/// <param name="third">The third key value.</param>
		/// <param name="closeOnSubmit">Set to true to indicate the editing window should close when the model is successfully submitted.</param>
		/// <returns>An IActionResult representing the edit view.</returns>
		[HttpGet]
		public IActionResult Edit(TFirst first, TSecond second, TThird third, bool closeOnSubmit = true) => EditCore(first, second, third, closeOnSubmit);

		/// <summary>
		/// Processes the given model when submitted.
		/// </summary>
		/// <param name="model">The model to edit.</param>
		/// <returns>If closeOnSubmit is true, returns a view that will close the current fancybox.  Otherwise returns an IActionResult representing the edit view.</returns>
		[HttpPost]
		public IActionResult Edit(TEdit model) => EditCore(model);

		/// <summary>
		/// The core edit functionality.
		/// </summary>
		/// <param name="first">The first key value.</param>
		/// <param name="second">The second key value.</param>
		/// <param name="third">The third key value.</param>
		/// <param name="closeOnSubmit">Set to true to indicate the editing window should close when the model is successfully submitted.</param>
		/// <returns>An IActionResult representing the edit view.</returns>
		/// <remarks>In an inherited class, overload this function rather than <see cref="QfCompositeController{TEntity, TFirst, TSecond, TThird, TIndex, TEdit}.Edit(TFirst, TSecond, TThird, bool)"/> </remarks>
		protected virtual IActionResult EditCore(TFirst first, TSecond second, TThird third, bool closeOnSubmit) {
			HttpContext.Session.SetBoolean("closeOnSubmit", closeOnSubmit);
			return View(EditPage, (_dataService as IDataServiceComposite<TEntity, TFirst, TSecond, TThird>).Get<TEdit>(first, second, third));
		}

		/// <summary>
		/// The core edit functionality.
		/// </summary>
		/// <param name="model">The model to edit.</param>
		/// <returns>If closeOnSubmit is true, returns a view that will close the current fancybox.  Otherwise returns an IActionResult representing the edit view.</returns>
		/// <remarks>In an inherited class, overload this function rather than <see cref="QfCompositeController{TEntity, TFirst, TSecond, TThird, TIndex, TEdit}.Edit(TEdit)/> </remarks>
		protected virtual IActionResult EditCore(TEdit model) {
			_dataService.Save(model);
			var closeOnSubmit = HttpContext.Session.GetBoolean("closeOnSubmit");
			if(closeOnSubmit == true)
				return View("CloseCurrentView");
			return View(EditPage, model);
		}

		/// <summary>
		/// The core delete functionality
		/// </summary>
		/// <param name="first">The first key value.</param>
		/// <param name="second">The second key value.</param>
		/// <param name="third">The third key value.</param>
		/// <returns>An IActionResult encapsulating a boolean value to indicate success or failure of the call.</returns>
		///<remarks>In an inherited class, overload this function rather than <see cref="QfCompositeController{TEntity, TFirst, TSecond, TThird, TIndex, TEdit}.Delete(TFirst, TSecond, TThird)"/> </remarks>
		protected virtual IActionResult DeleteCore(TFirst first, TSecond second, TThird third) {
			(_dataService as IDataServiceComposite<TEntity, TFirst, TSecond, TThird>).Delete(first, second, third);
			return new JsonResult("OK");
		}
	}
}