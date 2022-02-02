using System;
using System.Diagnostics.CodeAnalysis;

namespace Promact.Trappist.Repository.Questions;

public class QuestionNotFoundException : Exception
{
    private readonly string _message;
    [ExcludeFromCodeCoverage]
    public QuestionNotFoundException(int questionId)
    {
        _message = $"Question with Id: {questionId} was not found";
    }
}