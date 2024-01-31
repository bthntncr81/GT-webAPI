using System;
using System.Xml.Serialization;
namespace GTBack.Core.DTO.Shopping;

public class ProductBPM
{
    [XmlRoot(ElementName = "Urunler")]
    public class ProductBpms
    {
        [XmlElement(ElementName = "Product")]
        public ElementBpm[] ProductList { get; set; }
    }

    public class ElementBpm
    {
        [XmlElement(ElementName = "Product_code")]
        public string Product_code { get; set; }
        [XmlElement(ElementName = "Product_id")]
        public string Product_id { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "mainCategory")]
        public string MainCategory { get; set; }
        [XmlElement(ElementName = "mainCategory_id")]
        public string MainCategory_id { get; set; }
        [XmlElement(ElementName = "category")]
        public string Category { get; set; }
        [XmlElement(ElementName = "category_id")]
        public string Category_id { get; set; }
        [XmlElement(ElementName = "subCategory")]
        public string SubCategory { get; set; }
        [XmlElement(ElementName = "subCategory_id")]
        public string SubCategory_id { get; set; }
        [XmlElement(ElementName = "Price")]
        public string Price { get; set; }
        [XmlElement(ElementName = "Price2")]
        public string Price2 { get; set; }
        [XmlElement(ElementName = "CurrencyType")]
        public string CurrencyType { get; set; }
        [XmlElement(ElementName = "Tax")]
        public string Tax { get; set; }
        [XmlElement(ElementName = "Stock")]
        public string Stock { get; set; }
        [XmlElement(ElementName = "Brand")]
        public string Brand { get; set; }
        [XmlElement(ElementName = "Image1")]
        public string Image1 { get; set; }
        [XmlElement(ElementName = "Image2")]
        public string Image2 { get; set; }
        [XmlElement(ElementName = "Image3")]
        public string Image3 { get; set; }
        [XmlElement(ElementName = "Image4")]
        public string Image4 { get; set; }
        [XmlElement(ElementName = "Image5")]
        public string Image5 { get; set; }
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "Variant")]
        public Variant Variant { get; set; }
    }

    public class Variant
    {
        [XmlElement(ElementName = "Color")]
        public string Color { get; set; }
        [XmlElement(ElementName = "Size")]
        public string Size { get; set; }
        [XmlElement(ElementName = "Weight")]
        public string Weight { get; set; }
    }
}
