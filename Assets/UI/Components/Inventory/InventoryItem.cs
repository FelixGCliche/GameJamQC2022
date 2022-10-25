using UnityEngine.UIElements;

namespace UI.Components.Inventory
{
  public class InventoryItem: VisualElement
  {
    public new class UxmlFactory : UxmlFactory<InventoryItem, UxmlTraits> { }
    
    private readonly VisualElement icon;
    private readonly Label countLabel;

    private int count;
    public int Count
    {
      get => count;
      set
      {
        count = value;
        countLabel.text = count.ToString();
      }
    }

    public InventoryItem()
    {
      styleSheets.Add();
      AddToClassList("inventory-item");
      
      icon = new VisualElement();
      icon.name = "inventory-item-icon";
      icon.AddToClassList(icon.name);
      hierarchy.Add(icon);

      countLabel = new Label();
      countLabel.name = "inventory-item-count";
      icon.AddToClassList(countLabel.name);
      Count = 0;
      hierarchy.Add(countLabel);
      
      RegisterCallback<ClickEvent>(OnClick);
    }

    private void OnClick(ClickEvent e)
    {
      Count++;
    }
  }
}