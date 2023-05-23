using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.TodoItems.Commands.CreateTodoItem;
using mentor_v1.Application.TodoItems.Commands.DeleteTodoItem;
using mentor_v1.Application.TodoLists.Commands.CreateTodoList;
using mentor_v1.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace mentor_v1.Application.IntegrationTests.TodoItems.Commands;

using static Testing;

public class DeleteTodoItemTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new DeleteTodoItemCommand(Guid.NewGuid());

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteTodoItem()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        var itemId = await SendAsync(new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "New Item"
        });

        await SendAsync(new DeleteTodoItemCommand(itemId));

        var item = await FindAsync<TodoItem>(itemId);

        item.Should().BeNull();
    }
}
