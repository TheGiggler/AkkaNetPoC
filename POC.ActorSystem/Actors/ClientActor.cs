using Akka.Actor;
using Akka.Event;
using POC.ActorSystem.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace POC.ActorSystem.Actors
{

    public class ClientActor : ReceiveActor
    {
        private readonly ILoggingAdapter log = Context.GetLogger();

        public ClientActor()
        {

            Ready();
        }

        private void Ready()
        {
            log.Info("ClientActor became Ready");
            Receive<DoSomeWork>(_ =>
            {
                log.Info("ClientActor received DoSomeWork");
                Console.WriteLine("ClientActor received DoSomeWork");
                //get reference to worker actor
                var selection = Context.ActorSelection("/user/Worker");
                selection.Tell(new DoSomeWork(1));
                System.Threading.Thread.Sleep(3000);
                selection.Tell(new DoSomeWork(2));
                System.Threading.Thread.Sleep(3000);
                selection.Tell(new GiveUp());

            });
        }
    }
}
