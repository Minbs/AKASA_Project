using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    List<Transform> children;

    // Start is called before the first frame update
    void Start()
    {
        children = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateChildren();
    }

    public void UpdateChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == children.Count)
            {
                children.Add(null);
            }

            var child = transform.GetChild(i);

            if (child != children[i])
            {
                children[i] = child;
            }
        }

        children.RemoveRange(transform.childCount, children.Count - transform.childCount);
    }

    public void InsertCharacter(Transform character, int index)
    {
        children.Add(character);
        character.SetSiblingIndex(index);
        UpdateChildren();
    }

    public int GetIndexByPosition(Transform character, int skipIndex = -1)
    {
        int result = 0;

        for (int i = 0; i < children.Count; i++)
        {
            if (character.position.x < children[i].position.x)
            {
                break;
            }
            else if (skipIndex != 0)
            {
                result++;
            }
        }

        return result;
    }

    public void SwapCharacter(int index1, int index2)
    {
        Central.SwapCharacters(children[index1], children[index2]);
        UpdateChildren();
    }
}
