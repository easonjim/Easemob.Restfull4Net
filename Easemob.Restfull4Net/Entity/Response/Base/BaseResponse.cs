using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Easemob.Restfull4Net.Entity.Response
{
    public class BaseResponse
    {
        /// <summary>
        /// 状态码，详见：http://docs.easemob.com/start/450errorcode/10restapierrorcode
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// 错误信息，详见：http://docs.easemob.com/start/450errorcode/10restapierrorcode
        /// </summary>
        public ErrorResponse ErrorMessage { get; set; }
    }
}
