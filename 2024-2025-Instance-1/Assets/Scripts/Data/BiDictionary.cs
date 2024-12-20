using System;
using System.Collections;
using System.Collections.Generic;

    [Serializable]
    public class BiDictionary<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>
    {
        [NonSerialized] private bool ambiguous;

        private Dictionary<T1, T2> forward = new();
        private Dictionary<T2, T1> reverse = new();

        public class AmbiguousReferenceExeception : Exception
        {
        }

        #region Constructor

        public BiDictionary()
        {
            ambiguous = typeof(T1) == typeof(T2);
        }

        public T2 this[T1 key]
        {
            get
            {
                try
                {
                    if (ambiguous)
                        throw new AmbiguousReferenceExeception();
                    return forward[key];
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public T1 this[T2 key]
        {
            get
            {
                try
                {
                    if (ambiguous)
                        throw new AmbiguousReferenceExeception();
                    return reverse[key];
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        #endregion

        #region Contains

        public bool Contains(T1 t1)
        {
            if (ambiguous)
                throw new AmbiguousReferenceExeception();
            return ContainsForward(t1);
        }

        public bool Contains(T2 t2)
        {
            if (ambiguous)
                throw new AmbiguousReferenceExeception();
            return ContainsReverse(t2);
        }

        public bool ContainsForward(T1 t1)
        {
            return forward.ContainsKey(t1);
        }

        public bool ContainsReverse(T2 t2)
        {
            return reverse.ContainsKey(t2);
        }

        #endregion

        #region Add

        public void Add(T1 t1, T2 t2)
        {
            if (ambiguous)
                throw new AmbiguousReferenceExeception();

            AddForward(t1, t2);
        }

        public void Add(T2 t2, T1 t1)
        {
            if (ambiguous)
                throw new AmbiguousReferenceExeception();

            AddForward(t1, t2);
        }

        public void AddForward(T1 t1, T2 t2)
        {
            try
            {
                forward.Add(t1, t2);
                reverse.Add(t2, t1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddReverse(T2 t2, T1 t1)
        {
            AddForward(t1, t2);
        }

        #endregion

        #region Remove

        public void Remove(T1 t1)
        {
            if (ambiguous)
                throw new AmbiguousReferenceExeception();
            RemoveForward(t1);
        }

        public void Remove(T2 t2)
        {
            if (ambiguous)
                throw new AmbiguousReferenceExeception();
            RemoveReverse(t2);
        }

        public void RemoveForward(T1 t1)
        {
            try
            {
                var reverseKey = forward[t1];
                forward.Remove(t1);
                reverse.Remove(reverseKey);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveReverse(T2 t2)
        {
            try
            {
                var forwardKey = reverse[t2];
                forward.Remove(forwardKey);
                reverse.Remove(t2);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region GetEnumerator

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            return forward.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return forward.GetEnumerator();
        }

        #endregion

        #region enumerate

        public IEnumerable<KeyValuePair<T1, T2>> Forward()
        {
            return forward;
        }

        public IEnumerable<KeyValuePair<T2, T1>> Reverse()
        {
            return reverse;
        }

        #endregion
    }