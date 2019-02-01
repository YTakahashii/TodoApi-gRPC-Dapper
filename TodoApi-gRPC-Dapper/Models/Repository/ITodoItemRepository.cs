using System;
using Proto.Todo;

namespace TodoApi_gRPC_Dapper.Models.Repository
{
    public interface ITodoItemRepository: IBaseRepository<Todo>
    {
    }
}
