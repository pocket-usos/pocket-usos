namespace App.Infrastructure.Exceptions;

public class RepositoryException(string messageTemplate, Type objectType) : Exception
{
    public override string Message => string.Format(MessageTemplate, MessageArguments);

    protected string MessageTemplate = messageTemplate;

    protected object[] MessageArguments = { objectType };
}

public class RepositoryException<T> : RepositoryException
{
    protected RepositoryException(string messageTemplate) : base(messageTemplate, typeof(T))
    {
        MessageTemplate = messageTemplate;
        MessageArguments = [typeof(T)];
    }
}
