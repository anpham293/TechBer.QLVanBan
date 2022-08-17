using System;
using Abp.AutoMapper;
using TechBer.ChuyenDoiSo.Sessions.Dto;

namespace TechBer.ChuyenDoiSo.Models.Common
{
    [AutoMapFrom(typeof(ApplicationInfoDto)),
     AutoMapTo(typeof(ApplicationInfoDto))]
    public class ApplicationInfoPersistanceModel
    {
        public string Version { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}