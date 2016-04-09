using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Tol.Common;
using Tol.Dal;
using Tol.IA;
using TolWcf.Impl;

namespace TolWcf
{
    [GlobalErrorBehavior(typeof(GlobalErrorHandler))]
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any, InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    public class TolService : ITolService
    {
        public IEnumerable<Avatar> GetAvatars()
        {
            var avatars = new List<Avatar>();
            using (var tol = new TOLEntities())
            {
                foreach (var avatar in tol.AVATAR)
                {
                    var currentAvatar = new Avatar(avatar);

                    avatars.Add(currentAvatar);
                }
            }
            return avatars;
        }

        /// <summary>
        /// Return bubbles
        /// </summary>
        /// <param name="idFiltre">null if you want start bubble</param>
        /// <param name="idTime">null if you want actual</param>
        /// <param name="idProfil"></param>
        /// <returns></returns>
        public IEnumerable<Bubble> GetBubbles(int idProfil, int? idFiltre, int? idTime)
        {
            var bubbles = new List<Bubble>();
            using (var tol = new TOLEntities())
            {
                if (idTime != null)
                {
                    if (idFiltre == null)
                    {
                        foreach (
                            var bubble in
                                tol.BUBBLE.Where(w => w.ID_TIME == idTime && w.ID_PROFIL == idProfil)
                                    .OrderByDescending(o => o.SIZE)
                                    .Take(Const.NumberOfBubble))
                        {
                            var tmp = new Bubble(bubble.ID_BUBBLE);
                            bubbles.Add(tmp);
                        }
                    }
                    else
                    {
                        foreach (
                            var bubble in
                                tol.BUBBLE.Where(w => w.ID_TIME == idTime && w.ID_FILTER == idFiltre && w.ID_PROFIL == idProfil)
                                    .OrderByDescending(o => o.SIZE)
                                    .Take(Const.NumberOfBubble))
                        {
                            var tmp = new Bubble(bubble.ID_BUBBLE);
                            bubbles.Add(tmp);
                        }
                    }
                }
                else
                {
                    //if no bubble create default bubble
                    if (tol.BUBBLE.Count(w => w.ID_PROFIL == idProfil) == 0)
                    {
                        BubbleIA();
                    }
                    else
                    {
                        if (idFiltre == null)
                        {
                            foreach (
                                var bubble in
                                    tol.BUBBLE.Where(w => w.FILTER.ID_PARENT_FILTER == null && w.ID_PROFIL == idProfil)
                                        .OrderByDescending(o => o.SIZE)
                                        .OrderByDescending(o => o.ID_TIME)
                                        .Take(Const.NumberOfBubble))
                            {
                                var tmp = new Bubble(bubble.ID_BUBBLE);
                                bubbles.Add(tmp);
                            }
                        }
                        else
                        {
                            foreach (var bubble in tol.BUBBLE.Where(w => w.ID_FILTER == idFiltre && w.ID_PROFIL == idProfil).OrderByDescending(o => o.SIZE).OrderByDescending(o => o.ID_TIME).Take(Const.NumberOfBubble))
                            {
                                var tmp = new Bubble(bubble.ID_BUBBLE);
                                bubbles.Add(tmp);
                            }
                        }
                    }

                }
            }
            return bubbles;
        }

        public TimeLine GetTimeLine()
        {
            return new TimeLine();
        }

        public void BubbleClick(int idProfil, int idFilter, int idTime, string session, int iteration)
        {
            var bubbles = GetBubbles(idProfil, idFilter, idTime);

            var stats = new List<STAT>();
            foreach (var bubble in bubbles)
            {
                var stat = new STAT();
                stat.ID_PROFIL = idProfil;
                stat.ID_FILTER = bubble.Filter.Id;
                stat.CHOOSED = bubble.Filter.Id == idFilter;
                stat.SESSION = session;
                stat.ITERATION = iteration;
                stats.Add(stat);
            }

            using (var tol = new TOLEntities())
            {
                tol.STAT.AddRange(stats);
                tol.SaveChanges();
            }
        }

        public void BubbleIA()
        {
            //var activation0 = new Activation(0);
            //activation0.ComputeIA();

            var activation1 = new Activation(new double[] { 1 });
            activation1.ComputeIA();
        }
    }
}
