﻿using System;

namespace ZQNB.Web.Models
{
    public interface INbEntity<TPk>
    {
        /// <summary>
        /// 主键
        /// </summary>
        TPk Id { get; set; }
    }


    public interface IGuidPk : INbEntity<Guid>
    {
    }
}