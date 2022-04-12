using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    public Node node = new Node();

    public int height;

    public bool isBlock;

    public bool isOnUnit = false;
    //타일 타입 추가


    //길찾기 알고리즘

    public int G { get; set; }
    public int H { get; set; }
    public Tile parentTile { get; set; }

    Vector3 center;
    Vector3 size;
    Renderer renderer;
    List<Color> colors = new List<Color>();
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        center = renderer.bounds.center;
        size = renderer.bounds.size;

        for (int i = 0; i < renderer.materials.Length; i++)
        {
            colors.Add(renderer.materials[i].color);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsCanSetUnit(MinionClass minionClass)
    {

        if (!isBlock && !isOnUnit)
        {
            if (minionClass == MinionClass.Buster || minionClass == MinionClass.Paladin || minionClass == MinionClass.Guardian || minionClass == MinionClass.Assassin)
            {
                if (height == 0)
                    return true;
                else
                    return false;
            }
            else if (minionClass == MinionClass.Chaser || minionClass == MinionClass.Mage || minionClass == MinionClass.TacticalSupport)
            {
                if (height != 0)
                    return true;
                else
                    return false;
            }
            else
            {
                Debug.Log(minionClass);
            }
        }


        return false;
    }


    public void canUnitSetTile(MinionClass minionClass, bool isActive)
    {

        for (int i = 0; i < renderer.materials.Length; i++)
        {
            renderer.materials[i].color = colors[i];
        }

        if (!isActive)
            return;

        for (int i = 0; i < renderer.materials.Length; i++)
        {
            if (IsCanSetUnit(minionClass))
                renderer.materials[i].color = Color.Lerp(colors[i], new Color(0, 1, 0), 0.3f);
            else
                renderer.materials[i].color = colors[i];
        }



    }

    public bool onTile(Transform enemy)
    {
        if (enemy.transform.position.x > transform.position.x - size.x / 2 &&
            enemy.transform.position.x < transform.position.x + size.x / 2 &&
            enemy.transform.position.z > transform.position.z - size.z / 2 &&
            enemy.transform.position.z < transform.position.z + size.z / 2)
        {
            //renderer.material.color = new Color(255, 0, 0);
            return true;
        }
        else
        {
            return false;

        }
    }

    public void on(bool b)
    {
        if (b)
            renderer.material.color = new Color(255, 0, 0);
        else
        {
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                renderer.materials[i].color = colors[i];
            }
        }
    }

    public void SetTileInfo(string[] s)
    {
        node.row = int.Parse(s[1]);
        node.column = int.Parse(s[2]);
        height = int.Parse(s[3]);
        isBlock = (s[4] == "BLOCK") ? true : false;
    }
}
