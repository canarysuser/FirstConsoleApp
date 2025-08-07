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

    public class EventPublisher
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
            for (int i = 0; i < 10; i++)
            {
                if (i % 4 == 0)
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
        public WorkingWithDelegates2()
        {
            Console.WriteLine($"{nameof(WorkingWithDelegates2)}.ctor() invoked.");
        }

        internal static void Test()
        {
            EventPublisher publisher = new EventPublisher();
            EventSubscriber subscriber = new EventSubscriber();
            //Subscribe to the event
            publisher.CustomEvent = new CustomEventHandler(subscriber.HandleCustomEvent);
            //Raise the event
            publisher.RaiseEvent();

            ParentClass parent = new ParentClass();
            parent.AddItem(10);
            parent.UpdateItem(0, 25);
            parent.AddItem(20);
            parent.UpdateItem(1, 25);
            // parent.AddItem(30);
        }
    }
    public class ParentClass
    {
        public int MaxItems = 50;
        public List<DetailClass> Details { get; set; } = new List<DetailClass>();
        public ParentClass()
        {

        }
        public void AddItem(int number)
        {
            DetailClass item = new DetailClass();
            item.ItemAdded += ItemAdded;
            item.StoredItems = number;
            Details.Add(item);
            Details.ForEach(d => Console.WriteLine($"Stored Items: {d.StoredItems}"));
        }
        public void UpdateItem(int index, int newValue)
        {
            Details[index].StoredItems = newValue;
            Console.WriteLine($"Updated item at index {index} to {newValue}");
        }
        private void ItemAdded(object sender, DetailClass item)
        {
            if (Details.Sum(c=>c.StoredItems) < MaxItems)
            {
                Console.WriteLine($"Item added. Total items: {Details.Sum(c => c.StoredItems)}");
            }
            else
            {
                Console.WriteLine("Max items reached. Cannot add more.");
            }
        }
    }
    public delegate void ItemAddedHandler(object sender, DetailClass item);
    public class DetailClass
    {
        public event ItemAddedHandler ItemAdded;
        private int _storedItems;
        public int StoredItems
        {
            get { return _storedItems; }
            set
            {
                _storedItems = value;
                ItemAdded?.Invoke(this, this);
            }
        }
        

    }
}
