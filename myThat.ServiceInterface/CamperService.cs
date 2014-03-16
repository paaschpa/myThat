using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Common;
using ServiceStack.ServiceInterface;
using SharpStack.Data.UnitsOfWork;
using SharpStack.Services.UnitsOfWork;
using myThat.ServiceInterface.QueryObjects;
using myThat.ServiceModel.Data;
using myThat.ServiceModel.Request;

namespace myThat.ServiceInterface
{
    public class CamperService : Service
    {
        public virtual IUnitOfWork GetUnitOfWork()
        {
            return new UnitOfWork("");
        }

        public virtual CamperQueryObject GetCamperQueryObject(IUnitOfWork uow)
        {
            return new CamperQueryObject(uow);
        }

        public List<Camper> Get(Campers request)
        {
            return new List<Camper>();
        }

        public Camper Put(EditCamper request)
        {
            using (var uow = GetUnitOfWork())
            {
                var camperQueryObject = GetCamperQueryObject(uow);
                var camper = camperQueryObject.RestrictByEmail(request.Email).GetSingle();
                camper.PopulateWithNonDefaultValues(request);
                uow.Save(camper);
                uow.CommitTransaction();
                return camper;
            }
        }
    }
}
