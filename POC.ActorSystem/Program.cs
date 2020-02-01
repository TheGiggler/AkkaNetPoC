using System;
using Akka.Actor;
using POC.ActorSystem.Actors;
using POC.ActorSystem.Messages;

namespace POC.ActorSystem
{
    class Program
    {
        static Akka.Actor.ActorSystem actorSystem;
        
        static void Main(string[] args)
        {
            actorSystem = Akka.Actor.ActorSystem.Create("Maddog19");
            IActorRef worker = actorSystem.ActorOf<WorkerActor>("Worker");
            IActorRef client = actorSystem.ActorOf<ClientActor>("Client");

            client.Tell(new DoSomeWork(1));

            while (1 == 1)
            {
            }

        }

       
    }
}
