using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace PointOfSale.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class PoProduct : BaseObject
    { 
        public PoProduct(Session session)
            : base(session)
        {
        }

        private Item _item;
        [RuleRequiredField]

        [ImmediatePostData]
        [Association("item-PoProducts")]
        public Item Item
        {
            get
            {
                return _item;
            }
            set
            {
                SetPropertyValue("Item", ref _item, value);
                if(_item != null)
                {
                    UnitPrice = (float)_item.DefaultBuyingPrice;
                }
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public int ItemID
        {
            get
            {
                if (Item != null)
                    return Item.Number;
                return 0;
            }

        }

        private PurchaseOrder _purchaseOrder;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [Association("PurchaseOrder-PoProducts")]
        public PurchaseOrder PurchaseOrder
        {
            get
            {
                return _purchaseOrder;

            }
            set
            {
                SetPropertyValue("PurchaseOrder", ref _purchaseOrder, value);
            }
        }
        public int PurchaseOrderID
        {
            get
            {
                if (PurchaseOrder != null)
                    return PurchaseOrder.Number;
                else
                    return 0;
            }
        }

        private int _quantity;
        [ImmediatePostData]
        [RuleRequiredField]
        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                SetPropertyValue("Quantity", ref _quantity, value);
            }
        }

        [ImmediatePostData]
        public float UnitPrice
        {
            get;
            set;
        }

        [ModelDefault("EditMask","f2")]
        [ModelDefault("DisplayFormat", "f2")]
        public decimal TotalLineAmount
        {
            get
            {
                return (decimal)(Quantity * UnitPrice);
            }
        }

        public bool Returned
        {
            get;
            set;
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);

            if(propertyName.Equals("Quantity") || propertyName.Equals("UnitPrice"))
            {
               
            }

        }

        [Action(Caption = "Return Item")]
        public void CancelPurchasedItem()
        {
            Item.AvailableQuantity -= Quantity;
            Session.Save(Item);
            Returned = true;
            Session.Save(this);
            Session.CommitTransaction();
        }


    }
}