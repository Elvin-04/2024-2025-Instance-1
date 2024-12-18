using System.Collections.Generic;
using UnityEngine;

    public class ComponentPoolAudio<T> where T : Component
    {
        private readonly Transform _parent;
        private readonly List<T> _pool;
        private readonly GameObject _prefab;

        public ComponentPoolAudio(GameObject prefab, Transform parent, int initialSize)
        {
            _prefab = prefab;
            _parent = parent;
            _pool = new List<T>();

            // Pré-allocation des composants
            for (var i = 0; i < initialSize; i++)
            {
                var obj = Object.Instantiate(_prefab, _parent);
                var component = obj.GetComponent<T>();
                obj.SetActive(false); // Garde l'objet inactif
                _pool.Add(component);
            }
        }

        public T Get()
        {
            // Rechercher un objet inactif dans la liste
            foreach (var component in _pool)
                if (!component.gameObject.activeSelf)
                {
                    component.gameObject.SetActive(true); // Activer l'objet
                    return component;
                }

            // Si aucun objet inactif n'est trouvé, retourner null
            Debug.LogWarning("No available object in the pool.");
            return null;
        }

        public T Get(Transform newTransform)
        {
            // Rechercher un objet inactif dans la liste
            foreach (var component in _pool)
                if (!component.gameObject.activeSelf)
                {
                    component.gameObject.SetActive(true); // Activer l'objet
                    component.transform.position = newTransform.position;
                    return component;
                }

            // Si aucun objet inactif n'est trouvé, retourner null
            Debug.LogWarning("No available object in the pool.");
            return null;
        }

        public void Release(GameObject obj)
        {
            var component = obj.GetComponent<T>();
            if (!component)
            {
                Debug.LogWarning("The object to release does not contain the correct component.");
                return;
            }

            // Désactiver l'objet et s'assurer qu'il appartient bien au pool
            if (_pool.Contains(component))
                obj.SetActive(false);
            else
                Debug.LogWarning("The object does not belong to this pool.");
        }

        public IEnumerable<T> GetAll()
        {
            foreach (var component in _pool) yield return component;
        }

        public void AddToPool(int additionalSize)
        {
            // Ajouter des objets supplémentaires au pool
            for (var i = 0; i < additionalSize; i++)
            {
                var obj = Object.Instantiate(_prefab, _parent);
                var component = obj.GetComponent<T>();
                obj.SetActive(false);
                _pool.Add(component);
            }
        }
    }