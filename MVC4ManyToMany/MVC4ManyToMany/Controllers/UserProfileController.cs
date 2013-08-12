using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;

using MVC4ManyToMany.Models.ViewModels;
using MVC4ManyToManyDatabase;
using MVC4ManyToManyDomain;

namespace MVC4ManyToMany.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly MVC4ManyToManyContext db = new MVC4ManyToManyContext();

        public ActionResult Index()
        {
            var userProfiles = db.UserProfiles.ToList();
            var userProfileViewModels = userProfiles.Select(userProfile => userProfile.ToViewModel()).ToList();

            return View(userProfileViewModels);
        }

        public ActionResult Create()
        {
            var userProfileViewModel = new UserProfileViewModel { Courses = PopulateCourseData() };

            return View(userProfileViewModel);
        }

        [HttpPost]
        public ActionResult Create(UserProfileViewModel userProfileViewModel)
        {
            if (ModelState.IsValid)
            {
                var userProfile = userProfileViewModel.ToDomainModel();
                AddOrUpdateCourses(userProfile, userProfileViewModel.Courses);
                db.UserProfiles.Add(userProfile);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(userProfileViewModel);
        }

        public ActionResult Edit(int id = 0)
        {
            // Get all courses
            var allDbCourses = db.Courses.ToList();

            // Get the user we are editing and include the courses already subscribed to
            var userProfile = db.UserProfiles.Include("Courses").FirstOrDefault(x => x.UserProfileID == id);
            var userProfileViewModel = userProfile.ToViewModel(allDbCourses);

            return View(userProfileViewModel);
        }

        [HttpPost]
        public ActionResult Edit(UserProfileViewModel userProfileViewModel)
        {
            if (ModelState.IsValid)
            {
                var originalUserProfile = db.UserProfiles.Find(userProfileViewModel.UserProfileID);

                // Add or update new courses
                // AddOrUpdateCourses(originalUserProfile, userProfileViewModel.Courses);

                // Add or update courses keeping original
                AddOrUpdateKeepExistingCourses(originalUserProfile, userProfileViewModel.Courses);
                
                db.Entry(originalUserProfile).CurrentValues.SetValues(userProfileViewModel);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(userProfileViewModel);
        }

        public ActionResult Details(int id = 0)
        {
            // Get all courses
            var allDbCourses = db.Courses.ToList();

            // Get the user we are editing and include the courses already subscribed to
            var userProfile = db.UserProfiles.Include("Courses").FirstOrDefault(x => x.UserProfileID == id);
            var userProfileViewModel = userProfile.ToViewModel(allDbCourses);

            return View(userProfileViewModel);
        }

        public ActionResult Delete(int id = 0)
        {
            var userProfileIQueryable = from u in db.UserProfiles.Include("Courses")
                                  where u.UserProfileID == id
                                  select u;

            if (!userProfileIQueryable.Any())
            {
                return HttpNotFound("User not found.");
            }

            var userProfile = userProfileIQueryable.First();
            var userProfileViewModel = userProfile.ToViewModel();

            return View(userProfileViewModel);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var userProfile = db.UserProfiles.Include("Courses").Single(u => u.UserProfileID == id);
            DeleteUserProfile(userProfile);

            return RedirectToAction("Index");
        }

        private void DeleteUserProfile(UserProfile userProfile)
        {
            if (userProfile.Courses != null)
            {
                foreach (var course in userProfile.Courses.ToList())
                {
                    userProfile.Courses.Remove(course);
                }
            }

            db.UserProfiles.Remove(userProfile);
            db.SaveChanges();
        }

        private ICollection<AssignedCourseData> PopulateCourseData()
        {
            var courses = db.Courses;
            var assignedCourses = new List<AssignedCourseData>();

            foreach (var item in courses)
            {
                assignedCourses.Add(new AssignedCourseData
                {
                    CourseID = item.CourseID,
                    CourseDescription = item.CourseDescripcion,
                    Assigned = false
                });
            }

            return assignedCourses;
        }

        private void AddOrUpdateCourses(UserProfile userProfile, IEnumerable<AssignedCourseData> assignedCourses)
        {
            if (assignedCourses == null) return;

            if (userProfile.UserProfileID != 0)
            {
                // Existing user - drop their existing courses and add the new ones if any
                foreach (var course in userProfile.Courses.ToList())
                {
                    userProfile.Courses.Remove(course);
                }

                foreach (var course in assignedCourses.Where(c => c.Assigned))
                {
                    userProfile.Courses.Add(db.Courses.Find(course.CourseID));
                }
            }
            else
            {
                // New user
                foreach (var assignedCourse in assignedCourses.Where(c => c.Assigned))
                {
                    var course = new Course { CourseID = assignedCourse.CourseID };
                    db.Courses.Attach(course);
                    userProfile.Courses.Add(course);
                }                
            }
        }

        private void AddOrUpdateKeepExistingCourses(UserProfile userProfile, IEnumerable<AssignedCourseData> assignedCourses)
        {
            var webCourseAssignedIDs = assignedCourses.Where(c => c.Assigned).Select(webCourse => webCourse.CourseID);
            var dbCourseIDs = userProfile.Courses.Select(dbCourse => dbCourse.CourseID);
            var courseIDs = dbCourseIDs as int[] ?? dbCourseIDs.ToArray();
            var coursesToDeleteIDs = courseIDs.Where(id => !webCourseAssignedIDs.Contains(id)).ToList();

            // Delete removed courses
            foreach (var id in coursesToDeleteIDs)
            {
                userProfile.Courses.Remove(db.Courses.Find(id));
            }

            // Add courses that user doesn't already have
            foreach (var id in webCourseAssignedIDs)
            {
                if (!courseIDs.Contains(id))
                {
                    userProfile.Courses.Add(db.Courses.Find(id));
                }            
            }
        }
    }
}
