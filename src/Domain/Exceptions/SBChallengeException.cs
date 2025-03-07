namespace MsClean.Domain;
using System;

public class SBChallengeException : Exception
{
    public SBChallengeException()
    { }

    public SBChallengeException(string message)
        : base(message)
    { }

    public SBChallengeException(string message, Exception innerException)
        : base(message, innerException)
    { }

}
