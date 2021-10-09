using OnoStore.Core.DomainObjects;
using System;

namespace OnoStore.Catalog.API.Models
{
    // Product é a propria agregacao (nao ha classe filha)
    public class Product : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public decimal Value { get; set; }
        public DateTime DataInsert { get; set; }
        public string Image { get; set; }
        public int QuantityInventory { get; set; }

        public void RetirarEstoque(int quantidade)
        {
            if (QuantityInventory >= quantidade)
                QuantityInventory -= quantidade;
        }

        public bool EstaDisponivel(int quantidade)
        {
            return Active && QuantityInventory >= quantidade;
        }
    }
}
