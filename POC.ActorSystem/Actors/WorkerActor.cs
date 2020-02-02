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
            log.Info($"WorkerActor became Ready at {DateTime.Now.ToLongTimeString()}");
             Receive<DoSomeWork>(m =>
            {
               // CurrentMessage = m;
                Become(Blocked);
                Self.Tell(new DoCurrentWork() { Work = m });
              
            });


        }

        private void Blocked()
        {

            Receive<DoSomeWork>(m2 => { Stash.Stash(); log.Info($"Blocked Stashing WorkId {m2.WorkID} at {DateTime.Now.ToLongTimeString()}"); });
            Receive<DoCurrentWork>(m=>
            {
                DoSomeWork work = m.Work;
                log.Info($"WorkerActor finished processing DoSomeWork {work.WorkID} at {DateTime.Now.ToLongTimeString()}");
                Become(Ready);
                Stash.Unstash();

            });


            log.Info($"Blocked returning at {DateTime.Now.ToLongTimeString()}");
            return;

        }
        //private void Processing()
        //{
        //    Receive<DoSomeWork>(m =>
        //    {
        //        log.Info($"WorkerActor processing DoSomeWork {m.WorkID}");
        //        BecomeStacked(() =>
        //        {

        //            UnbecomeStacked();

        //            Become(Ready);
        //            Stash.UnstashAll();

        //        });
        //    });

        //    Receive<GiveUp>(m =>
        //    {
        //        //stop processing this work id
        //    });
        //        //Stash.UnstashAll();

        //        //log.Info($"WorkerActor received DoSomeWork {m.WorkID}");
        //        //BecomeStacked(() =>
        //        //{
        //        //    ReceiveAny(_ => Stash.Stash());


        //        //});
        //        //log.Info($"Processing WorkId {m.WorkID}");
        //        //System.Threading.Thread.Sleep(3000); //fake i/o work 
        //        //log.Info($"Done processing WorkId {m.WorkID}");
        //    }

        private void DoSomeWork(DoSomeWork m)
        {
            log.Info($"DoSomeWork WorkId {m.WorkID}");
            System.Threading.Thread.Sleep(2500);
        }
    }
}
