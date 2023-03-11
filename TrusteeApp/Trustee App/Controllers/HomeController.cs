using System.Reflection;
using System.Text.Json;
using log4net;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using TrusteeApp;
using TrusteeApp.Domain.Dtos;
using TrusteeApp.Errors;
using TrusteeApp.Filters;
using TrusteeApp.Models;
using TrusteeApp.Repo;
using TrusteeApp.Services;
using TrusteeApp.ViewModels;

namespace TrusteeApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;


        public HomeController(IConfiguration configuration, IHttpClientFactory httpClientFactory, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }

        [ViewLayout("_IndexLayout")]
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                var Email = ControllerHelper.GetAppUserFromHttpContext(HttpContext);
                var user = await _userManager.FindByEmailAsync(Email!);

                if (user == null) throw new Exception("Invalid login attempt.");

                if (!user!.EmailConfirmed) return RedirectToAction("RegisterConfirmation", "Account", new { Email });

                var userId = Repository<ApplicationUserDto>.Find(u => u.Email == Email, WebConstants.ApplicationUser).Id;

                var products = new List<TrusteePackage>();

                var existingProducts = Repository<TrusteePackage>.GetAll(WebConstants.TrusteePackage, u => u.OwnerId == userId);

                if (existingProducts != null) products = existingProducts;

                return View(products);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Submit Trust Package \r\n Unable to Submit Trust Package";

                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                return RedirectToAction("Login", "Account");
            }
        }

        [ViewLayout("_IndexLayout")]
        [HttpGet]
        public IActionResult CreateProduct() => View();

        [HttpPost]
        public IActionResult CreateProductPost(TrusteePackage trusteePackage)
        {
            try
            {
                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var userId = Repository<ApplicationUserDto>.Find(u => u.Email == userEmail, WebConstants.ApplicationUser).Id;

                trusteePackage.Id = ControllerHelper.GenerateReferenceNumberByGuid();
                trusteePackage.OwnerEmail = userEmail;
                trusteePackage.CreateDate = DateTime.Now;
                trusteePackage.PackageStatus = "NEW";
                trusteePackage.OwnerId = userId;

                var isSuccessful = RoutesController<TrusteePackage>.PostDbSet(trusteePackage, WebConstants.TrusteePackage);

                if (isSuccessful) return RedirectToAction("ContactDetails", new { trusteePackage.Id });

                else throw new Exception($"Was unable to create new package.");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Submit Contact Details\r\n Unable to Submit Contact Details";

                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                return RedirectToAction("CreateProduct");
            }
        }

        [HttpGet]
        [ViewLayout("_KycLayout")]
        public IActionResult ContactDetails(string id)
        {
            try
            {
                if (id == null) throw new Exception("Couldn't find package Id.");

                ViewBag.CurrentState = 1;

                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var userId = Repository<ApplicationUserDto>.Find(u => u.Email == userEmail, WebConstants.ApplicationUser).Id;

                var contactDetail = new UserContactDetails
                {
                    PackageId = id!
                };

                var existingContactDetails = Repository<UserContactDetails>.Find(u => u.PackageId == id, WebConstants.UserContactDetails);

                if (existingContactDetails != null) contactDetail = existingContactDetails;

                return View(contactDetail);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Submit Trust Package \r\n Unable to Submit Trust Package";

                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult SaveContactDetails(UserContactDetails userContactDetails)
        {
            try
            {
                var isSuccessful = RoutesController<UserContactDetails>.UpdateDbSet(userContactDetails, WebConstants.UserContactDetails, "Id", userContactDetails.PackageId!);

                if (isSuccessful) return RedirectToAction("MeansOfIdentification", new { userContactDetails.PackageId, userContactDetails });

                else throw new Exception($"Was unable to save contact details for package {userContactDetails.PackageId}");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Submit Contact Details\r\n Unable to Submit Contact Details";

                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                return RedirectToAction("ContactDetails", new { userContactDetails.PackageId });
            }
        }

        [HttpGet]
        [ViewLayout("_KycLayout")]
        public IActionResult MeansOfIdentification(string packageId)
        {
            try
            {
                ViewBag.CurrentState = 1;

                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);
                var userId = Repository<ApplicationUserDto>.Find(u => u.Email == userEmail, WebConstants.ApplicationUser).Id;

                var meansOfIdentification = new MeansOfIdentification
                {
                    PackageId = packageId
                };

                var existingTrusteePackage = Repository<TrusteePackage>.Find(u => u.Id == packageId, WebConstants.TrusteePackage);

                if (existingTrusteePackage == null) throw new Exception($"No Trustee package found for package {packageId}.");

                var existingMeansOfIdentification = Repository<MeansOfIdentification>.Find(u => u.PackageId == existingTrusteePackage.Id, WebConstants.MeansOfIdentification);

                if (existingMeansOfIdentification != null) meansOfIdentification = existingMeansOfIdentification;

                return View(meansOfIdentification);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Fetch Means of Identification page \r\n Trustee package not available for signed user.";

                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult SaveMeansOfIdentification(MeansOfIdentification meansOfIdentification)
        {
            try
            {
                var isSuccessful = RoutesController<MeansOfIdentification>.UpdateDbSet(meansOfIdentification, WebConstants.MeansOfIdentification, "Id", meansOfIdentification.PackageId!);

                if (isSuccessful) return RedirectToAction("SimpleWillMoney", new { meansOfIdentification.PackageId });

                else throw new Exception($"Was unable to save identification details of package {meansOfIdentification.PackageId}");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Submit Identification Details\r\n Unable to Submit Identification Details";

                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                return RedirectToAction("MeansOfIdentification", new { meansOfIdentification.PackageId });
            }
        }

        [HttpGet]
        [ViewLayout("_WillLayout")]
        public IActionResult SimpleWillMoney(string packageId)
        {
            try
            {
                ViewBag.CurrentState = 2;

                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var userId = Repository<ApplicationUserDto>.Find(u => u.Email == userEmail, WebConstants.ApplicationUser).Id;

                var existingTrusteePackage = Repository<TrusteePackage>.Find(u => u.Id == packageId, WebConstants.TrusteePackage);

                if (existingTrusteePackage == null) throw new Exception($"No Trustee package found for package {packageId}.");

                var simpleWillMoney = new SimpleWillMoneyVM
                {
                    TrusteePackage = existingTrusteePackage,

                    SimpleWillMoneyAccountDetails = new List<SimpleWillMoneyAccountDetails>
                    {
                        new SimpleWillMoneyAccountDetails
                        {
                            PackageId = packageId,
                        }
                    },

                    SimpleWillMoneyBeneficiary = new List<SimpleWillMoneyBeneficiary>
                    {
                        new SimpleWillMoneyBeneficiary
                        {
                            PackageId = packageId
                        }
                    }
                };

                var existingSimpleWillAccountDetails = Repository<SimpleWillMoneyAccountDetails>.GetAll(WebConstants.SimpleWillMoneyAccountDetails, u => u.PackageId == existingTrusteePackage.Id);

                if (existingSimpleWillAccountDetails != null && existingSimpleWillAccountDetails.Count() > 0)
                {
                    simpleWillMoney.SimpleWillMoneyAccountDetails = existingSimpleWillAccountDetails;

                    var existingSimpleWillMoneyBeneficiary = Repository<SimpleWillMoneyBeneficiary>.GetAll(WebConstants.SimpleWillMoneyBeneficiary, u => u.PackageId == existingTrusteePackage.Id);

                    if (existingSimpleWillMoneyBeneficiary != null && existingSimpleWillMoneyBeneficiary.Count() > 0) simpleWillMoney.SimpleWillMoneyBeneficiary = existingSimpleWillMoneyBeneficiary;
                }

                return View(simpleWillMoney);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Fetch SimpleWill Money page \r\n Trustee package not available for signed user.";

                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult SaveSimpleWillMoney([FromBody] SimpleWillMoneyVM simpleWillMoney)
        {
            try
            {
                bool isSuccessful = true;

                var existingSimpleWillAccountDetails = Repository<SimpleWillMoneyAccountDetails>.GetAll(WebConstants.SimpleWillMoneyAccountDetails, u => u.PackageId == simpleWillMoney.TrusteePackage.Id);

                if (existingSimpleWillAccountDetails != null && existingSimpleWillAccountDetails.Count() > 0)
                {
                    foreach (var item in existingSimpleWillAccountDetails)
                    {
                        isSuccessful = RoutesController<SimpleWillMoneyAccountDetails>.DeleteDbSetMod("Id", simpleWillMoney.TrusteePackage.Id!, WebConstants.SimpleWillMoneyAccountDetails);
                    }
                }

                var existingSimpleWillMoneyBeneficiary = Repository<SimpleWillMoneyBeneficiary>.GetAll(WebConstants.SimpleWillMoneyBeneficiary, u => u.PackageId == simpleWillMoney.TrusteePackage.Id);

                if (existingSimpleWillMoneyBeneficiary != null && existingSimpleWillMoneyBeneficiary.Count() > 0)
                {
                    foreach (var item in existingSimpleWillMoneyBeneficiary)
                    {
                        isSuccessful = RoutesController<SimpleWillMoneyBeneficiary>.DeleteDbSetMod("Id", simpleWillMoney.TrusteePackage.Id!, WebConstants.SimpleWillMoneyBeneficiary);
                    }
                }

                if (isSuccessful)
                {
                    foreach (var accountDetail in simpleWillMoney.SimpleWillMoneyAccountDetails)
                    {
                        isSuccessful = RoutesController<SimpleWillMoneyAccountDetails>.PostDbSet(accountDetail, WebConstants.SimpleWillMoneyAccountDetails);
                    }

                    if (isSuccessful)
                    {
                        foreach (var benDetail in simpleWillMoney.SimpleWillMoneyBeneficiary)
                        {
                            isSuccessful = RoutesController<SimpleWillMoneyBeneficiary>.PostDbSet(benDetail, WebConstants.SimpleWillMoneyBeneficiary);
                        }

                        //if (isSuccessful) return RedirectToAction("SimpleWillAccount", new { simpleWillMoney.TrusteePackage.Id, simpleWillMoney.TrusteePackage.PackageType });
                        if (isSuccessful)
                        {
                            return StatusCode(201, new
                            {
                                RequestedAction = "Save SimpleWill Account Details",
                                OperationResult = "Saved"
                            });
                        }
                        else throw new Exception($"Was unable to save simpleWill beneficiary details of package {simpleWillMoney.TrusteePackage.Id}");
                    }
                    else throw new Exception($"Was unable to save simpleWill money account details of package {simpleWillMoney.TrusteePackage.Id}");
                }

                else throw new Exception($"Was unable to save simpleWill account details of package {simpleWillMoney.TrusteePackage.Id}");
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SavedSimpleWillAccountError"
                });
            }
        }

        [HttpGet]
        [ViewLayout("_WillLayout")]
        public IActionResult SimpleWillAccount(string packageId)
        {
            try
            {
                ViewBag.CurrentState = 2;

                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var userId = Repository<ApplicationUserDto>.Find(u => u.Email == userEmail, WebConstants.ApplicationUser).Id;

                var existingTrusteePackage = Repository<TrusteePackage>.Find(u => u.Id == packageId, WebConstants.TrusteePackage);

                if (existingTrusteePackage == null) throw new Exception($"No Trustee package found for package {packageId}.");

                var simpleWillAccount = new SimpleWillAccountVM
                {
                    TrusteePackage = existingTrusteePackage,

                    SimpleWillRetirementAccount = new List<SimpleWillRetirementAccount>
                    {
                        new SimpleWillRetirementAccount
                        {
                            PackageId = packageId,
                        }
                    },

                    SimpleWillRetirementBeneficiary = new List<SimpleWillRetirementBeneficiary>
                    {
                        new SimpleWillRetirementBeneficiary
                        {
                            PackageId = packageId
                        }
                    }
                };

                var existingSimpleWillRetirementAccount = Repository<SimpleWillRetirementAccount>.GetAll(WebConstants.SimpleWillRetirementAccount, u => u.PackageId == existingTrusteePackage.Id);

                if (existingSimpleWillRetirementAccount != null && existingSimpleWillRetirementAccount.Count() > 0)
                {
                    simpleWillAccount.SimpleWillRetirementAccount = existingSimpleWillRetirementAccount;

                    var existingSimpleWillMoneyBeneficiary = Repository<SimpleWillRetirementBeneficiary>.GetAll(WebConstants.SimpleWillRetirementBeneficiary, u => u.PackageId == existingTrusteePackage.Id);

                    if (existingSimpleWillMoneyBeneficiary != null && existingSimpleWillMoneyBeneficiary.Count() > 0) simpleWillAccount.SimpleWillRetirementBeneficiary = existingSimpleWillMoneyBeneficiary;
                }

                return View(simpleWillAccount);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Fetch SimpleWill Account page \r\n Trustee package not available for signed user.";

                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult SaveSimpleWillAccount([FromBody] SimpleWillAccountVM simpleWillAccount)
        {
            try
            {
                bool isSuccessful = true;

                var existingSimpleWillRetirementAccount = Repository<SimpleWillRetirementAccount>.GetAll(WebConstants.SimpleWillRetirementAccount, u => u.PackageId == simpleWillAccount.TrusteePackage.Id);

                if (existingSimpleWillRetirementAccount != null && existingSimpleWillRetirementAccount.Count() > 0)
                {
                    foreach (var item in existingSimpleWillRetirementAccount)
                    {
                        isSuccessful = RoutesController<SimpleWillRetirementAccount>.DeleteDbSetMod("Id", simpleWillAccount.TrusteePackage.Id!, WebConstants.SimpleWillRetirementAccount);
                    }
                }

                var existingSimpleWillRetirementBeneficiary = Repository<SimpleWillRetirementBeneficiary>.GetAll(WebConstants.SimpleWillRetirementBeneficiary, u => u.PackageId == simpleWillAccount.TrusteePackage.Id);

                if (existingSimpleWillRetirementBeneficiary != null && existingSimpleWillRetirementBeneficiary.Count() > 0)
                {
                    foreach (var item in existingSimpleWillRetirementBeneficiary)
                    {
                        isSuccessful = RoutesController<SimpleWillRetirementBeneficiary>.DeleteDbSetMod("Id", simpleWillAccount.TrusteePackage.Id!, WebConstants.SimpleWillRetirementBeneficiary);
                    }
                }

                if (isSuccessful)
                {
                    foreach (var accountDetail in simpleWillAccount.SimpleWillRetirementAccount)
                    {
                        isSuccessful = RoutesController<SimpleWillRetirementAccount>.PostDbSet(accountDetail, WebConstants.SimpleWillRetirementAccount);
                    }

                    if (isSuccessful)
                    {
                        foreach (var benDetail in simpleWillAccount.SimpleWillRetirementBeneficiary)
                        {
                            isSuccessful = RoutesController<SimpleWillRetirementBeneficiary>.PostDbSet(benDetail, WebConstants.SimpleWillRetirementBeneficiary);
                        }

                        if (isSuccessful)
                        {
                            return StatusCode(201, new
                            {
                                RequestedAction = "Save SimpleWill Account Details",
                                OperationResult = "Saved"
                            });
                        }
                        else throw new Exception($"Was unable to save simpleWill beneficiary details of package {simpleWillAccount.TrusteePackage.Id}");
                    }
                    else throw new Exception($"Was unable to save simpleWill retirement account details of package {simpleWillAccount.TrusteePackage.Id}");
                }
                else throw new Exception($"Was unable to save simpleWill retirement details of package {simpleWillAccount.TrusteePackage.Id}");
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SavedSimpleWillAccountError"
                });
            }
        }

        [HttpGet]
        [ViewLayout("_WillLayout")]
        public IActionResult SimpleWillOthers(string packageId)
        {
            try
            {
                ViewBag.CurrentState = 2;

                var userEmail = ControllerHelper.GetAppUserFromHttpContext(HttpContext);

                var userId = Repository<ApplicationUserDto>.Find(u => u.Email == userEmail, WebConstants.ApplicationUser).Id;

                var existingTrusteePackage = Repository<TrusteePackage>.Find(u => u.Id == packageId, WebConstants.TrusteePackage);

                if (existingTrusteePackage == null) throw new Exception($"No Trustee package found for package {packageId}.");

                var simpleWillOthers = new SimpleWillOthersVM
                {
                    TrusteePackage = existingTrusteePackage,

                    SimpleWillOthers = new SimpleWillOthers
                    {
                        PackageId = packageId
                    },

                    SimpleWillGuardians = new List<SimpleWillGuardian>
                    {
                        new SimpleWillGuardian
                        {
                            PackageId = packageId,
                        }
                    },

                    SimpleWillExecutors = new List<SimpleWillExecutor>
                    {
                        new SimpleWillExecutor
                        {
                            PackageId = packageId
                        }
                    }
                };

                var existingSimpleWillOthers = Repository<SimpleWillOthers>.Find(u => u.PackageId == existingTrusteePackage.Id, WebConstants.SimpleWillOthers);

                if (existingSimpleWillOthers != null)
                {
                    simpleWillOthers.SimpleWillOthers = existingSimpleWillOthers;

                    var existingSimpleWillGuardian = Repository<SimpleWillGuardian>.GetAll(WebConstants.SimpleWillGuardian, u => u.PackageId == existingTrusteePackage.Id);

                    var existingSimpleWillExecutor = Repository<SimpleWillExecutor>.GetAll(WebConstants.SimpleWillExecutor, u => u.PackageId == existingTrusteePackage.Id);

                    if (existingSimpleWillGuardian != null && existingSimpleWillGuardian.Count() > 0) simpleWillOthers.SimpleWillGuardians = existingSimpleWillGuardian;

                    if (existingSimpleWillExecutor != null && existingSimpleWillExecutor.Count() > 0) simpleWillOthers.SimpleWillExecutors = existingSimpleWillExecutor;
                }

                return View(simpleWillOthers);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Fetch SimpleWill Others page \r\n Trustee package not available for signed user.";

                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult SaveSimpleWillOthers([FromBody] SimpleWillOthersVM simpleWillOthersVm)
        {
            try
            {
                bool isSuccessful = true;

                var existingSimpleWillOthers = Repository<SimpleWillOthers>.GetAll(WebConstants.SimpleWillOthers, u => u.PackageId == simpleWillOthersVm.TrusteePackage.Id);

                if (existingSimpleWillOthers != null && existingSimpleWillOthers.Count() > 0)
                {
                    foreach (var item in existingSimpleWillOthers)
                    {
                        isSuccessful = RoutesController<SimpleWillOthers>.DeleteDbSetMod("Id", simpleWillOthersVm.TrusteePackage.Id!, WebConstants.SimpleWillOthers);
                    }
                }

                var existingSimpleWillExecutor = Repository<SimpleWillExecutor>.GetAll(WebConstants.SimpleWillExecutor, u => u.PackageId == simpleWillOthersVm.TrusteePackage.Id);

                if (existingSimpleWillExecutor != null && existingSimpleWillExecutor.Count() > 0)
                {
                    foreach (var item in existingSimpleWillExecutor)
                    {
                        isSuccessful = RoutesController<SimpleWillExecutor>.DeleteDbSetMod("Id", simpleWillOthersVm.TrusteePackage.Id!, WebConstants.SimpleWillExecutor);
                    }
                }

                var existingSimpleWillGuardian = Repository<SimpleWillGuardian>.GetAll(WebConstants.SimpleWillGuardian, u => u.PackageId == simpleWillOthersVm.TrusteePackage.Id);

                if (existingSimpleWillGuardian != null && existingSimpleWillGuardian.Count() > 0)
                {
                    foreach (var item in existingSimpleWillGuardian)
                    {
                        isSuccessful = RoutesController<SimpleWillGuardian>.DeleteDbSetMod("Id", simpleWillOthersVm.TrusteePackage.Id!, WebConstants.SimpleWillGuardian);
                    }
                }

                if (isSuccessful)
                {
                    isSuccessful = RoutesController<SimpleWillOthers>.UpdateDbSet(simpleWillOthersVm.SimpleWillOthers, WebConstants.SimpleWillOthers, "Id", simpleWillOthersVm.TrusteePackage.Id!);

                    if (isSuccessful)
                    {
                        foreach (var executor in simpleWillOthersVm.SimpleWillExecutors)
                        {
                            isSuccessful = RoutesController<SimpleWillExecutor>.UpdateDbSet(executor, WebConstants.SimpleWillExecutor, "Id", simpleWillOthersVm.TrusteePackage.Id!);
                        }

                        if (isSuccessful)
                        {
                            foreach (var guardian in simpleWillOthersVm.SimpleWillGuardians)
                            {
                                isSuccessful = RoutesController<SimpleWillGuardian>.UpdateDbSet(guardian, WebConstants.SimpleWillGuardian, "Id", simpleWillOthersVm.TrusteePackage.Id!);
                            }

                            if (isSuccessful)
                            {
                                var existingTrusteePackage = Repository<TrusteePackage>.Find(u => u.Id == simpleWillOthersVm.TrusteePackage.Id, WebConstants.TrusteePackage);

                                if (existingTrusteePackage != null)
                                {
                                    existingTrusteePackage.PackageStatus = "DRAFT";

                                    isSuccessful = RoutesController<TrusteePackage>.UpdateDbSet(existingTrusteePackage, WebConstants.TrusteePackage, "Id", simpleWillOthersVm.TrusteePackage.Id!);

                                    if (isSuccessful)
                                    {
                                        return StatusCode(201, new
                                        {
                                            RequestedAction = "Save SimpleWill Other Details",
                                            OperationResult = "Saved"
                                        });
                                    }

                                    else throw new Exception($"Was unable to update product status of user ${simpleWillOthersVm.TrusteePackage.OwnerId} with package {simpleWillOthersVm.TrusteePackage.Id}");
                                }

                                else throw new Exception($"Was unable to update product status of user ${simpleWillOthersVm.TrusteePackage.OwnerId} with package {simpleWillOthersVm.TrusteePackage.Id}");
                            }

                            else throw new Exception($"Was unable to save simpleWill guardian(s) details of user ${simpleWillOthersVm.TrusteePackage.OwnerId} with package {simpleWillOthersVm.TrusteePackage.Id}");
                        }

                        else throw new Exception($"Was unable to save simpleWill executor(s) details of user ${simpleWillOthersVm.TrusteePackage.OwnerId} with package {simpleWillOthersVm.TrusteePackage.Id}");
                    }

                    else throw new Exception($"Was unable to save other SimpleWill details of user ${simpleWillOthersVm.TrusteePackage.OwnerId} with package {simpleWillOthersVm.TrusteePackage.Id}");
                }

                else throw new Exception($"Was unable to save other SimpleWill details of user ${simpleWillOthersVm.TrusteePackage.OwnerId} with package {simpleWillOthersVm.TrusteePackage.Id}");
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

                return StatusCode(500, new
                {
                    ErrorDescription = ex.Message,
                    ExceptionType = "SavedSimpleWillothersError"
                });
            }
        }

        public IActionResult Summary(string packageId)
        {
            ViewBag.CurrentState = 3;

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [ViewLayout("_LoginLayout")]
        public IActionResult Error([FromQuery] string errorcode, string errortype, string message, string detail)
        {
            TempData["Error"] = $"Error message\r\n {message}.";

            ViewBag.ShowLayout = false;

            return View(new ApiExceptionsResponse(errorcode, errortype, message, detail));
        }
    }
}






//catch (Exception ex)
//{
//    TempData["Error"] = "Submit SimpleWill Money Details\r\n Unable to Submit SimpleWill Money Details";

//    log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

//    return RedirectToAction("SimpleWillMoney", new { simpleWillMoney.TrusteePackage.Id, simpleWillMoney.TrusteePackage.PackageType });
//}



//[HttpPost]
//public IActionResult SaveSimpleWillMoney(SimpleWillMoneyVM simpleWillMoney)
//{
//    try
//    {
//        bool isSuccessful = false;

//        foreach (var accountDetail in simpleWillMoney.SimpleWillMoneyAccountDetails)
//        {
//            isSuccessful = RoutesController<SimpleWillMoneyAccountDetails>.UpdateDbSet(accountDetail, WebConstants.SimpleWillMoneyAccountDetails, "Id", simpleWillMoney.TrusteePackage.Id);
//        }

//        if (isSuccessful)
//        {
//            foreach (var benDetail in simpleWillMoney.SimpleWillMoneyBeneficiary)
//            {
//                isSuccessful = RoutesController<SimpleWillMoneyBeneficiary>.UpdateDbSet(benDetail, WebConstants.SimpleWillMoneyBeneficiary, "Id", simpleWillMoney.TrusteePackage.Id);
//            }

//            if (isSuccessful) return RedirectToAction("SimpleWillAccount", new { simpleWillMoney.TrusteePackage.Id, simpleWillMoney.TrusteePackage.PackageType });

//            else throw new Exception($"Was unable to save simpleWill beneficiary details of user ${simpleWillMoney.TrusteePackage.OwnerId} with package {simpleWillMoney.TrusteePackage.Id}");
//        }

//        else throw new Exception($"Was unable to save simpleWill money account details of user ${simpleWillMoney.TrusteePackage.OwnerId} with package {simpleWillMoney.TrusteePackage.Id}");
//    }
//    catch (Exception ex)
//    {
//        TempData["Error"] = "Submit SimpleWill Money Details\r\n Unable to Submit SimpleWill Money Details";

//        log.Error(DateTime.Now.ToString(), ex); LogWriterController.Write(ex.Message);

//        return RedirectToAction("SimpleWillMoney", new { simpleWillMoney.TrusteePackage.Id, simpleWillMoney.TrusteePackage.PackageType });
//    }
//}