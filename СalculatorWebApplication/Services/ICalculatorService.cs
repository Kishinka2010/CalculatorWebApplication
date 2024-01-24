using СalculatorWebApplication.Models;

namespace СalculatorWebApplication.Services.Calculator
{
    public interface ICalculatorService
    {
        ResultModelDto CalculateExpression(string expression);

        ResultModelDto CalculateSquareRoot(string value);

        ResultModelDto CalculateExponentiation(string baseValue, double exponent);

    }
}
