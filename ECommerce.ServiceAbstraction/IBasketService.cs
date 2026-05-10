using ECommerce.Shared.DTOS.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServiceAbstraction
{
    public interface IBasketService
    {
        Task<BasketDTO> GetBasketAsync(string id);
        Task<BasketDTO> CreateOrUpdateBaskeAsync(BasketDTO basketDTO);
        Task<bool> DeleteBaskeAsync(string id);
    }
}
