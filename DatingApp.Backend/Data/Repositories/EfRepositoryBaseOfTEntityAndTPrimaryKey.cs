using DatingApp.Backend.Core;
using DatingApp.Backend.Core.Auditing.Interfaces;
using DatingApp.Backend.Core.Repositories;
using DatingApp.Backend.Extensions.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Data.Common;
using System.Linq.Expressions;

namespace DatingApp.Backend.Data.Repositories
{
    /// <summary>
    /// Implements IRepository for Entity Framework.
    /// </summary>
    /// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key of the entity</typeparam>
    public class EfRepositoryBase<TEntity, TPrimaryKey> : RepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {

        public DatingAppContext Context { get; private set; }
        public virtual DbSet<TEntity> Table => Context.Set<TEntity>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContextProvider"></param>
        public EfRepositoryBase(DatingAppContext appContext)
        {
            Context = appContext;
        }

        #region Select/Get/Query
        public override IQueryable<TEntity> GetAll()
        {
            return Table;
        }

        public override IQueryable<TEntity> GetAllReadonly()
        {
            return GetAll().AsNoTracking();
        }

        public override Task<IQueryable<TEntity>> GetAllAsync()
        {
            return Task.FromResult(Table.AsQueryable());
        }

        public override Task<IQueryable<TEntity>> GetAllReadonlyAsync()
        {
            return Task.FromResult(Table.AsNoTracking().AsQueryable());
        }

        public override IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            if (propertySelectors.IsNullOrEmpty())
            {
                return GetAll();
            }

            var query = GetAll();

            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }

