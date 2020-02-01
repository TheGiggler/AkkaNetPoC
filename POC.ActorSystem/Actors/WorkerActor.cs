using Akka.Actor;
using Akka.Event;
using POC.ActorSystem.Messages;
using System;
using System.Collections.Generic;
using System.Text;

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

            Receive<DoSomeWork>(_ =>
            {
                Stash.UnstashAll();

                log.Info("WorkerActor received DoSomeWork");
                Become(Processing);
            });


        }

        private void Processing() 
        { 
            log.Info("WorkerActor became Processing");
            Receive<GiveUp>(x =>
            {
                log.Info("WorkerActor received GiveUp");
                Become(Ready);
            });

            Receive<DoSomeWork>(_ =>
            {
                BecomeStacked(() =>
                {

                    Receive<DoSomeWork>(_ => Stash.Stash());
                });


            });

            //simulate work
            System.Threading.Thread.Sleep(2000);
            //work is done
            Become(Ready);
        }

    }
}
