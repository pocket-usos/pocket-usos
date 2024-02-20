using App.Domain.Calendar;
using App.Domain.Courses;
using App.Domain.Users;
using App.Infrastructure.Integration.Usos.Terms;
using App.Infrastructure.Integration.Usos.TimeTable;

namespace App.Infrastructure.Domain.Calendar;

public class CalendarRepository(ITimeTableProvider timeTableProvider, IUserRepository userRepository, ITermsProvider termsProvider) : ICalendarRepository
{
    public async Task<IEnumerable<CalendarItem>> GetCalendar(DateOnly? start, int? days)
    {
        var timeTable = (await timeTableProvider.GetUserTimeTable(start, days)).ToList();

        var calendar = new List<CalendarItem>();
        foreach (var item in timeTable)
        {
            var lecturers = await userRepository.GetMultipleAsync(item.LecturerIds.Select(id => id.ToString()).ToArray());

            var calendarItem = new CalendarItem
            {
                Start = DateTime.Parse(item.StartTime),
                End = DateTime.Parse(item.EndTime),
                Name = item.Name["pl"],
                CourseId = item.CourseId,
                CourseUnitId = item.UnitId,
                ClassType = new ClassType(item.ClasstypeId, item.ClasstypeName["pl"]),
                GroupNumber = item.GroupNumber,
                Room = new Room
                {
                    Id = item.RoomId,
                    Name = item.RoomNumber
                },
                Lecturers = lecturers.Select(l => new Lecturer
                {
                    Id = l.Id,
                    FirstName = l.FirstName,
                    LastName = l.LastName,
                    PhotoUrl = l.PhotoUrl
                }).ToList()
            };

            calendar.Add(calendarItem);
        }

        return calendar;
    }

    public async Task<IEnumerable<Term>> GetTerms()
    {
        var terms = await termsProvider.GetTerms();

        return terms.Terms.Select(term => new Term
        {
            Id = term.Id,
            Name = term.Name["pl"],
            StartDate = DateOnly.Parse(term.StartDate),
            EndDate = DateOnly.Parse(term.EndDate)
        });
    }
}
