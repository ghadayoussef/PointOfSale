using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Module.BusinessObjects
{
    [ImageName("BO_Product")]
    [NavigationItem("Items Catalog")]
    public class Item : XPCustomObject
    {
        public Item(Session session) : base(session)
        {

        }

        [Key(AutoGenerate = true)]
        public int Number
        {
            get;
            set;
        }

        public string ItemName
        {
            get;
            set;
        }

        [Association("Category-Items")]
        public Category Category
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string ManualCode
        {
            get;
            set;
        }

        public int AvailableQuantity
        {
            get;
            set;
        }

        public int MinimumQuantity
        {
            get;
            set;
        }

        [ModelDefault("EditMask","f2")]
        [ModelDefault("DisplayFormat", "f2")]
        public decimal DefaultBuyingPrice
        {
            get;
            set;
        }

        [ModelDefault("EditMask", "f2")]
        [ModelDefault("DisplayFormat", "f2")]
        public decimal DefaultSellingPrice
        {
            get;
            set;
        }

        [Association("item-PoProducts")]
        public XPCollection<PoProduct> PoProducts
        {
            get { return GetCollection<PoProduct>("PoProducts"); }
        }
        [Association("item-SalesOProducts")]
        public XPCollection<SalesOProducts> SalesOProducts
        {
            get { return GetCollection<SalesOProducts>("SalesOProducts"); }
        }
    }
}






