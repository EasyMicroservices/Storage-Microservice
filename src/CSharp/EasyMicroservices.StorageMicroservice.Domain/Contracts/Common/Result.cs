using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMicroservices.StorageMicroservice.Contracts
{
    public static class Extensions
    {
        public static ResultDto SetResultState(this ResultDto result, int status, object outputRes)
        {
            result.StatusId = (int)status;
            result.Message = string.Empty;
            result.IsSuccessful = status == 200;
            result.OutputRes = outputRes;
            return result;
        }
    }
    public class ResultDto
    {
        public ResultDto()
        {
            this.SetResultState(0, new object());
        }
        public int StatusId { get; set; }
        public string Message { get; set; }
        public bool IsSuccessful { get; set; }
        public object OutputRes { get; set; }
    }
}
