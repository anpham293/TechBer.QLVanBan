﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.CayTieuChi
{
    public class CreateTieuChiInputModel
    {
        public int? ParentId { get; set; }

        public int LoaiPhuLuc { get; set; }
    }
}
