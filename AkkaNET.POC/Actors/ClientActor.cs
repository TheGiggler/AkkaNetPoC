using Akka.Actor;
using Akka.Event;
using AkkaNET.POC.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaNET.POC.Actors
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
                Console.WriteLine("ClientActor sent 1");
               // System.Threading.Thread.Sleep(5000);
                selection.Tell(new DoSomeWork(2));
                Console.WriteLine("ClientActor sent 2");
               // System.Threading.Thread.Sleep(5000);
                selection.Tell(new DoSomeWork(3));
                Console.WriteLine("ClientActor sent 3");
               // System.Threading.Thread.Sleep(5000);
                selection.Tell(new GiveUp());

            });
        }
    }
}
