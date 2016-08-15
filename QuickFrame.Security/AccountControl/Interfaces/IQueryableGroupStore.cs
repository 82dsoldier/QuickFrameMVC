using System.Linq;

namespace QuickFrame.Security.AccountControl.Interfaces {

	public interface IQueryableGroupStore<TGroup> where TGroup : class {
		IQueryable<TGroup> Groups { get; }
	}
}