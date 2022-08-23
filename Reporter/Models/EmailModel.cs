
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Reporter.CustomAtributes;

namespace Reporter.Models
{
    public class EmailModel
    {
        [Display(Name = "Procedure")]
        public string procedureName { get; set; }
        [Display(Name = "Product")]
        public string productName { get; set; }
        [Display(Name = "Segment")]
        public string segmentName { get; set; }

        [Display(Name = "County")]
        public string countryName { get; set; }
        [Required(ErrorMessage = "Start date Field is required.")]
        [Display(Name = "Start Date")]
        public DateTime SDATE { get; set; }

        [Required(ErrorMessage = "End Date Field is required.")]
        [DateGreaterThanAttribute(otherPropertyName = "SDATE", ErrorMessage = "End date must be greater than start date")]
        [Display(Name = "End Date")]
        public DateTime EDATE { get; set; }

        [Required(ErrorMessage = "To Field is required.")]
        [Display(Name = "To")]
        [AllowedEmailExtensionsAttribute(new string[] { "@code.edu.az" })]
        public string To { get; set; }

        [Display(Name = "CC")]
        public string CC { get; set; }

        [Required(ErrorMessage = "Subject Field is required.")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        public IFormFile Attachment { get; set; }

    }
    public enum Procedure
    {
        [Display(Name = "Discounts for product")]
        prc_DiscountsForProduct = 1,
        [Display(Name = "Sales for product")]
        prc_SalesForProduct = 2,
        [Display(Name = "Sales for country")]
        prc_SalesForCountry = 3,
        [Display(Name = "Sales for segment")]
        prc_SalesForSegment = 4
    }
    public enum Product
    {
        Carretera = 1,
        Amarilla = 2,
        Paseo = 3,
        VTT = 4,
        Montana = 5,
        Velo = 6
    }
    public enum Country
    {
        [Display(Name = "United States of America")]
        USA = 1,
        Germany = 2,
        Mexico = 3,
        Canada = 4,
        France = 5
    }
    public enum Segment
    {
        Midmarket = 1,
        [Display(Name = "Channel Partners")]
        ChannelPartners = 2,
        Government = 3,
        Enterprise = 4,
        [Display(Name = "Small Business")]
        SmallBusiness = 5
    }
}
