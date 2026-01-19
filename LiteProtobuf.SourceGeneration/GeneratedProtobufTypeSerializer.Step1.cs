using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Myitian.LiteProtobuf.SourceGeneration;

static partial class GeneratedProtobufTypeSerializer
{
    readonly record struct Step1
    {
        public readonly INamedTypeSymbol Target;
        public readonly ImmutableArray<ProtobufField> FieldsToRead;
        public readonly ImmutableArray<ProtobufField> FieldsToWrite;
        public readonly ProtobufRemeaningFields RemeaningFields;
        public readonly ImmutableArray<Diagnostic> Diagnostics;


        public Step1(Step0 step0, RequiredSymbols symbols)
        {
            Target = step0.Target!;
            RemeaningFields = default;
            ProtobufField.Collection fieldsToRead = ProtobufField.Collection.Create(step0.NoSort);
            ProtobufField.Collection fieldsToWrite = ProtobufField.Collection.Create(step0.NoSort);
            List<Diagnostic> diagnostics = [];
            bool error = false;
            foreach (ISymbol member in Target.AllAccessiblePropertiesAndFields(symbols.Compilation))
                error |= ProcessMember(step0, symbols, member, fieldsToRead, fieldsToWrite, ref RemeaningFields, diagnostics);
            if (!error)
            {
                FieldsToRead = [.. fieldsToRead];
                FieldsToWrite = [.. fieldsToWrite];
            }
            Diagnostics = [.. diagnostics];
        }
        /// <returns><see langword="true"/> if any error occurred</returns>
        static bool ProcessMember(
            Step0 step0,
            RequiredSymbols symbols,
            ISymbol member,
            ProtobufField.Collection fieldsToRead,
            ProtobufField.Collection fieldsToWrite,
            ref ProtobufRemeaningFields remeaningFields,
            List<Diagnostic> diagnostics)
        {
            ImmutableArray<AttributeData> attributes = member.GetAttributes();
            if (attributes.FirstOrDefault(symbols.IsProtobufFieldAttribute) is
                {
                    ConstructorArguments: [
                    { Kind: TypedConstantKind.Primitive, Value: int number },
                    { Kind: TypedConstantKind.Enum, Value: int fieldType }]
                } attribute)
            {
                RawProtobufFieldInfo info = new(attribute);
                info.NoRead |= step0 is { Read: false, TryRead: false };
                info.NoWrite |= step0 is { Write: false };
                if (info.NoRead && info.NoWrite)
                    goto PF_EXIT;
                if (TryLoadProtobufFieldMember(member, diagnostics, attribute, ref info) is not ITypeSymbol type)
                    goto PF_EXIT;
                if (info.NoRead && info.NoWrite)
                    goto PF_EXIT;
                object? factory = info.Factory;
                object? readHandler = info.ReadHandler;
                object? writeHandler = info.WriteHandler;
                if (info.NoRead)
                {
                    factory = null;
                    readHandler = null;
                }
                else
                {
                    if (factory is null)
                    {
                        factory = "";
                    }
                    if (readHandler is null)
                    {
                        readHandler = "";
                    }
                }

                ProtobufField f = new(type, member.Name, number, fieldType, info);
                // error |= TryAddTo(fieldsToRead, fieldsToWrite, diagnostics, number, attribute, info, f);
            }
        PF_EXIT:
            if ((attribute = attributes.FirstOrDefault(symbols.IsProtobufRemainingFieldsAttribute)) is not null)
            {
                bool scopedError = false;
                RawProtobufRemeaningFieldsInfo info = new(attribute);
                switch (member)
                {
                    case IFieldSymbol field when field.IsImplicitlyDeclared:
                        scopedError = true;
                        diagnostics.Add(DiagnosticHelper.AttributeOnBackingFieldNotSupported(
                            attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                            attribute.AttributeClass?.Name,
                            field.AssociatedSymbol?.Name));
                        break;
                    case IPropertySymbol property when property.GetMethod is null:
                        scopedError = true;
                        diagnostics.Add(DiagnosticHelper.AttributeOnWriteOnlyPropertyNotSupported(
                            attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                            attribute.AttributeClass?.Name,
                            property.Name));
                        break;
                }
                if (remeaningFields.IsValid)
                {
                    //    error = true;
                }
                else
                {

                }
                //  error |= scopedError;
            }
            return false;
        }
        static ITypeSymbol? TryLoadProtobufFieldMember(
            ISymbol member,
            List<Diagnostic> diagnostics,
            AttributeData attribute,
            ref RawProtobufFieldInfo info)
        {
            switch (member)
            {
                case IFieldSymbol field:
                    if (!field.IsImplicitlyDeclared)
                    {
                        info.NoWrite |= field.IsReadOnly;
                        return field.Type;
                    }
                    diagnostics.Add(DiagnosticHelper.AttributeOnBackingFieldNotSupported(
                        attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                        attribute.AttributeClass?.Name,
                        field.AssociatedSymbol?.Name));
                    break;
                case IPropertySymbol property:
                    if (property.ExplicitInterfaceImplementations.IsDefaultOrEmpty)
                    {
                        info.NoRead |= property.GetMethod is null;
                        info.NoWrite |= property.SetMethod is not { IsInitOnly: false };
                        return property.Type;
                    }
                    diagnostics.Add(DiagnosticHelper.AttributeOnBackingFieldNotSupported(
                        attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                        attribute.AttributeClass?.Name,
                        property.Name));
                    break;
            }
            return null;
        }
        /// <returns><see langword="true"/> if any error occurred</returns>
        static bool TryAddTo(
            ProtobufField.Collection fieldsToRead,
            ProtobufField.Collection fieldsToWrite,
            List<Diagnostic> diagnostics,
            int number,
            AttributeData attribute,
            RawProtobufFieldInfo info,
            ProtobufField field)
        {
            ProtobufField oldR = default;
            if (!info.NoRead && (oldR = fieldsToRead.Add(field)).IsValid)
            {
                diagnostics.Add(DiagnosticHelper.DuplicateFieldNumber(
                    attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                    number,
                    oldR.Name));
                return true;
            }
            ProtobufField oldW;
            if (!info.NoWrite && (oldW = fieldsToWrite.Add(field)).IsValid)
            {
                if (oldR.Number != oldW.Number)
                {
                    diagnostics.Add(DiagnosticHelper.DuplicateFieldNumber(
                        attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                        number,
                        oldW.Name));
                }
                return true;
            }
            return false;
        }
    }
}
