using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MVC4ManyToManyDomain;

namespace MVC4ManyToManyDatabase
{
    // Note: you can also use DropCreateDatabaseAlways to force a re-creation of your database
    //public class MockInitializer : DropCreateDatabaseIfModelChanges<MVC4ManyToManyContext>
    public class MockInitializer : DropCreateDatabaseAlways<MVC4ManyToManyContext>
    {
        protected override void Seed(MVC4ManyToManyContext context)
        {
            base.Seed(context);

            //  Add more courses here - also be sure to change to DropCreateDatabaseAlways above to re-initialise the db

            var course1 = new Course { CourseID = 1, CourseDescripcion = "Bird Watching" };
            var course2 = new Course { CourseID = 2, CourseDescripcion = "Basket weaving for beginners" };
            var course3 = new Course { CourseID = 3, CourseDescripcion = "Photography 101" };
            var course4 = new Course { CourseID = 4, CourseDescripcion = "Cooking with seaweed" };
            var course5 = new Course { CourseID = 5, CourseDescripcion = "Social media - 1 hour expert" };

            context.Courses.Add(course1);
            context.Courses.Add(course2);
            context.Courses.Add(course3);
            context.Courses.Add(course4);
            context.Courses.Add(course5);
        }
    }
}
