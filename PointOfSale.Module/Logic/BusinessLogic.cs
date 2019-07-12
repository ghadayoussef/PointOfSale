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
                if(so.SalesOProducts.Count > 0)
                {
                    if (so)
                }

            }

        }




    }
}
