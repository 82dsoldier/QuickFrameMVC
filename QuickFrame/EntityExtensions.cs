using System;
using System.Linq;
using System.Linq.Expressions;

namespace QuickFrame {

	/// <summary>
	/// Extensions made for use with Entity Framwork to provide specialized Linq queries.
	/// </summary>
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

		/// <summary>
		/// Adds a String.Contains clause to a query.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="query">The query to order.</param>
		/// <param name="filterColumn">The column on which to add Contains.</param>
		/// <param name="val">The value to test for.</param>
		/// <returns>An IQueryable representing the original query with the Contains clause appended.</returns>
		public static IQueryable<T> QueryContains<T>(this IQueryable<T> query, string filterColumn, string val) {
			var parameterExp = Expression.Parameter(typeof(T), "type");
			var propertyExp = Expression.Property(parameterExp, filterColumn);
			var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
			var constantVal = Expression.Constant(val, typeof(string));
			var resultExpression = Expression.Call(propertyExp, method, constantVal);
			return query.Provider.CreateQuery<T>(resultExpression);
		}

		/// <summary>
		/// Adds a Where clause to the specified query.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <typeparam name="TConfigurable">The type of the configurable.</typeparam>
		/// <param name="source">The query to order.</param>
		/// <param name="column">The column on which to apply the Where clause.</param>
		/// <param name="val">The value to test for.</param>
		/// <returns></returns>
		public static IQueryable<TSource> Where<TSource, TConfigurable>(this IQueryable<TSource> source, TConfigurable val, string column) {
			var parameter = Expression.Parameter(typeof(TSource), "obj");
			var propertyAccess = Expression.MakeMemberAccess(parameter, typeof(TSource).GetProperty(column));
			var constant = Expression.Constant(val, typeof(TConfigurable));
			var exp = Expression.Equal(propertyAccess, constant);
			Expression result = Expression.Call(typeof(Queryable), "Where", new[] { source.ElementType }, source.Expression,
				Expression.Lambda<Func<TSource, bool>>(exp, parameter));
			return source.Provider.CreateQuery<TSource>(result);
		}
	}
}