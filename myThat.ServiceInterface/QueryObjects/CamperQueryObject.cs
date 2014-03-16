using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using SharpStack.Data.QueryObjects;
using SharpStack.Models.Tools;
using SharpStack.Services.UnitsOfWork;
using myThat.ServiceModel.Data;

namespace myThat.ServiceInterface.QueryObjects
{
    public class CamperQueryObject : TrackedClassQueryObjectBase<Camper,CamperQueryObject>
    {
        public CamperQueryObject(IUnitOfWork uow) : base(uow)
        {
        }

        public CamperQueryObject RestrictByEmail(string emailAddress)
        {
            Criteria.Add(Restrictions.Eq(PropertyReflector.GetName<Camper>(x => x.Email), emailAddress));
            return this;
        }
    }
}
