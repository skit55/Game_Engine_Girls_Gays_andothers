public interface IAction
{
    string PromptText { get; }
    void Execute(PlayerTriggerSensor player);
}
