﻿using QuickFrame.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFrame.Mvc
{
	public class ControllerGuid<TEntity, TIndex, TEdit>
		: ControllerCore<Guid, TEntity, TIndex, TEdit>
		where TEntity : IDataModelGuid
		where TIndex : IDataTransferObjectGuid<TEntity, TIndex>
		where TEdit : IDataTransferObjectGuid<TEntity, TEdit> {

		public ControllerGuid(IDataServiceGuid<TEntity> dataService)
			: base(dataService) {
		}
	}
}
