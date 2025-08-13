using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmProject.Application.Dtos.AuthDtos
{
    public class AdminStatusUpdateDto
    {
        public string UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
