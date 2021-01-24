using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLWriter
{
    public class DefinitionSet
    {
        private List<FunctionDefinition> _functions = new List<FunctionDefinition>();
        private List<DelegateDefinition> _delegates = new List<DelegateDefinition>();
        private List<FieldDefinition> _fields = new List<FieldDefinition>();

        private Dictionary<string, FieldDefinition> _fieldByName = new Dictionary<string, FieldDefinition>();
        private Dictionary<string, DelegateDefinition> _delegateByName = new Dictionary<string, DelegateDefinition>();
        private Dictionary<string, string> _oldByNewDelegates = new Dictionary<string, string>();

        public void AddDefinition(FunctionDefinition definition) => _functions.Add(definition);
        public void AddDefinition(DelegateDefinition definition) => _delegates.Add(definition);
        public void AddDefinition(FieldDefinition definition) => _fields.Add(definition);

        public void Process()
        {
            var functions = new List<FunctionDefinition>();
            var delegates = new List<DelegateDefinition>();
            var fields = new List<FieldDefinition>();
            var delegateNames = new HashSet<string>();
            
            foreach (var definition in _functions.OrderBy(f => f.FieldName))
            {
                functions.Add(definition);
            }

            foreach (var definition in _delegates.OrderBy(d => d.DelegateMaskName))
            {
                if (!delegateNames.Contains(definition.DelegateMaskName))
                {
                    delegateNames.Add(definition.DelegateMaskName);
                    delegates.Add(definition);
                }

                _oldByNewDelegates.Add(definition.DelegateName, definition.DelegateMaskName);
                _delegateByName.Add(definition.DelegateName, definition);
            }

            foreach (var definition in _fields.OrderBy(f => f.FieldName))
            {
                fields.Add(definition);
                _fieldByName.Add(definition.FieldName, definition);
            }

            _functions = functions;
            _delegates = delegates;
            _fields = fields;
        }

        public IEnumerable<string> GetFieldDefinitions()
        {
            foreach (var definition in _fields)
            {
                var builder = new StringBuilder();
                builder.Append("private static ");
                builder.Append(_oldByNewDelegates[definition.DelegateName]);
                builder.Append(" ");
                builder.Append(definition.FieldName);
                builder.Append(";");

                yield return builder.ToString();
            }

            yield return "";
        }

        public IEnumerable<string> GetLoadDefinitions()
        {
            foreach (var definition in _fields)
            {
                var builder = new StringBuilder();
                builder.Append(definition.FieldName);
                builder.Append(" = ");
                builder.Append("GetFunctionDelegate");
                builder.Append("<");
                builder.Append(_oldByNewDelegates[definition.DelegateName]);
                builder.Append(">");
                builder.Append("(\"");
                builder.Append(definition.FieldName.Substring(1));
                builder.Append("\");");

                yield return builder.ToString();
            }
        }

        public IEnumerable<string> GetFunctionDefinitions()
        {
            foreach (var definition in _functions)
            {
                var builder = new StringBuilder();
                builder.Append("public static ");
                builder.Append(DataTypeExtensions.ToText(definition.ReturnType));
                builder.Append(" ");
                builder.Append(definition.FunctionName);
                builder.Append("(");

                for (var i = 0; i < definition.Parameters.Count; i++)
                {
                    if (i > 0)
                    {
                        builder.Append(", ");
                    }

                    var parameter = definition.Parameters[i];
                    var parameterType = DataTypeExtensions.ToText(parameter.DataType);
                    builder.Append(parameterType);
                    builder.Append(" ");
                    builder.Append(parameter.Name);
                }

                builder.Append(") => ");
                builder.Append(definition.FieldName);
                builder.Append("(");

                var fieldDefinition = _fieldByName[definition.FieldName];
                var delegateDefinition = _delegateByName[fieldDefinition.DelegateName];

                for (var i = 0; i < definition.Parameters.Count; i++)
                {
                    if (i > 0)
                    {
                        builder.Append(", ");
                    }

                    var parameter = definition.Parameters[i];
                    var parameterType = delegateDefinition.Parameters[i];

                    if (parameter.DataType == DataTypes.INTPTR && parameterType == DataTypes.VOIDPTR)
                    {
                        builder.Append(parameter.Name + ".ToPointer()");
                    }
                    else if ((parameter.DataType == DataTypes.INTEGER || parameter.DataType == DataTypes.LONG) && parameterType == DataTypes.INTPTR)
                    {
                        builder.Append("new IntPtr(" + parameter.Name + ")");
                    }
                    else if (parameter.DataType == DataTypes.UINT && parameterType == DataTypes.UINTPTR)
                    {
                        builder.Append("&" + parameter.Name);
                    }
                    else
                    {
                        builder.Append(parameter.Name);
                    }
                }

                builder.Append(");");
                yield return builder.ToString();
            }

            yield return "";
        }

        public IEnumerable<string> GetDelegateDefinitions()
        {
            foreach (var definition in _delegates)
            {
                var builder = new StringBuilder();
                builder.Append("private delegate ");
                builder.Append(DataTypeExtensions.ToText(definition.ReturnType));
                builder.Append(" ");
                builder.Append(definition.DelegateMaskName);
                builder.Append("(");

                for (var i = 0; i < definition.Parameters.Count; i++)
                {
                    var parameter = definition.Parameters[i];
                    var parameterType = DataTypeExtensions.ToText(parameter);

                    if (i > 0)
                    {
                        builder.Append(", ");
                    }

                    builder.Append(parameterType);
                    builder.Append(" ");
                    builder.Append("v" + i);
                }

                builder.Append(");");
                yield return builder.ToString();
            }
        }
    }
}
