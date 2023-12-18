namespace TheRemembererDiscordBot
{
    public class ChanceCommand : Command
    {
        public ChanceCommand()
        {
            Program.Commands.Add(this);
        }

        public override string CommandName() => "chance";
        public override string CommandDescription() => "Enter a decimal percentage 0-100 and randomize a number to see if it falls into the set range.";
        public override object? CommandAction(List<object> args)
        {
            if (args[0] is decimal chance)
            {
                int result = Random.Shared.Next(101);
                if (result < Math.Floor(chance))
                {
                    return "Success " + result;
                }
                else if (result > Math.Floor(chance))
                {
                    return "Failure " + result;
                }
                else
                {
                    string decimalsString = chance.ToString().Split('.')[1];
                    string resultDecimal = result.ToString() + ".";
                    int nextDecimal = Random.Shared.Next(10);
                    int i;
                    for (i = 0; nextDecimal == (byte)decimalsString[i] && i < decimalsString.Length; i++)
                    {
                        resultDecimal += nextDecimal.ToString();
                        nextDecimal = Random.Shared.Next(10);
                    }
                    resultDecimal += nextDecimal.ToString();
                    if (i < decimalsString.Length)
                    {
                        if (nextDecimal <= (byte)decimalsString[i])
                        {
                            return "Success " + resultDecimal;
                        }
                        else if (nextDecimal > (byte)decimalsString[i])
                        {
                            return "Failure " + resultDecimal;
                        }
                    }
                }
            }
            return null;
        }
    }
}
