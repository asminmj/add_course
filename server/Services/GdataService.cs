using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Data;
using System.Text.Encodings.Web;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using G.Data;

namespace G
{
    public partial class GdataService
    {
        GdataContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly GdataContext context;
        private readonly NavigationManager navigationManager;

        public GdataService(GdataContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public async Task ExportCoursesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/gdata/courses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/gdata/courses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCoursesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/gdata/courses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/gdata/courses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCoursesRead(ref IQueryable<Models.Gdata.Course> items);

        public async Task<IQueryable<Models.Gdata.Course>> GetCourses(Query query = null)
        {
            var items = Context.Courses.AsQueryable();

            items = items.Include(i => i.Department);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnCoursesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCourseCreated(Models.Gdata.Course item);
        partial void OnAfterCourseCreated(Models.Gdata.Course item);

        public async Task<Models.Gdata.Course> CreateCourse(Models.Gdata.Course course)
        {
            OnCourseCreated(course);

            var existingItem = Context.Courses
                              .Where(i => i.CourseID == course.CourseID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Courses.Add(course);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(course).State = EntityState.Detached;
                course.Department = null;
                throw;
            }

            OnAfterCourseCreated(course);

            return course;
        }
        public async Task ExportCourseInstructorsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/gdata/courseinstructors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/gdata/courseinstructors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCourseInstructorsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/gdata/courseinstructors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/gdata/courseinstructors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCourseInstructorsRead(ref IQueryable<Models.Gdata.CourseInstructor> items);

        public async Task<IQueryable<Models.Gdata.CourseInstructor>> GetCourseInstructors(Query query = null)
        {
            var items = Context.CourseInstructors.AsQueryable();

            items = items.Include(i => i.Course);

            items = items.Include(i => i.Instructor);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnCourseInstructorsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCourseInstructorCreated(Models.Gdata.CourseInstructor item);
        partial void OnAfterCourseInstructorCreated(Models.Gdata.CourseInstructor item);

        public async Task<Models.Gdata.CourseInstructor> CreateCourseInstructor(Models.Gdata.CourseInstructor courseInstructor)
        {
            OnCourseInstructorCreated(courseInstructor);

            var existingItem = Context.CourseInstructors
                              .Where(i => i.CoursesCourseID == courseInstructor.CoursesCourseID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.CourseInstructors.Add(courseInstructor);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(courseInstructor).State = EntityState.Detached;
                courseInstructor.Course = null;
                courseInstructor.Instructor = null;
                throw;
            }

            OnAfterCourseInstructorCreated(courseInstructor);

            return courseInstructor;
        }
        public async Task ExportDepartmentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/gdata/departments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/gdata/departments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportDepartmentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/gdata/departments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/gdata/departments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnDepartmentsRead(ref IQueryable<Models.Gdata.Department> items);

        public async Task<IQueryable<Models.Gdata.Department>> GetDepartments(Query query = null)
        {
            var items = Context.Departments.AsQueryable();

            items = items.Include(i => i.Instructor);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnDepartmentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnDepartmentCreated(Models.Gdata.Department item);
        partial void OnAfterDepartmentCreated(Models.Gdata.Department item);

        public async Task<Models.Gdata.Department> CreateDepartment(Models.Gdata.Department department)
        {
            OnDepartmentCreated(department);

            var existingItem = Context.Departments
                              .Where(i => i.DepartmentID == department.DepartmentID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Departments.Add(department);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(department).State = EntityState.Detached;
                department.Instructor = null;
                throw;
            }

            OnAfterDepartmentCreated(department);

            return department;
        }
        public async Task ExportEnrollmentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/gdata/enrollments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/gdata/enrollments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportEnrollmentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/gdata/enrollments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/gdata/enrollments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnEnrollmentsRead(ref IQueryable<Models.Gdata.Enrollment> items);

        public async Task<IQueryable<Models.Gdata.Enrollment>> GetEnrollments(Query query = null)
        {
            var items = Context.Enrollments.AsQueryable();

            items = items.Include(i => i.Course);

            items = items.Include(i => i.Student);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnEnrollmentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEnrollmentCreated(Models.Gdata.Enrollment item);
        partial void OnAfterEnrollmentCreated(Models.Gdata.Enrollment item);

        public async Task<Models.Gdata.Enrollment> CreateEnrollment(Models.Gdata.Enrollment enrollment)
        {
            OnEnrollmentCreated(enrollment);

            var existingItem = Context.Enrollments
                              .Where(i => i.EnrollmentID == enrollment.EnrollmentID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Enrollments.Add(enrollment);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(enrollment).State = EntityState.Detached;
                enrollment.Course = null;
                enrollment.Student = null;
                throw;
            }

            OnAfterEnrollmentCreated(enrollment);

            return enrollment;
        }
        public async Task ExportInstructorsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/gdata/instructors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/gdata/instructors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportInstructorsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/gdata/instructors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/gdata/instructors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnInstructorsRead(ref IQueryable<Models.Gdata.Instructor> items);

        public async Task<IQueryable<Models.Gdata.Instructor>> GetInstructors(Query query = null)
        {
            var items = Context.Instructors.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnInstructorsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnInstructorCreated(Models.Gdata.Instructor item);
        partial void OnAfterInstructorCreated(Models.Gdata.Instructor item);

        public async Task<Models.Gdata.Instructor> CreateInstructor(Models.Gdata.Instructor instructor)
        {
            OnInstructorCreated(instructor);

            var existingItem = Context.Instructors
                              .Where(i => i.ID == instructor.ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Instructors.Add(instructor);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(instructor).State = EntityState.Detached;
                throw;
            }

            OnAfterInstructorCreated(instructor);

            return instructor;
        }
        public async Task ExportOfficeAssignmentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/gdata/officeassignments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/gdata/officeassignments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportOfficeAssignmentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/gdata/officeassignments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/gdata/officeassignments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnOfficeAssignmentsRead(ref IQueryable<Models.Gdata.OfficeAssignment> items);

        public async Task<IQueryable<Models.Gdata.OfficeAssignment>> GetOfficeAssignments(Query query = null)
        {
            var items = Context.OfficeAssignments.AsQueryable();

            items = items.Include(i => i.Instructor);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnOfficeAssignmentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnOfficeAssignmentCreated(Models.Gdata.OfficeAssignment item);
        partial void OnAfterOfficeAssignmentCreated(Models.Gdata.OfficeAssignment item);

        public async Task<Models.Gdata.OfficeAssignment> CreateOfficeAssignment(Models.Gdata.OfficeAssignment officeAssignment)
        {
            OnOfficeAssignmentCreated(officeAssignment);

            var existingItem = Context.OfficeAssignments
                              .Where(i => i.InstructorID == officeAssignment.InstructorID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.OfficeAssignments.Add(officeAssignment);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(officeAssignment).State = EntityState.Detached;
                officeAssignment.Instructor = null;
                throw;
            }

            OnAfterOfficeAssignmentCreated(officeAssignment);

            return officeAssignment;
        }
        public async Task ExportStudentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/gdata/students/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/gdata/students/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStudentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/gdata/students/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/gdata/students/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStudentsRead(ref IQueryable<Models.Gdata.Student> items);

        public async Task<IQueryable<Models.Gdata.Student>> GetStudents(Query query = null)
        {
            var items = Context.Students.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnStudentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStudentCreated(Models.Gdata.Student item);
        partial void OnAfterStudentCreated(Models.Gdata.Student item);

        public async Task<Models.Gdata.Student> CreateStudent(Models.Gdata.Student student)
        {
            OnStudentCreated(student);

            var existingItem = Context.Students
                              .Where(i => i.ID == student.ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Students.Add(student);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(student).State = EntityState.Detached;
                throw;
            }

            OnAfterStudentCreated(student);

            return student;
        }

        partial void OnCourseDeleted(Models.Gdata.Course item);
        partial void OnAfterCourseDeleted(Models.Gdata.Course item);

        public async Task<Models.Gdata.Course> DeleteCourse(int? courseId)
        {
            var itemToDelete = Context.Courses
                              .Where(i => i.CourseID == courseId)
                              .Include(i => i.CourseInstructors)
                              .Include(i => i.Enrollments)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCourseDeleted(itemToDelete);

            Context.Courses.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCourseDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnCourseGet(Models.Gdata.Course item);

        public async Task<Models.Gdata.Course> GetCourseByCourseId(int? courseId)
        {
            var items = Context.Courses
                              .AsNoTracking()
                              .Where(i => i.CourseID == courseId);

            items = items.Include(i => i.Department);

            var itemToReturn = items.FirstOrDefault();

            OnCourseGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.Gdata.Course> CancelCourseChanges(Models.Gdata.Course item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCourseUpdated(Models.Gdata.Course item);
        partial void OnAfterCourseUpdated(Models.Gdata.Course item);

        public async Task<Models.Gdata.Course> UpdateCourse(int? courseId, Models.Gdata.Course course)
        {
            OnCourseUpdated(course);

            var itemToUpdate = Context.Courses
                              .Where(i => i.CourseID == courseId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(course);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterCourseUpdated(course);

            return course;
        }

        partial void OnCourseInstructorDeleted(Models.Gdata.CourseInstructor item);
        partial void OnAfterCourseInstructorDeleted(Models.Gdata.CourseInstructor item);

        public async Task<Models.Gdata.CourseInstructor> DeleteCourseInstructor(int? coursesCourseId)
        {
            var itemToDelete = Context.CourseInstructors
                              .Where(i => i.CoursesCourseID == coursesCourseId)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCourseInstructorDeleted(itemToDelete);

            Context.CourseInstructors.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCourseInstructorDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnCourseInstructorGet(Models.Gdata.CourseInstructor item);

        public async Task<Models.Gdata.CourseInstructor> GetCourseInstructorByCoursesCourseId(int? coursesCourseId)
        {
            var items = Context.CourseInstructors
                              .AsNoTracking()
                              .Where(i => i.CoursesCourseID == coursesCourseId);

            items = items.Include(i => i.Course);

            items = items.Include(i => i.Instructor);

            var itemToReturn = items.FirstOrDefault();

            OnCourseInstructorGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.Gdata.CourseInstructor> CancelCourseInstructorChanges(Models.Gdata.CourseInstructor item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCourseInstructorUpdated(Models.Gdata.CourseInstructor item);
        partial void OnAfterCourseInstructorUpdated(Models.Gdata.CourseInstructor item);

        public async Task<Models.Gdata.CourseInstructor> UpdateCourseInstructor(int? coursesCourseId, Models.Gdata.CourseInstructor courseInstructor)
        {
            OnCourseInstructorUpdated(courseInstructor);

            var itemToUpdate = Context.CourseInstructors
                              .Where(i => i.CoursesCourseID == coursesCourseId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(courseInstructor);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterCourseInstructorUpdated(courseInstructor);

            return courseInstructor;
        }

        partial void OnDepartmentDeleted(Models.Gdata.Department item);
        partial void OnAfterDepartmentDeleted(Models.Gdata.Department item);

        public async Task<Models.Gdata.Department> DeleteDepartment(int? departmentId)
        {
            var itemToDelete = Context.Departments
                              .Where(i => i.DepartmentID == departmentId)
                              .Include(i => i.Courses)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnDepartmentDeleted(itemToDelete);

            Context.Departments.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterDepartmentDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnDepartmentGet(Models.Gdata.Department item);

        public async Task<Models.Gdata.Department> GetDepartmentByDepartmentId(int? departmentId)
        {
            var items = Context.Departments
                              .AsNoTracking()
                              .Where(i => i.DepartmentID == departmentId);

            items = items.Include(i => i.Instructor);

            var itemToReturn = items.FirstOrDefault();

            OnDepartmentGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.Gdata.Department> CancelDepartmentChanges(Models.Gdata.Department item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnDepartmentUpdated(Models.Gdata.Department item);
        partial void OnAfterDepartmentUpdated(Models.Gdata.Department item);

        public async Task<Models.Gdata.Department> UpdateDepartment(int? departmentId, Models.Gdata.Department department)
        {
            OnDepartmentUpdated(department);

            var itemToUpdate = Context.Departments
                              .Where(i => i.DepartmentID == departmentId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(department);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterDepartmentUpdated(department);

            return department;
        }

        partial void OnEnrollmentDeleted(Models.Gdata.Enrollment item);
        partial void OnAfterEnrollmentDeleted(Models.Gdata.Enrollment item);

        public async Task<Models.Gdata.Enrollment> DeleteEnrollment(int? enrollmentId)
        {
            var itemToDelete = Context.Enrollments
                              .Where(i => i.EnrollmentID == enrollmentId)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnEnrollmentDeleted(itemToDelete);

            Context.Enrollments.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterEnrollmentDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnEnrollmentGet(Models.Gdata.Enrollment item);

        public async Task<Models.Gdata.Enrollment> GetEnrollmentByEnrollmentId(int? enrollmentId)
        {
            var items = Context.Enrollments
                              .AsNoTracking()
                              .Where(i => i.EnrollmentID == enrollmentId);

            items = items.Include(i => i.Course);

            items = items.Include(i => i.Student);

            var itemToReturn = items.FirstOrDefault();

            OnEnrollmentGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.Gdata.Enrollment> CancelEnrollmentChanges(Models.Gdata.Enrollment item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnEnrollmentUpdated(Models.Gdata.Enrollment item);
        partial void OnAfterEnrollmentUpdated(Models.Gdata.Enrollment item);

        public async Task<Models.Gdata.Enrollment> UpdateEnrollment(int? enrollmentId, Models.Gdata.Enrollment enrollment)
        {
            OnEnrollmentUpdated(enrollment);

            var itemToUpdate = Context.Enrollments
                              .Where(i => i.EnrollmentID == enrollmentId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(enrollment);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterEnrollmentUpdated(enrollment);

            return enrollment;
        }

        partial void OnInstructorDeleted(Models.Gdata.Instructor item);
        partial void OnAfterInstructorDeleted(Models.Gdata.Instructor item);

        public async Task<Models.Gdata.Instructor> DeleteInstructor(int? id)
        {
            var itemToDelete = Context.Instructors
                              .Where(i => i.ID == id)
                              .Include(i => i.CourseInstructors)
                              .Include(i => i.Departments)
                              .Include(i => i.OfficeAssignments)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnInstructorDeleted(itemToDelete);

            Context.Instructors.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterInstructorDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnInstructorGet(Models.Gdata.Instructor item);

        public async Task<Models.Gdata.Instructor> GetInstructorById(int? id)
        {
            var items = Context.Instructors
                              .AsNoTracking()
                              .Where(i => i.ID == id);

            var itemToReturn = items.FirstOrDefault();

            OnInstructorGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.Gdata.Instructor> CancelInstructorChanges(Models.Gdata.Instructor item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnInstructorUpdated(Models.Gdata.Instructor item);
        partial void OnAfterInstructorUpdated(Models.Gdata.Instructor item);

        public async Task<Models.Gdata.Instructor> UpdateInstructor(int? id, Models.Gdata.Instructor instructor)
        {
            OnInstructorUpdated(instructor);

            var itemToUpdate = Context.Instructors
                              .Where(i => i.ID == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(instructor);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterInstructorUpdated(instructor);

            return instructor;
        }

        partial void OnOfficeAssignmentDeleted(Models.Gdata.OfficeAssignment item);
        partial void OnAfterOfficeAssignmentDeleted(Models.Gdata.OfficeAssignment item);

        public async Task<Models.Gdata.OfficeAssignment> DeleteOfficeAssignment(int? instructorId)
        {
            var itemToDelete = Context.OfficeAssignments
                              .Where(i => i.InstructorID == instructorId)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnOfficeAssignmentDeleted(itemToDelete);

            Context.OfficeAssignments.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterOfficeAssignmentDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnOfficeAssignmentGet(Models.Gdata.OfficeAssignment item);

        public async Task<Models.Gdata.OfficeAssignment> GetOfficeAssignmentByInstructorId(int? instructorId)
        {
            var items = Context.OfficeAssignments
                              .AsNoTracking()
                              .Where(i => i.InstructorID == instructorId);

            items = items.Include(i => i.Instructor);

            var itemToReturn = items.FirstOrDefault();

            OnOfficeAssignmentGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.Gdata.OfficeAssignment> CancelOfficeAssignmentChanges(Models.Gdata.OfficeAssignment item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnOfficeAssignmentUpdated(Models.Gdata.OfficeAssignment item);
        partial void OnAfterOfficeAssignmentUpdated(Models.Gdata.OfficeAssignment item);

        public async Task<Models.Gdata.OfficeAssignment> UpdateOfficeAssignment(int? instructorId, Models.Gdata.OfficeAssignment officeAssignment)
        {
            OnOfficeAssignmentUpdated(officeAssignment);

            var itemToUpdate = Context.OfficeAssignments
                              .Where(i => i.InstructorID == instructorId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(officeAssignment);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterOfficeAssignmentUpdated(officeAssignment);

            return officeAssignment;
        }

        partial void OnStudentDeleted(Models.Gdata.Student item);
        partial void OnAfterStudentDeleted(Models.Gdata.Student item);

        public async Task<Models.Gdata.Student> DeleteStudent(int? id)
        {
            var itemToDelete = Context.Students
                              .Where(i => i.ID == id)
                              .Include(i => i.Enrollments)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStudentDeleted(itemToDelete);

            Context.Students.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStudentDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnStudentGet(Models.Gdata.Student item);

        public async Task<Models.Gdata.Student> GetStudentById(int? id)
        {
            var items = Context.Students
                              .AsNoTracking()
                              .Where(i => i.ID == id);

            var itemToReturn = items.FirstOrDefault();

            OnStudentGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.Gdata.Student> CancelStudentChanges(Models.Gdata.Student item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStudentUpdated(Models.Gdata.Student item);
        partial void OnAfterStudentUpdated(Models.Gdata.Student item);

        public async Task<Models.Gdata.Student> UpdateStudent(int? id, Models.Gdata.Student student)
        {
            OnStudentUpdated(student);

            var itemToUpdate = Context.Students
                              .Where(i => i.ID == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(student);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterStudentUpdated(student);

            return student;
        }
    }
}
