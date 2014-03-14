using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;

namespace myThat.ServiceModel.Request
{
    [Route("/Camper", "GET")]
    public class Campers
    {
    }

    [Route("/Camper", "POST")]
    public class RegisterCamper
    {
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Company { get; set; }
        public virtual string Twitter { get; set; }
        public virtual string Website { get; set; }
        public virtual string Bio { get; set; }
    }
}
