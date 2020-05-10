using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CyberShop.Controllers;
using CyberShop.Models;
using Data;
namespace CyberShop.Common
{
    public class OrderHistory
    {
        ShopPCComponentsEntities data = new ShopPCComponentsEntities();
        public List<InvoiceDetailViewModel> listOrderHistory(int id)
        {
            var res = (from a in data.Invoices
                       join b in data.Invoice_Detail on a.Id equals b.Invoice_id
                       join c in data.Products on b.Product_id equals c.id
                       where a.User_id == id
                       select new InvoiceDetailViewModel
                       {
                           Id = b.Id,
                           Invoice_id = a.Id,
                           Product_id = c.id,
                           ProductName = c.ProductName,
                           Amount = b.Amount,
                           Price = b.Price,
                           Status = a.Status,
                           CreateDate = b.CreateDate
                       }).ToList();
            return res;
        }
    }
}