using System;
using System.Linq;
using System.Runtime.Serialization;
using Tol.Dal;
using TolWcf.Interface;

namespace TolWcf.Impl
{
    [DataContract]
    public class Bubble : IId
    {
        private readonly BUBBLE _bubble;
        private readonly Filter _filter;
        private readonly BubbleContent _bubbleContent;
        private readonly TimePeak _timePeak;
        public Bubble(int id)
        {
            using (var tol = new TOLEntities())
            {
                _bubble = tol.BUBBLE.First(w => w.ID_BUBBLE == id);

                _filter = new Filter(_bubble.ID_FILTER);
                
                var link = tol.LINK_ITEM_FILTER.FirstOrDefault(w => w.ID_FILTER == Filter.Id);
                var filter = tol.FILTER.FirstOrDefault(w => w.ID_FILTER == link.ID_FILTER);
                if (filter != null)
                {
                    link = tol.LINK_ITEM_FILTER.FirstOrDefault(w => w.ID_FILTER == filter.ID_FILTER);
                }
                if (link != null)
                {
                    Random rand = new Random();

                    _bubbleContent = new BubbleContent(rand.Next(1,10));
                }
            }

            _timePeak = new TimePeak(_bubble.ID_TIME);
        }

        [DataMember]
        public BubbleContent Content
        {
            get
            {
                return _bubbleContent;
            }
            set { }
        }

        [DataMember]
        public decimal Size
        {
            get { return _bubble.SIZE; }
            set { }
        }

        [DataMember]
        public Filter Filter
        {
            get { return _filter; }
            set { }
        }

        [DataMember]
        public TimePeak TimePeak
        {
            get { return _timePeak; }
            set { }
        }

        [DataMember]
        public int Id
        {
            get { return _bubble.ID_BUBBLE; }
            set { }
        }
    }
}