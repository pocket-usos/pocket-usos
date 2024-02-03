using App.Application.Contracts;
using App.Infrastructure.Configuration.Processing;

namespace App.Infrastructure;

public class Gateway : IGateway
{
    public async Task ExecuteCommandAsync(ICommand command)
    {
        await CommandExecutor.Execute(command);
    }

    public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        return await CommandExecutor.Execute(command);
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
    {
        return await QueryExecutor.Execute(query);
    }
}