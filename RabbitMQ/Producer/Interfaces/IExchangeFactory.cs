using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Producer.Interfaces
{
    public interface IExchangeFactory
    {
        void CreateExChangeAndQueue();
    }
}
