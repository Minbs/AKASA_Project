using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diction
{
    public class PublicDictionary
    {
        [System.Serializable]
        public class MinionsData<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
        {
            [SerializeField] List<TKey> keys;
            [SerializeField] List<TValue> values;

            public void OnAfterDeserialize()
            {
                this.Clear();

                if (keys.Count != values.Count)
                    throw new System.NotImplementedException();

                for (int i = 0; i < keys.Count; i++)
                    this.Add(keys[i], values[i]);
            }

            public void OnBeforeSerialize()
            {
                keys.Clear();
                values.Clear();
                foreach (KeyValuePair<TKey, TValue> pair in this)
                {
                    keys.Add(pair.Key);
                    values.Add(pair.Value);
                }
            }
        }

    }

}