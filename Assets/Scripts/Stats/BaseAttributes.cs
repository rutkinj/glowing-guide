using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    // this class lives on a character object
    // configured in the inspector
    // accessed by base stats (?)
    // access point for attribute progression SO (?)
    // will have to be saveable (?)
    // find way to make this a dictionary instead of a list
  public class BaseAttributes : MonoBehaviour
  {
    [SerializeField] AttributeLevel[] attributeTable;
    // Dictionary<E_Attribute, int> attributeDictionary;

    [System.Serializable]
    class AttributeLevel
    {
      public E_Attribute name;
      public int level;
    }

    public int GetAttributeLevel(E_Attribute attribute){
        foreach(AttributeLevel att in attributeTable){
            if (att.name == attribute){
                return att.level;
            }
        }
        return 0;
    }


  }
}
