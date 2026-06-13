using System.Collections;
using System.Collections.Generic;
using Games.TileMatch.Level.Data;
using UnityEngine;

namespace Project.Manager
{
  
    public class PoolManager<T> where T: Component
    {
        private readonly List<T> _poolActive = new();
        private readonly List<T> _poolInactive = new();

        public T GetPool(T prefab, Vector2 position)
        {
            if (_poolInactive.Count <= 0)
            {
                var creObj = Object.Instantiate(prefab, position, Quaternion.identity);
                creObj.gameObject.SetActive(false);
                _poolInactive.Add(creObj);
            }
            
            Debug.Log(_poolInactive.Count);
            var lastIndex = _poolInactive.Count - 1;
            T obj = _poolInactive[lastIndex];
            obj.transform.position = position;
            _poolInactive.RemoveAt(lastIndex);
            _poolActive.Add(obj);
            obj.gameObject.SetActive(true);
            return obj;
            
        }


        public void Release(T obj)
        { 
            obj.gameObject.SetActive(false);
            _poolActive.Remove(obj);
            _poolInactive.Add(obj);
           
            
        }
        
    }

   
}
