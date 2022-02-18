using CustomerManagerApp.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagerApp.Backend.Repository.Customer
{
    public class QueryBaseClass
    {
        static private protected Queue<QueueQuery> Queue = new();

        static private protected bool QueueRunning = false;
        private protected async Task RunQueueAsync()
        {
            //This means this meathod was already called. We only need to have one called at a time.
            if (QueueRunning) return;
            QueueRunning = true;
            while (Queue.Count > 0)
            {
                var query = Queue.Dequeue();
                if (query.RequestCompleted)
                {
                    throw new ArgumentException("Trying to handle a query that has already been handled.");
                }

                switch (query.Request)
                {
                    case QueueRequest.Read:
                        if(ReadRequest == null) throw new ArgumentException(nameof(ReadRequest));
                        await ReadRequest.Invoke(query);
                        query.RequestCompleted = true;
                        break;

                    case QueueRequest.Write:
                        if(WriteRequest == null) throw new ArgumentException(nameof(WriteRequest));
                        await WriteRequest.Invoke(query);
                        query.RequestCompleted = true;
                        break;

                    case QueueRequest.Delete:
                        if(DeleteRequest == null) throw new ArgumentException(nameof(DeleteRequest));
                        await DeleteRequest.Invoke(query);
                        query.RequestCompleted = true;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            QueueRunning = false;
        }

        private protected event HandleReadQueueRequest? ReadRequest;

        private protected event HandleWriteQueueRequest? WriteRequest;

        private protected event HandleDeleteQueueRequest? DeleteRequest;


        private protected delegate Task HandleReadQueueRequest(QueueQuery query);

        private protected delegate Task HandleWriteQueueRequest(QueueQuery query);

        private protected delegate Task HandleDeleteQueueRequest(QueueQuery query);

        private protected async Task EnqueueQuery(QueueQuery item)
        {
            Queue.Enqueue(item);
            await RunQueueAsync();
        }
    }
}
internal struct QueueQuery
{
    public QueueRequest Request { get; set; }
    public CustomerEntity? Customer { get; set; }

    public bool RequestCompleted = false;

    public QueueQuery(QueueRequest request, CustomerEntity? customer = null) : this()
    {
        Customer = customer;
        Request = request;
    }
}

internal enum QueueRequest
{
    Read,
    Write,
    Delete
}