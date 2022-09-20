/*******************************************************************************
 * Copyright © 2016 WaterCloud.Framework 版权所有
 * Author: WaterCloud
 * Description: WaterCloud快速开发平台
 * Website：
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace WaterCloud.Code
{
	public static partial class ExtLinq
	{
		public static Expression Property(this Expression expression, string propertyName)
		{
			return Expression.Property(expression, propertyName);
		}

		public static Expression AndAlso(this Expression left, Expression right)
		{
			return Expression.AndAlso(left, right);
		}

		public static Expression OrElse(this Expression left, Expression right)
		{
			return Expression.OrElse(left, right);
		}

		public static Expression Call(this Expression instance, string methodName, params Expression[] arguments)
		{
			return Expression.Call(instance, instance.Type.GetMethod(methodName), arguments);
		}

		public static Expression GreaterThan(this Expression left, Expression right)
		{
			return Expression.GreaterThan(left, right);
		}

		public static Expression<T> ToLambda<T>(this Expression body, params ParameterExpression[] parameters)
		{
			return Expression.Lambda<T>(body, parameters);
		}

		public static Expression<Func<T, bool>> True<T>()
		{ return param => true; }

		public static Expression<Func<T, bool>> False<T>()
		{ return param => false; }

		public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
		{
			return first.Compose(second, Expression.AndAlso);
		}

		public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
		{
			return first.Compose(second, Expression.OrElse);
		}

		public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
		{
			var map = first.Parameters
				.Select((f, i) => new { f, s = second.Parameters[i] })
				.ToDictionary(p => p.s, p => p.f);
			var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
			return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
		}

		private class ParameterRebinder : ExpressionVisitor
		{
			private readonly Dictionary<ParameterExpression, ParameterExpression> map;

			/// <summary>
			/// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
			/// </summary>
			/// <param name="map">The map.</param>
			private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
			{
				this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
			}

			/// <summary>
			/// Replaces the parameters.
			/// </summary>
			/// <param name="map">The map.</param>
			/// <param name="exp">The exp.</param>
			/// <returns>Expression</returns>
			public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
			{
				return new ParameterRebinder(map).Visit(exp);
			}

			protected override Expression VisitParameter(ParameterExpression p)
			{
				ParameterExpression replacement;

				if (map.TryGetValue(p, out replacement))
				{
					p = replacement;
				}
				return base.VisitParameter(p);
			}
		}

		public static ParameterExpression CreateLambdaParam<T>(string name)
		{
			return Expression.Parameter(typeof(T), name);
		}

		/// <summary>
		/// 创建完整的lambda
		/// </summary>
		public static LambdaExpression GenerateLambda(this ParameterExpression param, Expression body)
		{
			//c=>c.XXX=="XXX"
			return Expression.Lambda(body, param);
		}

		public static Expression<Func<T, bool>> GenerateTypeLambda<T>(this ParameterExpression param, Expression body)
		{
			return (Expression<Func<T, bool>>)(param.GenerateLambda(body));
		}

		public static Expression Or(this Expression expression, Expression expressionRight)
		{
			return Expression.Or(expression, expressionRight);
		}

		public static Expression And(this Expression expression, Expression expressionRight)
		{
			return Expression.And(expression, expressionRight);
		}

		public static IOrderedQueryable<TEntity> SortBy<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, dynamic>> sortPredicate)
		   where TEntity : class, new()
		{
			return InvokeSortBy(query, sortPredicate, SortOrder.Ascending);
		}

		public static IOrderedQueryable<TEntity> SortByDescending<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, dynamic>> sortPredicate)
			where TEntity : class, new()
		{
			return InvokeSortBy(query, sortPredicate, SortOrder.Descending);
		}

		private static IOrderedQueryable<TEntity> InvokeSortBy<TEntity>(IQueryable<TEntity> query,
			Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder)
			where TEntity : class, new()
		{
			var param = sortPredicate.Parameters[0];
			string propertyName = null;
			Type propertyType = null;
			Expression bodyExpression = null;
			if (sortPredicate.Body is UnaryExpression)
			{
				var unaryExpression = sortPredicate.Body as UnaryExpression;
				bodyExpression = unaryExpression.Operand;
			}
			else if (sortPredicate.Body is MemberExpression)
			{
				bodyExpression = sortPredicate.Body;
			}
			else
				throw new ArgumentException(@"The body of the sort predicate expression should be
                either UnaryExpression or MemberExpression.", "sortPredicate");
			var memberExpression = (MemberExpression)bodyExpression;
			propertyName = memberExpression.Member.Name;
			if (memberExpression.Member.MemberType == MemberTypes.Property)
			{
				var propertyInfo = memberExpression.Member as PropertyInfo;
				if (propertyInfo != null) propertyType = propertyInfo.PropertyType;
			}
			else
				throw new InvalidOperationException(@"Cannot evaluate the type of property since the member expression
                represented by the sort predicate expression does not contain a PropertyInfo object.");

			var funcType = typeof(Func<,>).MakeGenericType(typeof(TEntity), propertyType);
			var convertedExpression = Expression.Lambda(funcType,
				Expression.Convert(Expression.Property(param, propertyName), propertyType), param);

			var sortingMethods = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static);
			var sortingMethodName = GetSortingMethodName(sortOrder);
			var sortingMethod = sortingMethods.First(sm => sm.Name == sortingMethodName &&
														   sm.GetParameters().Length == 2);
			return (IOrderedQueryable<TEntity>)sortingMethod
				.MakeGenericMethod(typeof(TEntity), propertyType)
				.Invoke(null, new object[] { query, convertedExpression });
		}

		private static string GetSortingMethodName(SortOrder sortOrder)
		{
			switch (sortOrder)
			{
				case SortOrder.Ascending:
					return "OrderBy";

				case SortOrder.Descending:
					return "OrderByDescending";

				default:
					throw new ArgumentException("Sort Order must be specified as either Ascending or Descending.",
			"sortOrder");
			}
		}
	}
}