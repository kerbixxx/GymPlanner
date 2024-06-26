﻿using GymPlanner.Application.Models.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymPlanner.Application.Interfaces.Services
{
    public interface IRabbitMQProducer
    {
        void SendMessageToRabbit<T>(T message);
        void NotifySubscribersAboutEdit(MessageEditNotifier message);
        void Dispose();
    }
}
