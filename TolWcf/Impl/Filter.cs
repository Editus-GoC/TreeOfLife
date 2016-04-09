using System.Linq;
using System.Runtime.Serialization;
using Tol.Dal;
using TolWcf.Interface;

namespace TolWcf.Impl
{
    [DataContract]
    public class Filter : IId
    {
        private readonly FILTER _filter;
        public Filter(int id)
        {
            using (var tol = new TOLEntities())
            {
                _filter = tol.FILTER.First(w => w.ID_FILTER == id);
            }
        }

        [DataMember]
        public int Id { get { return _filter.ID_FILTER; } set {} }

        [DataMember]
        public string Color { get { return _filter.COLOR; } set { } }

        [DataMember]
        public string Title { get { return _filter.VALUE; } set { } }
    }
}