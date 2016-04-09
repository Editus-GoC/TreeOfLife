using System.Runtime.Serialization;
using Tol.Common;
using Tol.Dal;
using TolWcf.Common;
using TolWcf.Interface;

namespace TolWcf.Impl
{
    [DataContract]
    public class Avatar : IId
    {
        private readonly AVATAR _avatar;
        public Avatar(AVATAR avatar)
        {
            _avatar = avatar;
        }

        [DataMember]
        public AvatarType Type
        {
            get { return EnumHelper.IsEnum<AvatarType>(_avatar.ID_PROFIL); }
            set
            {

            }
        }

        [DataMember]
        public int Id
        {
            get { return _avatar.ID_AVATAR; }
            set { }
        }

        [DataMember]
        public Gender Gender
        {
            get { return EnumHelper.IsEnum<Gender>(_avatar.GENDER); }
            set { }
        }
    }
}