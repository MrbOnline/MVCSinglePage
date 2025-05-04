using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FinalProject.BackEnd.Models.DomainModels.ProductAggregates;
using FinalProject.BackEnd.Models.DomainModels.OrderAggregates.OrderHeaderAggregates;

namespace FinalProject.BackEnd.Models.DomainModels.OrderAggregates.OrderDetailsAggregates
{
    public class OrderDetail
    {
        public Guid? Id { get; set; } 
        public Guid? OrderHeaderId { get; set; }
        public Guid? ProductId { get; set; }
        public decimal UnitPrice { get; set; }
       
        public decimal Amount { get; set; }
        public OrderHeader? OrderHeader { get; set; }
        public Product? Product { get; set; }
        public decimal TotalPrice{ get{return UnitPrice * Amount; }
        }

    }
}
