using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using СalculatorWebApplication.Exceptions;
using СalculatorWebApplication.Models;
using System;
using System.Text.RegularExpressions;
using СalculatorWebApplication.Helpers;

namespace СalculatorWebApplication.Services.Filters
{
    public class ValidateExpressionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var inputModel = context.ActionArguments["input"] as InputModelDto;

            if (inputModel == null)
            {
                context.Result = new BadRequestObjectResult(new BusinessException("Invalid input model.").Message);
                return;
            }

            if (string.IsNullOrEmpty(inputModel.Expression))
            {
                context.Result = new BadRequestObjectResult(new BusinessException("Expression is required.").Message);
                return;
            }

            inputModel.Expression = ExpressionProcessor.ProcessExpression(inputModel.Expression);

            if (!IsValidMathExpression(inputModel.Expression))
            {
                context.Result = new BadRequestObjectResult(new BusinessException("Invalid mathematical expression.").Message);
                return;
            }

            base.OnActionExecuting(context);
        }

        public static bool IsValidMathExpression(string expression)
        {
            if (expression.Contains("/0"))
            {
                return false;
            }

            var regex = new Regex(@"^[0-9+\-*/^()sqrt{}\s.]+$");
            return regex.IsMatch(expression);
        }
    }
}
