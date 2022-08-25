using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Reporter.Models;

namespace Reporter.Controllers
{
    public class ImportController : Controller
    {


        private IWebHostEnvironment _env;
        private IConfiguration _config;
        public ImportController(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _env = environment;
            _config = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(Excel excel)
        {
            IFormFile file = excel.ExcelFile;
            string filePath = "";
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    try
                    {
                        //Create a Folder.
                        string path = Path.Combine(_env.WebRootPath, "Uploads");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        //Save the uploaded Excel file.
                        string fileName = Path.GetFileName(file.FileName);
                        filePath = Path.Combine(path, fileName);
                        using (FileStream stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        //Read the connection string for the Excel file.
                        string connectionString = _config.GetConnectionString("ExcelConString");
                        DataTable dt = new DataTable();
                        connectionString = string.Format(connectionString, filePath);

                        using (OleDbConnection connExcel = new OleDbConnection(connectionString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;

                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);
                                    connExcel.Close();
                                }
                            }
                        }

                        //Insert the Data read from the Excel file to Database Table.
                        connectionString = _config.GetConnectionString("connectionString");
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                            {
                                //Set the database table name.
                                sqlBulkCopy.DestinationTableName = "dbo.SalesTable";

                                //[OPTIONAL]: Map the Excel columns with that of the database table.


                                sqlBulkCopy.ColumnMappings.Add("Segment", "Segment");
                                sqlBulkCopy.ColumnMappings.Add("Country", "Country");
                                sqlBulkCopy.ColumnMappings.Add("Product", "Product");
                                sqlBulkCopy.ColumnMappings.Add("Discount Band", "DiscountBand");
                                sqlBulkCopy.ColumnMappings.Add("Units Sold", "UnitSold");
                                sqlBulkCopy.ColumnMappings.Add("Manufacturing Price", "ManufacturingPrice");
                                sqlBulkCopy.ColumnMappings.Add("Sale Price", "SalePrice");
                                sqlBulkCopy.ColumnMappings.Add("Gross Sales", "GrossSales");
                                sqlBulkCopy.ColumnMappings.Add("Discounts", "Discounts");
                                sqlBulkCopy.ColumnMappings.Add(" Sales", "Sales");
                                sqlBulkCopy.ColumnMappings.Add("COGS", "COGS");
                                sqlBulkCopy.ColumnMappings.Add("Profit", "Profit");
                                sqlBulkCopy.ColumnMappings.Add("Date", "Date");

                                con.Open();
                                sqlBulkCopy.WriteToServer(dt);
                                con.Close();
                                ViewBag.Message = "Import Successfully";

                            }
                        }


                    }
                    catch (System.Exception ex)
                    {

                        ViewBag.ErrorMessage = ex.Message;
                    }
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

            }
            return View("Index", excel);
        }
    }
}
