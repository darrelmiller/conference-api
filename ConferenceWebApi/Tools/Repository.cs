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
    }
}
