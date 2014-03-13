using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpStack.Models;

namespace myThat.ServiceModel.Data
{
    public class Speaker : TrackedClass
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Twitter { get; set; }
        public virtual string Bio { get; set; }
    }
}
