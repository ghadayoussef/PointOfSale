using DevExpress.ExpressApp;
using DevExpress.Xpo;
using PointOfSale.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Module.Logic
{
    public static class BusinessLogic
    {
        public static class PurchasingLogic
        {
            public static void UpdateItemQuantities(PurchaseOrder po)
            {
                if (po.PoProducts.Count > 0)
                {
                    if (po.Status == OrderStatus.GoodsDelivered)
                    {
                        for (int i = 0; i < po.PoProducts.Count; i++)
                        {
                            Item x = po.PoProducts[i].Item;
                            x.AvailableQuantity += po.PoProducts[i].Quantity;
                            x.Save();
                        }
                    }


                }
            }

            public static void CancelPurchaseOrderItems(PurchaseOrder po,Session session)
            {
                List<Item> itemsToSave = new List<Item>();

                if (po.Status == OrderStatus.GoodsDelivered)
                {
                    for(int i = 0;i<po.PoProducts.Count;i++)
                    {
                        Item x = po.PoProducts[i].Item;
                        int qty = po.PoProducts[i].Quantity;
                        x.AvailableQuantity = x.AvailableQuantity - qty;
                        itemsToSave.Add(x);
                    }

                    session.Save(itemsToSave);

                    po.Status = OrderStatus.Cancelled;
                    po.Save();
                    session.Save(po);

                    session.CommitTransaction();

                }
            }


        }

        public static class SalesLogic
        {
            public static void UpdateItemQuantityOnSale(SalesOrder so) 
            {

                //Validation of user entries
                if (so.Customer == null)
                    throw new UserFriendlyException("Choose a Customer to procceed");

                if(so.Status != SalesOrderStatus.InProgress)
                    throw new UserFriendlyException("You cannot Finalize an order that is not in progress");

                if (so.SalesOProducts.Count == 0)
                    throw new UserFriendlyException("You invoice must include items");

                if(so.SalesOProducts.Any(x=>x.Item == null || 
                        x.Quantity <= 0 || 
                        x.UnitPrice <= 0))
                {
                    throw new UserFriendlyException("Kindly review your items");
                }

                //Validate Stock quantities
                if(so.SalesOProducts.Any(x=>x.Quantity > x.Item.AvailableQuantity))
                {

                    SalesOProducts sop = so.SalesOProducts.FirstOrDefault(x => x.Quantity > x.Item.AvailableQuantity);
                    throw new UserFriendlyException("Not enough quantity in stock of item:"+sop.Item.ItemName);

                }

                //decrease qty from stock
                for (int i = 0; i < so.SalesOProducts.Count; i++)
                {
                    Item x = so.SalesOProducts[i].Item;
                    x.AvailableQuantity -= so.SalesOProducts[i].Quantity;
                    x.Save();
                }

                //Update Sales Order status
                so.Status = SalesOrderStatus.Finalized;
                so.Save();

                //ZEY ELLY FO2HA
                //for (int i =0; i < so.SalesOProducts.Count;i++)
                //{
                //    SalesOProducts obj = so.SalesOProducts[i];
                //    if (obj.Item == null)
                //        throw new UserFriendlyException("Item not found");
                //    if (obj.Quantity <= 0)
                //        throw new UserFriendlyException("Item:" + obj.Item.ItemName + " quantity is invalid");
                //    if (obj.UnitPrice <= 0)
                //        throw new UserFriendlyException("Item:" + obj.Item.ItemName + " UNIT price is invalid");

                //}










            }

        }

        public static class Statistics
        {
            public static decimal GetTotalItemSales(Item item)
            {

                decimal total = item.SalesOProducts.Sum(x => (decimal)(x.Quantity * x.UnitPrice));
                return total;
            }
        }




    }
}
