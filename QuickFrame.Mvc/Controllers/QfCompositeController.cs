using Microsoft.AspNetCore.Mvc;
using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Services;
using QuickFrame.Security;

namespace QuickFrame.Mvc.Controllers {

	public class QfCompositeController<TEntity, TFirst, TSecond, TIndex, TEdit>
		: QfControllerBase<TEntity, TIndex>
		where TEntity : class
		where TIndex : class, IDataTransferObjectCore
		where TEdit : class, IDataTransferObjectCore {
		protected string EditPage = "EditOrCreate";

		public QfCompositeController(IDataServiceComposite<TEntity, TFirst, TSecond> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}

		[HttpDelete]
		public IActionResult Delete(TFirst first, TSecond second) => DeleteCore(first, second);

		[HttpGet]
		public IActionResult Edit(TFirst first, TSecond second, bool closeOnSubmit = true) => EditCore(first, second, closeOnSubmit);

		[HttpPost]
		public IActionResult Edit(TEdit model) => EditCore(model);

		protected virtual IActionResult EditCore(TFirst first, TSecond second, bool closeOnSubmit) {
			HttpContext.Session.SetBoolean("closeOnSubmit", closeOnSubmit);
			return View(EditPage, (_dataService as IDataServiceComposite<TEntity, TFirst, TSecond>).Get<TEdit>(first, second));
		}

		protected virtual IActionResult EditCore(TEdit model) {
			_dataService.Save(model);
			var closeOnSubmit = HttpContext.Session.GetBoolean("closeOnSubmit");
			if(closeOnSubmit == true)
				return View("CloseCurrentView");
			return View(EditPage, model);
		}

		protected virtual IActionResult DeleteCore(TFirst first, TSecond second) {
			(_dataService as IDataServiceComposite<TEntity, TFirst, TSecond>).Delete(first, second);
			return new JsonResult("OK");
		}
	}

	public class QfCompositeController<TEntity, TFirst, TSecond, TThird, TIndex, TEdit>
	: QfControllerBase<TEntity, TIndex>
	where TEntity : class
	where TIndex : class, IDataTransferObjectCore
	where TEdit : class, IDataTransferObjectCore {
		protected string EditPage = "EditOrCreate";

		public QfCompositeController(IDataServiceComposite<TEntity, TFirst, TSecond, TThird> dataService, QuickFrameSecurityManager securityManager)
			: base(dataService, securityManager) {
		}

		[HttpDelete]
		public IActionResult Delete(TFirst first, TSecond second, TThird third) => DeleteCore(first, second, third);

		[HttpGet]
		public IActionResult Edit(TFirst first, TSecond second, TThird third, bool closeOnSubmit = true) => EditCore(first, second, third, closeOnSubmit);

		[HttpPost]
		public IActionResult Edit(TEdit model) => EditCore(model);

		protected virtual IActionResult EditCore(TFirst first, TSecond second, TThird third, bool closeOnSubmit) {
			HttpContext.Session.SetBoolean("closeOnSubmit", closeOnSubmit);
			return View(EditPage, (_dataService as IDataServiceComposite<TEntity, TFirst, TSecond, TThird>).Get<TEdit>(first, second, third));
		}

		protected virtual IActionResult EditCore(TEdit model) {
			_dataService.Save(model);
			var closeOnSubmit = HttpContext.Session.GetBoolean("closeOnSubmit");
			if(closeOnSubmit == true)
				return View("CloseCurrentView");
			return View(EditPage, model);
		}

		protected virtual IActionResult DeleteCore(TFirst first, TSecond second, TThird third) {
			(_dataService as IDataServiceComposite<TEntity, TFirst, TSecond, TThird>).Delete(first, second, third);
			return new JsonResult("OK");
		}
	}
}