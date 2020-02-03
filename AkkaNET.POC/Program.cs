using System;
using Akka.Actor;
using AkkaNET.POC.Actors;
using AkkaNET.POC.Data;
using AkkaNET.POC.Events;
using AkkaNET.POC.Messages;

namespace POC.ActorSystem
{
    class Program
    {
        static Akka.Actor.ActorSystem actorSystem;
        
        static void Main(string[] args)
        {
            actorSystem = Akka.Actor.ActorSystem.Create("Maddog19");
            //IActorRef worker = actorSystem.ActorOf<WorkerActor>("Worker");
            //IActorRef client = actorSystem.ActorOf<ClientActor>("Client");
            //client.Tell(new DoSomeWork(1));


            IActorRef worker = actorSystem.ActorOf<FsmWorkerActor>("FsmWorker");
            worker.Tell(new NewWorkArrived(1));
            worker.Tell(new NewWorkArrived(2));
            worker.Tell(new NewWorkArrived(3));
           // worker.Tell(new StopBlocking());
            while (1 == 1)
            {
            }

        }

       
    }
}
