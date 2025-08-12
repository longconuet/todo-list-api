using TodoListApi.Models;

namespace TodoListApi.Endpoints
{
    public static class TodoEndpoints
    {
        public static void MapTodoEndpoints(this WebApplication app)
        {
            var todos = new List<Todo>
            {
                new() { Id = Guid.NewGuid(), Title = "Learn .NET 9", IsCompleted = false },
                new() { Id = Guid.NewGuid(), Title = "Build a Web API", IsCompleted = false }
            };

            app.MapGet("/todos", () => todos);

            app.MapGet("/todos/{id}", (Guid id) =>
            {
                var todo = todos.FirstOrDefault(t => t.Id == id);
                return todo is not null ? Results.Ok(todo) : Results.NotFound();
            });

            app.MapPost("/todos", (Todo todo) =>
            {
                todo.Id = Guid.CreateVersion7();
                todos.Add(todo);
                return Results.Created($"/todos/{todo.Id}", todo);
            });

            app.MapPut("/todos/{id}", (Guid id, Todo inputTodo) =>
            {
                var todo = todos.FirstOrDefault(t => t.Id == id);

                if (todo is null)
                {
                    return Results.NotFound();
                }

                todo.Title = inputTodo.Title;
                todo.IsCompleted = inputTodo.IsCompleted;

                return Results.NoContent();
            });

            app.MapDelete("/todos/{id}", (Guid id) =>
            {
                var todo = todos.FirstOrDefault(t => t.Id == id);

                if (todo is null)
                {
                    return Results.NotFound();
                }

                todos.Remove(todo);

                return Results.NoContent();
            });
        }
    }
}
