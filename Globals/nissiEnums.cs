using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globals
{
    public enum ePmsEnum { Korean, English }
    public enum WorkingForm { 사무직, 일용직, 생산직, 기술직, 기타 }
    public enum Sex { 남성, 여성 }
    public enum EstimateType { 요약, 상세 }
    public enum IsLocal { 내국인, 외국인 }
    public enum IsHeadOfHousehold { 유, 무 }
    public enum IsDisabled { 해당, 비해당 }
    public enum IsReligion { 해당, 비해당 }
    public enum IsResident { 거주, 비거주 }
    public enum IsWorking { 재직, 휴직 }
    public enum IsApprenticeship { 여, 부 }
    public enum LongevityIncluded { 여, 부 }
    public enum Calendar { 양력, 음력 }
    public enum CalculationUnit { percent, 금액 }
    public enum InventoryStatus { 유, 무 }
    public enum TrackingProgress { 출하대기, 출하완료 }
    public enum PlateForm { 신관, 구관 }
    public enum MasterPlateForm { 습식A, 습식B }
    public enum LeaveOfAbsence { 유, 무 }//
}
