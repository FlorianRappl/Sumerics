using System;
using YAMP.Attributes;
using YAMP.Core;

namespace YAMP.Core.Functions
{
    [Description("The date() function allows you to get access to the current time and date.")]
    [Kind(KindAttribute.FunctionKind.System)]
    sealed class DateFunction : YFunction
    {
        [Description("Gets the current time, taken at the moment of the query request.")]
        [Example("date()", "Gets a date object with the current time.")]
        public Date Invoke()
        {
            return new Date();
        }

        public class Date
        {
            DateTime _value;

            public Date()
            {
                _value = DateTime.Now;
            }

            [Description("Gets or sets the second component of the current time.")]
            public Int64 second
            {
                get { return _value.Second; }
                set
                {
                    _value = new DateTime(_value.Year, _value.Month, _value.Day, _value.Hour, _value.Minute, (int)value);
                }
            }

            [Description("Gets or sets the minute component of the current time.")]
            public Int64 minute
            {
                get { return _value.Minute; }
                set
                {
                    _value = new DateTime(_value.Year, _value.Month, _value.Day, _value.Hour, (int)value, _value.Second);
                }
            }

            [Description("Gets or sets the hour component of the current time.")]
            public Int64 hour
            {
                get { return _value.Hour; }
                set
                {
                    _value = new DateTime(_value.Year, _value.Month, _value.Day, (int)value, _value.Minute, _value.Second);
                }
            }

            [Description("Gets or sets the day component of the current time.")]
            public Int64 day
            {
                get { return _value.Day; }
                set
                {
                    _value = new DateTime(_value.Year, _value.Month, (int)value, _value.Hour, _value.Minute, _value.Second);
                }
            }

            [Description("Gets or sets the month component of the current time.")]
            public Int64 month
            {
                get { return _value.Month; }
                set
                {
                    _value = new DateTime(_value.Year, (int)value, _value.Day, _value.Hour, _value.Minute, _value.Second);
                }
            }

            [Description("Gets or sets the year component of the current time.")]
            public Int64 year
            {
                get { return _value.Year; }
                set
                {
                    _value = new DateTime((int)value, _value.Month, _value.Day, _value.Hour, _value.Minute, _value.Second);
                }
            }

            [Description("Adds the given number of minutes.")]
            public Date addMinutes(Int64 min)
            {
                _value = _value.AddMinutes(min);
                return this;
            }

            [Description("Adds the given number of seconds.")]
            public Date addSeconds(Int64 sec)
            {
                _value = _value.AddSeconds(sec);
                return this;
            }

            [Description("Adds the given number of hours.")]
            public Date addHours(Int64 h)
            {
                _value = _value.AddHours(h);
                return this;
            }

            [Description("Adds the given number of days.")]
            public Date addDays(Int64 d)
            {
                _value = _value.AddDays(d);
                return this;
            }

            [Description("Subtracts the given number of minutes.")]
            public Date subtractMinutes(Int64 min)
            {
                _value = _value.AddMinutes(-min);
                return this;
            }

            [Description("Subtracts the given number of seconds.")]
            public Date subtractSeconds(Int64 sec)
            {
                _value = _value.AddSeconds(-sec);
                return this;
            }

            [Description("Subtracts the given number of hours.")]
            public Date subtractHours(Int64 h)
            {
                _value = _value.AddHours(-h);
                return this;
            }

            [Description("Subtracts the given number of days.")]
            public Date subtractDays(Int64 d)
            {
                _value = _value.AddDays(-d);
                return this;
            }

            public override string ToString()
            {
                return _value.ToString();
            }
        }
    }
}
