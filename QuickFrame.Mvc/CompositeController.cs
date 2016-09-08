using Microsoft.AspNetCore.Mvc;
using QuickFrame.Data.Interfaces;
using QuickFrame.Security.Attributes;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using static QuickFrame.Security.AuthorizationExtensions;

namespace QuickFrame.Mvc {
	[Obsolete("Use ControllerCore<TEntity, T1, T2, TIndex, TEdit> instead")]
	public abstract class CompositeController<TPrimaryDataType, TSecondaryDataType, TEntity, TIndex, TEdit>
		: Controller
		where TIndex : IGenericDataTransferObject<TEntity, TIndex>
		where TEdit : IGenericDataTransferObject<TEntity, TEdit> {
		protected readonly ICompositeDataService<TPrimaryDataType, TSecondaryDataType, TEntity> _dataService;

		[HttpGet]
		public IActionResult Create(bool closeOnSubmit = false) => CreateBase<TEdit>(closeOnSubmit);

		[HttpPost]
		public IActionResult Create(TEdit model) => CreateBase(model);

		[HttpGet]
		public IActionResult Details(TPrimaryDataType primaryId, TSecondaryDataType secondaryId) => DetailsBase<TEdit>(primaryId, secondaryId);

		[HttpGet]
		public IActionResult Edit(TPrimaryDataType primaryId, TSecondaryDataType secondaryId, bool closeOnSubmit = false) => EditBase<TEdit>(primaryId, secondaryId, closeOnSubmit);

		[HttpPost]
		public IActionResult Edit(TEdit model) => EditBase(model);

		[HttpGet]
		public virtual IActionResult Get() => GetBase();

		[HttpGet]
		public virtual IActionResult CloseCurrentView() => View();

		[HttpGet]
		public IActionResult Index(int page = 1, int itemsPerPage = 25, string sortColumn = "Name", SortOrder sortOrder = SortOrder.Ascending)
			=> IndexBase<TIndex>(page, itemsPerPage, sortColumn, sortOrder);

		protected virtual IActionResult CreateBase<TReturn>(bool closeOnSubmit = false, string modelName = "CreateOrEdit")
			where TReturn : IGenericDataTransferObject<TEntity, TReturn> => Authorize(User, () => {
				TempData["CloseOnSubmit"] = closeOnSubmit;
				return View(modelName, Activator.CreateInstance(typeof(TReturn)));
			});

		protected virtual IActionResult CreateBase<TModel>(TModel model, string modelName = "CreateOrEdit")
			where TModel : IGenericDataTransferObject<TEntity, TModel> => Authorize(User, () => {
				if(ModelState.IsValid) {
					ViewData["CloseOnSubmit"] = TempData["CloseOnSubmit"];
					_dataService.Create(model);
					ModelState.Clear();
					return View(modelName, Activator.CreateInstance(typeof(TModel)));
				}

				return View(modelName, model);
			});

		protected virtual IActionResult DetailsBase<TModel>(TPrimaryDataType primaryId, TSecondaryDataType secondaryId)
			where TModel : IGenericDataTransferObject<TEntity, TModel>
			=> Authorize(User, () => View(_dataService.Get<TModel>(primaryId, secondaryId)));

		protected virtual IActionResult EditBase<TModel>(TPrimaryDataType primaryId, TSecondaryDataType secondaryId, bool closeOnSubmit = false, string modelName = "CreateOrEdit")
			where TModel : IGenericDataTransferObject<TEntity, TModel> => Authorize(User, () => {
				TempData["CloseOnSubmit"] = closeOnSubmit;
				return View(modelName, _dataService.Get<TModel>(primaryId, secondaryId));
			});

		protected virtual IActionResult EditBase<TModel>(TModel model, string modelName = "CreateOrEdit")
			where TModel : IGenericDataTransferObject<TEntity, TModel> => Authorize(User, () => {
				if(ModelState.IsValid) {
					_dataService.Save(model);
				}

				return View(modelName, model);
			});

		protected virtual IActionResult GetBase() => Authorize(User, () => new ObjectResult(_dataService.GetList<TIndex>()));

		protected virtual IActionResult IndexBase<TResult>
			(int page = 1, int itemsPerPage = 25, string sortColumn = "Name", SortOrder sortOrder = SortOrder.Ascending)
			where TResult : IGenericDataTransferObject<TEntity, TResult> => this.Authorize(User, () => {
				ViewData["totalItems"] = _dataService.GetCount();
				return View("Index", _dataService.GetList<TResult>(itemsPerPage * (page - 1), itemsPerPage, sortColumn, sortOrder).ToList());
			});

		protected virtual IActionResult Authorize(ClaimsPrincipal user, Func<IActionResult> func) => AuthorizeExecution(user, CurrentUrl, func);

		protected string CurrentUrl
		{
			get
			{
				var builder = new UriBuilder {
					Scheme = Request.Scheme,
					Host = Request.Host.Value,
					Path = Request.Path,
					Query = Request.QueryString.ToUriComponent()
				};

				return builder.Uri.ToString();
			}
		}

		public CompositeController(ICompositeDataService<TPrimaryDataType, TSecondaryDataType, TEntity> dataService) {
			_dataService = dataService;
		}
	}
}