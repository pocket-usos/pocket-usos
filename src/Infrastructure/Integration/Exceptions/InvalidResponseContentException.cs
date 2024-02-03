namespace App.Infrastructure.Integration.Exceptions;

internal class InvalidResponseContentException(string? message) : Exception(message);
