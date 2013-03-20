using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFramework.Extensions
{
	/// <summary>
	/// Defines extension methods for <see cref="IQueryable{T}"/> to enable prefetching.
	/// </summary>
	public static class PrefetchExtensions
	{
		/// <summary>
		/// Specifies the related objects to include in the query results by issuing a seperate query.
		/// </summary>
		/// <typeparam name="TSource">The type of the entity being queried.</typeparam>
		/// <typeparam name="TProperty">The type of the navigation property being included. (cannot be a collection)</typeparam>
		/// <param name="source">The source <see cref="IQueryable{TSource}"/>.</param>
		/// <param name="path">A lambda expression representing the path to include.</param>
		/// <returns>A new <see cref="IQueryable{TSource}"/>.</returns>
		/// <exception cref="ArgumentNullException">source or path are null</exception>
		/// <exception cref="ArgumentException">path resolves to a collection</exception>
		public static IQueryable<TSource> Prefetch<TSource, TProperty>(this IQueryable<TSource> source,
			Expression<Func<TSource, TProperty>> path)
			where TSource : class
			where TProperty : class
		{
			if (source == null)
				throw new ArgumentNullException("source");

			if (path == null)
				throw new ArgumentNullException("path");

			if (typeof(IEnumerable).IsAssignableFrom(typeof(TProperty)))
				throw new ArgumentException("Prefetch cannot be used with a path that resolves to a collection. Use the PrefetchMany method instead.", "path");

			// create object set for entities of type TProperty
			var objectQuery = source.ToObjectQuery();
			var objectSet = objectQuery.Context.CreateObjectSet<TProperty>() as IQueryable<TProperty>;

			// load the entities in the object set
			objectSet.Load();

			return source;
		}

		/// <summary>
		/// Specifies the related objects to include in the query results by issuing a seperate query.
		/// </summary>
		/// <typeparam name="TSource">The type of the entity being queried.</typeparam>
		/// <typeparam name="TProperty">The type of the navigation property being included. (cannot be a collection)</typeparam>
		/// <param name="source">The source <see cref="IQueryable{TSource}"/>.</param>
		/// <param name="path">A lambda expression representing the path to include.</param>
		/// <param name="query">The query to apply on the entities of the type of the navigation property.</param>
		/// <returns>A new <see cref="IQueryable{TSource}"/>.</returns>
		/// <exception cref="ArgumentNullException">source, path or query are null</exception>
		/// <exception cref="ArgumentException">path resolves to a collection</exception>
		public static IQueryable<TSource> Prefetch<TSource, TProperty>(this IQueryable<TSource> source,
			Expression<Func<TSource, TProperty>> path, Func<IQueryable<TProperty>, IQueryable<TProperty>> query)
			where TSource : class
			where TProperty : class
		{
			if (source == null)
				throw new ArgumentNullException("source");

			if (path == null)
				throw new ArgumentNullException("path");

			if (typeof(IEnumerable).IsAssignableFrom(typeof(TProperty)))
				throw new ArgumentException("Prefetch cannot be used with a path that resolves to a collection. Use the PrefetchMany method instead.", "path");

			if (query == null)
				throw new ArgumentNullException("query");

			// create object set for entities of type TProperty
			var objectQuery = source.ToObjectQuery();
			var objectSet = objectQuery.Context.CreateObjectSet<TProperty>() as IQueryable<TProperty>;

			// apply the query on the object set
			objectSet = query(objectSet);

			// load the entities in the object set as specified by the query
			objectSet.Load();

			return source;
		}

		/// <summary>
		/// Specifies the related objects to include in the query results by issuing a seperate query.
		/// </summary>
		/// <typeparam name="TSource">The type of the entity being queried.</typeparam>
		/// <typeparam name="TProperty">The type of the navigation property being included. (must be a collection)</typeparam>
		/// <param name="source">The source <see cref="IQueryable{TSource}"/>.</param>
		/// <param name="path">A lambda expression representing the path to include.</param>
		/// <returns>A new <see cref="IQueryable{TSource}"/>.</returns>
		/// <exception cref="ArgumentNullException">source or path are null</exception>
		public static IQueryable<TSource> PrefetchMany<TSource, TProperty>(this IQueryable<TSource> source,
			Expression<Func<TSource, ICollection<TProperty>>> path)
			where TSource : class
			where TProperty : class
		{
			if (source == null)
				throw new ArgumentNullException("source");

			if (path == null)
				throw new ArgumentNullException("path");

			// create object set for entities of type TProperty
			var objectQuery = source.ToObjectQuery();
			var objectSet = objectQuery.Context.CreateObjectSet<TProperty>() as IQueryable<TProperty>;

			// load the entities in the object set as specified by the query
			objectSet.Load();

			return source;
		}

		/// <summary>
		/// Specifies the related objects to include in the query results by issuing a seperate query.
		/// </summary>
		/// <typeparam name="TSource">The type of the entity being queried.</typeparam>
		/// <typeparam name="TProperty">The type of the navigation property being included. (must be a collection)</typeparam>
		/// <param name="source">The source <see cref="IQueryable{TSource}"/>.</param>
		/// <param name="path">A lambda expression representing the path to include.</param>
		/// <param name="query">The query to apply on the entities of the type of the navigation property.</param>
		/// <returns>A new <see cref="IQueryable{TSource}"/>.</returns>
		/// <exception cref="ArgumentNullException">source, path or query are null</exception>
		public static IQueryable<TSource> PrefetchMany<TSource, TProperty>(this IQueryable<TSource> source,
			Expression<Func<TSource, ICollection<TProperty>>> path, Func<IQueryable<TProperty>, IQueryable<TProperty>> query)
			where TSource : class
			where TProperty : class
		{
			if (source == null)
				throw new ArgumentNullException("source");

			if (path == null)
				throw new ArgumentNullException("path");

			if (query == null)
				throw new ArgumentNullException("query");

			// create object set for entities of type TProperty
			var objectQuery = source.ToObjectQuery();
			var objectSet = objectQuery.Context.CreateObjectSet<TProperty>() as IQueryable<TProperty>;

			// apply the query on the object set
			objectSet = query(objectSet);

			// load the entities in the object set as specified by the query
			objectSet.Load();

			return source;
		}
	}
}