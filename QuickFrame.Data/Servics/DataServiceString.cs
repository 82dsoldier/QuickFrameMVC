using ExpressMapper;
using QuickFrame.Data.Interfaces;
using QuickFrame.Di;
using System.Data.Entity;
using System.Linq;

namespace QuickFrame.Data.Services {

	public class DataServiceString<TContext, TEntity>
		: GenericDataService<TContext, string, TEntity>,
		IDataServiceString<TEntity>
		where TContext : DbContext
		where TEntity : class, IDataModelString {

		public override string Create(TEntity model) {
			using(var context = ComponentContainer.Component<TContext>()) {
				context.Component.Set<TEntity>().Add(model);
				context.Component.SaveChanges();
				return model.Id;
			}
		}

		public override bool Delete(string id) {
			using(var context = ComponentContainer.Component<TContext>()) {
				context.Component.Set<TEntity>().Remove(context.Component.Set<TEntity>().First(obj => obj.Id == id));
				context.Component.SaveChanges();
				return true;
			}
		}

		public override TEntity Get(string id) {
			using(var context = ComponentContainer.Component<TContext>()) {
				return context.Component.Set<TEntity>().FirstOrDefault(obj => obj.Id == id);
			}
		}

		public override TResult Get<TResult>(string id) {
			using(var context = ComponentContainer.Component<TContext>()) {
				var entry = context.Component.Set<TEntity>().FirstOrDefault(obj => obj.Id == id);
				return Mapper.Map<TEntity, TResult>(entry);
			}
		}

		public override void Save(TEntity model) {
			using(var context = ComponentContainer.Component<TContext>()) {
				context.Component.Set<TEntity>().Attach(model);
				context.Component.Entry(model).State = EntityState.Modified;
				context.Component.SaveChanges();
			}
		}
	}
}