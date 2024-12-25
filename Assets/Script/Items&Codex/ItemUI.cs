using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUI : MonoBehaviour
{
    public TMP_Text Quantity;
    public LootBox.LootType LootType;

    public void SetItem(){
        Quantity.text = Player.Instance.Inventory.GetItemCount(LootType).ToString();
    }

}
