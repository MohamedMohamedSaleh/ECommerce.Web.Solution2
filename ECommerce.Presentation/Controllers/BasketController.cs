using ECommerce.ServiceAbstraction;
using ECommerce.Shared.DTOS.BasketDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{

    public class BasketController : ApiBaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        #region Get Basket By Id

        [HttpGet()]
        public async Task<ActionResult<BasketDTO>> GetBasket(string id)
        {
            var basket = await _basketService.GetBasketAsync(id);
            if (basket == null)
            {
                return NotFound();
            }
            return Ok(basket);
        }
        #endregion

        #region Create Or Update Basket
        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket(BasketDTO basket)
        {
            var updatedBasket = await _basketService.CreateOrUpdateBaskeAsync(basket);
            if (updatedBasket == null)
            {
                return BadRequest("Failed to create or update the basket.");
            }
            return Ok(updatedBasket);
        }

        #endregion


        #region Delete Basket By Id
        // DELETE api/basket/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            var isDeleted = await _basketService.DeleteBaskeAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return Ok("Basket Deleted Successfully!");
        }
        #endregion
    }
}
