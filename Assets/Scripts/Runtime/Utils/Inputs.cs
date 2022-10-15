namespace Runtime.Utils
{
  public static class Inputs
  {
    private static InputActions actions;
    public static InputActions Actions => actions ??= new InputActions();

  }
}