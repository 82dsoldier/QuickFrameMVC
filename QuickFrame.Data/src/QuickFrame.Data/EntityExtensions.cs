using System;
using System.Linq;
using System.Linq.Expressions;

namespace QuickFrame.Data {

	public static class EntityExtensions {

		/// <summary>
		/// Orders a query by the specified property.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <param name="source">The query to order.</param>
		/// <param name="propertyName">Name of the property to use for ordering.</param>
		/// <returns>An IQueryable representing the original query with the OrderBy clause appended.</returns>
		public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string propertyName) {
			var parameter = Expression.Parameter(typeof(TSource), "obj");
			var member = Expression.PropertyOrField(parameter, propertyName);
			var lambda = Expression.Lambda(member, parameter);
			Type[] argTypes = { source.ElementType, lambda.Body.Type };
			var methodCall = Expression.Call(typeof(Queryable), "OrderBy", argTypes, source.Expression, lambda);
			return source.Provider.CreateQuery<TSource>(methodCall);
		}

		/// <summary>
		/// Orders a query by the specified property.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <param name="source">The query to order.</param>
		/// <param name="propertyName">Name of the property to use for ordering.</param>
		/// <returns>An IQueryable representing the original query with the OrderBy clause appended.</returns>
		public static IQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> source, string propertyName) {
			var parameter = Expression.Parameter(typeof(TSource), "obj");
			var member = Expression.PropertyOrField(parameter, propertyName);
			var lambda = Expression.Lambda(member, parameter);
			Type[] argTypes = { source.ElementType, lambda.Body.Type };
			var methodCall = Expression.Call(typeof(Queryable), "OrderByDescending", argTypes, source.Expression, lambda);
			return source.Provider.CreateQuery<TSource>(methodCall);
		}

		public static IQueryable<TSource> IsNotDeleted<TSource>(this IQueryable<TSource> source) {
			var parameterExpression = Expression.Parameter(typeof(TSource));
			var propertyExpression = Expression.Property(parameterExpression, "IsDeleted");
			var notExpression = Expression.Not(propertyExpression);
			var lambdaExpression = Expression.Lambda<Func<TSource, bool>>(notExpression, parameterExpression);
			var compiled = lambdaExpression.Compile();
			return source.Where(lambdaExpression);
		}
	}
}