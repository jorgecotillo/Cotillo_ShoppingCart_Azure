using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Entity.ModelConfiguration;
using System.Data.Common;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
//using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using Cotillo_ShoppingCart_Services.Integration.Interfaces.EF;
using Cotillo_ShoppingCart_Services.Domain;
using Cotillo_ShoppingCart_Services.Domain.Model;
using System.Dynamic;

namespace Cotillo_ShoppingCart_Services.Integration.Implementation.EF
{
    public class EFContext : DbContext, IDbContext
    {
        public EFContext()
        {

        }
        public EFContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
#if DEBUG
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
#endif

            //((IObjectContextAdapter) this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
            ((IObjectContextAdapter)this).ObjectContext.SavingChanges += new EventHandler(ObjectContext_SavingChanges);
        }

        /// <summary>
        /// Method required in order to insert or update the concurrency token (in Oracle TimeStamp type would not work, nor a byte[], therefore we need
        /// to specify a string property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void ObjectContext_SavingChanges(object sender, EventArgs e)
        {

            System.Data.Entity.Core.Objects.ObjectContext context = (System.Data.Entity.Core.Objects.ObjectContext)sender;

            /* 
             * NOTE: Uncomment these lines to enable Concurrency in EF 6 (validate if latest versions don't need this workaround.

            var addedOrModified = context.ObjectStateManager
              .GetObjectStateEntries(System.Data.Entity.EntityState.Added | System.Data.Entity.EntityState.Modified)
              
              //If we need to capture more entities (in order to validate concurrency) then just add OR condition -> like:
              //if(entry.Entity is Application || entry.Entity is SomeOtherEntity || ...)
              //finally in your foreach validate if item is Application, else if (item is SomeOtherEntity) else if (more)
              //if(item is Application { //Do Something }
              //elseif(item is SomethingElse { //Do more stuff }
              .Where(entry => entry.Entity is Application)
              .Select(entry => entry.Entity)
              .ToList();

            foreach (var item in addedOrModified)
            {
                if (item is Application)
                {
                    ((Application)item).Timestamp = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                }
            }
            End of commented code*/

            context.DetectChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            /* 
             * NOTE: Uncomment these lines if you want to set schema name to your DB.

            string dbSchema = ConfigurationManager.AppSettings["DBSchema"];

            if (!String.IsNullOrWhiteSpace(dbSchema))
                modelBuilder.HasDefaultSchema(dbSchema);
            */
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// This method is usually used in conjuction with MonitorInterceptor class, because in the event that there was an update database error, if we want to save the log in the Db and if the
        /// context is created per web request or as a singleton, then the context will retry to insert/update/delete the failed entity and this will produce another exception.
        /// </summary>
        public void MarkAllEntitiesAsUnchanged()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                dbEntityEntry.State = EntityState.Unchanged;
            }
        }

        /// <summary>
        /// Attach an entity to the context or return an already attached entity (if it was already attached)
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached entity</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity)
            where TEntity : BaseEntity, new()
        {
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                //attach new entity
                Set<TEntity>().Attach(entity);
                return entity;
            }
            else
            {
                //entity is already loaded.
                return alreadyAttached;
            }
        }

        /// <summary>
        /// Create database script
        /// </summary>
        /// <returns>SQL to generate database</returns>
        public string CreateDatabaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DbSet</returns>
        public new IDbSet<TEntity> Set<TEntity>()
            where TEntity : class
        {
            return base.Set<TEntity>();
        }

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, bool attachToEntity, params object[] parameters)
            where TEntity : class, new()
        {
            bool hasOutputParameters = false;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    var outputP = p as DbParameter;
                    if (outputP == null)
                        continue;

                    if (outputP.Direction == ParameterDirection.InputOutput ||
                        outputP.Direction == ParameterDirection.Output)
                        hasOutputParameters = true;
                }
            }

            var context = ((IObjectContextAdapter)(this)).ObjectContext;
            if (!hasOutputParameters)
            {
                //no output parameters
                var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();
                if (attachToEntity)
                {
                    BaseEntity baseEntity;
                    for (int i = 0; i < result.Count; i++)
                    {
                        baseEntity = AttachEntityToContext(result[i] as BaseEntity);
                        if (baseEntity != null)
                        {
                            result[i] = baseEntity as TEntity;
                        }
                    }
                }
                return result;
            }
            else
            {

                //var connection = context.Connection;
                var connection = this.Database.Connection;
                //Don't close the connection after command execution


                //open the connection for use
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    //command to execute
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // move parameters to command object
                    if (parameters != null)
                        foreach (var p in parameters)
                            cmd.Parameters.Add(p);

                    //database call
                    var reader = cmd.ExecuteReader();
                    //return reader.DataReaderToObjectList<TEntity>();
                    var result = context.Translate<TEntity>(reader).ToList();
                    if (attachToEntity)
                    {
                        BaseEntity baseEntity;
                        for (int i = 0; i < result.Count; i++)
                        {
                            baseEntity = AttachEntityToContext(result[i] as BaseEntity);
                            if (baseEntity != null)
                            {
                                result[i] = baseEntity as TEntity;
                            }
                        }
                    }
                    //close up the reader, we're done saving results
                    reader.Close();
                    return result;
                }

            }
        }

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public int ExecuteSqlCommand(string sql, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var result = this.Database.ExecuteSqlCommand(sql, parameters);

            if (timeout.HasValue)
            {
                //Set previous timeout back
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            //return result
            return result;
        }

        public T ExecuteStoredProcedureSingleResult<T>(string commandText, params object[] parameters)
        {
            bool hasOutputParameters = false;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    var outputP = p as DbParameter;
                    if (outputP == null)
                        continue;

                    if (outputP.Direction == ParameterDirection.InputOutput ||
                        outputP.Direction == ParameterDirection.Output)
                        hasOutputParameters = true;
                }
            }


            var context = ((IObjectContextAdapter)(this)).ObjectContext;
            if (!hasOutputParameters)
            {
                //no output parameters
                var result = this.Database.SqlQuery<T>(commandText, parameters).FirstOrDefault();

                if (result != null)
                    return result;
            }
            else
            {

                //var connection = context.Connection;
                var connection = this.Database.Connection;
                //Don't close the connection after command execution


                //open the connection for use
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    //command to execute
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Remove any existing parameters so we can properly add parameters in any execution
                    if (cmd.Parameters.Count > 0)
                    {
                        cmd.Parameters.Clear();
                    }

                    // move parameters to command object
                    if (parameters != null)
                    {
                        foreach (var p in parameters)
                        {
                            cmd.Parameters.Add(p);
                        }
                    }
                    //database call
                    var reader = cmd.ExecuteReader();
                    reader.Close();

                    foreach (DbParameter item in cmd.Parameters)
                    {
                        if (item.Direction == ParameterDirection.Output)
                        {
                            if (item.Value != null)
                                return (T)item.Value;
                        }
                    }
                }
            }
            //If we get to here means that there was no result
            return default(T);
        }

        public object[] ExecuteStoredProcedureGetOutputParametersAsArray(string commandText, params object[] parameters)
        {
            bool hasOutputParameters = false;
            int countOutputParameters = 0;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    var outputP = p as DbParameter;
                    if (outputP == null)
                        continue;

                    if (outputP.Direction == ParameterDirection.InputOutput ||
                        outputP.Direction == ParameterDirection.Output)
                    {
                        hasOutputParameters = true;
                        countOutputParameters++;
                    }
                }
            }


            var context = ((IObjectContextAdapter)(this)).ObjectContext;
            if (!hasOutputParameters)
            {
                //no output parameters
                var result = this.Database.SqlQuery<object>(commandText, parameters).FirstOrDefault();

                if (result != null)
                    return new object[] { result };
            }
            else
            {

                //var connection = context.Connection;
                var connection = this.Database.Connection;
                //Don't close the connection after command execution


                //open the connection for use
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                bool hasCursors = false;
                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    //command to execute
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Clear parameters in case there are some already attached
                    if (cmd.Parameters.Count > 0)
                    {
                        cmd.Parameters.Clear();
                    }

                    // move parameters to command object
                    if (parameters != null)
                    {
                        foreach (var p in parameters)
                        {
                            if (!hasCursors && ((DbParameter)p).DbType == DbType.Object)
                            {
                                hasCursors = true;
                            }
                            cmd.Parameters.Add(p);
                        }
                    }
                    //database call
                    var reader = cmd.ExecuteReader();

                    List<dynamic> listOfCursorResult = new List<dynamic>();
                    List<dynamic> listOfInnerEntities = new List<dynamic>();
                    var dynamicProperties = new ExpandoObject() as IDictionary<string, object>;
                    //verify if we have cursors
                    if (hasCursors)
                    {
                        //read the cursors
                        do
                        {
                            listOfInnerEntities = new List<dynamic>();
                            while (reader.Read())
                            {
                                dynamicProperties = new ExpandoObject() as IDictionary<string, object>;
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    dynamicProperties.Add(reader.GetName(i), reader.GetValue(i));
                                }

                                listOfInnerEntities.Add(dynamicProperties);
                            }

                            //Add inner properties to main list -> dynamic properties is an object similar to what an Entity is, it will have as many properties
                            //as columns the cursor has. Once this property is constructed we add it to Inner Entities, is a list that will contain as many results
                            //the cursor has, is like the cursor has 10 results, then listOfInnerEntities will have 10 items like 10 entieis
                            listOfCursorResult.Add(listOfInnerEntities);

                        } while (reader.NextResult());
                    }

                    //After getting all the values as a List of Lists(outer list -> listOfCursorResult is the main object that contains as many items as output 
                    //cursors the sproc has

                    reader.Close();

                    object[] objectArray = new object[countOutputParameters];
                    int count = 0;
                    int cursorIndex = 0;
                    foreach (DbParameter item in cmd.Parameters)
                    {
                        if (item.Direction == ParameterDirection.Output)
                        {
                            if (item.DbType == DbType.Object)
                            {
                                //Relay on position, since we are looping output parameters, if position X is a cursor, it means
                                //that the code that created the dynamic cursor result (do - while from above) added the cursor result in position X, 
                                //we relay on position because there is no way to identify the name of the output parameter while doing reader.Read()
                                objectArray[count] = listOfCursorResult[cursorIndex];
                                cursorIndex++;
                                count++;
                            }
                            else if (item.Value != null)
                            {
                                objectArray[count] = item.Value;
                                count++;
                            }
                        }
                    }
                    return objectArray;
                }
            }
            //If we get to here means that there was no result
            return null;
        }

        public IList<KeyValuePair<string, object>> ExecuteStoredProcedureGetOutputParameters(string commandText, params object[] parameters)
        {
            bool hasOutputParameters = false;
            int countOutputParameters = 0;

            IList<KeyValuePair<string, object>> outputParams = new List<KeyValuePair<string, object>>();

            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    var outputP = p as DbParameter;
                    if (outputP == null)
                        continue;

                    if (outputP.Direction == ParameterDirection.InputOutput ||
                        outputP.Direction == ParameterDirection.Output)
                    {
                        hasOutputParameters = true;
                        countOutputParameters++;
                    }
                }
            }


            var context = ((IObjectContextAdapter)(this)).ObjectContext;
            if (!hasOutputParameters)
            {
                //no output parameters
                var result = this.Database.SqlQuery<object>(commandText, parameters).FirstOrDefault();

                if (result != null)
                {
                    outputParams.Add(new KeyValuePair<string, object>("General", result));
                    return outputParams;
                }
            }
            else
            {

                //var connection = context.Connection;
                var connection = this.Database.Connection;
                //Don't close the connection after command execution


                //open the connection for use
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    //command to execute
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // move parameters to command object
                    if (parameters != null)
                        foreach (var p in parameters)
                            cmd.Parameters.Add(p);

                    //database call
                    var reader = cmd.ExecuteReader();
                    reader.Close();

                    foreach (DbParameter item in cmd.Parameters)
                    {
                        if (item.Direction == ParameterDirection.Output)
                        {
                            if (item.Value != null)
                            {
                                outputParams.Add(new KeyValuePair<string, object>(item.ParameterName, item.Value));
                            }
                        }
                    }
                    return outputParams;
                }
            }
            //If we get to here means that there was no result
            return null;
        }

        public void ExecuteNonQueryStoredProcedure(string commandText, params object[] parameters)
        {
            var connection = this.Database.Connection;
            //Don't close the connection after command execution


            //open the connection for use
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            //create a command object
            using (var cmd = connection.CreateCommand())
            {
                //command to execute
                cmd.CommandText = commandText;
                cmd.CommandType = CommandType.StoredProcedure;

                // move parameters to command object
                if (parameters != null)
                    foreach (var p in parameters)
                        cmd.Parameters.Add(p);

                //execute command
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Execute stores procedure and load a list of Dictionaries at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Dictionary</returns>
        public IDictionary<string, object> ExecuteStoredProcedureListOfLists<TEntity>(string commandText, params object[] parameters)
            where TEntity : class, new()
        {
            var context = ((IObjectContextAdapter)(this)).ObjectContext;
            //var connection = context.Connection;
            var connection = this.Database.Connection;
            //Don't close the connection after command execution


            //open the connection for use
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            //create a command object
            using (var cmd = connection.CreateCommand())
            {
                //command to execute
                cmd.CommandText = commandText;
                cmd.CommandType = CommandType.StoredProcedure;

                // move parameters to command object
                if (parameters != null)
                    foreach (var p in parameters)
                        cmd.Parameters.Add(p);

                var dynamicCursor = new ExpandoObject() as IDictionary<string, object>;

                //database call
                var reader = cmd.ExecuteReader();


                int x = 1;
                while (reader.Read())
                {
                    var dynamicProperties = new ExpandoObject() as IDictionary<string, object>;
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dynamicProperties.Add(reader.GetName(i), reader.GetValue(i));
                    }
                    dynamicCursor.Add("item" + x, dynamicProperties);
                    x++;
                }
                var result = dynamicCursor;
                reader.Close();
                return result;
            }
        }
    }
}