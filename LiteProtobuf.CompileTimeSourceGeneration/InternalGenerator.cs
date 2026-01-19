using Microsoft.CodeAnalysis;
using Myitian.LiteProtobuf.CompileTimeSourceGeneration.Templates;

namespace Myitian.LiteProtobuf.CompileTimeSourceGeneration;


/// <summary>
/// The definition of <c>formatArgs</c> array:
/// <list type="bullet">
/// <item><c>{0}</c>: base mode</item>
/// <item><c>{1}</c>: mode</item>
/// <item><c>{2}</c>: type param</item>
/// <item><c>{3}</c>: type param with comma</item>
/// <item><c>{4}</c>: private use</item>
/// <item><c>{5}</c>: private use</item>
/// <item><c>{6}</c>: private use</item>
/// </list>
/// </summary>
[Generator]
public class InternalGenerator : IIncrementalGenerator
{
    public static readonly Model[] Models = [
        new("VarInt",  "VarInt",       "T",    "where T : IBinaryInteger<T>"),
        new("VarInt",  "VarIntZigZag", "T",    "where T : IBinaryInteger<T>, ISignedNumber<T>"),
        new("Fixed32", "Fixed32",      "T",    "where T : struct"),
        new("Fixed64", "Fixed64",      "T",    "where T : struct"),
        new("VarInt",  "Bool",         "bool", null)];
    public readonly struct Model(string baseMode, string mode, string typeParam, string? constraint)
    {
        /// <summary>
        /// Used for wire types.
        /// </summary>
        public readonly string BaseMode = baseMode;
        /// <summary>
        /// The main mode of the model.
        /// </summary>
        public readonly string Mode = mode;
        /// <summary>
        /// The type parameter of the model, usually T for generics.
        /// </summary>
        public readonly string TypeParam = typeParam;
        /// <summary>
        /// The constraint on the type parameter, or <see langword="true"/> if the type parameter is not generic type parameter.
        /// </summary>
        public readonly string? Constraint = constraint;
    }
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // new System.Threading.Thread(static () => System.Threading.Thread.Sleep(100000)) { IsBackground = false }.Start(); // Keep console not to close to check the console output
        RepeatedUtility.RegisterAll(context);
        DefaultHandler.RegisterAll(context);
    }
}