namespace AssetStudioCLI.Options
{
    internal class Option<T>
    {
        public string Name { get; }
        public string Description { get; }
        public string Example { get; }
        public T Value { get; set; }
        public T DefaultValue { get; }
        public HelpGroups HelpGroup { get; }
        public bool IsFlag { get; }

        public Option(T optionDefaultValue, string optionName, string optionDescription, string optionExample, HelpGroups optionHelpGroup, bool isFlag)
        {
            Name = optionName;
            Description = optionDescription;
            Example = optionExample;
            DefaultValue = optionDefaultValue;
            Value = DefaultValue;
            HelpGroup = optionHelpGroup;
            IsFlag = isFlag;
        }

        public override string ToString()
        {
            return Value != null ? Value.ToString() : string.Empty;
        }
    }
}
