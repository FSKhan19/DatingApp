using DatingApp.Backend.Core.Auditing.Interfaces;
using System.Linq.Expressions;

namespace DatingApp.Backend.Core.Repositories
{
    public interface IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Select/Get/Query

        /// <summary>
        /// Used to get a IQueryable that is used to retrieve entities from entire table.
        /// </summary>
        /// <returns>IQueryable to be used to select entities from database</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Used to get a IQueryable that is used to retrieve readonly entities from entire table.
        /// </summary>
        /// <returns>Readonly IQueryable to be used to select entities from database</returns>
        IQueryable<TEntity> GetAllReadonly();

        /// <summary>
        /// Used to get async IQueryable that is used to retrieve readonly entities from entire table.
        /// </summary>
        /// <returns>IQueryable to be used to select entities from database</returns>
        Task<IQueryable<TEntity>> GetAllReadonlyAsync();

        /// <summary>
        /// Used to get a IQueryable that is used to retrieve entities from entire table.
        /// </summary>
        /// <returns>IQueryable to be used to select entities from database</returns>
        Task<IQueryable<TEntity>> GetAllAsync();

        /// <summary>
        /// Used to get a IQueryable that is used to retrieve entities from entire table.
        /// One or more 
        /// </summary>
        /// <param name="propertySelectors">A list of include expressions.</param>
        /// <returns>IQueryable to be used to select entities from database</returns>
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

