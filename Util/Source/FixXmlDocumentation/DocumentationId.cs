using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FixXmlDocumentation
{
    public sealed class DocumentationId
    {
        public DocumentationId(string documentationId)
        {
            if (documentationId.StartsWith("N:"))
            {
                this.ObjectType = Type.Namespace;
                this.PopulateNameAndContext(documentationId.Substring(2), false);
            }
            else if (documentationId.StartsWith("T:"))
            {
                this.ObjectType = Type.Type;
                this.PopulateNameAndContext(documentationId.Substring(2), true);
            }
            else if (documentationId.StartsWith("F:"))
            {
                this.ObjectType = Type.Field;
                this.PopulateNameAndContext(documentationId.Substring(2), false);
            }
            else if (documentationId.StartsWith("M:"))
            {
                // Note: We don't handle private virtual methods.
                this.ObjectType = Type.Method;
                int openParenthesisIndex = documentationId.IndexOf('(');
                int closeParenthesisIndex = documentationId.LastIndexOf(')');
                if (openParenthesisIndex == -1 || closeParenthesisIndex == -1 || openParenthesisIndex > closeParenthesisIndex)
                {
                    this.PopulateNameAndContext(documentationId.Substring(2), false);
                }
                else
                {
                    this.PopulateNameAndContext(documentationId.Substring(2, openParenthesisIndex - 2), true);
                    this.PopulateParameters(documentationId.Substring(openParenthesisIndex + 1, closeParenthesisIndex - openParenthesisIndex - 1));
                }
            }
            else if (documentationId.StartsWith("E:"))
            {
                // Note: We don't handle private virtual events.
                this.ObjectType = Type.Event;
                this.PopulateNameAndContext(documentationId.Substring(2), false);
            }
            else if (documentationId.StartsWith("P:"))
            {
                // Note: We don't handle private virtual properties.
                this.ObjectType = Type.Property;
                int openParenthesisIndex = documentationId.IndexOf('(');
                int closeParenthesisIndex = documentationId.LastIndexOf(')');
                if (openParenthesisIndex == -1 || closeParenthesisIndex == -1 || openParenthesisIndex > closeParenthesisIndex)
                {
                    this.PopulateNameAndContext(documentationId.Substring(2), false);
                }
                else
                {
                    this.PopulateNameAndContext(documentationId.Substring(2, openParenthesisIndex - 2), false);
                    this.PopulateParameters(documentationId.Substring(openParenthesisIndex + 1, closeParenthesisIndex - openParenthesisIndex - 1));
                }
            }
            else if (documentationId.StartsWith("O:"))
            {
                this.ObjectType = Type.Overload;
                this.PopulateNameAndContext(documentationId.Substring(2), false);
            }
            else if (documentationId.StartsWith("Overload:"))
            {
                this.ObjectType = Type.Overload;
                this.PopulateNameAndContext(documentationId.Substring(9), false);
            }
            else if (documentationId.StartsWith("!:"))
            {
                this.ObjectType = Type.Error;
                this.Context = new List<NamespaceOrGenericType>();
                this.Name = documentationId.Substring(2);
            }
            else
            {
                throw new FormatException("Unknown prefix: " + documentationId);
            }
        }

        private void PopulateNameAndContext(string documentationId, bool allowGenericParameters)
        {
            var contextIncludingName = documentationId.Split('.');
            this.Context = contextIncludingName.Take(contextIncludingName.Length - 1).Select(x => new NamespaceOrGenericType(x)).ToList();
            if (allowGenericParameters)
            {
                int genericParameterCount;
                this.Name = ParseGenericName(contextIncludingName[contextIncludingName.Length - 1], out genericParameterCount);
                this.GenericParameterCount = genericParameterCount;
            }
            else
            {
                this.Name = contextIncludingName[contextIncludingName.Length - 1];
                this.Context = new List<NamespaceOrGenericType>();
            }
        }

        private void PopulateParameters(string parameterString)
        {
            this.Parameters = new ParameterParser(parameterString).Parse();
        }

        public List<NamespaceOrGenericType> Context { get; private set; }

        public string Name { get; set; }

        public int GenericParameterCount { get; set; }

        public List<MethodParameter> Parameters { get; private set; }

        public Type ObjectType { get; set; }

        public override string ToString()
        {
            string ret = ObjectType.ToString() + ": " + string.Join(".", Context.Select(x => x.ToString()));
            if (Context.Count != 0)
                ret += ".";
            ret += Name;
            if (GenericParameterCount != 0)
                ret += "`" + GenericParameterCount;
            if (Parameters != null)
                ret += "(" + string.Join(", ", Parameters.Select(x => x.ToString())) + ")";
            return ret;
        }

        /// <summary>
        /// Parses a name which may be generic (containing a '`' or '``'). Returns the parsed name (without the '`').
        /// </summary>
        /// <param name="name">The possibly-generic name.</param>
        /// <param name="genericParameterCount">The number of generic parameters, if the name was generic. If the name was not generic, this parameter is set to 0.</param>
        /// <returns>The parsed name (without the '`').</returns>
        private static string ParseGenericName(string name, out int genericParameterCount)
        {
            int delimiterIndex = name.IndexOf('`');
            if (delimiterIndex == -1)
            {
                genericParameterCount = 0;
                return name;
            }

            var ret = name.Substring(0, delimiterIndex);
            if (delimiterIndex < name.Length - 1 && name[delimiterIndex + 1] == '`')
            {
                ++delimiterIndex;
            }

            if (int.TryParse(name.Substring(delimiterIndex + 1), out genericParameterCount))
            {
                return ret;
            }
            else
            {
                return name;
            }
        }

        public enum Type
        {
            Namespace,
            Type,
            Field,
            Method,
            Event,
            Property,
            Overload,
            Error,
        }

        public sealed class NamespaceOrGenericType
        {
            public NamespaceOrGenericType(string name)
            {
                int genericParameterCount;
                this.Name = ParseGenericName(name, out genericParameterCount);
                this.GenericParameterCount = genericParameterCount;
            }

            public string Name { get; set; }

            public int GenericParameterCount { get; set; }

            public override string ToString()
            {
                if (GenericParameterCount == 0)
                    return Name;
                return Name + "`" + GenericParameterCount;
            }
        }

        public sealed class MethodParameter
        {
            public bool ByRef { get; set; }

            public TypeRef Type { get; set; }

            public override string ToString()
            {
                if (ByRef)
                    return Type + " (byref)";
                return Type.ToString();
            }
        }

        public abstract class TypeRef { };

        public sealed class GenericParameter : TypeRef
        {
            public int ParameterIndex { get; set; }

            /// <summary>
            /// <c>true</c> if this is a generic method parameter; <c>false</c> if it is a generic type parameter.
            /// </summary>
            public bool IsMethodParameter { get; set; }

            public override string ToString()
            {
                if (IsMethodParameter)
                    return "{ GenericMethodParameter Index=" + ParameterIndex + " }";
                return "{ GenericTypeParameter Index=" + ParameterIndex + " }";
            }
        }

        public sealed class NamespaceOrConcreteType
        {
            public NamespaceOrConcreteType()
            {
                this.GenericArguments = new List<TypeRef>();
            }

            public string Name { get; set; }

            public List<TypeRef> GenericArguments { get; private set; }

            public override string ToString()
            {
                if (GenericArguments.Count != 0)
                    return Name + "<" + string.Join(", ", GenericArguments.Select(x => x.ToString())) + ">";
                else
                    return Name;
            }
        }

        public sealed class TypeReference : TypeRef
        {
            public TypeReference()
            {
                this.Context = new List<NamespaceOrConcreteType>();
                this.GenericArguments = new List<TypeRef>();
            }

            public string Name { get; set; }

            public List<NamespaceOrConcreteType> Context { get; private set; }

            public List<TypeRef> GenericArguments { get; private set; }

            public override string ToString()
            {
                string name = Name;
                if (GenericArguments.Count != 0)
                    name += "<" + string.Join(", ", GenericArguments.Select(x => x.ToString())) + ">";

                if (Context.Count == 0)
                    return name;
                return string.Join(".", Context.Select(x => x.ToString())) + "." + name;
            }
        }

        public sealed class Array : TypeRef
        {
            public Array()
            {
                this.LowerBoundsAndSizes = new List<Tuple<int?, int?>>();
            }

            public TypeRef ElementType { get; set; }

            public List<Tuple<int?, int?>> LowerBoundsAndSizes { get; private set; }

            public override string ToString()
            {
                return "{ Array Bounds=" + string.Join(",", this.LowerBoundsAndSizes.Select(x => Convert.ToString(x.Item1) + ":" + Convert.ToString(x.Item2))) + ", ElementType=" + this.ElementType + " }";
            }
        }

        public sealed class Pointer : TypeRef
        {
            public TypeRef ElementType { get; set; }

            public override string ToString()
            {
                return "{ Pointer ElementType=" + this.ElementType + " }";
            }
        }

        private class ParameterParser
        {
            private readonly string data;
            private int index;

            public ParameterParser(string data)
            {
                // Kick out any input we can't handle (yet).
                if (data.Contains('^'))
                {
                    throw new NotSupportedException("Pinned modifiers (^) are not supported: " + data);
                }
                else if (data.Contains('|'))
                {
                    throw new NotSupportedException("Required modifiers (|) are not supported: " + data);
                }
                else if (data.Contains('!'))
                {
                    throw new NotSupportedException("Optional modifiers (!) are not supported: " + data);
                }
                else if (data.Contains("[?]"))
                {
                    throw new NotSupportedException("Generic arrays ([?]) are not supported: " + data);
                }
                else if (data.Contains("=FUNC:"))
                {
                    throw new NotSupportedException("Function pointers (=FUNC:) are not supported: " + data);
                }

                this.data = data;
            }

            public List<MethodParameter> Parse()
            {
                // MethodParameterList = MethodParameter (',' MethodParameter)*

                var ret = new List<MethodParameter>();
                while (index != data.Length)
                {
                    ret.Add(ParseParameter());
                    if (index != data.Length)
                    {
                        if (data[index] == ',')
                        {
                            ++index;
                        }
                        else
                        {
                            throw new FormatException("Unexpected character at index " + index + " (expecting ','): " + data);
                        }
                    }
                }

                return ret;
            }

            private MethodParameter ParseParameter()
            {
                // MethodParameter = Type '@'?

                var ret = new MethodParameter();
                ret.Type = ParseType();
                if (index != data.Length && data[index] == '@')
                {
                    ret.ByRef = true;
                    ++index;
                }

                return ret;
            }

            private TypeRef ParseType()
            {
                // Type = GenericParameter |
                //        BaseType (ArraySpec | '*')*
                // GenericParameter = '`' INTEGER |
                //                    '`' '`' INTEGER

                if (data[index] == '`')
                {
                    ++index;
                    var ret1 = new GenericParameter();
                    if (index == data.Length)
                        throw new FormatException("Unexpected end of input after grave accent: " + data);

                    if (data[index] == '`')
                    {
                        ++index;
                        ret1.IsMethodParameter = true;
                        if (index == data.Length)
                            throw new FormatException("Unexpected end of input after grave accent: " + data);
                    }

                    var parameterIndex = TryParseInteger();
                    if (parameterIndex == null)
                        throw new FormatException("Grave accent not followed by an integer (index " + index + "): " + data);
                    ret1.ParameterIndex = parameterIndex.Value;
                    return ret1;
                }

                var ret2 = ParseBaseType();
                while (index != data.Length)
                {
                    if (data[index] == '[')
                    {
                        ++index;
                        if (index == data.Length)
                            throw new FormatException("Unexpected end of input after array designation ('['): " + data);

                        ret2 = ParseArraySpec(ret2);
                    }
                    else if (data[index] == '*')
                    {
                        ++index;
                        ret2 = new Pointer { ElementType = ret2 };
                    }
                    else
                    {
                        break;
                    }
                }

                return ret2;
            }

            private TypeRef ParseBaseType()
            {
                // BaseType = Id ('.' Id)*

                var ret = new TypeReference();
                ret.Context.Add(ParseId());
                while (index != data.Length && data[index] == '.')
                {
                    ++index;
                    if (index == data.Length)
                        throw new FormatException("Unexpected end of input after '.': " + data);
                    ret.Context.Add(ParseId());
                }

                // The last Id is the actual type reference.
                ret.Name = ret.Context.Last().Name;
                ret.GenericArguments.AddRange(ret.Context.Last().GenericArguments);
                ret.Context.RemoveAt(ret.Context.Count - 1);
                return ret;
            }

            private NamespaceOrConcreteType ParseId()
            {
                // Id = NAME ('{' GenericArgumentList '}')?

                var ret = new NamespaceOrConcreteType { Name = ParseName() };
                if (index != data.Length && data[index] == '{')
                {
                    ++index;
                    if (index == data.Length)
                        throw new FormatException("Unexpected end of input after '{': " + data);
                    ParseGenericArgumentList(ret);
                }

                return ret;
            }

            private void ParseGenericArgumentList(NamespaceOrConcreteType ret)
            {
                // GenericArgumentList = Type (',' Type)*

                while (true)
                {
                    ret.GenericArguments.Add(ParseType());
                    if (index == data.Length)
                        throw new FormatException("Unexpected end of input in generic argument list: " + data);
                    if (data[index] == '}')
                    {
                        ++index;
                        return;
                    }
                    else if (data[index] == ',')
                    {
                        ++index;
                    }
                }
            }

            private static readonly Regex nameRegex = new Regex(@"[A-Za-z_\$@\?][A-Za-z0-9_\$@\?]+");

            private string ParseName()
            {
                string ret = nameRegex.Match(data, index).Value;
                index += ret.Length;
                return ret;
            }

            private Array ParseArraySpec(TypeRef elementType)
            {
                // ArraySpec = '[' ArrayBounds (',' ArrayBounds) ']'

                var ret = new Array { ElementType = elementType };
                var closeBracketIndex = data.IndexOf(']', index);
                if (closeBracketIndex == -1)
                    throw new FormatException("Array does not have an end (']'): " + data);
                if (closeBracketIndex == index)
                {
                    // Special case: the empty array index "[]" is defined as a single-rank, zero-indexed array.
                    ++index;
                    ret.LowerBoundsAndSizes.Add(new Tuple<int?, int?>(0, null));
                    return ret;
                }

                while (true)
                {
                    if (index == closeBracketIndex)
                    {
                        ++index;
                        ret.LowerBoundsAndSizes.Add(new Tuple<int?, int?>(null, null));
                        return ret;
                    }

                    ret.LowerBoundsAndSizes.Add(ParseArrayBounds());
                    if (index == closeBracketIndex)
                    {
                        return ret;
                    }

                    if (index == data.Length)
                        throw new FormatException("Unexpected end of input while parsing array bounds: " + data);
                    if (data[index] != ',')
                        throw new FormatException("Could not parse array bounds; comma expected at index " + index + ": " + data);
                    ++index;
                }
            }

            private Tuple<int?, int?> ParseArrayBounds()
            {
                // ArrayBounds = |
                //               INTEGER? ':' INTEGER?

                if (data[index] == ',')
                {
                    return new Tuple<int?, int?>(null, null);
                }

                int? lowerBound;
                if (data[index] == ':')
                {
                    ++index;
                    lowerBound = null;
                }
                else
                {
                    lowerBound = TryParseInteger();
                    if (lowerBound == null)
                        throw new FormatException("Could not parse lower array bound at index " + index + ": " + data);
                    if (data[index] != ':')
                        throw new FormatException("Lower array bound not followed by colon at index " + index + ": " + data);
                    ++index;
                }

                if (data[index] == ',' || data[index] == ']')
                {
                    return new Tuple<int?, int?>(lowerBound, null);
                }

                int? size = TryParseInteger();
                if (size == null)
                    throw new FormatException("Could not parse array size at index " + index + ": " + data);
                return new Tuple<int?, int?>(lowerBound, size);
            }

            private int? TryParseInteger()
            {
                var numericString = new string(data.Skip(index).TakeWhile(x => "0123456789".Contains(x)).ToArray());
                int result;
                if (!int.TryParse(numericString, out result))
                    return null;
                index += numericString.Length;
                return result;
            }
        }
    }
}
