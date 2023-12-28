using Hr_policy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System;
using System.IO;
using System.Threading.Tasks;
using Hr_policy.Models;

namespace Hr_policy.Controllers
{
    public class HomeController : Controller
    {
        private readonly HrPolicy2Context context;

        public HomeController(HrPolicy2Context context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }


        
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("DashBoard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            var myUser = context.Users.Where(x => x.Email == user.Email).FirstOrDefault();
            if (myUser != null)
            {

                HttpContext.Session.SetString("UserSession", myUser.Email);

                string role = GetRoleName(myUser.RoleId);
                HttpContext.Session.SetString("UserRole", role);

                return RedirectToAction("DashBoard");
            }
            else
            {
                ViewBag.Message = "Login Failed";
            }
            return View();
        }



        private string GetRoleName(int roleId)
        {
            var role = context.MasterRoles.FirstOrDefault(x => x.RoleId == roleId);
            return role != null ? role.RoleName : "Unknown";
        }



        public IActionResult DashBoard()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();

                // Retrieve user role from session
                string userRole = HttpContext.Session.GetString("UserRole");
                ViewBag.UserRole = userRole;

                // Check if the user has the required role for Add Document
                ViewBag.CanAddDocument = userRole == "Admin" || userRole == "Super Admin";

                // Assuming you have a DbContext with DbSet for PolicyTopic and HrPolicy
                var policyTopics = context.PolicyTopics.ToList();
                var hrPolicies = context.HrPolicies.ToList();

                ViewBag.PolicyTopics = new SelectList(policyTopics, "Id", "Name");
                ViewBag.HrPolicies = new SelectList(hrPolicies, "Id", "PolicyName");
            }
            else
            {
                return RedirectToAction("Login");
            }

            return View();
        }


        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login");
            }
            return View();
        }


        //public IActionResult AddDocument()
        //{
        //    // Check if the user is authorized to add a document
        //    // For example, you may have roles like "Admin" or "Super Admin" who can add documents
        //    string userRole = HttpContext.Session.GetString("UserRole");
        //    if (userRole != "Admin" && userRole != "Super Admin")
        //    {
        //        return RedirectToAction("DashBoard");
        //    }

        //    // Fetch existing topics and policies for dropdowns
        //    var policyTopics = context.PolicyTopics.ToList();
        //    var hrPolicies = context.HrPolicies.ToList();

        //    // Populate dropdown lists
        //    ViewBag.TopicId = new SelectList(policyTopics, "Id", "Name");
        //    ViewBag.PolicyId = new SelectList(hrPolicies, "Id", "PolicyName");

        //    return View();
        //}

        [HttpGet]
        public IActionResult AddDocument()
        {

            string userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin" && userRole != "Super Admin")
            {
                return RedirectToAction("DashBoard");
            }

            // Fetch existing topics and policies for dropdowns
            var policyTopics = context.PolicyTopics.ToList();
            var hrPolicies = context.HrPolicies.ToList();

            // Populate dropdown lists
            ViewBag.TopicId = new SelectList(policyTopics, "Id", "Name");
            ViewBag.PolicyId = new SelectList(hrPolicies, "Id", "PolicyName");

            // Create and initialize the ViewModel for capturing document information
            var addDocumentViewModel = new PolicyDocument
            {
                // Initialize default values or leave them as null/empty as needed
                SortOrder = 1,
                Status = 1,
                InsertedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                ArchivedOn = DateTime.Now
            };

            return View(addDocumentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDocument(PolicyDocument model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the current user's email
                    string currentUserEmail = HttpContext.Session.GetString("UserSession");

                    // Map the ViewModel to your PolicyDocument entity
                    var policyDocument = new PolicyDocument
                    {
                        // Map properties from ViewModel
                        TopicId = model.TopicId,
                        PolicyId = model.PolicyId,
                        DocumentCaption = model.DocumentCaption,
                        DocumentDate = model.DocumentDate,
                        KeywordsString = model.KeywordsString,
                        DocumentTypeId = model.DocumentTypeId,
                        Remarks = model.Remarks,
                        SortOrder = model.SortOrder,
                        Extension = Path.GetExtension(model.File.FileName),
                        ContentType = model.File.ContentType,
                        FileSize = (int)model.File.Length,
                        FileName = model.File.FileName,
                        Status = model.Status,
                        InsertedBy = currentUserEmail,
                        InsertedOn = model.InsertedOn,
                        InsertedFrom = currentUserEmail,
                        UpdatedBy = currentUserEmail,
                        UpdatedOn = model.UpdatedOn,
                        UpdatedFrom = currentUserEmail,
                        ArchivedBy = currentUserEmail,
                        ArchivedOn = model.ArchivedOn,
                        ArchivedFrom = currentUserEmail,
                    };

                    // Read content from the uploaded file and populate HTML and Text content
                    using (var reader = new StreamReader(model.File.OpenReadStream()))
                    {
                        policyDocument.HtmlContent = reader.ReadToEnd();
                        // You can process the content to extract plain text if needed
                        policyDocument.TextContent = policyDocument.HtmlContent;
                    }

                    // Save the PolicyDocument to the database
                    await context.PolicyDocuments.AddAsync(policyDocument);
                    await context.SaveChangesAsync();

                    TempData["insert_success"] = "Document inserted successfully.";
                    return RedirectToAction("DashBoard");


                }
                catch (Exception ex)
                {
                    // Handle exceptions appropriately
                    TempData["error_message"] = $"An error occurred: {ex.Message}";
                }
            }

            // If ModelState is not valid, re-populate dropdowns and return to the view
            var policyTopics = context.PolicyTopics.ToList();
            var hrPolicies = context.HrPolicies.ToList();
            ViewBag.TopicId = new SelectList(policyTopics, "Id", "Name");
            ViewBag.PolicyId = new SelectList(hrPolicies, "Id", "PolicyName");

            return View(model);
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddDocument(PolicyDocument doc)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (doc.File != null && doc.File.Length > 0)
        //            {
        //                // Use a relative path or a configuration setting for file storage
        //                var filePath = Path.Combine("Uploads", doc.File.FileName);

        //                // Save the file to the specified path
        //                using (var fileStream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await doc.File.CopyToAsync(fileStream);
        //                }

        //                // Set the FileName property of PolicyDocument to the saved file path
        //                doc.FileName = filePath;
        //            }

        //            await context.PolicyDocuments.AddAsync(doc);
        //            await context.SaveChangesAsync();
        //            TempData["insert_success"] = "Inserted..";
        //            return RedirectToAction("EditBooks");
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle exceptions appropriately
        //            TempData["error_message"] = $"An error occurred: {ex.Message}";
        //        }
        //    }

        //    return View(doc);
        //}







        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
