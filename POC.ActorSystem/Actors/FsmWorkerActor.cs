using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using Akka.Event;
using POC.ActorSystem.Events;
using POC.ActorSystem.States;

namespace POC.ActorSystem.Actors
{
    public class FsmWorkerActor:FSM<IState,IEvent>, IWithUnboundedStash
    {
        private readonly ILoggingAdapter log = Context.GetLogger();
        public IStash Stash { get; set; }

        public FsmWorkerActor()
        {
            StartWith(Ready.Instance,null);
            Initialize();

            When(Ready.Instance, @event =>
            {
                Stash.Unstash(); 

                if (@event.FsmEvent is NewWorkArrived)
                {
                    GoTo(Blocked.Instance);
                }

                return Stay(); //some other event
            });

            When(Blocked.Instance, @event =>
            {
                if (@event.FsmEvent is WorkFinished)
                {
                    GoTo(Ready.Instance);
                }

                if (@event.FsmEvent is NewWorkArrived)
                {
                    Stash.Stash();  //stash until ready
                }

                return Stay(); //some other event
            });

        }
    }
}
