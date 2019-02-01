using System;
using TodoApi_gRPC_Dapper.Models.Repository;

namespace TodoApi_gRPC_Dapper.Models.Persistance
{
    public interface IUnitOfWork
    {
        ITodoItemRepository TodoItems { get; }
    }
}
