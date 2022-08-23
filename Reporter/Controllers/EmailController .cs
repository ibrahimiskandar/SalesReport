using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Reporter.Models;

namespace Reporter.Controllers
{

    public class EmailController : Controller
    {
        private readonly SMTPSettings _smtpSettings;
        private readonly IConfiguration _config;

        public EmailController(IConfiguration configuration)
        {
            _config = configuration;
            _smtpSettings = new SMTPSettings();
            _config.GetSection("SMTP_Settings").Bind(_smtpSettings);

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SendEmail(EmailModel email)
        {
            if (ModelState.IsValid)
            {
                DataTable dataTable = new DataTable();
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                try
                {
                    int procedureValue = Convert.ToInt32(email.procedureName);
                    string procedure = Enum.GetName(typeof(Procedure), procedureValue);

                    int productValue = Convert.ToInt32(email.productName);
                    string product = Enum.GetName(typeof(Product), productValue);

                    int countryValue = Convert.ToInt32(email.countryName);
                    string country = Enum.GetName(typeof(Country), countryValue);

                    int segmentValue = Convert.ToInt32(email.segmentName);
                    string segment = Enum.GetName(typeof(Segment), segmentValue);

                    string connectionString = _config.GetConnectionString("connectionString");
                    SqlConnection connection = new SqlConnection(connectionString);
                    SqlCommand command = new SqlCommand(procedure, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SDATE", email.SDATE);
                    command.Parameters.AddWithValue("@EDATE", email.EDATE);


                    switch (procedureValue)
                    {
                        case 2:
                            command.Parameters.AddWithValue("@PRODUCT", product);
                            break;
                        case 3:
                            command.Parameters.AddWithValue("@COUNTRY", country);
                            break;
                        case 4:
                            command.Parameters.AddWithValue("@SEGMENT", segment);
                            break;
                        default:
                            break;
                    }
                    dataAdapter.SelectCommand = command;
                    dataAdapter.Fill(dataTable);
                    if (dataTable.Rows.Count > 0)
                    {
                        dataTable.TableName = procedure;
                        using (XLWorkbook workbook = new XLWorkbook())
                        {
                            workbook.Worksheets.Add(dataTable);
                            workbook.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            workbook.Style.Font.Bold = true;
                            MemoryStream ms = new MemoryStream();
                            workbook.SaveAs(ms);

                            byte[] result = ms.ToArray();
                            var stream = new MemoryStream(result);

                            IFormFile file = new FormFile(stream, 0, result.Length, "Report", "Report.xlsx");


                            email.Attachment = file;


                        }
                    }
                }
                catch (System.Exception ex)
                {

                    ViewBag.ErrorMessage = ex.Message;
                }
                try
                {
                    using (MailMessage mm = new MailMessage(_smtpSettings.Email, email.To))
                    {
                        mm.Subject = email.Subject;
                        mm.Body = email.Description;


                        if (!string.IsNullOrEmpty(email.CC))
                            mm.CC.Add(email.CC);


                        string fileName = Path.GetFileName(email.Attachment.FileName);
                        mm.Attachments.Add(new Attachment(email.Attachment.OpenReadStream(), fileName));


                        mm.IsBodyHtml = false;
                        using (SmtpClient smtp = new SmtpClient())
                        {
                            smtp.Host = _smtpSettings.Host;
                            smtp.EnableSsl = true;
                            NetworkCredential NetworkCred = new NetworkCredential(_smtpSettings.Email, _smtpSettings.Password);
                            smtp.UseDefaultCredentials = true;
                            smtp.Credentials = NetworkCred;
                            smtp.Port = _smtpSettings.Port;
                            smtp.Send(mm);

                            ViewBag.Message = "Email Sent Successfully";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }
            return View("Index", email);
        }
    }

}
