using Expenses.API.Data.Services;
using Expenses.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Expenses.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Authorize]
    public class TransactionsController(ITransactionsService transactionsService) : ControllerBase
    {

        [HttpGet("All")]
        public IActionResult GetAll()
        {
            var nameIdentifierClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(nameIdentifierClaim))
                return BadRequest("Could not get the user id");

            if (!int.TryParse(nameIdentifierClaim, out int userId))
                return BadRequest("Invalid user id");

            var transactions = transactionsService.GetAll(userId);
            return Ok(transactions);
        }

        [HttpGet("Details/{id}")]
        public IActionResult Get(int id)
        {
            var transaction = transactionsService.GetById(id);
            if(transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpPost("Create")]
        public IActionResult CreateTransaction([FromBody]PostTransactionDto payload)
        {

            var nameIdentifierClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(nameIdentifierClaim))
                return BadRequest("Could not get the user id");

            if(!int.TryParse(nameIdentifierClaim, out int userId))
                return BadRequest("Invalid user id");

            var newTransaction = transactionsService.Add(payload, userId);
            return Ok(newTransaction);
        }

        [HttpPut("Update/{id}")]
        public IActionResult UpdateTransaction(int id, [FromBody]PutTransactionDto payload)
        {
            var updatedTransaction = transactionsService.Update(id, payload);
            if(updatedTransaction == null)
            {
                return NotFound();
            }
            return Ok(updatedTransaction);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteTransaction(int id)
        {
            transactionsService.Delete(id);
            return Ok();
        }


    }
}
