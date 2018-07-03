# PipeStream

> Pipeline DSL in C#

```csharp
var output = Console.Out.AsPipedStream();
var input = Console.In.AsPipedStream()
output |= (s) => s.Trim() | input;
output |= "Done!";
```

Pipeline reads data from right to left

## Examples

```csharp
int[] input = { 1, 2, 3, 4, 5 };
var output = new List<int>();
var from = input.AsInputPipedStream();
var to = output.AsPipedStream();

for (int i = 0; i < input.Length; i++)
    to |= from;
```