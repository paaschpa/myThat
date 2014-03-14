using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface;
using SharpStack.Data.Database;
using myThat.ServiceInterface.Mappings;
using myThat.ServiceModel.Request;

namespace myThat.ServiceInterface
{
    public class SchemaService : Service
    {
        public string Get(Schema request)
        {
            NHibernateConfigurator.Initialize<CamperMapping>();
            return NHibernateConfigurator.GetSchema();
        }
    }
}
