using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonScript<T>
{
    List<T> Data;
    public List<T> ToList() { return Data; }

    public JsonScript(List<T> target)
    {
        Data = target;
    }
    //public static List<T> FromJson<T> (string json)
    //{

    //    Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
    //    return wrapper.items;
    //}


    //public static string ToJson<T> (List<T> list)
    //{
    //    Wrapper<T> wrapper = new Wrapper<T>();
    //    wrapper.items = list;
    //    return JsonUtility.ToJson(wrapper);
    //}

    //public class Wrapper<T>
    //{
    //    public List<T> items;
    //}
}
