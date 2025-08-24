

namespace DoverFueling.UI.Strategies
{
    public class UserInputStrategy
    {
        private readonly Dictionary<string, Action> _commandStrategies;

        public UserInputStrategy()
        {
            _commandStrategies = new Dictionary<string, Action>();
        }

        public void RegisterCommand(string command, Action action)
        {
            _commandStrategies[command.ToLower()] = action;
        }

        public void RegisterCommands(string[] commands, Action action)
        {
            foreach (var command in commands)
            {
                RegisterCommand(command, action);
            }
        }

        public Action? GetCommandAction(string? command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                return null; // Empty command - caller can handle this case
            }

            _commandStrategies.TryGetValue(command.ToLower(), out Action? action);
            return action;
        }

     
        //public bool IsCommandRegistered(string? command)
        //{
        //    if (string.IsNullOrWhiteSpace(command))
        //    {
        //        return false;
        //    }

        //    return _commandStrategies.ContainsKey(command.ToLower());
        //}

        //public IEnumerable<string> GetAllCommands()
        //{
        //    return _commandStrategies.Keys;
        //}
    }
}
