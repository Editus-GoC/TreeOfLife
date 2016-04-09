using System;
using System.Collections.Generic;
using System.ServiceModel;
using TolWcf.Impl;

namespace TolWcf
{
    [ServiceContract]
    public interface ITolService
    {
        [OperationContract]
        IEnumerable<Avatar> GetAvatars();

        [OperationContract]
        IEnumerable<Bubble> GetBubbles(int idProfil, int? idFilter, int? idTime);

        [OperationContract]
        TimeLine GetTimeLine();

        [OperationContract]
        void BubbleClick(int idProfil, int idFilter, int idTime, string session, int iteration);

        [OperationContract]
        void BubbleIA();
    }
}
