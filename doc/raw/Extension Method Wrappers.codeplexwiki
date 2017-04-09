The syntax for directly using connected properties is a bit awkward. One way to make it less awkward for end-users is to wrap a connector in extension methods, as such:

{code:C#}
// Represents a piece of data that has been read out of a file. We want to connect a property containing the source line number.
public class Token { .. }

// Allow access to the connected property via extension methods.
using Nito.ConnectedProperties;
public static class TokenExtensions
{
  private const string SourceLineNumber = Guid.NewGuid().ToString("N");

  public static int GetSourceLineNumber(Token token)
  {
    return PropertyConnector.Default.Get(token, SourceLineNumber).Get();
  }

  public static void SetSourceLineNumber(Token token, int lineNumber)
  {
    PropertyConnector.Default.Get(token, SourceLineNumber).Set(lineNumber);
  }
}
{code:C#}

The end-user then has {{GetSourceLineNumber}} and {{SetSourceLineNumber}} extension methods that they may use, instead of having to use the Connected Properties library directly.
