namespace TheRemembererDiscordBot.CommandComponents
{
    public class CommandArgument
    {
        private readonly string ArgumentName;
        public readonly List<object> ExpectedInputs;
        public readonly ArgType ArgumentType;
        public readonly bool MayBeSkipped;

        public enum ArgType
        {
            PositiveIntegerRange,
            PositiveIntegerRangeOrText,
            PositiveIntegerRangeOrTextConcat,
            Text,
            TextConcat,
            CustomText,
            CustomTextConcat
        }

        public CommandArgument(string argumentName, List<object> expectedInputs, bool concat = false, bool mayBeSkipped = false, bool forceNotCustom = false)
        {
            //Assumptions:

            //First, inputs will be any of these things (and only these things):
            //  {int, int}            NOT concat : Integer range
            //	{int, int, string...} NOT concat : Integer range OR Expected single word
            //	{int, int, string...} IS  concat : Integer range OR Expected word or phrase, pulled from the user arguments up to the next integer or the end of the arguments
            //	{string...}           NOT concat : Expected single word
            //	{string...}           IS  concat : Expected word or phrase, pulled from the user arguments up to the next integer or the end of the arguments
            //	{}                    NOT concat : Any single word
            //	{}                    IS  concat : Any word or phrase, pulled from the user arguments up to the next integer or the end of the arguments

            //the argumentName parameter is the display name that will be shown when necessary. It should be capitalized correctly.
            //the first InputArgument that is set to mayBeSkipped in a list of InputArgument makes all subsequent InputArgument also mayBeSkipped.
            //If any option would be better off handling the arguments by itself, the list of InputArguments should be inputted blank when creating the option.
            //All input strings that are to be compared with expected strings will be case-insensitive.
            //All input strings that are NOT to be compared with expected strings will be case-sensitive.
            //All options that use this should work on the assumption that the incoming arguments have been pre-processed: If syntax is wrong when input is received,
            //it will be caught there. If not, then all variables are ready to use.

            ArgumentName = argumentName;
            ExpectedInputs = expectedInputs;
            ArgumentType = concat switch
            {
                false when ExpectedInputs.Count == 2 && ExpectedInputs[0] is int && ExpectedInputs[1] is int => ArgType.PositiveIntegerRange,
                false when ExpectedInputs.Count > 0 && ExpectedInputs[0] is int => ArgType.PositiveIntegerRangeOrText,
                true when ExpectedInputs.Count > 0 && ExpectedInputs[0] is int => ArgType.PositiveIntegerRangeOrTextConcat,
                false when (ExpectedInputs.Count > 0 || forceNotCustom) && ExpectedInputs[0] is not int => ArgType.Text,
                true when (ExpectedInputs.Count > 0 || forceNotCustom) && ExpectedInputs[0] is not int => ArgType.TextConcat,
                false when ExpectedInputs.Count == 0 => ArgType.CustomText,
                true when ExpectedInputs.Count == 0 => ArgType.CustomTextConcat,
                _ => ArgType.CustomText
            };
            MayBeSkipped = mayBeSkipped;
        }

        public override string ToString() => ArgumentName;
    }
}
