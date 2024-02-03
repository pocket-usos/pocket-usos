namespace App.Infrastructure.Exceptions;

public class NotFoundRepositoryException<T> : RepositoryException<T>
{
    private const string NotFoundMessageTemplate = "Object {0} with id '{1}' was not found";

    public NotFoundRepositoryException(Guid objectId) : base(NotFoundMessageTemplate)
    {
        MessageArguments = MessageArguments.Append(objectId).ToArray();
    }

    public NotFoundRepositoryException(string template, object[] arguments) : base(template)
    {
        MessageTemplate = template;
        MessageArguments = arguments;
    }
}
