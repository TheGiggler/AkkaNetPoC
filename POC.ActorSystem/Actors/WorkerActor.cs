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
            Ready();//ready to receive work requests
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
                
                Become(Blocked);//block and process this request
                Self.Tell(new DoCurrentWork() { Work = m });
              
            });


        }

        private void Blocked()
        {

            Receive<DoSomeWork>(m => { Stash.Stash(); log.Info($"Blocked Stashing WorkId {m.WorkID} at {DateTime.Now.ToLongTimeString()}"); });
            Receive<DoCurrentWork>(m=>
            {
                DoSomeWork work = m.Work;
                DoTheWork(work);
                log.Info($"WorkerActor finished processing DoSomeWork {work.WorkID} at {DateTime.Now.ToLongTimeString()}");
                Become(Ready);//unblock and process first stashed request
                Stash.Unstash();

            });


            log.Info($"Blocked returning at {DateTime.Now.ToLongTimeString()}");
            return;

        }


        private void DoTheWork(DoSomeWork m)
        {
            log.Info($"DoTheWork WorkId {m.WorkID}");
            System.Threading.Thread.Sleep(2500); //simulate i/o latency
        }
    }
}
