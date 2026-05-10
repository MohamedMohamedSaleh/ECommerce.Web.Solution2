using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOS.BasketDtos
{
    // record to DTO because it is immutable 
    public record BasketDTO(string Id, ICollection<BasketItemDTO> Items);
    //{
    //    public string Id { get; set; } = default!;
    //    public ICollection<BasketItemDTO> Item { get; set; } = [];
    //}
}
