using System.ComponentModel.DataAnnotations.Schema;

namespace Micro.FetchProduct.Models
{
    public class ProductModel
    {
        public long ProductID { get; set; }





        public int LineItemID { get; set; }





        public string ProductName { get; set; }





        public string ProductDescription { get; set; }





        public string ProductCode { get; set; }





        public string ProductCodeOld { get; set; }





        public string ProductPicture { get; set; }





        public int? ProductCategoryID { get; set; }





        public int? ProductGenderID { get; set; }





        public decimal ProductAveragePurchaseCost { get; set; }





        public decimal ProductAverageSalesCost { get; set; }





        public int? CalendarSeasonID { get; set; }





        public int? ProductSupplierID { get; set; }





        public int? ProductAcquireTypeID { get; set; }





        public int? PurchaseOrderID { get; set; }





        public int? CreatedBy { get; set; }





        public int? UpdatedBy { get; set; }





        public DateTime? CreatedAt { get; set; }





        public DateTime? UpdatedAt { get; set; }





        public int? TaxID { get; set; }





        public bool? IsDiscountable { get; set; }


    }


}
