using System;

namespace AssetStudioCLI.Options
{
    internal static class OptionExtensions
    {
        public static Action<string, string, string, HelpGroups, bool> OptionGrouping = (name, desc, example, group, isFlag) => { };
    }

    internal class GroupedOption<T> : Option<T>
    {
        public GroupedOption(T optionDefaultValue, string optionName, string optionDescription, string optionExample, HelpGroups optionHelpGroup, bool isFlag = false) : base(optionDefaultValue, optionName, optionDescription, optionExample, optionHelpGroup, isFlag)
        {
            OptionExtensions.OptionGrouping(optionName, optionDescription, optionExample, optionHelpGroup, isFlag);
        }
    }
}
