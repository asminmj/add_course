using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using G.Models.Gdata;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using G.Models;

namespace G.Pages
{
    public partial class CourseComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        protected GdataService Gdata { get; set; }
        protected RadzenDataGrid<G.Models.Gdata.Course> grid0;

        bool _hasChanges;
        protected bool hasChanges
        {
            get
            {
                return _hasChanges;
            }
            set
            {
                if (!object.Equals(_hasChanges, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "hasChanges", NewValue = value, OldValue = _hasChanges };
                    _hasChanges = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        bool _canEdit;
        protected bool canEdit
        {
            get
            {
                return _canEdit;
            }
            set
            {
                if (!object.Equals(_canEdit, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "canEdit", NewValue = value, OldValue = _canEdit };
                    _canEdit = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<G.Models.Gdata.Department> _getDepartmentsResult;
        protected IEnumerable<G.Models.Gdata.Department> getDepartmentsResult
        {
            get
            {
                return _getDepartmentsResult;
            }
            set
            {
                if (!object.Equals(_getDepartmentsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getDepartmentsResult", NewValue = value, OldValue = _getDepartmentsResult };
                    _getDepartmentsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<G.Models.Gdata.Course> _getCoursesResult;
        protected IEnumerable<G.Models.Gdata.Course> getCoursesResult
        {
            get
            {
                return _getCoursesResult;
            }
            set
            {
                if (!object.Equals(_getCoursesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getCoursesResult", NewValue = value, OldValue = _getCoursesResult };
                    _getCoursesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        G.Models.Gdata.Course _course;
        protected G.Models.Gdata.Course course
        {
            get
            {
                return _course;
            }
            set
            {
                if (!object.Equals(_course, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "course", NewValue = value, OldValue = _course };
                    _course = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        bool _isEdit;
        protected bool isEdit
        {
            get
            {
                return _isEdit;
            }
            set
            {
                if (!object.Equals(_isEdit, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "isEdit", NewValue = value, OldValue = _isEdit };
                    _isEdit = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Security.InitializeAsync(AuthenticationStateProvider);
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                await Load();
            }
        }
        protected async System.Threading.Tasks.Task Load()
        {
            hasChanges = false;

            canEdit = true;

            var gdataGetDepartmentsResult = await Gdata.GetDepartments();
            getDepartmentsResult = gdataGetDepartmentsResult;

            var gdataGetCoursesResult = await Gdata.GetCourses(new Query() { Expand = "Enrollments,Department,CourseInstructors" });
            getCoursesResult = gdataGetCoursesResult;

            course = gdataGetCoursesResult.FirstOrDefault();

            isEdit = true;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            course = new G.Models.Gdata.Course();

            isEdit = false;
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(G.Models.Gdata.Course args)
        {
            isEdit = true;

            course = args;
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var gdataDeleteCourseResult = await Gdata.DeleteCourse(data.CourseID);
                    if (gdataDeleteCourseResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception gdataDeleteCourseException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Course" });
            }
        }

        protected async System.Threading.Tasks.Task Form0Submit(G.Models.Gdata.Course args)
        {
            try
            {
                if (isEdit)
                {
                    var gdataUpdateCourseResult = await Gdata.UpdateCourse(course.CourseID, course);
                        NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Success,Summary = $"Success",Detail = $"Course updated!" });


                }
            }
            catch (System.Exception gdataUpdateCourseException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update Course" });

            hasChanges = gdataUpdateCourseException is DbUpdateConcurrencyException;
            }

            try
            {
                if (!this.isEdit)
                {
                    var gdataCreateCourseResult = await Gdata.CreateCourse(args);
                    course = new G.Models.Gdata.Course();

                        NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Success,Summary = $"Success",Detail = $"Course created!" });
                }
            }
            catch (System.Exception gdataCreateCourseException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to create new Course!" });
            }
        }
    }
}
