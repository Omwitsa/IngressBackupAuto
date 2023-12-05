using IngressBkpAutomation.ViewModel;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IngressBkpAutomation.Utilities
{
    public class InputValidator
    {
        public ReturnData<string> Validate(List<Tuple<string, string, InputDataType>> requiredFields)
        {
            var response = new ReturnData<string>();
            foreach (Tuple<string, string, InputDataType> tuple in requiredFields)
            {
                var error = $"{tuple.Item1} is required";

                if (string.IsNullOrEmpty(tuple.Item2))
                {
                    response.Message = "Validation failed: " + error;
                    response.Success = false;
                    return response;
                }

                var dataType = tuple.Item3;
                var value = tuple.Item2;
                switch (dataType)
                {
                    case InputDataType.Integer:
                        response = ParseInt(value, response, tuple);
                        if (!response.Success) return response;
                        break;
                    case InputDataType.Decimal:
                        response = ParseDecimal(value, response, tuple);
                        if (!response.Success) return response;
                        break;
                    case InputDataType.Float:
                        response = ParseFloat(value, response, tuple);
                        if (!response.Success) return response;
                        break;
                    case InputDataType.Email:
                        response = CheckEmail(value, response, tuple);
                        if (!response.Success) return response;
                        break;
                    case InputDataType.Password:
                        response = CheckPassword(value, response, tuple);
                        if (!response.Success) return response;
                        break;
                }
            }
            response.Success = true;
            return response;
        }

        private static ReturnData<string> ParseInt(string value, ReturnData<string> response, Tuple<string, string, InputDataType> tuple)
        {
            try
            {
                if (int.Parse(value).GetType() != typeof(int))
                {
                    response.Success = false;
                    response.Message = $"{tuple.Item1} is not of datatype {tuple.Item3}";
                    return response;
                }
                response.Success = true;
                return response;
            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = $"{tuple.Item1} is not of datatype {tuple.Item3}";
                return response;
            }
        }

        private static ReturnData<string> ParseDecimal(string value, ReturnData<string> response, Tuple<string, string, InputDataType> tuple)
        {
            try
            {
                if (decimal.Parse(value).GetType() != typeof(decimal))
                {
                    response.Success = false;
                    response.Message = $"{tuple.Item1} is not of datatype {tuple.Item3}";
                    return response;
                }
                response.Success = true;
                return response;
            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = $"{tuple.Item1} is not of datatype {tuple.Item3}";
                return response;
            }
        }

        private static ReturnData<string> ParseFloat(string value, ReturnData<string> response, Tuple<string, string, InputDataType> tuple)
        {
            try
            {
                if (decimal.Parse(value).GetType() != typeof(decimal))
                {
                    response.Success = false;
                    response.Message = $"{tuple.Item1} is not of datatype {tuple.Item3}";
                    return response;
                }
                response.Success = true;
                return response;
            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = $"{tuple.Item1} is not of datatype {tuple.Item3}";
                return response;
            }
        }

        private ReturnData<string> CheckEmail(string value, ReturnData<string> response, Tuple<string, string, InputDataType> tuple)
        {
            if (!Regex.IsMatch(value, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                response.Success = false;
                response.Message = $"{tuple.Item1} is not a valid {tuple.Item3}";
                return response;
            }
            response.Success = true;
            return response;
        }

        private static ReturnData<string> CheckPassword(string value, ReturnData<string> response, Tuple<string, string, InputDataType> tuple)
        {
            try
            {
                value = value ?? "null";
                if (value.Length < 6)
                {
                    response.Success = false;
                    response.Message = $"{tuple.Item1} should not be less than 6 characters";
                    return response;
                }

                response.Success = true;
                return response;
            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = $"{tuple.Item1} is not of datatype {tuple.Item3}";
                return response;
            }
        }

    }
}
