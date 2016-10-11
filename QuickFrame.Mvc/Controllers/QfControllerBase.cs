using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Services;
using QuickFrame.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Security.Claims;

namespace QuickFrame.Mvc.Controllers {

	public class QfControllerBase<TEntity, TIndex>
		: Controller
		where TEntity : class
		where TIndex : class, IDataTransferObjectCore {
		protected IDataServiceBase<TEntity> _dataService;
		protected QuickFrameSecurityManager _securityManager;
		protected string IndexPage = "Index";

		public QfControllerBase(IDataServiceBase<TEntity> dataService, QuickFrameSecurityManager securityManager) {
			_dataService = dataService;
			_securityManager = securityManager;
		}

		[HttpGet]
		public IActionResult Index(string searchTerm = "", int page = 1, int itemsPerPage = 25, string sortColumn = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false)
			=> IndexCore<TIndex>(searchTerm, page, itemsPerPage, sortColumn, sortOrder, includeDeleted);

		protected virtual IActionResult IndexCore<TResult>(string searchTerm = "", int page = 1, int itemsPerPage = 25, string sortColumn = "Name", SortOrder sortOrder = SortOrder.Ascending, bool includeDeleted = false) where TResult : IDataTransferObjectCore {
			ViewBag.TotalItems = _dataService.GetCount();
			IEnumerable<TResult> model = _dataService.GetList<TResult>(searchTerm, page, itemsPerPage, sortColumn, sortOrder, includeDeleted);
			return View(IndexPage, model);
		}

		protected virtual IActionResult Authorize(ClaimsPrincipal user, Func<IActionResult> func) => _securityManager.AuthorizeExecution(user, CurrentUrl, func);

		protected virtual IActionResult Pdf(object dataModel, Rectangle pageSize = null) {
			return Pdf(String.Empty, dataModel, pageSize);
		}

		protected virtual IActionResult Pdf(string viewName, object dataModel, Rectangle pageSize = null) {
			if(String.IsNullOrEmpty(viewName))
				viewName = ControllerContext.RouteData.Values["action"].ToString();
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
						var viewEngine = (ICompositeViewEngine)HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine));
						var contextAccessor = (IActionContextAccessor)HttpContext.RequestServices.GetService(typeof(IActionContextAccessor));
						var viewResult = viewEngine.FindView(contextAccessor.ActionContext, viewName, true);
						var viewContext = new ViewContext(contextAccessor.ActionContext, viewResult.View, ViewData, TempData, sw, new HtmlHelperOptions());
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
	}
}