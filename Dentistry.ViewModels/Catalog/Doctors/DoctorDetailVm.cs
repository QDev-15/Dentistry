using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Doctors
{
    public class DoctorDetailVm
    {
        public DoctorVm Doctor { set; get; }
        public List<DoctorVm> Doctors { set; get; } = new List<DoctorVm>();
    }
}
