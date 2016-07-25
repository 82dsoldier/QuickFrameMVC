﻿using QuickFrame.Data.Attachments.Dtos;
using QuickFrame.Data.Attachments.Models;
using QuickFrame.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickFrame.Data.Interfaces;
using QuickFrame.Data.Attachments.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace QuickFrame.Data.Attachments.Ui.Areas.Attachments.Controllers
{
	[Area("Attachments")]
	public class MimeTypesController : ControllerBase<MimeType, MimeTypeDto, MimeTypeDto> {
		public MimeTypesController(IMimeTypesDataService dataService) : base(dataService) {
		}
	}
}
