using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleApp
{

    public class CustomEventArgs : EventArgs
    {
        public int Number { get; set; }
        public string Message { get; set; }
        public CustomEventArgs(int number, string message) => (Number, Message) = (number, message);
        
        public override string ToString() => $"[Number: {Number}, Message: {Message}]";
    }

    //Delegate for the custom event
    public delegate void CustomEventHandler(object sender, CustomEventArgs e);

    public class  EventPublisher
    {
        //Event declaration
        public CustomEventHandler CustomEvent;
        protected virtual void OnCustomEvent(int number, string message)
        {
            CustomEventArgs e = new CustomEventArgs(number, message);
            //Check if there are any subscribers
            CustomEvent?.Invoke(this, e);
        }
        public void RaiseEvent()
        {
            for(int i = 0; i < 10; i++)
            {
                if(i % 4==0)
                    OnCustomEvent(i, $"Event raised with number {i}");
            }
        }
    }
    public class EventSubscriber
    {
        public void HandleCustomEvent(object sender, CustomEventArgs e)
        {
            Console.WriteLine($"Event received: {e}");
        }
    }

    internal class WorkingWithDelegates2
    {

        internal static void Test()
        {
            EventPublisher publisher = new EventPublisher();
            EventSubscriber subscriber = new EventSubscriber();
            //Subscribe to the event
            publisher.CustomEvent = new CustomEventHandler(subscriber.HandleCustomEvent);
            //Raise the event
            publisher.RaiseEvent();
            


        }
    }
}
