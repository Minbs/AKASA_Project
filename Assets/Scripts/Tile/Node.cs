using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Node
{
    public int row;
    public int column;

    public Node(int row, int column)
    {
        this.row = row;
        this.column = column;
    }
    

    public static Node operator +(Node p1, Node p2)
    {
        return new Node(p1.row + p2.row, p1.column + p2.column);
    }

    public static Node operator-(Node p1, Node p2)
    {
        return new Node(p1.row - p2.row, p1.column - p2.column);
    }

    public static bool operator==(Node p1, Node p2)
    {
        return ((p1.row == p2.row) && (p1.column == p2.column));
    }

    public static bool operator !=(Node p1, Node p2)
    {
        return ((p1.row != p2.row) || (p1.column != p2.column));
    }

    public override string ToString()
    {
        return string.Format("({0} , {1})", row, column);
    }
}
