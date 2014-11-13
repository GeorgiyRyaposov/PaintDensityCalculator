using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaintDensityCalculator.Models;
using PaintDensityCalculator.Utils;

namespace PaintDensityCalculator.BLL
{
    public class DensityBll
    {
        private const string DensityFileName = "Density";

        private GenericSerializer<Density> _dataContext;
        public GenericSerializer<Density> DataContext
        {
            get
            {
                if (_dataContext == null)
                {
                    _dataContext = new GenericSerializer<Density>();
                }
                return _dataContext;
            }
            set { _dataContext = value; }
        }

        public void SaveDensityItem(Density newDensityItem)
        {
            GenericSerializer<Density>.WriteFile(DensityFileName, newDensityItem);
        }
    }
}
