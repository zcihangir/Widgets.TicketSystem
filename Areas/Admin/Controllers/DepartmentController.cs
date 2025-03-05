using Grand.Business.Core.Interfaces.Common.Localization;
using Grand.Business.Core.Interfaces.Common.Security;
using Grand.Business.Core.Interfaces.Common.Stores;
using Grand.Domain.Permissions;
using Grand.Web.Common.Controllers;
using Grand.Web.Common.Filters;
using Grand.Web.Common.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Widgets.TicketSystem.Domain;
using Widgets.TicketSystem.Models;
using Widgets.TicketSystem.Services;
using StandardPermission = Widgets.TicketSystem.Infrastructure.StandardPermission;

namespace Widgets.TicketSystem.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.Widgets)]
    public class DepartmentController : BaseAdminPluginController
    {
        #region Fields
        private readonly IDepartmentService _departmentService;
        private readonly ITranslationService _translationService;
        private readonly IPermissionService _permissionService;
        private readonly IStoreService _storeService;
        #endregion

        #region Ctor
        public DepartmentController(
            IDepartmentService departmentService,
            ITranslationService translationService,
            IPermissionService permissionService,
            IStoreService storeService)
        {
            _departmentService = departmentService;
            _translationService = translationService;
            _permissionService = permissionService;
            _storeService = storeService;
        }
        #endregion

        #region Utilities
        protected async Task<bool> HasAccessToDepartments()
        {
            return await _permissionService.Authorize(StandardPermission.ManageTickets);
        }
        
     
        #endregion

        #region Departments
        public async Task<IActionResult> List()
        {
            if (!await HasAccessToDepartments())
                return AccessDeniedView();

            var model = new DepartmentListModel();
            
            // Mağaza seçeneklerini doldur
            var stores = await _storeService.GetAllStores();
            model.AvailableStores.Add(new SelectListItem { Text = _translationService.GetResource("Admin.Common.All"), Value = "" });
            foreach (var store in stores)
            {
                model.AvailableStores.Add(new SelectListItem
                {
                    Text = store.Name,
                    Value = store.Id
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DepartmentList(DepartmentListModel model)
        {
            if (!await HasAccessToDepartments())
                return AccessDeniedView();

            var departments = await _departmentService.GetAllDepartments(
                storeId: model.SearchStoreId,
                active: model.SearchActive,
                name: model.SearchName,
                pageIndex: model.PageIndex,
                pageSize: model.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = departments.Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Active = x.Active,
                    DisplayOrder = x.DisplayOrder,
                    LimitedToStores = false,
                    CreatedOn = x.CreatedOnUtc.ToString("g")
                }),
                Total = departments.TotalCount
            };

            return Json(gridModel);
        }

        public async Task<IActionResult> Create()
        {
            if (!await HasAccessToDepartments())
                return AccessDeniedView();

            var model = new DepartmentModel
            {
                Active = true,
                DisplayOrder = 0,
                CreatedOnUtc = DateTime.UtcNow
            };

            // Mağaza seçeneklerini doldur

            return View(model);
        }

        [HttpPost, ArgumentNameFilter(KeyName = "save-continue", Argument = "continueEditing")]
        public async Task<IActionResult> Create(DepartmentModel model, bool continueEditing)
        {
            if (!await HasAccessToDepartments())
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var department = new Department
                {
                    Name = model.Name,
                    Description = model.Description,
                    Active = model.Active,
                    DisplayOrder = model.DisplayOrder,
                    LimitedToStores = model.LimitedToStores,
                    CreatedOnUtc = DateTime.UtcNow
                };

                await _departmentService.InsertDepartment(department);

                Success(_translationService.GetResource("Plugins.Widgets.TicketSystem.Department.Added"));

                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = department.Id });
                }
                return RedirectToAction("List");
            }

            // Mağaza seçeneklerini doldur

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (!await HasAccessToDepartments())
                return AccessDeniedView();

            var department = await _departmentService.GetDepartmentById(id);
            if (department == null)
                return RedirectToAction("List");

            var model = new DepartmentModel
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                Active = department.Active,
                DisplayOrder = department.DisplayOrder,
                LimitedToStores = department.LimitedToStores,
                CreatedOnUtc = department.CreatedOnUtc,
                CreatedOn = department.CreatedOnUtc.ToString("g")
            };

            // Mağaza seçeneklerini doldur

            return View(model);
        }

        [HttpPost, ArgumentNameFilter(KeyName = "save-continue", Argument = "continueEditing")]
        public async Task<IActionResult> Edit(DepartmentModel model, bool continueEditing)
        {
            if (!await HasAccessToDepartments())
                return AccessDeniedView();

            var department = await _departmentService.GetDepartmentById(model.Id);
            if (department == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                department.Name = model.Name;
                department.Description = model.Description;
                department.Active = model.Active;
                department.DisplayOrder = model.DisplayOrder;
                department.LimitedToStores = model.LimitedToStores;

                await _departmentService.UpdateDepartment(department);

                Success(_translationService.GetResource("Plugins.Widgets.TicketSystem.Department.Updated"));

                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = department.Id });
                }
                return RedirectToAction("List");
            }

            // Mağaza seçeneklerini doldur

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!await HasAccessToDepartments())
                return AccessDeniedView();

            var department = await _departmentService.GetDepartmentById(id);
            if (department == null)
                return RedirectToAction("List");

            await _departmentService.DeleteDepartment(department);

            Success(_translationService.GetResource("Plugins.Widgets.TicketSystem.Department.Deleted"));

            return RedirectToAction("List");
        }
        #endregion
    }
} 