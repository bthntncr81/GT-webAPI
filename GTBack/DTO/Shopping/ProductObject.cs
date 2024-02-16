using System.Xml.Serialization;

namespace GTBack.Core.DTO.Shopping;



    [XmlRoot("products")]
    public class ProductsTarzYeri
    {
        [XmlElement("product")]
        public ProductTarzYeri[] ProductList { get; set; }
    }

    public class ProductTarzYeri:BaseEntity
    {
        public string id { get; set; }
        public string productCode { get; set; }
        public string barcode { get; set; }
        public string main_category { get; set; }
        public string top_category { get; set; }
        public string sub_category { get; set; }
        public string categoryID { get; set; }
        public string category { get; set; }
        public string active { get; set; }
        public string brandID { get; set; }
        public string brand { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public string image4 { get; set; }
        public string image5 { get; set; }
        public string specCode1 { get; set; }
        public string listPrice { get; set; }
        public string price { get; set; }
        public string tax { get; set; }
        public string currency { get; set; }
        public string desi { get; set; }
        public string quantity { get; set; }
        public string domestic { get; set; }
        public string show_home { get; set; }
        public string in_discount { get; set; }
        public Variants variants { get; set; }
        public string detail { get; set; }
    }

    public class Variants
    {
        [XmlElement("variant")]
        public Variant[] VariantList { get; set; }
    }

    public class Variant
    {
        public string name1 { get; set; }
        public string value1 { get; set; }
        public string name2 { get; set; }
        public string value2 { get; set; }
        public string quantity { get; set; }
        public string barcode { get; set; }
    }

