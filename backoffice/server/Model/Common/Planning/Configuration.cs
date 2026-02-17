using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.NetworkInformation;

namespace ECCSA.BackOffice.Server.Model.Common.Planning
{
    public class Configuration
    {
        public const string RedColor = "#FF0000;";
        public const string OrangeColor = "#FFA200;";
        public const string YelloColor = "#F7FF00;";
        public const string YellowGreen = "#A3FF00;";
        public const string GreenColor = "#00FF00;";
        public Configuration()
        {
           
        }
        public int Id { get; set; }

       
        public string BackgroundColor
        {
            get
            {
                

                return GreenColor;

            }
        }
       
        public List<int> ClosedYears { get; set; } 
        public int PlanYear { get; set; }
        public int VersionNo { get; set; }
        public int CurrentYear { get; set; }
        public int CurrentMonth { get; set; }

        [NotMapped]
        public int MinOperationYear { get; set; }
        [NotMapped]
        public int MaxOperationYear { get; set; }

        public PlanningPhaseEnum PlanningPhase { get; set; }
        public int PlanVersionNo { get; set; }
        public decimal Yello { get; set; } = 0.20m; // 21 - 50%

        public decimal Blue { get; set; } = 0.50m; // 51 - 65 %

        public decimal BlueGreen { get; set; } = 0.65m; // 61 - 79%

        public decimal Green { get; set; } = 0.80m; // 80 - 100%

        [NotMapped] public int YelloValue => (int)(Yello * 100);

        [NotMapped] public int BlueValue => (int)(Blue * 100);

        [NotMapped] public int BlueGreenValue => (int)(BlueGreen * 100);

        [NotMapped] public int GreenValue => (int)(Green * 100);

        public string Calendar { get; set; } = "E";

        [NotMapped]
        public List<QuarterMonthMap> Map { get; set; }
        [NotMapped]
        public DateTime PlanStartDate
        {
            get
            {
                if (Map == null)
                {
                    return new DateTime(PlanYear + 8, 7, 1);
                }
                var m = Map.Where(x => x.Quarter == 1).Min(x => x.Month);
                return new DateTime(PlanYear + 8, m, 1);
            }
        }
        [NotMapped]
        public DateTime CurrentStartDate
        {
            get
            {
                if (Map == null)
                {
                    return new DateTime(CurrentYear + 8, 7, 1);
                }
                var m = Map.Where(x => x.Quarter == 1).Min(x => x.Month);
                return new DateTime(CurrentYear + 8, m, 1);
            }
        }

        [NotMapped]
        public DateTime PlanEndDate => PlanStartDate.AddYears(1).AddDays(-1).Date;
        [NotMapped]
        public DateTime CurrentEndDate => CurrentStartDate.AddYears(1).AddDays(-1).Date;


    }
}