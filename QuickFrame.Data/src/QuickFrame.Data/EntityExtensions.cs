using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

		[Obsolete("Use IsDeleted(false)")]
		public static IQueryable<TSource> IsNotDeleted<TSource>(this IQueryable<TSource> source) {
			var parameterExpression = Expression.Parameter(typeof(TSource));
			var propertyExpression = Expression.Property(parameterExpression, "IsDeleted");
			var notExpression = Expression.Not(propertyExpression);
			var lambdaExpression = Expression.Lambda<Func<TSource, bool>>(notExpression, parameterExpression);
			var compiled = lambdaExpression.Compile();
			return source.Where(lambdaExpression);
		}

		public static IQueryable<TSource> IsDeleted<TSource>(this IQueryable<TSource> source, bool val) {
			var parameterExpression = Expression.Parameter(typeof(TSource));
			var propertyExpression = Expression.Property(parameterExpression, "IsDeleted");
			var boolExpression = Expression.Equal(propertyExpression, Expression.Constant(val));
			var lambdaExpression = Expression.Lambda<Func<TSource, bool>>(boolExpression, parameterExpression);
			var compiled = lambdaExpression.Compile();
			return source.Where(lambdaExpression);
		}

		public static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName) {
			string[] props = property.Split('.');
			Type type = typeof(T);
			ParameterExpression arg = Expression.Parameter(type, "x");
			Expression expr = arg;
			foreach(string prop in props) {
				// use reflection (not ComponentModel) to mirror LINQ
				PropertyInfo pi = type.GetProperty(prop);
				expr = Expression.Property(expr, pi);
				type = pi.PropertyType;
			}
			Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
			LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

			object result = typeof(Queryable).GetMethods().Single(
					method => method.Name == methodName
							&& method.IsGenericMethodDefinition
							&& method.GetGenericArguments().Length == 2
							&& method.GetParameters().Length == 2)
					.MakeGenericMethod(typeof(T), type)
					.Invoke(null, new object[] { source, lambda });
			return (IOrderedQueryable<T>)result;
		}
	}
}