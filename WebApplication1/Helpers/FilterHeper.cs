using System.Linq.Expressions;
using System.Reflection;
using WebApplication1.DTOs;

namespace WebApplication1.Helpers
{
    public static class FilterHelper
    {
        // Lấy MethodInfo 1 lần, dùng lại
        private static readonly MethodInfo _contains = typeof(string).GetMethod("Contains", [typeof(string)])!;
        private static readonly MethodInfo _startsWith = typeof(string).GetMethod("StartsWith", [typeof(string)])!;
        private static readonly MethodInfo _endsWith = typeof(string).GetMethod("EndsWith", [typeof(string)])!;

        public static IQueryable<T> ApplySorts<T>(
            IQueryable<T> query,
            List<FilterDto.AgGridSort> sorts)
        {
            if (sorts == null || sorts.Count == 0) return query;

            IOrderedQueryable<T>? ordered = null;

            foreach (var (sort, index) in sorts.Select((s, i) => (s, i)))
            {
                var propInfo = typeof(T).GetProperty(
                    sort.Key,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (propInfo == null) continue;

                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, propInfo);
                var lambda = Expression.Lambda(property, parameter);

                // lần đầu dùng OrderBy, các lần sau dùng ThenBy
                var method = (sort.Sort == "desc")
                    ? (index == 0 ? "OrderByDescending" : "ThenByDescending")
                    : (index == 0 ? "OrderBy" : "ThenBy");

                var result = typeof(Queryable)
                    .GetMethods()
                    .First(m => m.Name == method && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), propInfo.PropertyType)
                    .Invoke(null, [ordered ?? (object)query, lambda]);

                ordered = (IOrderedQueryable<T>)result!;
            }

            return ordered ?? query;
        }
        public static IQueryable<T> ApplyFilters<T>(
            IQueryable<T> query,
            Dictionary<string, FilterDto.AgGridFilter> filters)
        {
            if (filters == null || filters.Count <= 0)
                return query;

            // x =>
            var parameter = Expression.Parameter(typeof(T), "x");

            foreach (var (fieldName, filterObj) in filters)
            {
                if (string.IsNullOrEmpty(filterObj.Filter)) continue;

                // Tìm property theo tên field (case-insensitive)
                // "name" → x.Name
                var propInfo = typeof(T).GetProperty(
                    fieldName,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (propInfo == null) continue; // bỏ qua field không tồn tại

                // x.Name
                var property = Expression.Property(parameter, propInfo);

                // Chỉ hỗ trợ filter kiểu string
                if (propInfo.PropertyType != typeof(string)) continue;

                // "john"
                var constant = Expression.Constant(filterObj.Filter);

                // x.Name.Contains("john")
                Expression? condition = filterObj.Type switch
                {
                    "contains" => Expression.Call(property, _contains, constant),
                    "equals" => Expression.Equal(property, constant),
                    "startsWith" => Expression.Call(property, _startsWith, constant),
                    "endsWith" => Expression.Call(property, _endsWith, constant),
                    _ => null
                };

                if (condition == null) continue;

                var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
                query = query.Where(lambda);
            }

            return query;
        }
    }
}