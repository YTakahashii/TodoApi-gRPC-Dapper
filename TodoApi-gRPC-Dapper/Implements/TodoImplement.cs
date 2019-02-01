using System;
using System.Threading.Tasks;
using Grpc;
using Grpc.Core;
using Grpc.Core.Utils;
using TodoApi_gRPC_Dapper.Models;
using TodoApi_gRPC_Dapper.Models.Persistance;
using Proto.Todo;

namespace TodoApi_gRPC_Dapper.Implements
{
    public class TodoImplement: TodoService.TodoServiceBase
    {
        private IUnitOfWork _unitOfWork;

        public TodoImplement(IUnitOfWork unitOfWork): base()
        {
            _unitOfWork = unitOfWork;
            if (_unitOfWork.TodoItems.Count() == 0)
            {
                _unitOfWork.TodoItems.Add(new Todo { Name = "Item1", IsComplete = false });
            }
        }

        public override async Task<GetTodoItemsResponse> GetTodoItems(Empty request, ServerCallContext context)
        {
            var result = await _unitOfWork.TodoItems.FindAllAsync();
            var response = new GetTodoItemsResponse();
            foreach (var todo in result)
            {
                response.Todos.Add(todo);
            }

            return response;
        }
    }
}
