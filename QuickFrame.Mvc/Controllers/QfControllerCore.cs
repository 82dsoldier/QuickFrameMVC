using Microsoft.AspNetCore.Mvc;
using QuickFrame.Data.Dtos;
using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Services;
using QuickFrame.Security;
using System.Data.SqlClient;

namespace QuickFrame.Mvc.Controllers {

	public class QfControllerCore<TEntity, TIdType, TIndex>
		: QfControllerBase<TEntity, TIndex>
		where TEntity : class
		where TIndex : class, IDataTransferObjectCore {

		public QfControllerCore(IDataServiceCore<TEntity, TIdType> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}

		[HttpDelete]
		public IActionResult Delete(TIdType id) => DeleteCore(id);

		[HttpGet]
		public IActionResult GetLookupTableData() => GetLookupTableDataCore();

		protected virtual IActionResult DeleteCore(TIdType id) {
			(_dataService as IDataServiceCore<TEntity, TIdType>).Delete(id);
			return new JsonResult("OK");
		}

		protected virtual IActionResult GetLookupTableDataCore() {
			return new JsonResult(_dataService.GetList<LookupTableDto<TEntity, TIdType>>(string.Empty, 0, 0, string.Empty, SortOrder.Ascending, false));
		}
	}

	public class QfControllerCore<TEntity, TIdType, TIndex, TEdit>
		: QfControllerCore<TEntity, TIdType, TIndex>
		where TEntity : class, new()
		where TIndex : class, IDataTransferObjectCore
		where TEdit : class, IDataTransferObjectCore {
		protected string EditPage = "CreateOrEdit";
		protected string CreatePage = "CreateOrEdit";

		public QfControllerCore(IDataServiceCore<TEntity, TIdType> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}

		[HttpGet]
		public IActionResult Create(bool closeOnSubmit = true) => CreateCore(closeOnSubmit);

		[HttpPost]
		public IActionResult Create(TEdit model) => CreateCore(model);

		[HttpGet]
		public IActionResult Edit(TIdType id, bool closeOnSubmit = true) => EditCore(id, closeOnSubmit);

		[HttpPost]
		public IActionResult Edit(TEdit model) => EditCore(model);

		public virtual IActionResult CreateCore(bool closeOnSubmit) {
			HttpContext.Session.SetBoolean("closeOnSubmit", closeOnSubmit);
			return View(CreatePage);
		}

		protected virtual IActionResult CreateCore<TModel>(TModel model) where TModel : IDataTransferObjectCore {
			if(ModelState.IsValid) {
				_dataService.Create(model);
				var closeOnSubmit = (bool)HttpContext.Session.GetBoolean("closeOnSubmit", true);
				HttpContext.Session.SetBoolean("closeOnSubmit", false);
				if(closeOnSubmit)
					return View("CloseCurrentView");
			}
			return View(CreatePage, model);
		}

		protected virtual IActionResult EditCore(TIdType id, bool closeOnSubmit) {
			HttpContext.Session.SetBoolean("closeOnSubmit", closeOnSubmit);
			return View(EditPage, (_dataService as IDataServiceCore<TEntity, TIdType>).Get<TEdit>(id));
		}

		protected virtual IActionResult EditCore(TEdit model) {
			if(ModelState.IsValid) {
				_dataService.Save(model);
				var closeOnSubmit = HttpContext.Session.GetBoolean("closeOnSubmit");
				HttpContext.Session.SetBoolean("closeOnSubmit", false);
				if(closeOnSubmit == true)
					return View("CloseCurrentView");
			}
			return View(EditPage, model);
		}
	}
}