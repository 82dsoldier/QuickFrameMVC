namespace QuickFrame.Security.AccountControl.ActiveDirectory.AdLookup.Interfaces {

	public interface ITransformData<TArg, TResult> {

		TResult Call(TArg arg);
	}
}