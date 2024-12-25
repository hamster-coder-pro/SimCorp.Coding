# SimCorp.Coding

## SimCorp.Coding.Triangles

The **SimCorp.Coding.Triangles** project initially built to identify triangle type.  
However later added different extension point to easely grow its abilities to other types of calucations of any figures.  

This project is designed to be extensible and testable, with clear separation of concerns. The project is modular and uses dependency injection to allow for flexible input, processing, and output strategies.

*Let me know if you need further details or assistance!*

### Run application

Run application in **TriangleTypeApp** mode.
```
SimCorp.Coding.Triangles.exe triangle-type
```

Run application in **TriangleAreaApp** mode.
```
SimCorp.Coding.Triangles.exe triangle-area
```

Run application in **TriangleLargestAngleApp** mode.
```
SimCorp.Coding.Triangles.exe triangle-largest-angle
```

---
### Extend application 

1.1. Define your input arguments model
```csharp
public sealed class YourArguments : IInputArguments
{
  // put needed properties here
}
```
1.2. Define your output result model
```csharp
public sealed class YourResult : IOutputResult
{
    // put needed properties here
}
```

2. Define input arguments parser
```csharp
internal sealed class YourArgumentsInputProvider : InputProviderBase<YourArguments>
{
  // put logic to convert user input to your input arguments model
}
```

3. Define simple processor for 

```csharp
internal class YourProcessor : InputProcessorBase<YourArguments, YourResult> 
{
	// override ProcessLogic and put logic there
}
```

4. In more complex cases you need to use MatchResult logic to chunk processing into smaller meaningful peaces.
```csharp
public class YourMatchResult : IMatchResult
{
	// your small peace of work model properties here
}

```csharp
internal class OneOfYoursResultMatcher : ResultMatcherBase<YourArguments, YourMatchResult>
{
    // override [Priority] to order your matchers if needed.
    // override [IsDefault] to make this matcher default logic if no match result found.
	// override [MatchLogic] to put your match logic
}

```csharp
internal class YourProcessor : InputProcessorBase<YourArguments, YourResult, YourMatchResult> 
{
    // override [BreakOnMatchFound] to define if exec flow should break on any match result found.
	// override [AggregateMatch] to aggregate MatchResult to YourResult
}
```

5. Define output builder

```csharp
internal class YourResultOutputBuilder : TypeOutputBuilderBase<YourResult>
{
    // convert result model to user expected output view
}
```

6. Create Application

```csharp
internal class YourApp : AppBase<YourArguments, YourResult>
{    
    // name your app (name will be used as argument in console application)
}
```

7. Register your classes in DI

```csharp
    serviceCollection.AddTransient<IApplication, YourApp>();

    // register matchers (if any)
    serviceCollection.AddTransient<IResultMatcher<YourArguments, YourMatchResult>, YourResultMatcherNo1>();
    ....
    serviceCollection.AddTransient<IResultMatcher<YourArguments, YourMatchResult>, YourResultMatcherNoN>();

    serviceCollection.AddTransient<IInputProcessor<YourArguments, YourResult>, YourProcessor>();
    serviceCollection.AddTransient<IInputProvider<YourArguments>, YourArgumentsInputProvider>();
    serviceCollection.AddTransient<IOutputBuilder<YourResult>, YourResultOutputBuilder>();
```

### *Examples are based on current codebase*

1. **TriangleAreaApp**:
   - Calculates the area of a triangle based on its sides.
   - Inherits from `AppBase<TriangleArguments, TriangleAreaResult>`.

2. **TriangleTypeApp**:
   - Determines the type of a triangle (e.g., equilateral, isosceles, scalene).
   - Inherits from `AppBase<TriangleArguments, TriangleTypeResult>`.

3. **TriangleLargestAngleApp**:
   - Identifies the largest angle in a triangle.
   - Inherits from `AppBase<TriangleArguments, TriangleLargestAngleResult>`.

## SimCorp.Coding.Triangles.Tests

## SimCorp.Coding.Words