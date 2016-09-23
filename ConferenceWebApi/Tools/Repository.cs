using System.Collections.Generic;

namespace ConferenceWebApi.Tools
{
    public class Repository<T>
    {
        protected Dictionary<int, T> _Entities = new Dictionary<int, T>();
        

        public IEnumerable<T> GetAll()
        {
            return _Entities.Values;
        }
        public T Get(int id)
        {
            return _Entities[id];
        }

        public bool TryGet(int id, out T entity)
        {
            return _Entities.TryGetValue(id,out entity);
        }

        public  bool Exists(int id)
        {
            return _Entities.ContainsKey(id);
        }

        internal void Delete(int id)
        {
            _Entities.Remove(id);
        }
    }
}
