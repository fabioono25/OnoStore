﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnoStore.Core.DomainObjects;

namespace OnoStore.Catalog.API.Models
{
    public class Product : Entity //, IAggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public decimal Value { get; set; }
        public DateTime DataInsert { get; set; }
        public string Image { get; set; }
        public int QuantityInventory { get; set; }
    }
}