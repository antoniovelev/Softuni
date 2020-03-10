namespace OverGas-Service.Web.Areas.Administration.Controllers
{
    using OverGas-Service.Common;
    using OverGas-Service.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
