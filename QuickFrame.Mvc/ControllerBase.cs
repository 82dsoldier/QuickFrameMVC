using Microsoft.AspNet.Mvc;
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
		public virtual IActionResult Get() => Authorize(User, () => new ObjectResult(_dataService.GetList<TIndex>()));
		[HttpGet]
		public IActionResult Index(int page = 1, int itemsPerPage = 25, string sortColumn = "Name", SortOrder sortOrder = SortOrder.Ascending)
			=> IndexBase<TIndex>(page, itemsPerPage, sortColumn, sortOrder);

		[HttpGet]
		public IActionResult Create(bool closeOnSubmit = false) => CreateBase<TEdit>(closeOnSubmit);

		[HttpPost]
		public IActionResult Create(TEdit model) => CreateBase(model);

		protected virtual IActionResult IndexBase<TResult>
			(int page = 1, int itemsPerPage = 25, string sortColumn = "Name", SortOrder sortOrder = SortOrder.Ascending)
			where TResult : IDataTransferObjectCore<TDataType, TEntity, TResult> => this.Authorize(User, () => {
				ViewData["totalItems"] = _dataService.GetCount();
				return View("Index", _dataService.GetList<TResult>(itemsPerPage * (page - 1), itemsPerPage, sortColumn, sortOrder).ToList());
			});

		protected virtual IActionResult CreateBase<TReturn>(bool closeOnSubmit = false, string modelName = "CreateOrEdit")
			where TReturn : IDataTransferObjectCore<TDataType, TEntity, TReturn> => Authorize(User, () => {
				TempData["CloseOnSubmit"] = closeOnSubmit;
				return View(modelName, Activator.CreateInstance(typeof(TReturn)));
			});

		protected virtual IActionResult CreateBase<TModel>(TModel model, string modelName = "CreateOrEdit")
			where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel> => Authorize(User, () => {
				if (ModelState.IsValid) {
					ViewData["CloseOnSubmit"] = TempData["CloseOnSubmit"];
					_dataService.Create(model);
					ModelState.Clear();
					return View(modelName, Activator.CreateInstance(typeof(TModel)));
				}

				return View(modelName, model);
			});

		protected virtual IActionResult EditBase<TModel>(TDataType id, bool closeOnSubmit = false, string modelName = "CreateOrEdit")
			where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel> => Authorize(User, () => {
				TempData["CloseOnSubmit"] = closeOnSubmit;
				return View(modelName, _dataService.Get<TModel>(id));
			});

		protected virtual IActionResult EditBase<TModel>(TModel model, string modelName = "CreateOrEdit")
			where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel> => Authorize(User, () => {
				if (ModelState.IsValid) {
					_dataService.Save(model);
				}

				return View(modelName, model);
			});

		protected virtual IActionResult DetailsBase<TModel>(TDataType id)
			where TModel : IDataTransferObjectCore<TDataType, TEntity, TModel>
			=> Authorize(User, () => View(_dataService.Get<TModel>(id)));

		protected virtual IActionResult Authorize(ClaimsPrincipal user, Func<IActionResult> func) => AuthorizeExecution(user, func);

		public ControllerCore(IDataServiceCore<TDataType, TEntity> dataService) {
			_dataService = dataService;
		}
	}

	public class ControllerInt<TEntity, TIndex, TEdit>
		: ControllerCore<int, TEntity, TIndex, TEdit>
		where TEntity : IDataModelInt
		where TIndex : IDataTransferObjectInt<TEntity, TIndex>
		where TEdit : IDataTransferObjectInt<TEntity, TEdit> {

		public ControllerInt(IDataServiceInt<TEntity> dataService)
			: base(dataService) {
		}
	}

	public class ControllerLong<TEntity, TIndex, TEdit>
	: ControllerCore<long, TEntity, TIndex, TEdit>
	where TEntity : IDataModelLong
	where TIndex : IDataTransferObjectLong<TEntity, TIndex>
	where TEdit : IDataTransferObjectLong<TEntity, TEdit> {

		public ControllerLong(IDataServiceLong<TEntity> dataService)
			: base(dataService) {
		}
	}

	public class ControllerGuid<TEntity, TIndex, TEdit>
	: ControllerCore<Guid, TEntity, TIndex, TEdit>
	where TEntity : IDataModelGuid
	where TIndex : IDataTransferObjectGuid<TEntity, TIndex>
	where TEdit : IDataTransferObjectGuid<TEntity, TEdit> {

		public ControllerGuid(IDataServiceGuid<TEntity> dataService)
			: base(dataService) {
		}
	}

	public class ControllerBase<TEntity, TIndex, TEdit>
	: ControllerInt<TEntity, TIndex, TEdit>
	where TEntity : IDataModelInt
	where TIndex : IDataTransferObjectInt<TEntity, TIndex>
	where TEdit : IDataTransferObjectInt<TEntity, TEdit> {

		public ControllerBase(IDataServiceInt<TEntity> dataService)
			: base(dataService) {
		}
	}
}