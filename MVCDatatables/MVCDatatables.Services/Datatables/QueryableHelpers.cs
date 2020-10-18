using System;
using System.Linq;
using System.Linq.Expressions;

namespace MVCDatatables.Services.Datatables
{
    /// <summary>
    /// Queryable helpers for the datatables service.
    /// </summary>
    public static class QueryableHelpers
    {
        /// <summary>
        /// A generic implementation of order by - used for the datatables service.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="source">The collection that needs ordering.</param>
        /// <param name="ordering">What to order by.</param>
        /// <returns>An IQueryable.</returns>
        public static IQueryable<T> GenericOrderBy<T>(this IQueryable<T> source, string ordering)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering.Trim());
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var resultExp = Expression.Call(typeof(Queryable), "OrderBy", new[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        /// <summary>
        /// A generic implementation of order by desc - used for the datatables service.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="source">The collection that needs ordering.</param>
        /// <param name="ordering">What to order by.</param>
        /// <returns>An IQueryable.</returns>
        public static IQueryable<T> GenericOrderByDesc<T>(this IQueryable<T> source, string ordering)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering.Trim());
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var resultExp = Expression.Call(typeof(Queryable), "OrderByDescending", new[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        /// <summary>
        /// A generic implementation of contains - used for the datatables service.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="source">The collection that needs ordering.</param>
        /// <param name="propertyName">The property name to search against.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>An IQueryable.</returns>
        public static IQueryable<T> GenericContains<T>(this IQueryable<T> source, string propertyName, string searchTerm)
        {
            var resultExp = GetContainsLambda(source, propertyName, searchTerm);
            return source.Provider.CreateQuery<T>(resultExp);
        }

        /// <summary>
        /// A generic implementation of or contains - used for the datatables service.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="source">The collection that needs ordering.</param>
        /// <param name="properties">The properties to search against.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>An IQueryable.</returns>
        public static IQueryable<T> GenericOrContains<T>(this IQueryable<T> source, string[] properties, string searchTerm)
        {
            var oredLambda = GetOrElseLambda<T>(properties, searchTerm);
            var sce = Expression.Call(typeof(Queryable), "Where", new[] { source.ElementType }, source.Expression, Expression.Quote(oredLambda));
            return source.Provider.CreateQuery<T>(sce);
        }

        /// <summary>
        /// Get an or else lambda expression.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="properties">The properties to search against.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>An IQueryable.</returns>
        public static LambdaExpression GetOrElseLambda<T>(string[] properties, string searchTerm)
        {
            var parameter = Expression.Parameter(typeof(T), "p");
            var resultExp = properties.Select(prop => GetContainsExpression<T>(prop, searchTerm, parameter)).Aggregate<Expression, Expression>(null, (current, contains) => current == null ? contains : Expression.OrElse(current, contains));
            var orElseLambda = Expression.Lambda(resultExp, parameter);
            return orElseLambda;
        }

        /// <summary>
        /// Get a contains lambda expression.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="source">The collection to run the contains against.</param>
        /// <param name="propertyName">The property to search against.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>A method call expression.</returns>
        public static MethodCallExpression GetContainsLambda<T>(this IQueryable<T> source, string propertyName, string searchTerm)
        {
            var parameter = Expression.Parameter(typeof(T), "p");
            var containsExpression = GetContainsExpression<T>(propertyName, searchTerm, parameter);
            var containsLambda = Expression.Lambda(containsExpression, parameter);
            return Expression.Call(typeof(Queryable), "Where", new[] { source.ElementType }, source.Expression, Expression.Quote(containsLambda));
        }

        /// <summary>
        /// Get a contains expression.
        /// </summary>
        /// <typeparam name="T">The class.</typeparam>
        /// <param name="propertyName">The property to search against.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>An expression.</returns>
        private static Expression GetContainsExpression<T>(string propertyName, string searchTerm, ParameterExpression parameter)
        {
            Expression containsInvocation;
            var type = typeof(T);
            var property = type.GetProperty(propertyName);
            var propertyAccess = Expression.Property(parameter, property);
            var propertyNameLambda = Expression.Lambda(propertyAccess, parameter);
            var searchTermExpression = Expression.Constant(searchTerm);

            if (propertyAccess.Type == typeof(DateTime))
            {
                // For datetime properties, do a ToString to convert them to dates.
                Expression toStringCall = Expression.Call(
                    propertyNameLambda.Body,
                    "ToString",
                    null,
                    null);
                containsInvocation = Expression.Call(toStringCall, typeof(string).GetMethod("Contains"), searchTermExpression);
            }
            else if (propertyAccess.Type.IsGenericType && propertyAccess.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var nullableType = propertyAccess.Type;
                var hasValueExpression = Expression.Property(propertyAccess, nullableType.GetProperty("HasValue"));
                var hasValueInvocation = Expression.Equal(hasValueExpression, Expression.Constant(true));
                var valueExpression = Expression.Property(propertyAccess, nullableType.GetProperty("Value"));
                var valueLambda = Expression.Lambda(valueExpression, parameter);
                Expression toStringCall = Expression.Call(
                   valueLambda.Body,
                   "ToString",
                   null,
                   null);
                var hasValueContainsInvocation = Expression.Call(toStringCall, typeof(string).GetMethod("Contains"), searchTermExpression);
                containsInvocation = Expression.AndAlso(hasValueInvocation, hasValueContainsInvocation);
            }
            else
            {
                containsInvocation = Expression.Call(propertyNameLambda.Body, typeof(string).GetMethod("Contains"), searchTermExpression);
            }

            return containsInvocation;
        }
    }

}
