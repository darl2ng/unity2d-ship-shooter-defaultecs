using System.Collections.Generic;
using DefaultEcs;
using DefaultEcs.System;

namespace System.Network
{
    /// <summary>
    /// TODO: 
    /// </summary>
    public sealed class NetworkSystem : ISystem<float>
    {
        private readonly List<Message> _messages;
        private readonly IDisposable _subscription;

        public NetworkSystem(World world)
        {
            _messages = new List<Message>(100);
            _subscription = world.Subscribe(this);
        }

        public void Dispose() { }

        [Subscribe]
        private void On(in Message message) => _messages.Add(message);

        public bool IsEnabled { get; set; } = true;

        public void Update(float state)
        {
            if(IsEnabled)
            {
                foreach(Message message in _messages)
                {
                    // setup things
                }

                _messages.Clear();
            }
        }

        internal record Message { }
    }
}
