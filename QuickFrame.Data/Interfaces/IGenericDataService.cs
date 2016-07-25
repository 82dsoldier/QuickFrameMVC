using System.Threading.Tasks;

namespace QuickFrame.Data.Interfaces {

	public interface IGenericDataService<TDataType, TEntity> : IReadOnlyDataService<TDataType, TEntity> {

		TDataType Create(TEntity model);

		Task<TDataType> CreateAsync(TEntity model);

		TDataType Create<TModel>(TModel model);

		Task<TDataType> CreateAsync<TModel>(TModel model);

		bool Delete(TDataType id);

		void DeleteAsync(TDataType id);

		void Save(TEntity model);

		void SaveAsync(TEntity model);

		void Save<TModel>(TModel model);

		void SaveAsync<TModel>(TModel model);
	}
}