using System.Collections.Generic;
using System.Runtime.Serialization;
using Tol.Dal;

namespace TolWcf.Impl
{
    [DataContract]
    public class TimeLine
    {
        private readonly List<TimePeak> _timePeaks;
        public TimeLine()
        {
            _timePeaks = new List<TimePeak>();
            using (var tol = new TOLEntities())
            {
                foreach (var time in tol.TIME)
                {
                    _timePeaks.Add(new TimePeak(time.ID_TIME));
                }
            }
        }

        [DataMember]
        public IEnumerable<TimePeak> TimePeaks
        {
            get { return _timePeaks; }
            set { }
        }
    }
}