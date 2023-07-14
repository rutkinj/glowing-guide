using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    // this class lives on a character object
    // configured in the inspector
    // accessed by base stats (?)
    // access point for attribute progression SO (?)
    // will have to be saveable
  public class BaseAttributes : MonoBehaviour
  {
    [SerializeField] AttributeLevel[] attributeTable;
    Dictionary<E_Attribute, int> attributeDictionary;

    [System.Serializable]
    class AttributeLevel
    {
      [SerializeField] E_Attribute name;
      [SerializeField] int level;
    }

    public int GetAttributeLevel(E_Attribute attribute){
        return 1;
    }


  }
}
