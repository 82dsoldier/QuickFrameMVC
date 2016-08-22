﻿using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using QuickFrame.Data.Interfaces;
using QuickFrame.Di;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Claims;
using static QuickFrame.Security.AuthorizationExtensions;

namespace QuickFrame.Mvc {

	public abstract class GenericControllerCore<TDataType, TEntity, TIndex, TEdit>
		: Controller
		where TIndex : IGenericDataTransferObject<TEntity, TIndex>
		where TEdit : IGenericDataTransferObject<TEntity, TEdit> {

		protected readonly IGenericDataService<TDataType, TEntity> _dataService;

		[HttpGet]
		public IActionResult Create(bool closeOnSubmit = false) => CreateBase<TEdit>(closeOnSubmit);

		[HttpPost]
		public IActionResult Create(TEdit model) => CreateBase(model);

		[HttpDelete]
		public IActionResult Delete(TDataType id) => new ObjectResult(DeleteBase(id));

		[HttpGet]
		public IActionResult Details(TDataType id) => DetailsBase<TEdit>(id);

		[HttpGet]
		public IActionResult Edit(TDataType id, bool closeOnSubmit = false) => EditBase<TEdit>(id, closeOnSubmit);

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

		protected bool DeleteBase(TDataType id) => _dataService.Delete(id);

		protected virtual IActionResult DetailsBase<TModel>(TDataType id)
			where TModel : IGenericDataTransferObject<TEntity, TModel>
			=> Authorize(User, () => View(_dataService.Get<TModel>(id)));

		protected virtual IActionResult EditBase<TModel>(TDataType id, bool closeOnSubmit = false, string modelName = "CreateOrEdit")
			where TModel : IGenericDataTransferObject<TEntity, TModel> => Authorize(User, () => {
				TempData["CloseOnSubmit"] = closeOnSubmit;
				return View(modelName, _dataService.Get<TModel>(id));
			});

		protected virtual IActionResult EditBase<TModel>(TModel model, string modelName = "CreateOrEdit")
			where TModel : IGenericDataTransferObject<TEntity, TModel> => Authorize(User, () => {
				if(ModelState.IsValid) {
					_dataService.Save(model);
				}

				return View(modelName, model);
			});

		protected virtual IActionResult GetBase() => /*Authorize(User, () =>*/ new ObjectResult(_dataService.GetList<TIndex>());//);

		protected virtual IActionResult IndexBase<TResult>
			(int page = 1, int itemsPerPage = 25, string sortColumn = "Name", SortOrder sortOrder = SortOrder.Ascending)
			where TResult : IGenericDataTransferObject<TEntity, TResult> => this.Authorize(User, () => {
				ViewData["totalItems"] = _dataService.GetCount();
				return View("Index", _dataService.GetList<TResult>(itemsPerPage * (page - 1), itemsPerPage, sortColumn, sortOrder).ToList());
			});

		protected virtual IActionResult Authorize(ClaimsPrincipal user, Func<IActionResult> func) => AuthorizeExecution(user, CurrentUrl, func);

		protected virtual IActionResult Pdf(string viewName, object dataModel, Rectangle pageSize = null) {
			using(var document = new Document()) {
				if(pageSize == null)
					pageSize = PageSize.LETTER;
				document.SetPageSize(pageSize);
				using(var ms = new MemoryStream()) {
					PdfWriter writer = PdfWriter.GetInstance(document, ms);
					writer.CloseStream = false;
					document.Open();
					using(var sw = new StringWriter()) {
						ViewData.Model = dataModel;
						var viewEngine = ComponentContainer.Component<ICompositeViewEngine>();
						var contextAccessor = ComponentContainer.Component<IActionContextAccessor>();
						var viewResult = viewEngine.Component.FindView(contextAccessor.Component.ActionContext, viewName, true);
						var viewContext = new ViewContext(contextAccessor.Component.ActionContext, viewResult.View, ViewData, TempData, sw, new HtmlHelperOptions());
						var viewTask = viewResult.View.RenderAsync(viewContext);
						viewTask.Wait();
						using(var reader = new StringReader(sw.ToString())) {
							XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, reader);
							document.Close();
							return new FileContentResult(ms.ToArray(), "application/pdf");
						}
					}
				}
			}
		}
		protected string CurrentUrl
		{
			get
			{
				var builder = new UriBuilder {
					Scheme = Request.Scheme,
					Host = Request.Host.Host,
					Port = Request.Host.Port ?? 80,
					Path = Request.Path,
					Query = Request.QueryString.ToUriComponent()
				};

				return builder.Uri.ToString();
			}
		}

		public GenericControllerCore(IGenericDataService<TDataType, TEntity> dataService) {
			_dataService = dataService;
		}
	}
}