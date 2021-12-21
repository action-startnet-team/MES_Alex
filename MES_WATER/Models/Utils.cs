using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MES_WATER.Models
{
    //程式固定 向下//
    public static class Utils
    {
        /// <summary>
        /// 判斷IEnumerable是否非null
        /// </summary>
        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }

    }
    //程式固定 向上//

}