using QuickFrame.Data.Interfaces.Dtos;
using QuickFrame.Data.Interfaces.Services;
#if NETSTANDARD1_6
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace QuickFrame.Data.Services {

	/// <summary>
	/// Abstract class that provides a base for data services that implement delete and get functionality.
	/// </summary>
	/// <typeparam name="TContext">The ntity framework context used to access the database</typeparam>
	/// <typeparam name="TEntity">The type of entity being used by this service.</typeparam>
	/// <typeparam name="TIdType">The typep of Id used by the entity.</typeparam>
	public abstract class DataServiceCore<TContext, TEntity, TIdType>
		: DataServiceBase<TContext, TEntity>, IDataServiceCore<TEntity, TIdType>
		where TContext : DbContext
		where TEntity : class {


		/// <summary>
		/// The data service constructor.
		/// </summary>
		/// <param name="dbContext">Accepts a copy of the context being used for this service.</param>
		public DataServiceCore(TContext dbContext)
			: base(dbContext) {
		}

		/// <summary>
		/// Sets IsDeleted to true on the specified model.
		/// </summary>
		/// <param name="id">THe id of the model to mark as deleted.</param>
		public abstract void Delete(TIdType id);

		/// <summary>
		/// Returns the specified entity.
		/// </summary>
		/// <param name="id">The id of the model to return.</param>
		/// <returns>The specified model.</returns>
		public abstract TEntity Get(TIdType id);

		/// <summary>
		/// Returns the specified entity converted to a TResult.
		/// </summary>
		/// <typeparam name="TResult">The type to convert the model to.</typeparam>
		/// <param name="id">The id of the model to return.</param>
		/// <returns>The specified model converted to the specified type.</returns>
		public abstract TResult Get<TResult>(TIdType id) where TResult : IDataTransferObjectCore;
	}
}