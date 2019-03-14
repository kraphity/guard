# Kraphity.Guard
Simple Guard developed for .NET Standard.

## Getting Started
### Installing
The package is available via [NuGet](https://www.nuget.org/packages/Kraphity.Guard)

### Usage
```
Check.NotNull(myString, nameof(myString));

Check.NotEmpty(myString, nameof(myString));
Check.NotEmpty(myList, nameof(myList));
Check.NotEmpty(myGuid, nameof(myGuid));

Check.NotWhitespace(myString, nameof(myString));
Check.NotNullOrEmpty(myString, nameof(myString));
Check.NotNullOrWhitespace(myString, nameof(myString));

Check.InRange(myInt > 0, nameof(myInt), myInt);

Check.If(myInt == 3, () => new ArgumentException());
```
