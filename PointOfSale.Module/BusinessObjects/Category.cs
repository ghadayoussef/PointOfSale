using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Module.BusinessObjects
{
    [NavigationItem("Items Catalog")]
    public class Category : XPCustomObject
    {
        public Category(Session session):base(session)
        {

        }

        [Key(AutoGenerate = true)]
        public int Number
        {
            get;
            set;
        }

        public string CategoryName
        {
            get;set;
        }

        [Association("Category-Items")]
        public XPCollection<Item> Items
        {
            get
            {
                return GetCollection<Item>("Items");

            }
        }

    }
}
