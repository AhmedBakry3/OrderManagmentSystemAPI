﻿

namespace Shared.DataTransferObject.ProductModuleDTos
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
