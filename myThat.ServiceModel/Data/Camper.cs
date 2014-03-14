using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpStack.Models;

namespace myThat.ServiceModel.Data
{
    public class Camper : TrackedClass
    {
        public virtual string Email { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Company { get; set; }
        public virtual string Twitter { get; set; }
        public virtual string Website { get; set; }
        public virtual string Bio { get; set; }

        public virtual string ImagePath { 
            get { return "/images/" + this.Id + ".png"; }
        }
    }
}
