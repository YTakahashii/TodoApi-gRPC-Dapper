using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using Moq;
using TodoApi_gRPC_Dapper.Models;
using TodoApi_gRPC_Dapper.Models.Persistance;
using TodoApi_gRPC_Dapper.Models.Repository;
using TodoApi_gRPC_Dapper.Implements;
using Proto.Todo;
using Grpc.Core;
using Grpc.Core.Utils;
using Grpc.Core.Testing;
using Google.Protobuf;

namespace TodoApi_gRPC_Dapper.Tests.Implements
{

    public class TodoImplementTests
    {
        private readonly ITestOutputHelper outputHelper;
        private List<Todo> expectedTodoItems;
        private Mock<IUnitOfWork> uowMoq;
        private Mock<ITodoItemRepository> todoItemRepoMoq;
        private TodoImplement implement;

        public TodoImplementTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
            expectedTodoItems = new List<Todo>()
            {
                new Todo{ Id = Guid.NewGuid().ToString("N"), Name = "Test1", IsComplete = false},
                new Todo{ Id = Guid.NewGuid().ToString("N"), Name = "Test2", IsComplete = false},
                new Todo{ Id = Guid.NewGuid().ToString("N"), Name = "Test3", IsComplete = false}
            };
            uowMoq = new Mock<IUnitOfWork>();
            // ITodoItemRepositoryのメソッドのダミーメソッドを設定する
            todoItemRepoMoq = new Mock<ITodoItemRepository>();
            // FindAllAsyncのダミーのReturnをReturnsAsyncで設定
            todoItemRepoMoq.Setup(x => x.FindAllAsync()).ReturnsAsync(expectedTodoItems);
            // ダミーメソッドに引数がある場合はIt.IsAny<引数の型>()で指定する
            todoItemRepoMoq.Setup(x => x.FindAsync(It.IsAny<String>()))
                .ReturnsAsync((String id) => expectedTodoItems.Find(x => x.Id == id));
            // ダミーメソッドの中身を書き換える場合はCallbackでoverrideする
            todoItemRepoMoq.SetupAsync(x => x.Add(It.IsAny<Todo>()))
                .Callback<Todo>(item => expectedTodoItems.Add(item));
            todoItemRepoMoq.SetupAsync(x => x.Update(It.IsAny<Todo>()))
                .Callback<Todo>(item =>
                {
                    var index = expectedTodoItems.FindIndex(x => x.Id == item.Id);
                    expectedTodoItems[index] = item;
                });
            todoItemRepoMoq.SetupAsync(x => x.Remove(It.IsAny<Todo>()))
                .Callback<Todo>(item => expectedTodoItems.Remove(item));
            todoItemRepoMoq.Setup(x => x.Count()).Returns(expectedTodoItems.Count);
            uowMoq.Setup(x => x.TodoItems).Returns(todoItemRepoMoq.Object);

            implement = new TodoImplement(uowMoq.Object);
        }

        [Fact(DisplayName = "GetTodoItems({})が正しく動作する")]
        public async Task OkGetTodoItemsTest()
        {
            var fakeServerCallContext = TestServerCallContext.Create("fooMethod", null, DateTime.UtcNow.AddHours(1), new Metadata(), CancellationToken.None, "127.0.0.1", null, null, (metadata) => TaskUtils.CompletedTask, () => new WriteOptions(), (writeOptions) => { });
            var response = await implement.GetTodoItems(new Empty(), fakeServerCallContext);

            Assert.Equal(expectedTodoItems, response.Todos);
        }

        [Fact(DisplayName = "GetTodoItem({Id:'Id'})が正しく動作する")]
        public async Task OkGetTodoItemTest()
        {
            var expectedTodoItem = expectedTodoItems[0];
            var request = new GetTodoItemRequest { Id = expectedTodoItem.Id };
            var fakeServerCallContext = TestServerCallContext.Create("fooMethod", null, DateTime.UtcNow.AddHours(1), new Metadata(), CancellationToken.None, "127.0.0.1", null, null, (metadata) => TaskUtils.CompletedTask, () => new WriteOptions(), (writeOptions) => { });
            var actual = await implement.GetTodoItem(request, fakeServerCallContext);

            Assert.Equal(expectedTodoItem, actual.Todo);
        }
    }
}
