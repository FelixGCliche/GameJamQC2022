using Runtime.Enum;
using Runtime.Event;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Components.Inventory
{
  public class InventoryItem: VisualElement
  {
    public new class UxmlFactory : UxmlFactory<InventoryItem, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
      protected UxmlEnumAttributeDescription<CraftingComponentType> inventoryItemType =
        new()
        {
          name = "ItemType"
        };
      
      public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
      {
        base.Init(ve, bag, cc);

        ((InventoryItem)ve).ComponentType = inventoryItemType.GetValueFromBag(bag, cc);
      }
    }
    
    private readonly VisualElement icon;
    private readonly Label countLabel;

    private CraftingComponentType componentType;
    public CraftingComponentType ComponentType
    {
      get => componentType;
      set
      {
        componentType = value;
      }
    }

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
      styleSheets.Add(Resources.Load<StyleSheet>("Styles/Inventory"));
      AddToClassList("inventory-item");

      icon = new VisualElement();
      icon.name = "inventory-item-icon";
      icon.AddToClassList(icon.name);
      hierarchy.Add(icon);

      countLabel = new Label();
      countLabel.name = "inventory-item-counter";
      icon.AddToClassList(countLabel.name);
      Count = 0;
      hierarchy.Add(countLabel);
    }
  }
}