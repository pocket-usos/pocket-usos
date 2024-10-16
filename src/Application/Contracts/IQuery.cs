using MediatR;

namespace App.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
    Guid Id { get; }
}
