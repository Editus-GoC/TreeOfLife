using System;
using System.Linq;
using System.Runtime.Serialization;
using Tol.Dal;
using TolWcf.Interface;

namespace TolWcf.Impl
{
    [DataContract]
    public class TimePeak : IId
    {
        private readonly TIME _time;
        public TimePeak(int id)
        {
            using (var tol = new TOLEntities())
            {
                _time = tol.TIME.First(w => w.ID_TIME == id);
            }
        }

        [DataMember]
        public int Id
        {
            get { return _time.ID_TIME; }
            set { }
        }

        [DataMember]
        public DateTime DateTime
        {
            get
            {
                return _time.TIME1;
            }
            set { }
        }

    }
}