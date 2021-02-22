using System.Text;

namespace GLWriter.CSharp
{
    public class EnumValue
    {
        public EnumValue(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }

        public string ToLine(string groupName)
        {
            var builder = new StringBuilder();

            var a = 3;
            if (groupName == "TextureTarget")
            {
                a = 4;
            }

            var nameWords = Name.Split("_");
            for (var i = 0; i < nameWords.Length; i++)
            {
                var nameWord = nameWords[i];

                if (i != 0 || nameWord != "GL")
                {
                    for (var j = 0; j < nameWord.Length; j++)
                    {
                        var character = nameWord[j];

                        if (j == 0)
                        {
                            if (builder.Length == 0 && char.IsNumber(character))
                            {
                                builder.Append(groupName);
                            }

                            builder.Append(character);
                        }
                        else
                        {
                            builder.Append(char.ToLower(character));
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(Value))
            {
                builder.Append(" = ");
                builder.Append(Value);
            }

            builder.Append(",");

            return builder.ToString();
        }
    }
}
