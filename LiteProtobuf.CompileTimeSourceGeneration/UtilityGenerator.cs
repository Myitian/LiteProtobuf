using Microsoft.CodeAnalysis;
using Myitian.LiteProtobuf.CompileTimeSourceGeneration.Templates;

namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration;

[Generator]
public class UtilityGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // new System.Threading.Thread(static () => System.Threading.Thread.Sleep(100000)) { IsBackground = false }.Start(); // Keep console not to close
        context.RegisterPostInitializationOutput(Repeated.TryRead.Apply);
        context.RegisterPostInitializationOutput(Repeated.Read.Apply);
        context.RegisterPostInitializationOutput(Repeated.Write.Apply);
    }
}