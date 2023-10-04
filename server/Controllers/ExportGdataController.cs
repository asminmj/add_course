using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using G.Data;

namespace G
{
    public partial class ExportGdataController : ExportController
    {
        private readonly GdataContext context;
        private readonly GdataService service;
        public ExportGdataController(GdataContext context, GdataService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/Gdata/courses/csv")]
        [HttpGet("/export/Gdata/courses/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCoursesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCourses(), Request.Query), fileName);
        }

        [HttpGet("/export/Gdata/courses/excel")]
        [HttpGet("/export/Gdata/courses/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCoursesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCourses(), Request.Query), fileName);
        }
        [HttpGet("/export/Gdata/courseinstructors/csv")]
        [HttpGet("/export/Gdata/courseinstructors/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCourseInstructorsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCourseInstructors(), Request.Query), fileName);
        }

        [HttpGet("/export/Gdata/courseinstructors/excel")]
        [HttpGet("/export/Gdata/courseinstructors/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCourseInstructorsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCourseInstructors(), Request.Query), fileName);
        }
        [HttpGet("/export/Gdata/departments/csv")]
        [HttpGet("/export/Gdata/departments/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportDepartmentsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetDepartments(), Request.Query), fileName);
        }

        [HttpGet("/export/Gdata/departments/excel")]
        [HttpGet("/export/Gdata/departments/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportDepartmentsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetDepartments(), Request.Query), fileName);
        }
        [HttpGet("/export/Gdata/enrollments/csv")]
        [HttpGet("/export/Gdata/enrollments/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportEnrollmentsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetEnrollments(), Request.Query), fileName);
        }

        [HttpGet("/export/Gdata/enrollments/excel")]
        [HttpGet("/export/Gdata/enrollments/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportEnrollmentsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetEnrollments(), Request.Query), fileName);
        }
        [HttpGet("/export/Gdata/instructors/csv")]
        [HttpGet("/export/Gdata/instructors/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportInstructorsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetInstructors(), Request.Query), fileName);
        }

        [HttpGet("/export/Gdata/instructors/excel")]
        [HttpGet("/export/Gdata/instructors/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportInstructorsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetInstructors(), Request.Query), fileName);
        }
        [HttpGet("/export/Gdata/officeassignments/csv")]
        [HttpGet("/export/Gdata/officeassignments/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportOfficeAssignmentsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetOfficeAssignments(), Request.Query), fileName);
        }

        [HttpGet("/export/Gdata/officeassignments/excel")]
        [HttpGet("/export/Gdata/officeassignments/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportOfficeAssignmentsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetOfficeAssignments(), Request.Query), fileName);
        }
        [HttpGet("/export/Gdata/students/csv")]
        [HttpGet("/export/Gdata/students/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportStudentsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStudents(), Request.Query), fileName);
        }

        [HttpGet("/export/Gdata/students/excel")]
        [HttpGet("/export/Gdata/students/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportStudentsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStudents(), Request.Query), fileName);
        }
    }
}
