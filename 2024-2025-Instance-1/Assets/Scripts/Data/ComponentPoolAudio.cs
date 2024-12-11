using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class ComponentPoolAudio<T> where T : Component
    {
        private readonly Transform _parent;
        private readonly Queue<T> _pool;
        private readonly GameObject _prefab;

        public ComponentPoolAudio(GameObject prefab, Transform parent, int initialSize)
        {
            _prefab = prefab;
            _parent = parent;
            _pool = new Queue<T>();

            // Pré-allocation des composants
            for (var i = 0; i < initialSize; i++)
            {
                var obj = Object.Instantiate(_prefab, _parent);
                var component = obj.GetComponent<T>();
                obj.SetActive(false); // Garde l'objet inactif
                _pool.Enqueue(component);
            }
        }

        public T Get()
        {
            // Si le pool est vide, retourner null
            if (_pool.Count == 0)
                return null;

            var component = _pool.Dequeue();
            component.gameObject.SetActive(true); // Activer le composant
            return component;
        }

        public void Release(GameObject obj)
        {
            obj.SetActive(false);
            _pool.Enqueue(obj.GetComponent<T>()); // Retourner le composant dans le pool
        }

        public IEnumerable<T> GetAll()
        {
            foreach (var component in _pool) yield return component;
        }
    }
}