        /// <summary>
        /// Used to get a IQueryable that is used to retrieve entities from entire table.
        /// One or more 
        /// </summary>
        /// <param name="propertySelectors">A list of include expressions.</param>
        /// <returns>IQueryable to be used to select entities from database</returns>
        Task<IQueryable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] propertySelectors);

        /// <summary>
        /// Used to get a IQueryable that is used to retrieve readonly entities from entire table.
        /// </summary>
        /// <param name="propertySelectors">A list of include expressions.</param>
        /// <returns>Readonly IQueryable to be used to select entities from database</returns>
        IQueryable<TEntity> GetAllReadonlyIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

        /// <summary>
        /// Used to get async IQueryable that is used to retrieve entities from entire table.
        /// One or more 
        /// </summary>
        /// <param name="propertySelectors">A list of include expressions.</param>
        /// <returns>IQueryable to be used to select entities from database</returns>
        Task<IQueryable<TEntity>> GetAllReadonlyIncludingAsync(params Expression<Func<TEntity, object>>[] propertySelectors);

        /// <summary>
        /// Used to get all entities.
        /// </summary>
        /// <returns>List of all entities</returns>
        List<TEntity> GetAllList();

        /// <summary>
        /// Used to get all entities.
        /// </summary>
        /// <returns>List of all entities</returns>
        Task<List<TEntity>> GetAllListAsync();

        /// <summary>
        /// Used to get all entities based on given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns>List of all entities</returns>
        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Used to get all entities based on given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns>List of all entities</returns>
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Used to run a query over entire entities.
        /// <see cref="UnitOfWorkAttribute"/> attribute is not always necessary (as opposite to <see cref="GetAll"/>)
        /// if <paramref name="queryMethod"/> finishes IQueryable with ToList, FirstOrDefault etc..
        /// </summary>
        /// <typeparam name="T">Type of return value of this method</typeparam>
        /// <param name="queryMethod">This method is used to query over entities</param>
        /// <returns>Query result</returns>
        T Query<T>(Func<IQueryable<TEntity>, T> queryMethod);

        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity</returns>
        TEntity Get(TPrimaryKey id);

        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity</returns>
        Task<TEntity> GetAsync(TPrimaryKey id);

        /// <summary>
        /// Gets exactly one entity with given predicate.
        /// Throws exception if no entity or more than one entity.
        /// </summary>
        /// <param name="predicate">Entity</param>
        TEntity Single(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets exactly one entity with given predicate.
        /// Throws exception if no entity or more than one entity.
        /// </summary>
        /// <param name="predicate">Entity</param>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets an entity with given primary key or null if not found.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity or null</returns>
        TEntity FirstOrDefault(TPrimaryKey id);

        /// <summary>
        /// Gets an entity with given primary key or null if not found.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity or null</returns>
        Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id);

        /// <summary>
        /// Gets an entity with given given predicate or null if not found.
        /// </summary>
        /// <param name="predicate">Predicate to filter entities</param>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets an entity with given given predicate or null if not found.
        /// </summary>
        /// <param name="predicate">Predicate to filter entities</param>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Creates an entity with given primary key without database access.
        /// </summary>
        /// <param name="id">Primary key of the entity to load</param>
        /// <returns>Entity</returns>
        TEntity Load(TPrimaryKey id);

        Task<IList<TEntity>> GetAllByCondition(Expression<Func<TEntity, bool>> expression, 
                                               Func<IQueryable<TEntity>, IQueryable<TEntity>> includes = null);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        #endregion

        #region Insert

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// Inserts a new entity and gets it's Id.
        /// It may require to save current unit of work
        /// to be able to retrieve id.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id of the entity</returns>
        TPrimaryKey InsertAndGetId(TEntity entity);

        /// <summary>
        /// Inserts a new entity and gets it's Id.
        /// It may require to save current unit of work
        /// to be able to retrieve id.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id of the entity</returns>
        Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);

        /// <summary>
        /// Adds a collection of entities to the database using EF Core's ChangeTracker.
        /// Suitable for small datasets requiring relationship maintenance and validation.
        /// </summary>
        /// <param name="entities">List of entities to add</param>
        /// <remarks>
        /// Characteristics:
        /// - Tracks entities through EF Core's ChangeTracker
        /// - Maintains navigation property relationships
        /// - Runs validation attributes and business rules
        /// - Populates auto-generated properties (e.g., IDs)
        /// - O(n) complexity (scales linearly with entity count)
        /// </remarks>
        Task AddRangeAsync(IList<TEntity> entities);

        #endregion

        #region Update

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Updates an existing entity. 
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction">Action that can be used to change values of the entity</param>
        /// <returns>Updated entity</returns>
        TEntity Update(TPrimaryKey id, Action<TEntity> updateAction);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction">Action that can be used to change values of the entity</param>
        /// <returns>Updated entity</returns>
        Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction);

        /// <summary>
        /// Updates a collection of entities using EF Core's ChangeTracker.
        /// Suitable for small datasets requiring business logic/validation.
        /// Tracks entities and maintains relationships.
        /// </summary>
        /// <param name="entities">List of entities to update</param>
        /// <remarks>
        /// - Use for updates requiring validation/concurrency checks
        /// - Maintains navigation property relationships
        /// - Not recommended for bulk operations (>100 records)
        /// - Triggers EF Core change tracking and events
        /// </remarks>
        void UpdateRange(IList<TEntity> entities);

        /// <summary>
        /// Executes a bulk update operation directly in the database.
        /// Bypasses EF Core change tracking for maximum performance.
        /// </summary>
        /// <param name="predicate">Filter condition for entities to update</param>
        /// <param name="updateExpression">Property update definition</param>
        /// <returns>Number of affected rows</returns>
        /// <remarks>
        /// - Use for large-scale updates (>100 records)
        /// - Does NOT track entities or maintain relationships
        /// - Ignores global query filters (explicitly include IsDeleted checks if needed)
        /// - Direct SQL translation (no entity materialization)
        /// - O(1) complexity regardless of dataset size
        /// </remarks>
        Task<int> ExecuteUpdateAsync(
            Expression<Func<TEntity, bool>> predicate,
            IDictionary<Expression<Func<TEntity, object>>, object> propertyUpdates);

        #endregion

        #region Delete

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">Entity to be deleted</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">Entity to be deleted</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Deletes an entity by primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity</param>
        void Delete(TPrimaryKey id);

        /// <summary>
        /// Deletes an entity by primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity</param>
        Task DeleteAsync(TPrimaryKey id);

        /// <summary>
        /// Deletes many entities by function.
        /// Notice that: All entities fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Deletes many entities by function.
        /// Notice that: All entities fits to given predicate are retrieved and deleted.
        /// This may cause major performance problems if there are too many entities with
        /// given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Deletes multiple entities by their primary keys using EF Core's ChangeTracker.
        /// Suitable for small datasets requiring relationship maintenance.
        /// </summary>
        /// <param name="entityIds">Primary keys of entities to delete</param>
        /// <remarks>
        /// - Tracks entities through EF Core
        /// - Maintains referential integrity constraints
        /// - Not recommended for bulk deletions (>100 records)
        /// - Triggers entity removal events
        /// - Handles navigation property cleanup
        /// </remarks>
        Task DeleteRangeAsync(IList<TPrimaryKey> entityIds);

        /// <summary>
        /// Executes a bulk delete operation directly in the database.
        /// Bypasses EF Core change tracking for maximum performance.
        /// </summary>
        /// <param name="predicate">Filter condition for entities to delete</param>
        /// <returns>Number of deleted rows</returns>
        /// <remarks>
        /// - Use for large-scale deletions (>100 records)
        /// - Does NOT track entities or maintain relationships
        /// - Ignores global query filters (explicitly include IsDeleted checks if needed)
        /// - Direct SQL translation (no entity materialization)
        /// - O(1) complexity regardless of dataset size
        /// - WARNING: Permanently deletes records (bypasses soft delete patterns)
        /// </remarks>
        Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate);
        #endregion

        #region Aggregates

        /// <summary>
        /// Gets count of all entities in this repository.
        /// </summary>
        /// <returns>Count of entities</returns>
        int Count();

        /// <summary>
        /// Gets count of all entities in this repository.
        /// </summary>
        /// <returns>Count of entities</returns>
        Task<int> CountAsync();

        /// <summary>
        /// Gets count of all entities in this repository based on given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A method to filter count</param>
        /// <returns>Count of entities</returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets count of all entities in this repository based on given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">A method to filter count</param>
        /// <returns>Count of entities</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets count of all entities in this repository (use if expected return value is greater than <see cref="int.MaxValue"/>.
        /// </summary>
        /// <returns>Count of entities</returns>
        long LongCount();

        /// <summary>
        /// Gets count of all entities in this repository (use if expected return value is greater than <see cref="int.MaxValue"/>.
        /// </summary>
        /// <returns>Count of entities</returns>
        Task<long> LongCountAsync();

        /// <summary>
        /// Gets count of all entities in this repository based on given <paramref name="predicate"/>
        /// (use this overload if expected return value is greater than <see cref="int.MaxValue"/>).
        /// </summary>
        /// <param name="predicate">A method to filter count</param>
        /// <returns>Count of entities</returns>
        long LongCount(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets count of all entities in this repository based on given <paramref name="predicate"/>
        /// (use this overload if expected return value is greater than <see cref="int.MaxValue"/>).
        /// </summary>
        /// <param name="predicate">A method to filter count</param>
        /// <returns>Count of entities</returns>
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region Raw SQL
        /// <summary>
        /// Executes a raw SQL command (e.g., INSERT, UPDATE, DELETE) against the database.
        /// </summary>
        /// <param name="sql">The raw SQL query or command to execute.</param>
        /// <param name="parameters">Optional parameters to pass to the SQL query.</param>
        /// <returns>The number of rows affected by the query.</returns>
        Task<int> ExecuteRawSqlAsync(string sql, params object[] parameters);

        /// <summary>
        /// Executes a raw SQL query and maps the results to a specified type.
        /// </summary>
        /// <typeparam name="TResult">The type of objects to map the query results to.</typeparam>
        /// <param name="sql">The raw SQL query to execute.</param>
        /// <param name="parameters">Optional parameters to pass to the SQL query.</param>
        /// <returns>A collection of objects representing the query results.</returns>
        Task<IEnumerable<TResult>> ExecuteQueryAsync<TResult>(string sql, params object[] parameters)
            where TResult : class;
        #endregion

        #region Store Procedure
        /// <summary>
        /// Executes a stored procedure that modifies data (e.g., INSERT, UPDATE, DELETE).
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">Optional parameters to pass to the stored procedure.</param>
        /// <returns>The number of rows affected by the stored procedure.</returns>
        Task<int> ExecuteStoredProcedureAsync(string procedureName, params object[] parameters);

        /// <summary>
        /// Executes a stored procedure that retrieves data and maps the results to a specified type.
        /// </summary>
        /// <typeparam name="TResult">The type of objects to map the query results to. Must be a reference type.</typeparam>
        /// <param name="procedureName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">Optional parameters to pass to the stored procedure.</param>
        /// <returns>A collection of objects representing the query results.</returns>
        Task<IEnumerable<TResult>> ExecuteStoredProcedureQueryAsync<TResult>(string procedureName, params object[] parameters)
            where TResult : class;
        #endregion
    }
}
