<!--
GENERATED FILE - DO NOT EDIT
This file was generated by [MarkdownSnippets](https://github.com/SimonCropp/MarkdownSnippets).
Source File: /readme.source.md
To change this file edit the source file and then run MarkdownSnippets.
-->

# <img src="/src/icon.png" height="30px"> Verify

[![Build status](https://ci.appveyor.com/api/projects/status/dpqylic0be7s9vnm/branch/master?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.Xunit.svg?cacheSeconds=86400)](https://www.nuget.org/packages/Verify.Xunit/)

Verification tool to enable simple approval of complex models using [Json.net](https://www.newtonsoft.com/json).

<!-- toc -->
## Contents

  * [NuGet package](#nuget-package)
  * [Usage](#usage)
    * [Class being tested](#class-being-tested)
    * [Test](#test)
    * [Initial Verification](#initial-verification)
    * [Subsequent Verification](#subsequent-verification)
  * [Received and Verified](#received-and-verified)
  * [Not valid json](#not-valid-json)
  * [Documentation](#documentation)
<!-- endtoc -->


## NuGet package

https://nuget.org/packages/Verify.Xunit/


## Usage


### Class being tested

Given a class to be tested:

<!-- snippet: ClassBeingTested -->
<a id='snippet-classbeingtested'/></a>
```cs
public static class ClassBeingTested
{
    public static Person FindPerson()
    {
        return new Person
        {
            Id = new Guid("ebced679-45d3-4653-8791-3d969c4a986c"),
            Title = Title.Mr,
            GivenNames = "John",
            FamilyName = "Smith",
            Spouse = "Jill",
            Children = new List<string>
            {
                "Sam",
                "Mary"
            },
            Address = new Address
            {
                Street = "4 Puddle Lane",
                Country = "USA"
            }
        };
    }
}
```
<sup>[snippet source](/src/Verify.Xunit.Tests/Snippets/ClassBeingTested.cs#L4-L29) / [anchor](#snippet-classbeingtested)</sup>
<!-- endsnippet -->


### Test

It can be tested as follows:

<!-- snippet: SampleTest -->
<a id='snippet-sampletest'/></a>
```cs
public class SampleTest :
    VerifyBase
{
    [Fact]
    public Task Simple()
    {
        var person = ClassBeingTested.FindPerson();
        return Verify(person);
    }

    public SampleTest(ITestOutputHelper output) :
        base(output)
    {
    }
}
```
<sup>[snippet source](/src/Verify.Xunit.Tests/Snippets/SampleTest.cs#L6-L22) / [anchor](#snippet-sampletest)</sup>
<!-- endsnippet -->


### Initial Verification

When the test is initially run will fail with:

```
First verification. SampleTest.Simple.verified.txt not found.
Verification command has been copied to the clipboard.
```

The clipboard will contain the following:

> cmd /c move /Y "C:\Code\Sample\SampleTest.Simple.received.txt" "C:\Code\Sample\SampleTest.Simple.verified.txt"

If a [Diff Tool](docs/diff-tool.md) is enable it will display the diff:

![InitialDiff](/docs/InitialDiff.png)

To verify the result:

 * Execute the command from the clipboard, or
 * Use the diff tool to accept the changes , or
 * Manually copy the text to the new file

This will result in the `SampleTest.Simple.verified.txt` being created:

<!-- snippet: SampleTest.Simple.verified.txt -->
<a id='snippet-SampleTest.Simple.verified.txt'/></a>
```txt
{
  GivenNames: 'John James',
  FamilyName: 'Smith',
  Spouse: 'Jill',
  Address: {
    Street: '64 Barnett Street',
    Country: 'USA'
  },
  Children: [
    'Sam',
    'Mary'
  ],
  Id: Guid_1
}
```
<sup>[snippet source](/src/Verify.Xunit.Tests/Snippets/SampleTest.Simple.verified.txt#L1-L14) / [anchor](#snippet-SampleTest.Simple.verified.txt)</sup>
<!-- endsnippet -->


### Subsequent Verification

If the implementation of `ClassBeingTested` changes:

<!-- snippet: ClassBeingTestedChanged -->
<a id='snippet-classbeingtestedchanged'/></a>
```cs
public static class ClassBeingTested
{
    public static Person FindPerson()
    {
        return new Person
        {
            Id = new Guid("ebced679-45d3-4653-8791-3d969c4a986c"),
            Title = Title.Mr,
            GivenNames = "John James",
            FamilyName = "Smith",
            Spouse = "Jill",
            Children = new List<string>
            {
                "Sam",
                "Mary"
            },
            Address = new Address
            {
                Street = "64 Barnett Street",
                Country = "USA"
            }
        };
    }
}
```
<sup>[snippet source](/src/Verify.Xunit.Tests/Snippets/ClassBeingTestedChanged.cs#L6-L31) / [anchor](#snippet-classbeingtestedchanged)</sup>
<!-- endsnippet -->

And the test is re run it will fail with

```
Verification command has been copied to the clipboard.
Assert.Equal() Failure
                                  ↓ (pos 21)
Expected: ···\n  GivenNames: 'John',\n  FamilyName: 'Smith',\n  Spouse: 'Jill···
Actual:   ···\n  GivenNames: 'John James',\n  FamilyName: 'Smith',\n  Spouse:···
                                  ↑ (pos 21)
```
The clipboard will again contain the following:

> cmd /c move /Y "C:\Code\Sample\SampleTest.Simple.received.txt" "C:\Code\Sample\SampleTest.Simple.verified.txt"

And the [Diff Tool](docs/diff-tool.md) is will display the diff:

![SecondDiff](/docs/SecondDiff.png)

The same approach can be used to verify the results and the change to `SampleTest.Simple.verified.txt` is committed to source control along with the change to `ClassBeingTested`.


## Received and Verified

 * **All `*.verified.txt` files should be committed to source control.**
 * **All `*.received.txt` files should be excluded from source control.**


## Not valid json

Note that the output is technically not valid json. [Single quotes are used](docs/serializer-settings.md#single-quotes-used) and [names are not quoted](docs/serializer-settings.md#quotename-is-false). The reason for this is to make the resulting output easier to read and understand.


## Documentation

 * [Serializer Settings](docs/serializer-settings.md)
 * [File Extension](docs/file-extension.md)
 * [Named Tuples](docs/named-tuples.md)
 * [Scrubbers](docs/scrubbers.md)
 * [Diff Tool](docs/diff-tool.md)
 * [Using anonymous types](docs/anonymous-types.md)


## Release Notes

See [closed milestones](../../milestones?state=closed).


## Icon

[Helmet](https://thenounproject.com/term/helmet/9554/) designed by [Leonidas Ikonomou](https://thenounproject.com/alterego) from [The Noun Project](https://thenounproject.com).
