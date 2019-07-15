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

    public class SalesOProducts : XPCustomObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SalesOProducts(Session session)
            : base(session)
        {
        }

        [Key(AutoGenerate = true)]
        public int Number
        {
            get;
            set;
        }


        private Item _item;
        [Association("item-SalesOProducts")]
        public Item Item
        {
            get
            {
                return _item;
            }
            set
            {
                SetPropertyValue("Item",ref _item, value);
            }
        }
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public int ItemID
        {
            get
            {
                if(Item!= null)
                    return Item.Number;
                return 0;
            }
        }

        private SalesOrder _salesOrder;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [Association("SalesOrders-SalesOProducts")]
        public SalesOrder SalesOrder
        {
            get
            {
                return _salesOrder;
            }
            set
            {
                SetPropertyValue("SalesOrder",ref _salesOrder,value);
            }
        }
      
        public int SalesOrderID
        {
           // get
            //{    //if doesn't work
               // if (SalesOrder ! = null)
                //    return SalesOrder.OrderID; 
               // return 0;
            //}

            get
            {
                if (SalesOrder != null)
                    return SalesOrder.OrderID;
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
        [ModelDefault("DisplayFormat","f2")]
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
        /// msh fakra da kan by3mel eh bzabt
        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);

            if (propertyName.Equals("Quantity") || propertyName.Equals("UnitPrice"))
            {

            }

        }


        [Action (Caption ="Cancel Sales Order")]
        public void CancelSalingItem()
        {
            Item.AvailableQuantity += Quantity;
            Returned = true;
            Session.Save(Item);
            Session.Save(this);
            Session.CommitTransaction();
        }
    }

   
}