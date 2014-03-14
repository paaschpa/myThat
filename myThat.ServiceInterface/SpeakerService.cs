﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Common;
using ServiceStack.ServiceInterface;
using SharpStack.Data.UnitsOfWork;
using myThat.ServiceModel.Data;
using myThat.ServiceModel.Request;

namespace myThat.ServiceInterface
{
    public class SpeakerService : Service
    {
        public List<Camper> Get(Campers request)
        {
            return new List<Camper>();
        }

        public Camper Post(RegisterCamper request)
        {
            var newSpeaker = request.TranslateTo<Camper>();
            using (var uow = new UnitOfWork(""))
            {
                uow.Save(newSpeaker);
            }

            return newSpeaker;
        }
    }
}
