using System;
using System.Threading.Tasks;
using VerifyTests;

readonly struct ResultBuilder
{
    public string Extension { get; }
    public OnDiff OnDiff { get; }
    public Func<FilePair, Task<EqualityResult>> GetResult { get; }

    public ResultBuilder(string extension, OnDiff onDiff, Func<FilePair, Task<EqualityResult>> getResult)
    {
        Extension = extension;
        OnDiff = onDiff;
        GetResult = getResult;
    }
}

