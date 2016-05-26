namespace QuickFrame.Data.Interfaces {

	public interface IGenericDataService<TDataType, TEntity> : IReadOnlyDataService<TDataType, TEntity> {

		void Create(TEntity model);

		void CreateAsync(TEntity model);

		void Create<TModel>(TModel model);

		void CreateAsync<TModel>(TModel model);

		void Delete(TDataType id);

		void DeleteAsync(TDataType id);

		void Save(TEntity model);

		void SaveAsync(TEntity model);

		void Save<TModel>(TModel model);

		void SaveAsync<TModel>(TModel model);
	}
}