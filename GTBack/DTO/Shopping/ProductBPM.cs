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
        [XmlElement(ElementName = "variants")]
        public Variants Variants { get; set; }
    }


    
    public class Variants
    {
        [XmlElement(ElementName = "variant")]
        public Variant[] Variant { get; set; }
    }
    
    public class Variant
    {
        [XmlElement(ElementName = "spec")]
        public Spec[] Specs { get; set; }
        [XmlElement(ElementName = "variantId")]
        public string VariantId { get; set; }
        [XmlElement(ElementName = "productCode")]
        public string ProductCode { get; set; }
        [XmlElement(ElementName = "barcode")]
        public string Barcode { get; set; }
        [XmlElement(ElementName = "gtin")]
        public string Gtin { get; set; }
        [XmlElement(ElementName = "mpn")]
        public string Mpn { get; set; }
        [XmlElement(ElementName = "rafno")]
        public string Rafno { get; set; }
        [XmlElement(ElementName = "depth")]
        public string Depth { get; set; }
        [XmlElement(ElementName = "height")]
        public string Height { get; set; }
        [XmlElement(ElementName = "width")]
        public string Width { get; set; }
        [XmlElement(ElementName = "agirlik")]
        public string Agirlik { get; set; }
        [XmlElement(ElementName = "desi")]
        public string Desi { get; set; }
        [XmlElement(ElementName = "quantity")]
        public string Quantity { get; set; }
        [XmlElement(ElementName = "price")]
        public string Price { get; set; }
        [XmlElement(ElementName = "hbSaticiStokKodu")]
        public string HbSaticiStokKodu { get; set; }
        [XmlElement(ElementName = "hbKodu")]
        public string HbKodu { get; set; }
    }

    public class Spec
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlText]
        public string Value { get; set; }
    }

}
