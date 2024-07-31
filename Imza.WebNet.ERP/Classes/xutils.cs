using System;
using System.Globalization;
using Imza.WebTools.Classes;

namespace Imza.WebNet.Erp.Classes
{
    public static class xutilsStatic
    {
                
    }

    public class xutils
    {

        public class getTarihBaslama
        {

            /// <summary>
            /// Yıl ve Hafta No Verilerek o Haftanın Başlangıç Gününü Alma
            /// </summary>
            /// <param name="year">Yıl</param>
            /// <param name="weekOfYear"> Hafta</param>
            /// <returns></returns>
            public DateTime HaftaninIlkGunu(int year, int weekOfYear)
            {
                DateTime jan1 = new DateTime(year, 1, 1);
                int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

                // Use first Thursday in January to get first week of the year as
                // it will never be in Week 52/53
                DateTime firstThursday = jan1.AddDays(daysOffset);
                var cal = CultureInfo.CurrentCulture.Calendar;
                int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                var weekNum = weekOfYear;
                // As we're adding days to a date in Week 1,
                // we need to subtract 1 in order to get the right date for week #1
                if (firstWeek == 1)
                {
                    weekNum -= 1;
                }

                // Using the first Thursday as starting week ensures that we are starting in the right year
                // then we add number of weeks multiplied with days
                var result = firstThursday.AddDays(weekNum * 7);

                // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
                return result.AddDays(-3);
            }

            public int GetWeekNumber(DateTime now)
            {
                CultureInfo ci = CultureInfo.CurrentCulture;
                int weekNumber = ci.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                return weekNumber;
            }
        }
        public int ComboTextToId(string pTableName,string pColumnName,string pColumnValue)
        {
            var dtTable = SQL.GetDataTable($"select * from {pTableName} where {pColumnName}='{pColumnValue}'");

            if (dtTable.Rows.Count==1)
            {
                return Convert.ToInt32(dtTable.Rows[0]["ID"]);
            }

            if (dtTable.Rows.Count==0)
            {
                var vId = dbStaticUtils.GetSequenceValue($"SQE_{pTableName}_ID");

                var vParamList = new dbStruct.StDbParamList();

                vParamList.Add("ID", vId);
                vParamList.Add(pColumnName, pColumnValue);
                vParamList.Add("INSERT_USERID", 1);

                SQL.ExecuteTableInsert(pTableName, vParamList);

                return Convert.ToInt32(vId);
            }

            return -1;
        }

        public string ConvertDateCustom(string pDate)
        {

            try
            {
                var vDate = Utility.Nvl(pDate);

                if (!string.IsNullOrWhiteSpace(vDate))
                {
                    if (vDate.Length>10)
                    {
                        return vDate.Substring(0, 10);
                    }
                    else
                    {
                        return vDate;
                    }
                }
            }
            catch
            {
                return "";
            }


            return "";

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pTarih"></param>
        /// <param name="pTarihStart"> Başlangıç Tarihi için true , bitiş tarihi için false</param>
        /// <returns></returns>
        public string SqlDateTimeFormatSpecial(string pTarih,bool pTarihStart)
        {
            try
            {

                if (string.IsNullOrEmpty(pTarih))
                {
                    return "";
                }
                else
                {

                    if (pTarih.Length > 14)
                    {
                        string value_from_start_date = pTarih.Substring(0, 10);
                        string value_from_end_date = pTarih.Substring(14, 10);

                        if (pTarihStart)
                        {
                            return value_from_start_date;
                        }
                        else
                        {
                            return value_from_end_date;
                        }
                    }
                    else
                    {
                        return pTarih;
                    }
                }
            }
            catch (Exception hata)
            {
                
                throw;
            }
        }

        public string SqlDateTimeFormat(string pTarih, string pKolonAdi, bool pBasinda_AND_Olacakmi = true)
        {
            if (string.IsNullOrEmpty(pTarih))
            {
                return "";
            }
            else
            {

                if (pTarih.Length > 14)
                {
                    string value_from_start_date = pTarih.Substring(0, 10);
                    string value_from_end_date = pTarih.Substring(14, 10);

                    string vAnd = "";

                    if (pBasinda_AND_Olacakmi)
                    {
                        vAnd = $" AND ";
                    }

                    return $" {vAnd} convert(date,{pKolonAdi},104) between convert(date,'{Convert.ToDateTime(value_from_start_date).ToShortDateString()}',104) AND convert(date,'{Convert.ToDateTime(value_from_end_date).ToShortDateString()}',104)";
                }
                else
                {

                    string vAnd = "";

                    if (pBasinda_AND_Olacakmi)
                    {
                        vAnd = $" AND ";
                    }

                    return $" {vAnd} convert(date,{pKolonAdi},104) = convert(date,'{Convert.ToDateTime(pTarih)}',104)";
                }
            }
        }
    }
}