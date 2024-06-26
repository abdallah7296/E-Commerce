﻿using E_come.DTO.ShopDTO;

namespace E_come.DTO.ProductDTO
{
    public class ProdRev
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }

        public string Item_Name { get; set; }
        public int priceProd { get; set; }
        public string Descrip { get; set; }
        public int solditemsProd { get; set; }
        public int quantityProd { get; set; }
        public int AverageRate { get; set; }
        public ShopDto? Shop { get; set; }
        public List<string> imageUrls { get; set; }

    }
}
