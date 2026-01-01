public interface IInteractAction
{
    string PromptText { get; }
    void Execute(PlayerInteractor player);
}
