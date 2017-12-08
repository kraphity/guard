# Kraphity.Guard
Simple Guard developed for .NET Standard.

## Getting Started
### Installing
The package is available via [NuGet](https://www.nuget.org/packages/Kraphity.Guard)

### Usage
```
Check.NotNull(myString, () => myString);

Check.NotEmpty(myString, () => myString);
Check.NotEmpty(myList, () => myList);
Check.NotEmpty(myGuid, () => myGuid);

Check.NotWhitespace(myString, () => myString);
Check.NotNullOrEmpty(myString, () => myString);
Check.NotNullOrWhitespace(myString, () => myString);

Check.InRange(myInt > 0, () => myInt, myInt);

Check.If(myInt == 3, () => new ArgumentException());
```