using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC4ManyToManyDomain
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseDescripcion { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
