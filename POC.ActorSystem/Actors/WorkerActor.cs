using Akka.Actor;
using Akka.Event;
using POC.ActorSystem.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POC.ActorSystem.Actors
{
    public class WorkerActor : ReceiveActor, IWithUnboundedStash
    {
        private readonly ILoggingAdapter log = Context.GetLogger();

        public IStash Stash { get; set; }
        public WorkerActor()
        {

            Ready();


        }

        //protected override void PreStart()
        //{
        //    Stash = StashFactory.CreateStash(Context, this);
        //    base.PreStart();
        //}
        private void Ready()
        {
            log.Info("WorkerActor became Ready");
            
            Receive<DoSomeWork>(m =>
            {
                Stash.UnstashAll();
                Become(Processing);
                Self.Tell(m);

            });


        }

        private void Processing()
        {
            Receive<DoSomeWork>(m =>
            {
                log.Info($"WorkerActor processing DoSomeWork {m.WorkID}");
                BecomeStacked(() =>
                {
                    Receive<DoSomeWork>(_ => Stash.Stash());
                    DoSomeWork(m);
                    log.Info($"WorkerActor finished processing DoSomeWork {m.WorkID}");

                    Context.UnbecomeStacked();
                    //Become(Ready);

                });
            });

            Receive<GiveUp>(m =>
            {
                //stop processing this work id
            });
                //Stash.UnstashAll();

                //log.Info($"WorkerActor received DoSomeWork {m.WorkID}");
                //BecomeStacked(() =>
                //{
                //    ReceiveAny(_ => Stash.Stash());


                //});
                //log.Info($"Processing WorkId {m.WorkID}");
                //System.Threading.Thread.Sleep(3000); //fake i/o work 
                //log.Info($"Done processing WorkId {m.WorkID}");
            }

        private void DoSomeWork(DoSomeWork m)
        {
            log.Info($"DoSomeWork WorkId {m.WorkID}");
            System.Threading.Thread.Sleep(2500);
        }
    }
}
