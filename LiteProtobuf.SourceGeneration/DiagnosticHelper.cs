using Microsoft.CodeAnalysis;
using Myitian.LiteProtobuf.SourceGeneration.Properties;

namespace Myitian.LiteProtobuf.SourceGeneration;

static class DiagnosticHelper
{
    public static LocalizableResourceString LocalizableResourceString(string name)
        => new(name, Resources.ResourceManager, typeof(Resources));

    public static readonly DiagnosticDescriptor ExplicitInterfaceImplementationNotSupportedRule = new(
        "LPSG001",
        LocalizableResourceString(nameof(Resources.LPSG001_ExplicitInterfaceImplementationNotSupported_Title)),
        LocalizableResourceString(nameof(Resources.LPSG001_ExplicitInterfaceImplementationNotSupported_Message)),
        "Usage",
        DiagnosticSeverity.Warning,
        true,
        LocalizableResourceString(nameof(Resources.LPSG001_ExplicitInterfaceImplementationNotSupported_Description)));

    public static readonly DiagnosticDescriptor AttributeOnBackingFieldNotSupportedRule = new(
        "LPSG002",
        LocalizableResourceString(nameof(Resources.LPSG002_AttributeOnBackingFieldNotSupported_Title)),
        LocalizableResourceString(nameof(Resources.LPSG002_AttributeOnBackingFieldNotSupported_Message)),
        "Usage",
        DiagnosticSeverity.Warning,
        true,
        LocalizableResourceString(nameof(Resources.LPSG002_AttributeOnBackingFieldNotSupported_Description)));

    public static readonly DiagnosticDescriptor AttributeOnWriteOnlyPropertyNotSupportedRule = new(
        "LPSG003",
        LocalizableResourceString(nameof(Resources.LPSG003_AttributeOnWriteOnlyPropertyNotSupported_Title)),
        LocalizableResourceString(nameof(Resources.LPSG003_AttributeOnWriteOnlyPropertyNotSupported_Message)),
        "Usage",
        DiagnosticSeverity.Warning,
        true,
        LocalizableResourceString(nameof(Resources.LPSG003_AttributeOnWriteOnlyPropertyNotSupported_Description)));

    public static readonly DiagnosticDescriptor DuplicateFieldNumberRule = new(
        "LPSG004",
        LocalizableResourceString(nameof(Resources.LPSG004_DuplicateFieldNumber_Title)),
        LocalizableResourceString(nameof(Resources.LPSG004_DuplicateFieldNumber_Message)),
        "Usage",
        DiagnosticSeverity.Warning,
        true,
        LocalizableResourceString(nameof(Resources.LPSG004_DuplicateFieldNumber_Description)));

    public static readonly DiagnosticDescriptor MissingInterfaceRule = new(
        "LPSG005",
        LocalizableResourceString(nameof(Resources.LPSG005_MissingInterface_Title)),
        LocalizableResourceString(nameof(Resources.LPSG005_MissingInterface_Message)),
        "Usage",
        DiagnosticSeverity.Error,
        true,
        LocalizableResourceString(nameof(Resources.LPSG005_MissingInterface_Description)));

    public static Diagnostic ExplicitInterfaceImplementationNotSupported(Location? location, string? member)
    {
        return Diagnostic.Create(ExplicitInterfaceImplementationNotSupportedRule, location, member);
    }
    public static Diagnostic AttributeOnBackingFieldNotSupported(Location? location, string? attribute, string? property)
    {
        return Diagnostic.Create(AttributeOnBackingFieldNotSupportedRule, location, attribute, property);
    }
    public static Diagnostic AttributeOnWriteOnlyPropertyNotSupported(Location? location, string? attribute, string? property)
    {
        return Diagnostic.Create(AttributeOnWriteOnlyPropertyNotSupportedRule, location, attribute, property);
    }
    public static Diagnostic DuplicateFieldNumber(Location? location, int fieldNumber, string? anotherMember)
    {
        return Diagnostic.Create(DuplicateFieldNumberRule, location, fieldNumber, anotherMember);
    }
    public static Diagnostic MissingInterface(Location? location, string? type, string? interfaceRequired)
    {
        return Diagnostic.Create(MissingInterfaceRule, location, type, interfaceRequired);
    }
}