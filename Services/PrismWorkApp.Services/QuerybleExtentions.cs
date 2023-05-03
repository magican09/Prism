using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PrismWorkApp.Services
{
    public static class QuerybleExtentions
    {
        public static IQueryable<T> AddFilter<T>(IQueryable<T> query, string propertyName, string searchTerm)
        {
            var param = Expression.Parameter(typeof(T), "e");
            var propExpression = Expression.Property(param, propertyName);

            object value = searchTerm;
            if (propExpression.Type != typeof(string))
                value = Convert.ChangeType(value, propExpression.Type);

            var filterLambda = Expression.Lambda<Func<T, bool>>(
                Expression.Equal(
                    propExpression,
                    Expression.Constant(value)
                ),
                param
            );

            return query.Where(filterLambda);
        }
    }
}
