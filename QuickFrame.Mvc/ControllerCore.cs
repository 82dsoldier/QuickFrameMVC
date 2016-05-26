using Microsoft.AspNetCore.Mvc;
using QuickFrame.Data.Interfaces;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using static QuickFrame.Security.AuthorizationExtensions;

namespace QuickFrame.Mvc {

	public class ControllerCore<TDataType, TEntity, TIndex, TEdit>
		 : Controller
		 where TEntity : IDataModelCore<TDataType>
		 where TIndex : IDataTransferObjectCore<TDataType, TEntity, TIndex>
		 where TEdit : IDataTransferObjectCore<TDataType, TEntity, TEdit> {
		protected readonly IDataServiceCore<TDataType, TEntity> _dataService;

		[HttpGet]
		public IActionResult Create(bool closeOnSubmit = false) => CreateBase<TEdit>(closeOnSubmit);

		[HttpPost]
		public IActionResult Create(TEdit model) => CreateBase(model);

		[HttpGet]
		public IActionResult Details(TDataType id) => DetailsBase<TEdit>(id);

		[HttpGet]
		public IActionResult Edit(TDataType id, bool closeOnSubmit = false) => EditBase<TEdit>(id, closeOnSubmit);

		[HttpPost]
		public IActionResult Edit(TEdit model) => EditBase(model);

		[HttpGet]
		public virtual IActionResult Get() => GetBase();

		[HttpGet]
		public IActionResult Index(int page = 1, int itemsPerPage = 25, string sortColumn = "Name", SortOrder sortOrder = SortOrder.Ascending)
			=> IndexBase<TIndex>(page, itemsPerPage, sortColumn, sortOrder);

		protected virtual IActionResult CreateBase<TReturn>(bool closeOnSubmit = false, string modelName = "CreateOrEdit")
			where TReturn : IDataTransferObjectCore<TDataType, TEntity, TReturn> => Authorize(User, () => {
				TempData["CloseOnSubmit"] = closeOnSubmit;
				return View(modelName, Activator.CreateInstance(typeof(TReturn)));
			});

		protected virtual IActionResult CreateBase<TModel>(TModel model, string modelName = "CreateOrEdit")
			where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel> => Authorize(User, () => {
				if(ModelState.IsValid) {
					ViewData["CloseOnSubmit"] = TempData["CloseOnSubmit"];
					_dataService.Create(model);
					ModelState.Clear();
					return View(modelName, Activator.CreateInstance(typeof(TModel)));
				}

				return View(modelName, model);
			});

		protected virtual IActionResult DetailsBase<TModel>(TDataType id)
			where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel>
			=> Authorize(User, () => View(_dataService.Get<TModel>(id)));

		protected virtual IActionResult EditBase<TModel>(TDataType id, bool closeOnSubmit = false, string modelName = "CreateOrEdit")
			where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel> => Authorize(User, () => {
				TempData["CloseOnSubmit"] = closeOnSubmit;
				return View(modelName, _dataService.Get<TModel>(id));
			});

		protected virtual IActionResult EditBase<TModel>(TModel model, string modelName = "CreateOrEdit")
			where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel> => Authorize(User, () => {
				if(ModelState.IsValid) {
					_dataService.Save(model);
				}

				return View(modelName, model);
			});

		protected virtual IActionResult GetBase() => Authorize(User, () => new ObjectResult(_dataService.GetList<TIndex>()));

		protected virtual IActionResult IndexBase<TResult>
			(int page = 1, int itemsPerPage = 25, string sortColumn = "Name", SortOrder sortOrder = SortOrder.Ascending)
			where TResult : IDataTransferObjectCore<TDataType, TEntity, TResult> => this.Authorize(User, () => {
				ViewData["totalItems"] = _dataService.GetCount();
				return View("Index", _dataService.GetList<TResult>(itemsPerPage * (page - 1), itemsPerPage, sortColumn, sortOrder).ToList());
			});

		protected virtual IActionResult Authorize(ClaimsPrincipal user, Func<IActionResult> func) => AuthorizeExecution(user, func);

		public ControllerCore(IDataServiceCore<TDataType, TEntity> dataService) {
			_dataService = dataService;
		}
	}
}