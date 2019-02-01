using System;
using TodoApi_gRPC_Dapper.Models.Repository;

namespace TodoApi_gRPC_Dapper.Models.Persistance
{
    public class UnitOfWork
    {
        private ITodoItemRepository todoItems;

        public UnitOfWork()
        {
        }

        public ITodoItemRepository TodoItems
        {
            get
            {
                if (todoItems == null)
                {
                    todoItems = new TodoItemRepository();
                }

                return todoItems;
            }
        }
    }
}