            return query;
        }

        public override async Task<IQueryable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            if (propertySelectors.IsNullOrEmpty())
            {
                return await GetAllAsync();
            }

            var query = await GetAllAsync();

            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }

            return query;
        }

        public override IQueryable<TEntity> GetAllReadonlyIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            if (propertySelectors.IsNullOrEmpty())
            {
                return GetAllReadonly();
            }

            var query = GetAllReadonly();

            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }

            return query;
        }

        public override async Task<IQueryable<TEntity>> GetAllReadonlyIncludingAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            if (propertySelectors.IsNullOrEmpty())
            {
                return await GetAllReadonlyAsync();
            }
            var query = await GetAllReadonlyAsync();

            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }

            return query;
        }

        public override async Task<List<TEntity>> GetAllListAsync()
        {
            var query = await GetAllAsync();
            return await query.ToListAsync();
        }

        public override async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = await GetAllAsync();
            return await query.Where(predicate).ToListAsync();
        }
        public override async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = await GetAllAsync();
            return await query.SingleAsync(predicate);
        }

        public override async Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            var query = await GetAllAsync();
            return await query.FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        public override async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = await GetAllAsync();
            return await query.FirstOrDefaultAsync(predicate);
        }

        public override async Task<IList<TEntity>> GetAllByCondition(Expression<Func<TEntity, bool>> expression, 
                                                                     Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null)
        {
            IQueryable<TEntity> queryable = Context.Set<TEntity>();
            if (includes != null)
            {
                queryable = includes(queryable);
            }

            return await queryable.Where(expression).ToListAsync();
        }
        public override async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = await GetAllAsync();
            return await query.AnyAsync(predicate);
        }
        #endregion

        #region Insert
        public override TEntity Insert(TEntity entity)
        {
            return Table.Add(entity).Entity;
        }
        public override async Task<TEntity> InsertAsync(TEntity entity)
        {
            var entry = await Table.AddAsync(entity);
            return entry.Entity;
        }
        public override async Task AddRangeAsync(IList<TEntity> entities)
        {
            await Table.AddRangeAsync(entities);
        }
        #endregion

        #region Update
        public override TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public override Task<TEntity> UpdateAsync(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return Task.FromResult(entity);
        }

        //Update the List of Entity
        public override void UpdateRange(IList<TEntity> entities)
        {
            Table.UpdateRange(entities);
        }

        public override async Task<int> ExecuteUpdateAsync(Expression<Func<TEntity, bool>> predicate,
        IDictionary<Expression<Func<TEntity, object>>, object> propertyUpdates)
        {
            var updateExpression = BuildSetPropertyCalls(propertyUpdates);
            return await this.Table
                .Where(predicate)
                .ExecuteUpdateAsync(updateExpression);

        }
        #endregion

        #region Delete
        public override void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        public override void Delete(TPrimaryKey id)
        {
            var entity = Table.Local.FirstOrDefault(ent => EqualityComparer<TPrimaryKey>.Default.Equals(ent.Id, id));
            if (entity == null)
            {
                entity = FirstOrDefault(id);
                if (entity == null)
                {
                    return;
                }
            }

            Delete(entity);
        }

        public override async Task DeleteRangeAsync(IList<TPrimaryKey> entityIds)
        {
            // Fetch the entities from the DbSet based on the provided IDs
            var entities = await Table.Where(ent => entityIds.Contains(ent.Id)).ToListAsync();

            if (entities.Any()) // Check if there are any entities to delete
            {
                Table.RemoveRange(entities); // Remove the entities from the DbSet
            }
        }

        public override async Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Table
                .Where(predicate)
                .ExecuteDeleteAsync();
        }
        #endregion

        #region Aggregates
        public override async Task<int> CountAsync()
        {
            var query = await GetAllReadonlyAsync();
            return await query.CountAsync();
        }

        public override async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = await GetAllReadonlyAsync();
            return await query.CountAsync(predicate);
        }

        public override async Task<long> LongCountAsync()
        {
            var query = await GetAllReadonlyAsync();
            return await query.LongCountAsync();
        }

        public override async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = await GetAllReadonlyAsync();
            return await query.LongCountAsync(predicate);
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            if (!Table.Local.Contains(entity))
            {
                Table.Attach(entity);
            }
        }

        public Task EnsureCollectionLoadedAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionExpression,
            CancellationToken cancellationToken) where TProperty : class
        {
            var expression = collectionExpression.Body as MemberExpression;
            if (expression == null)
            {
                throw new Exception($"Given {nameof(collectionExpression)} is not a {typeof(MemberExpression).FullName}");
            }

            return Context.Entry(entity)
                .Collection(expression.Member.Name)
                .LoadAsync(cancellationToken);
        }

        public void EnsureCollectionLoaded<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionExpression,
            CancellationToken cancellationToken) where TProperty : class
        {
            var expression = collectionExpression.Body as MemberExpression;
            if (expression == null)
            {
                throw new Exception($"Given {nameof(collectionExpression)} is not a {typeof(MemberExpression).FullName}");
            }

            Context.Entry(entity)
                .Collection(expression.Member.Name)
                .Load();
        }

        public Task EnsurePropertyLoadedAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken) where TProperty : class
        {
            return Context.Entry(entity).Reference(propertyExpression).LoadAsync(cancellationToken);
        }

        public void EnsurePropertyLoaded<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken) where TProperty : class
        {
            Context.Entry(entity).Reference(propertyExpression).Load();
        }
        #endregion

        #region RAW SQL
        /// <inheritdoc />
        public override async Task<int> ExecuteRawSqlAsync(string sql, params object[] parameters)
        {
            return await Context.Database.ExecuteSqlRawAsync(sql, parameters);
        }
        /// <inheritdoc />
        public override async Task<IEnumerable<TResult>> ExecuteQueryAsync<TResult>(string sql, params object[] parameters)
            where TResult : class
        {
            return await Context.Set<TResult>()
                .FromSqlRaw(sql, parameters)
                .ToListAsync();
        }
        #endregion

        #region Store Procedure
        /// <inheritdoc />
        public override async Task<int> ExecuteStoredProcedureAsync(string procedureName, params object[] parameters)
        {
            /// <summary>
            /// Executes a stored procedure that modifies data (e.g., INSERT, UPDATE, DELETE).
            /// </summary>
            /// <param name="procedureName">The name of the stored procedure to execute.</param>
            /// <param name="parameters">Optional parameters to pass to the stored procedure.</param>
            /// <returns>The number of rows affected by the stored procedure.</returns>
            var sql = $"EXEC {procedureName} {FormatParameters(parameters)}";
            return await Context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<TResult>> ExecuteStoredProcedureQueryAsync<TResult>(string procedureName, params object[] parameters)
            where TResult : class
        {
            /// <summary>
            /// Executes a stored procedure that retrieves data and maps the results to a specified type.
            /// </summary>
            /// <typeparam name="TResult">The type of objects to map the query results to. Must be a reference type.</typeparam>
            /// <param name="procedureName">The name of the stored procedure to execute.</param>
            /// <param name="parameters">Optional parameters to pass to the stored procedure.</param>
            /// <returns>A collection of objects representing the query results.</returns>
            var sql = $"EXEC {procedureName} {FormatParameters(parameters)}";
            return await Context.Set<TResult>()
                .FromSqlRaw(sql, parameters)
                .ToListAsync();
        }

        /// <summary>
        /// Formats parameters for inclusion in a SQL command.
        /// </summary>
        /// <param name="parameters">The parameters to format.</param>
        /// <returns>A formatted string of parameters.</returns>
        private static string FormatParameters(object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return string.Empty;
            }

            return string.Join(", ", parameters.Select((_, index) => $"@p{index}"));
        }
        #endregion

        #region Helper
        private static Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>>
       BuildSetPropertyCalls(IDictionary<Expression<Func<TEntity, object>>, object> updates)
        {
            var parameter = Expression.Parameter(typeof(SetPropertyCalls<TEntity>), "s");
            Expression body = parameter;

            foreach (var update in updates)
            {
                var (propertyType, propertyExpr) = GetPropertyInfo(update.Key);
                var value = update.Value;

                var setPropertyMethod = typeof(SetPropertyCalls<TEntity>)
                    .GetMethods()
                    .First(m => m.Name == "SetProperty" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(propertyType);

                var valueLambda = Expression.Lambda(
                    typeof(Func<,>).MakeGenericType(
                        typeof(SetPropertyCalls<TEntity>),
                        propertyType),
                    Expression.Constant(value),
                    Expression.Parameter(typeof(SetPropertyCalls<TEntity>))
                );

                body = Expression.Call(
                    body,
                    setPropertyMethod,
                    Expression.Quote(propertyExpr),
                    Expression.Quote(valueLambda)
                );
            }

            return Expression.Lambda<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>>(
                body,
                parameter
            );
        }

        private static (Type propertyType, LambdaExpression expression) GetPropertyInfo(
            Expression<Func<TEntity, object>> propertySelector)
        {
            if (propertySelector.Body is UnaryExpression unary &&
                unary.Operand is MemberExpression member)
            {
                return (member.Type, Expression.Lambda(member, propertySelector.Parameters));
            }

            if (propertySelector.Body is MemberExpression directMember)
            {
                return (directMember.Type, propertySelector);
            }

            throw new ArgumentException("Invalid property selector expression");
        }
        #endregion


        ///// <summary>
        ///// In ASP.NET Core, when you register DbContext with a scoped lifetime (e.g., services.AddDbContext<DatingAppContext>()), the DI container ensures that:.
        ///// If you manually dispose of the DbContext in the repository, it could cause issues because other parts of your application (such as other repositories or services) might still need to use the same instance of DbContext.
        ///// </summary>
        //public void Dispose()
        //{
        //    // Dispose of the DbContext if necessary.
        //    Context.Dispose();
        //}

    }
}
