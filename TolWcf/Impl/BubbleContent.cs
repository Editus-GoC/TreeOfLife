using System.Linq;
using System.Runtime.Serialization;
using Tol.Dal;

namespace TolWcf.Impl
{
    [DataContract]
    public class BubbleContent
    {
        private readonly ITEM _item;
        public BubbleContent(int id)
        {
            using (var tol = new TOLEntities())
            {
                _item = tol.ITEM.First(w => w.ID_ITEM == id);
            }
        }

        [DataMember]
        public string Title
        {
            get { return _item.TITEL; }
            set { }
        }

        [DataMember]
        public string Description
        {
            get { return _item.BODY; }
            set { }
        }

        [DataMember]
        public string Image
        {
            get { return _item.PICTURE_URL; }
            set { }
        }
    }
}