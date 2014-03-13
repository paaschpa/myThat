using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;

namespace myThat.ServiceModel.Request
{
    [Route("/Speaker", "GET")]
    public class Speakers
    {
    }

    [Route("/Speaker", "POST")]
    public class CreateSpeaker
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Twitter { get; set; }
        public virtual string Bio { get; set; }
    }
}
