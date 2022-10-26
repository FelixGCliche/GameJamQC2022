namespace Runtime.Interaction
{
  public interface IInteractable
  {
    void OnInteractStarted();
    void OnInteractFinished();
    void OnInteractCanceled();
  }
}