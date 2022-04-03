using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Central : MonoBehaviour
{
    public Transform invisableCharacter;
    List<Panel> panels;

    Panel workingPanel;
    int oriIndex;

    // Start is called before the first frame update
    void Start()
    {
        panels = new List<Panel>();
        var pans = transform.GetComponentsInChildren<Panel>();

        for (int i = 0; i < pans.Length; i++)
        {
            panels.Add(pans[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public static void SwapCharacters(Transform sour, Transform dest)
    {
        Transform sourParent = sour.parent;
        Transform destParent = dest.parent;

        int sourIndex = sour.GetSiblingIndex();
        int destIndex = dest.GetSiblingIndex();

        sour.SetParent(destParent);
        sour.SetSiblingIndex(destIndex);

        dest.SetParent(sourParent);
        dest.SetSiblingIndex(sourIndex);
    }

    void SwapCharacterHierarchy(Transform sour, Transform dest)
    {
        SwapCharacters(sour, dest);

        panels.ForEach(t => t.UpdateChildren());
    }

    bool ContainPos(RectTransform rect, Vector2 pos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rect, pos);
    }

    void BeginDrag(Transform character)
    {
        //Debug.Log("BeginDrag: " + character.name);

        workingPanel = panels.Find(t => ContainPos(t.transform as RectTransform, character.position));
        oriIndex = character.GetSiblingIndex();

        SwapCharacterHierarchy(invisableCharacter, character);
    }

    void Drag(Transform character)
    {
        //Debug.Log("Drag: " + character.name);

        var whichPanelCharacter = panels.Find(t => ContainPos(t.transform as RectTransform, character.position));

        if (whichPanelCharacter == null)
        {
            bool updateChildren = transform != invisableCharacter.parent;

            invisableCharacter.SetParent(transform);

            if (updateChildren)
            {
                panels.ForEach(t => t.UpdateChildren());
            }
        }
        else
        {
            bool insert = invisableCharacter.parent;

            if (insert)
            {
                int index = whichPanelCharacter.GetIndexByPosition(character);

                invisableCharacter.SetParent(whichPanelCharacter.transform);
                whichPanelCharacter.InsertCharacter(invisableCharacter, index);
            }
            else
            {
                int invisableCharacterIndex = invisableCharacter.GetSiblingIndex();
                int targetIndex = whichPanelCharacter.GetIndexByPosition(character, invisableCharacterIndex);

                if (invisableCharacterIndex != targetIndex)
                {
                    whichPanelCharacter.SwapCharacter(invisableCharacterIndex, targetIndex);
                }
            }
        }

    }

    void EndDrag(Transform character)
    {
        //Debug.Log("EndDrag: " + character.name);

        if (invisableCharacter.parent == transform)
        {
            workingPanel.InsertCharacter(character, oriIndex);
            workingPanel = null;
            oriIndex = -1;
        }
        else
        {
            SwapCharacterHierarchy(invisableCharacter, character);
        }
    }
}
