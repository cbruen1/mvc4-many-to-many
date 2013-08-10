using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MVC4ManyToMany.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public UserProfileViewModel()
        {
            Courses = new Collection<AssignedCourseData>();
        }

        public int UserProfileID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<AssignedCourseData> Courses { get; set; }
    }
}