using System.Collections.Generic;
using System.Linq;
using MVC4ManyToManyDomain;

namespace MVC4ManyToMany.Models.ViewModels
{
    public static class ViewModelHelpers
    {
        public static UserProfileViewModel ToViewModel(this UserProfile userProfile)
        {
            var userProfileViewModel = new UserProfileViewModel
            {
                Name = userProfile.Name,
                UserProfileID = userProfile.UserProfileID
            };

            foreach (var course in userProfile.Courses)
            {
                userProfileViewModel.Courses.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    CourseDescription = course.CourseDescripcion,
                    Assigned = true
                });
            }

            return userProfileViewModel;
        }

        public static UserProfileViewModel ToViewModel(this UserProfile userProfile, ICollection<Course> allDbCourses )
        {
            var userProfileViewModel = new UserProfileViewModel
            {
                Name = userProfile.Name,
                UserProfileID = userProfile.UserProfileID
            };

            // Collection for full list of courses with user's already assigned courses included
            ICollection<AssignedCourseData> allCourses = new List<AssignedCourseData>();

            foreach (var c in allDbCourses)
            {
                // Create new AssignedCourseData for each course and set Assigned = true if user already has course
                var assignedCourse = new AssignedCourseData
                    {
                    CourseID = c.CourseID,
                    CourseDescription = c.CourseDescripcion,
                    Assigned = userProfile.Courses.FirstOrDefault(x => x.CourseID == c.CourseID) != null
                };

                allCourses.Add(assignedCourse);
            }

            userProfileViewModel.Courses = allCourses;

            return userProfileViewModel;
        }

        public static UserProfile ToDomainModel(this UserProfileViewModel userProfileViewModel)
        {
            var userProfile = new UserProfile
            {
                Name = userProfileViewModel.Name,
                UserProfileID = userProfileViewModel.UserProfileID
            };

            return userProfile;
        }
    }
}