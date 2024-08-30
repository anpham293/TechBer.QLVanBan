using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace TechBer.ChuyenDoiSo.QuanLySdtZalo
{
	[Table("SdtZalos")]
    [Audited]
    public class SdtZalo : FullAuditedEntity<long> 
    {

		public virtual string Ten { get; set; }
		
		public virtual string Sdt { get; set; }
		

    }
}