using System.ComponentModel.DataAnnotations.Schema;

namespace Micro.FetchProduct.Models
{
    public class InventoryDataModelByShops
    {
        public short Inv_shopID { get; set; }
        public string Inv_shopName { get; set; }
        public List<InventoryDataModel> InventoryDataModel { get; set; }
        public ProductBasedDiscount DiscountByShop { get; set; }
        // Note: Inv_shopID2 is only used for transferring the inventory from shop 1 to shop 2 where "Inv_shopID2" will act as shop 2. This Inv_shopID2 is only used for shopToshop module.
        public short Inv_shopID2 { get; set; }
    }

    public class InventoryDataModel
    {
        public int ProductCombinationID { get; set; }
        public string ProductCombinationName { get; set; }
        public string ProductCombinationCode { get; set; }
        public string ProductCombinationID2 { get; set; }
        //public short Inv_shopID { get; set; }
        //public string Inv_shopName { get; set; }
        public long Product_ID { get; set; }
        public List<ProductItemModel> ProductItemModel { get; set; }
    }

    public partial class ProductBasedDiscount
    {



        public long ProductBasedDiscountID { get; set; }





        public int DiscountID { get; set; }





        public int LineItemID { get; set; }





        public long ProductID { get; set; }





        public decimal DiscountValue { get; set; }





        public DateTime? CreatedOn { get; set; }





        public DateTime? ModifiedOn { get; set; }





        public int? ShopID { get; set; }



    }
    public class ProductItemModel : ProductItem
    {
        
        public string ProductSizeName { get; set; }
        
        public string ProductSizeCode { get; set; }
        
        public long ProductInventoryID { get; set; }
        
        public short ShopID { get; set; }
        
        public string ShopName { get; set; }
        
        public string BarcodeByteImage { get; set; }
        
        public string ProductName { get; set; }
        
        public string ProductCode { get; set; }
        
        public short Status { get; set; }
        
        public int AssignedNumberOfItems_ByShop { get; set; }
    }
    public partial class ProductItem
    {



        public long ProductItemID { get; set; }





        public long ProductID { get; set; }





        public string ProductItemName { get; set; }





        public string ProductItemCode { get; set; }





        public string ProductItemCodeOld { get; set; }





        public int ProductSizeID { get; set; }





        public int? ProductCombinationID { get; set; }





        public decimal? ProductItemPurchasedCost { get; set; }





        public decimal? ProductItemSalesCost { get; set; }





        public int NumberOfItems { get; set; }



    }

    public partial class ProductCombination
    {



          public int ProductCombinationID { get; set; }





          public int? LineItemID { get; set; }





          public string ProductCombinationName { get; set; }





          public string ProductCombinationCode { get; set; }





          public string ProductCombinationDescription { get; set; }





          public int? DisplaySeqNo { get; set; }



    }


}
