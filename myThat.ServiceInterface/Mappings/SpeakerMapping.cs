using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpStack.Data.Mappings;
using myThat.ServiceModel.Data;
using FluentNHibernate.Mapping;

namespace myThat.ServiceInterface.Mappings
{
    public class SpeakerMapping : TrackedClassMap<Speaker>
    {
        public SpeakerMapping()
        {
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Twitter);
            Map(x => x.Bio);
        }
    }
}
