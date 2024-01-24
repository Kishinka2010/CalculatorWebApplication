// Ваш enum для операций
using Microsoft.AspNetCore.Mvc;
using СalculatorWebApplication.Exceptions;
using СalculatorWebApplication.Models;
using СalculatorWebApplication.Services.Calculator;
using СalculatorWebApplication.Services.Filters;

public class CalculatorController : ControllerBase
{
    private readonly ICalculatorService _calculatorService;

    public CalculatorController(ICalculatorService calculatorService)
    {
        _calculatorService = calculatorService;
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidateExpressionFilterAttribute))]
    [Route("Calculate")]
    public IActionResult Calculate([FromBody] InputModelDto input, string operation, double exponent)
    {
        try
        {
            ResultModelDto result;
            switch (operation)
            {
                case "evaluate":
                    result = _calculatorService.CalculateExpression(input.Expression);
                    break;
                case "exponentiation":
                    result = _calculatorService.CalculateExponentiation(input.Expression, exponent);
                    break;
                case "squareRoot":
                    result = _calculatorService.CalculateSquareRoot(input.Expression);
                    break;
                default:
                    return BadRequest(new BusinessException("An incorrect operation was entered").Message);
            }

            return Ok(result);
        }
        catch (BusinessException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(new { ErrorMessage = ex.Message });
        }
    }
}
