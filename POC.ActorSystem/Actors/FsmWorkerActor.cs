using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using POC.ActorSystem.Data;
using POC.ActorSystem.Events;
using POC.ActorSystem.States;

namespace POC.ActorSystem.Actors
{
    public class FsmWorkerActor : FSM<State, IData>, IWithUnboundedStash
    {
        private readonly ILoggingAdapter log = Context.GetLogger();
        public IStash Stash { get; set; }


        public FsmWorkerActor()
        {
            StartWith(State.Ready, Uninitialized.Instance);

            When(State.Ready, @event =>
            {
                var s = @event.StateData;
                var n = StateName;
                if (@event.FsmEvent is NewWorkArrived)
                {
                    var id = ((NewWorkArrived)@event.FsmEvent).WorkId;
                    
                    return GoTo(State.Blocked).Using(new WorkOrder(id));


                }

                return Stay(); //some other event
            });

            When(State.Blocked, @event =>
            {
                var s = StateData;
                var n = StateName;

                //Process((WorkOrder)StateData);


                if (@event.FsmEvent is StopBlocking)
                {
                    return GoTo(State.Ready);
                }

                if (@event.FsmEvent is NewWorkArrived)
                {
                    Stash.Stash();  //stash until ready
                    log.Info($"Stashed WorkID({((NewWorkArrived)@event.FsmEvent).WorkId})");
                    return Stay();
                }

                return Stay(); //some other event
            });

            //WhenUnhandled(state =>
            //{
            //    if (state.FsmEvent is NewWorkArrived x)
            //    {
            //        Stash.Stash();
            //        return Stay();
            //    }
            //    else
            //    {
            //        return Stay();
            //    }

            //});

            OnTransition((initialState, nextState) =>
            {
                if (initialState == State.Blocked && nextState == State.Ready)
                {
                    Stash.Unstash();
                }

                if (initialState == State.Ready && nextState == State.Blocked)
                {
                    log.Info($"Blocking and Processing WorkID {((WorkOrder)NextStateData).WorkId}");
                    Process((WorkOrder)NextStateData);
                }

            });
            Initialize();

        }

        private void Process(WorkOrder workOrder)
        {

            log.Info($"Processing WorkOrder {workOrder.WorkId}");
            System.Threading.Thread.Sleep(2000);
            log.Info($"Finished processing WorkOrder {workOrder.WorkId}");
            Self.Tell(new StopBlocking());
           // return GoTo(State.Ready);
        }
    }
}