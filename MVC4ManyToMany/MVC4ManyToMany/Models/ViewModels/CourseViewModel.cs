using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC4ManyToMany.Models.ViewModels
{
    public class CourseViewModel
    {
        public int CourseID { get; set; }
        public string CourseDescripcion { get; set; }
        public virtual ICollection<UserProfileViewModel> UserProfiles { get; set; }
    }
}