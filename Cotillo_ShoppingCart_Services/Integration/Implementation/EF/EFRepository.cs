using Cotillo_ShoppingCart_Services.Domain;
using Cotillo_ShoppingCart_Services.Domain.Model;
using Cotillo_ShoppingCart_Services.Integration.Interfaces.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using System.Linq.Expressions;

namespace Cotillo_ShoppingCart_Services.Integration.Implementation.EF
{
    /// <summary>
    /// Entity Framework repository
    /// </summary>
    public partial class EFRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly IDbContext context;
        protected IDbSet<TEntity> entities;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public EFRepository(IDbContext context)
        {
            this.context = context;
        }

        public async Task<IList<TEntity>> GetAllAsync(int page = 0, int pageSize = Int32.MaxValue, bool active = true)
        {
            var query = this.Table;

            /*
             * NOTE: Uncomment these lines if you want to account for Active records
            if (active)
            {
                query = query;
                    .Where(entity => entity.Active == true);
            }
            */

            query = query
                .OrderBy(entity => entity.Id)
                .Skip(page)
                .Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id, bool active = true)
        {
            /*
             * NOTE: Uncomment these lines if you want to account for Active records
            return
            await Table
                    .Where(entity => entity.Id == id && entity.Active == active)
                    .FirstOrDefaultAsync();
            */
            return
            await Table
                    .Where(entity => entity.Id == id)
                    .FirstOrDefaultAsync();
        }

        public async Task InsertAsync(TEntity entity, bool autoCommit)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                /*
                 * NOTE: Uncomment these lines if you want to account for Active records
                entity.Active = true;
                */

                this.Entities.Add(entity);

                if (autoCommit)
                    await CommitAsync();
            }
            catch (AggregateException ae)
            {
                ae.Handle((ex) =>
                {
                    if (ex is DbEntityValidationException) // This we know how to handle.
                    {
                        var msg = string.Empty;
                        var dbEx = ex as DbEntityValidationException;

                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                            foreach (var validationError in validationErrors.ValidationErrors)
                                msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                        var fail = new Exception(msg, dbEx);
                        throw fail;
                    }
                    return false; // Let anything else stop the application.
                });
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public async Task UpdateAsync(TEntity entity, bool autoCommit)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                if (autoCommit)
                    await CommitAsync();
            }
            catch (AggregateException ae)
            {
                ae.Handle((ex) =>
                {
                    if (ex is DbEntityValidationException) // This we know how to handle.
                    {
                        var msg = string.Empty;
                        var dbEx = ex as DbEntityValidationException;

                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                            foreach (var validationError in validationErrors.ValidationErrors)
                                msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                        var fail = new Exception(msg, dbEx);
                        throw fail;
                    }
                    throw ae; // Let anything else stop the application.
                });
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public async Task DeleteAsync(TEntity entity, bool autoCommit)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Remove(entity);

                if (autoCommit)
                    await CommitAsync();

            }
            catch (AggregateException ae)
            {
                ae.Handle((ex) =>
                {
                    if (ex is DbEntityValidationException) // This we know how to handle.
                    {
                        var msg = string.Empty;
                        var dbEx = ex as DbEntityValidationException;

                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                            foreach (var validationError in validationErrors.ValidationErrors)
                                msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                        var fail = new Exception(msg, dbEx);
                        throw fail;
                    }
                    throw ae; // Let anything else stop the application.
                });
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public IQueryable<TEntity> Table
        {
            get
            {
                return this.Entities;
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IList<TEntity> GetAll(int page = 0, int pageSize = int.MaxValue, bool active = true)
        {
            var query = this.Table;

            /*
             * NOTE: Uncomment these lines if you want to account for Active records
            if (active)
            {
                query = query;
                    .Where(entity => entity.Active == true);
            }
            */

            query = query
                .OrderBy(entity => entity.Id)
                .Skip(page)
                .Take(pageSize);

            return query.ToList();
        }

        public TEntity GetById(int id, bool active = true)
        {
            /*
             * NOTE: Uncomment these lines if you want to account for Active records
            return
            await Table
                    .Where(entity => entity.Id == id && entity.Active == active)
                    .FirstOrDefaultAsync();
            */
            return
                     Table
                    .Where(entity => entity.Id == id)
                    .FirstOrDefault();
        }

        public void Insert(TEntity entity, bool autoCommit)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                /*
                 * NOTE: Uncomment these lines if you want to account for Active records
                entity.Active = true;
                */

                this.Entities.Add(entity);

                if (autoCommit)
                    Commit();
            }
            catch (AggregateException ae)
            {
                ae.Handle((ex) =>
                {
                    if (ex is DbEntityValidationException) // This we know how to handle.
                    {
                        var msg = string.Empty;
                        var dbEx = ex as DbEntityValidationException;

                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                            foreach (var validationError in validationErrors.ValidationErrors)
                                msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                        var fail = new Exception(msg, dbEx);
                        throw fail;
                    }
                    return false; // Let anything else stop the application.
                });
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public void Update(TEntity entity, bool autoCommit)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                if (autoCommit)
                    Commit();
            }
            catch (AggregateException ae)
            {
                ae.Handle((ex) =>
                {
                    if (ex is DbEntityValidationException) // This we know how to handle.
                    {
                        var msg = string.Empty;
                        var dbEx = ex as DbEntityValidationException;

                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                            foreach (var validationError in validationErrors.ValidationErrors)
                                msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                        var fail = new Exception(msg, dbEx);
                        throw fail;
                    }
                    throw ae; // Let anything else stop the application.
                });
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public void Delete(TEntity entity, bool autoCommit)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Remove(entity);

                if (autoCommit)
                    Commit();

            }
            catch (AggregateException ae)
            {
                ae.Handle((ex) =>
                {
                    if (ex is DbEntityValidationException) // This we know how to handle.
                    {
                        var msg = string.Empty;
                        var dbEx = ex as DbEntityValidationException;

                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                            foreach (var validationError in validationErrors.ValidationErrors)
                                msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                        var fail = new Exception(msg, dbEx);
                        throw fail;
                    }
                    throw ae; // Let anything else stop the application.
                });
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public void Commit()
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AddOrUpdate(Expression<Func<TEntity, object>> expression, TEntity entity)
        {
            Entities.AddOrUpdate(expression, entity);
        }

        private IDbSet<TEntity> Entities
        {
            get
            {
                if (entities == null)
                    entities = context.Set<TEntity>();
                return entities;
            }
        }
    }
}
