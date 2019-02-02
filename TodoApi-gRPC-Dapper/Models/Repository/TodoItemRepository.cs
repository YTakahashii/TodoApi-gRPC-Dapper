using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Proto.Todo;
using TodoApi_gRPC_Dapper.Models.SqlHandler;

namespace TodoApi_gRPC_Dapper.Models.Repository {
    public class TodoItemRepository : IBaseRepository<Todo>, ITodoItemRepository {
        private string connectionString;
        private string tableName = "[dbo].[TodoItems]";

        internal IDbConnection Connection {
            get {
                var sql = new SqlHandler.SqlHandler ();
                return sql.GetConnection ();
            }
        }

        public TodoItemRepository () { }

        public async Task<Todo> FindAsync (String id) {
            using (IDbConnection db = Connection) {
                db.Open ();
                var query = $"SELECT * FROM {tableName} WHERE Id LIKE '{id}'";
                return (await db.QueryAsync<Todo> (query)).FirstOrDefault ();
            }
        }

        public async Task<IEnumerable<Todo>> FindAllAsync () {
            IEnumerable<Todo> result;
            using (IDbConnection db = Connection) {
                db.Open ();
                var query = $"SELECT * FROM {tableName}";
                result = await db.QueryAsync<Todo> (query);
                return result;
            }
        }

        public async Task<Todo> Add (Todo entity) {
            using (IDbConnection db = Connection) {
                db.Open ();
                using (var tran = db.BeginTransaction ()) {
                    try {
                        entity.Id = Guid.NewGuid ().ToString ("N");
                        entity.IsComplete = false;

                        String sql = $@"INSERT INTO {tableName} (Id ,Name, IsComplete) VALUES (@Id, @Name, @IsComplete)";
                        await db.ExecuteAsync (sql, entity, tran);
                        tran.Commit ();
                        return entity;
                    } catch {
                        tran.Rollback ();
                        throw new Exception ("Error: Rollback");
                    }
                }
            }
        }

        public async Task Remove (Todo entity) {
            using (IDbConnection db = Connection) {
                db.Open ();
                using (var tran = db.BeginTransaction ()) {
                    try {
                        var sql = $@"DELETE FROM {tableName} WHERE Id LIKE @Id";
                        await db.ExecuteAsync (sql, entity, tran);
                        tran.Commit ();
                    } catch {
                        tran.Rollback ();
                    }
                }
            }
        }

        public async Task Update (Todo entity) {
            using (IDbConnection db = Connection) {
                db.Open ();
                using (var tran = db.BeginTransaction ()) {
                    try {
                        var sql = $@"UPDATE {tableName} SET Name = @Name, IsComplete = @IsComplete WHERE Id LIKE @Id";
                        await db.ExecuteAsync (sql, entity, tran);
                        tran.Commit ();
                    } catch {
                        tran.Rollback ();
                    }
                }
            }
        }

        public int Count () {
            using (IDbConnection db = Connection) {
                var result = db.ExecuteScalar<int> ($"SELECT COUNT(*) FROM {tableName}");
                return result;
            }
        }
    }
}