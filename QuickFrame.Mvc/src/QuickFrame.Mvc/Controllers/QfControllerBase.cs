using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Services;
using QuickFrame.Mvc.Configuration;
using QuickFrame.Mvc.Utilities;
using QuickFrame.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace QuickFrame.Mvc.Controllers {
	/// <summary>
	/// The base class from which all MVC controllers should inherit.
	/// </summary>
	/// <typeparam name="TEntity">The type of entity that will be used in this controller.</typeparam>
	/// <typeparam name="TIndex">The type of class to which the entity will be mapped when returning the Index page.</typeparam>
	public class QfControllerBase<TEntity, TIndex>
		: Controller
		where TEntity : class
		where TIndex : class, IDataTransferObjectCore {
		protected IDataServiceBase<TEntity> _dataService;
		protected QuickFrameSecurityManager _securityManager;
		protected string IndexPage = "Index";
		protected ViewOptions _viewOptions;

		/// <summary>
		/// The constructor for the QFControllerBase class.
		/// </summary>
		/// <param name="dataService">The data service used to return data to the views contained within this class.</param>
		/// <param name="securityManager">The <see cref="QuickFrame.Security.QuickFrameSecurityManager"/>  used to apply security to functions within this class.</param>
		public QfControllerBase(IDataServiceBase<TEntity> dataService, QuickFrameSecurityManager securityManager) {
			_dataService = dataService;
			_securityManager = securityManager;
			_viewOptions = new ViewOptions {
				PerPageList = new List<SelectListItem>() {
					new SelectListItem {  Text = "25", Value = "25" },
					new SelectListItem {  Text = "50", Value = "50" },
					new SelectListItem {  Text = "75", Value = "75" },
					new SelectListItem {  Text = "100", Value = "100" },
				},
				PerPageDefault = "25"
			};
		}

		public QfControllerBase(IDataServiceBase<TEntity> dataService, QuickFrameSecurityManager securityManager, IOptions<ViewOptions> viewOptions) {
			_dataService = dataService;
			_securityManager = securityManager;
			_viewOptions = viewOptions.Value;
		}
		/// <summary>
		/// Returns the index page for the current controller
		/// </summary>
		/// <param name="searchTerm">The term for which to search, if any.</param>
		/// <param name="page">If paging is being used, the page on which to start.</param>
		/// <param name="itemsPerPage">If paging is being used, the number of items to return on each page.</param>
		/// <param name="sortColumn">If sorting or searching, the name of the column on which to sort or search.</param>
		/// <param name="sortOrder">If sorting, the direction in which to sort.</param>
		/// <param name="isDeleted">True to return records that have been marked as deleted. Null to return all records.</param>
		/// <returns>An IActionResult representing the index page for the current controller.</returns>
		[HttpGet]
		public IActionResult Index(string searchTerm, int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, bool? isDeleted = false)
			=> Authorize(() => {
				if(itemsPerPage == 0)
					Int32.TryParse(_viewOptions.PerPageDefault, out itemsPerPage);
				return IndexCore<TIndex>(searchTerm, page, itemsPerPage, sortColumn, sortOrder, isDeleted);
			});

		/// <summary>
		/// The core function called by Index in order to return the Index page for the current controller.
		/// </summary>
		/// <typeparam name="TResult">The type of class to return from the database.</typeparam>
		/// <param name="searchTerm">The term for which to search, if any.</param>
		/// <param name="page">If paging is being used, the page on which to start.</param>
		/// <param name="itemsPerPage">If paging is being used, the number of items to return on each page.</param>
		/// <param name="sortColumn">If sorting or searching, the name of the column on which to sort or search.</param>
		/// <param name="sortOrder">If sorting, the direction in which to sort.</param>
		/// <param name="isDeleted">True to return records that have been marked as deleted. Null to return all records.</param>
		/// <returns>An IActionResult representing the index page for the current controller.</returns>
		/// <remarks>If overriding the core index functionality, override this function rather than <see cref="QuickFrame.Mvc.Controllers.QfControllerBase{TEntity, TIndex}.Index(string, int, int, string, SortOrder, bool)"/> </remarks>
		protected virtual IActionResult IndexCore<TResult>(string searchTerm, int page, int itemsPerPage, string sortColumn, SortOrder sortOrder, bool? isDeleted = false) 
			where TResult : IDataTransferObjectCore {
			ViewBag.TotalItems = _dataService.GetCount("Name", searchTerm, isDeleted);
			IEnumerable<TResult> model = _dataService.GetList<TResult>(page, itemsPerPage, sortColumn, sortOrder, searchTerm, "Name", isDeleted);
			return View(IndexPage, model);
		}

		/// <summary>
		/// Wrapper function for authorizing a user to access a URL in the security manager.
		/// </summary>
		/// <param name="func">The function to execute if the user is authorized.</param>
		/// <returns>An IActionResult representing the page returned by the specified function.</returns>
		protected virtual IActionResult Authorize(Func<IActionResult> func) => _securityManager.AuthorizeExecution(User, CurrentUrl, func);

		/// <summary>
		/// Returns the specified web page as a PDF.
		/// </summary>
		/// <param name="dataModel">The data model to use.</param>
		/// <param name="pageSize">The size of the page to create.</param>
		/// <returns>A pdf as an IActionResult.</returns>
		protected virtual IActionResult Pdf(object dataModel, Rectangle pageSize = null) {
			return Pdf(String.Empty, dataModel, pageSize);
		}

		/// <summary>
		/// Returns the specified web page as a PDF.
		/// </summary>
		/// <param name="viewName">The name of the view to convert to PDF.</param>
		/// <param name="dataModel">The data model to use.</param>
		/// <param name="pageSize">The size of the page to create.</param>
		/// <returns>A pdf as an IActionResult.</returns>
		protected virtual IActionResult Pdf(string viewName, object dataModel, Rectangle pageSize = null) {
			return new FileContentResult(PdfBytes(viewName, dataModel, pageSize), "application/pdf");
		}

		/// <summary>
		/// Converts the specified view name and data model to a PDF.
		/// </summary>
		/// <param name="viewName">The name of the view to convert to PDF.</param>
		/// <param name="dataModel">The data model to use.</param>
		/// <param name="pageSize">The size of the page to create.</param>
		/// <returns>The raw data of the PDF as a byte array.</returns>
		protected virtual byte[] PdfBytes(string viewName, object dataModel, Rectangle pageSize) {
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
							var tagProcessors = (DefaultTagProcessorFactory)Tags.GetHtmlTagProcessorFactory();
							tagProcessors.RemoveProcessor(HTML.Tag.IMG); // remove the default processor
							tagProcessors.AddProcessor(HTML.Tag.IMG, new EmbeddedImageTagProcessor()); // use our new processor

							CssFilesImpl cssFiles = new CssFilesImpl();
							cssFiles.Add(XMLWorkerHelper.GetInstance().GetDefaultCSS());
							var cssResolver = new StyleAttrCSSResolver(cssFiles);
							var charset = Encoding.UTF8;
							var hpc = new HtmlPipelineContext(new CssAppliersImpl(new XMLWorkerFontProvider()));
							hpc.SetAcceptUnknown(true).AutoBookmark(true).SetTagFactory(tagProcessors); // inject the tagProcessors
							var htmlPipeline = new HtmlPipeline(hpc, new PdfWriterPipeline(document, writer));
							var pipeline = new CssResolverPipeline(cssResolver, htmlPipeline);
							var worker = new XMLWorker(pipeline, true);
							var xmlParser = new XMLParser(true, worker, charset);
							//XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, reader);
							xmlParser.Parse(reader);
							document.Close();
							return ms.ToArray();
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets the fully qualified URL of the current page.
		/// </summary>
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

		protected string CurrentAction
		{
			get
			{
				return $"{Request.Host.Host}.{Request.Host.Port ?? 80}.{Request.Path.ToString().Replace("/", ".")}";
			}
		}
	}
}