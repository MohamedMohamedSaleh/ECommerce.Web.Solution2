using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.ServiceAbstraction;
using ECommerce.Shared.DTOS.BasketDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<BasketDTO> CreateOrUpdateBaskeAsync(BasketDTO basketDTO)
        {
            var basketEntity = _mapper.Map<CustomerBasket>(basketDTO);
            var CreateOreUpdateBasket = await _basketRepository.CreateOrUpdateBasketAsync(basketEntity);
            return _mapper.Map<BasketDTO>(CreateOreUpdateBasket);
        }

        public async Task<bool> DeleteBaskeAsync(string id)
        {
            return await _basketRepository.DeleteBasketAsync(id);
        }

        public async Task<BasketDTO> GetBasketAsync(string id)
        {
            var basketEntity = await _basketRepository.GetBasketAsync(id);
            return _mapper.Map<BasketDTO>(basketEntity);
        }
    }
}
