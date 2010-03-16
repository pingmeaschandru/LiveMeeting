namespace TW.StateMachine
{
    public class Event
    {
        private readonly string name;
        private readonly IEventMessage message;

        public Event(string name, IEventMessage message)
        {
            this.name = name;
            this.message = message;
        }

        public string Name
        {
            get { return name; }
        }

        public IEventMessage Message
        {
            get { return message; }
        }
    }
}
