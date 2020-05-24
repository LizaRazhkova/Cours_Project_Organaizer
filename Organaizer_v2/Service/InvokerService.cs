using Organaizer_v2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organaizer_v2.Service
{
    public class InvokerService
    {
        private class Subscriber
        {
            public Subscriber(Type receiverType, Type dataType)
            {
                ReceiverType = receiverType;
                DataType = dataType;
            }

            public Type ReceiverType { get; }
            public Type DataType { get; }
        }

        private readonly Dictionary<Subscriber, Func<IData, Task>> receivers;

        public InvokerService()
        {
            this.receivers = new Dictionary<Subscriber, Func<IData, Task>>();
        }

        public async Task Invoke<TReceiver>(IData data)
        {
            var dataType = data.GetType();
            var receiverType = typeof(TReceiver);

            var receiversByCredentials = this.receivers
                .Where(receiver => receiver.Key.DataType == dataType
                    && receiver.Key.ReceiverType == receiverType)
                .Select(receiver => receiver.Value(data));

            await Task.WhenAll(receiversByCredentials);
        }

        public async Task Invoke(IData data, Type TReceiver)
        {
            var dataType = data.GetType();
            var receiverType = TReceiver;

            var receiversByCredentials = this.receivers
                .Where(receiver => receiver.Key.DataType == dataType
                    && receiver.Key.ReceiverType == receiverType)
                .Select(receiver => receiver.Value(data));

            await Task.WhenAll(receiversByCredentials);
        }

        public void Receive<TData>(object receiver, Func<TData, Task> handler) where TData : IData
        {
            var subscriber = new Subscriber(receiver.GetType(), typeof(TData));

            receivers.Add(subscriber, data => handler((TData)data));
        }
        public void DisableAllInvokers()
        {
            receivers.Clear();
        }
    }
}
