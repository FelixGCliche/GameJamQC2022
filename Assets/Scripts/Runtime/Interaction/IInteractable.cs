namespace Runtime.Interaction.Interactable
{
  public interface IInteractable
  {
    void OnInteractStarted();
    void OnInteractFinished();
    void OnInteractCanceled();
  }
}