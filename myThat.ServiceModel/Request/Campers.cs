using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;

namespace myThat.ServiceModel.Request
{
    [Route("/Camper/GetProfile", "GET")]
    public class CamperProfile
    {
    }

    [Route("/Camper", "PUT")]
    public class EditCamper
    {
        public virtual Guid Id { get; set; }
        public virtual string Email { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Company { get; set; }
        public virtual string Twitter { get; set; }
        public virtual string Website { get; set; }
        public virtual string Bio { get; set; }
    }
}
