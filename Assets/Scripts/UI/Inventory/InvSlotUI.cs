using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventory
{
[SerializeField] ItemIcon icon = null;

  public class InvSlotUI : MonoBehaviour
  {
    public int MaxAcceptable(Sprite item){
        if (GetItem() == null){
            return int.MaxValue;
        }
        return 0;
    }

    public Sprite GetItem(){
        return null;
    }

    public int GetNumber(){
        return 1;
    }
  }
}
