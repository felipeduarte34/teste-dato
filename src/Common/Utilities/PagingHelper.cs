using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TesteBackend.Common.Utilities
{
    public static class PagingHelper
    {
        public static async Task<PagedResult<T>> GetPaged<T>(this IQueryable<T> query,
            int page, int pageSize) where T : class
        {       
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }
        public static async Task<PagedResult<T>> GetPaged<T>(this IQueryable<T> query, Specification<T> specification, int page, int pageSize) where T : class
        {
            // Obtém a contagem total de linhas antes de aplicar qualquer filtro
            var totalRowCount = await query.CountAsync();
            
            // Aplica os critérios da especificação à consulta
            foreach (var criteria in specification.Criteria)
            {
                query = query.Where(criteria);
            }

            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = totalRowCount, 
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }
    }
    
    public static class MongoCollectionExtensions
    {
        public static async Task<PagedResult<T>> GetPaged<T>(this IMongoCollection<T> collection, int page, int pageSize, FilterDefinition<T> filter = null)
        {
            var totalCountTask = collection.CountDocumentsAsync(filter ?? Builders<T>.Filter.Empty);
            var itemsTask = collection.Find(filter ?? Builders<T>.Filter.Empty)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            await Task.WhenAll(totalCountTask, itemsTask);

            return new PagedResult<T>
            {
                Results = itemsTask.Result,
                RowCount = (int)totalCountTask.Result,
                CurrentPage = page,
                PageSize = pageSize
            };
        }
    }
    
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        
        public int FirstRowOnPage => (CurrentPage - 1) * PageSize + 1;

        public int LastRowOnPage => Math.Min(CurrentPage * PageSize, RowCount);
    }

    public class PagedResult<T> : PagedResultBase 
    {
        public IList<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}
