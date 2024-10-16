using App.Application.Configuration;
using App.Application.Schedule;
using App.Application.Shared;
using App.Application.Users;
using App.Infrastructure.Integration.Usos.Terms;
using App.Infrastructure.Integration.Usos.TimeTable;
using App.Infrastructure.Translations;

namespace App.Infrastructure.Application.Schedule;

public class ScheduleRepository(ITimeTableProvider timeTableProvider, IUserRepository userRepository, ITermsProvider termsProvider, IExecutionContextAccessor context) : IScheduleRepository
{
    private const int DefaultScheduleDaysCount = 7;

    public async Task<IEnumerable<ScheduleItem>> GetSchedule(DateOnly? start, int? days)
    {
        var timeTable = (await timeTableProvider.GetUserTimeTable(start ?? DateOnly.FromDateTime(DateTime.Now), days ?? DefaultScheduleDaysCount)).ToList();

        var schedule = new List<ScheduleItem>();
        foreach (var item in timeTable)
        {
            var lecturers = await userRepository.GetMultipleAsync(item.LecturerIds.Select(id => id.ToString()).ToArray());

            var scheduleItem = new ScheduleItem
            {
                Start = DateTime.Parse(item.StartTime),
                End = DateTime.Parse(item.EndTime),
                Name = item.Name.Translate(context.Language),
                CourseId = item.CourseId,
                CourseUnitId = item.UnitId,
                ClassType = new ClassType(item.ClasstypeId, item.ClasstypeName.Translate(context.Language)),
                GroupNumber = item.GroupNumber,
                Room = item.RoomId is not null ? new Room
                {
                    Id = item.RoomId.Value,
                    Name = item.RoomNumber
                } : null,
                Lecturers = lecturers.Select(l => new Lecturer
                {
                    Id = l.Id,
                    FirstName = l.FirstName,
                    LastName = l.LastName,
                    PhotoUrl = l.PhotoUrl
                }).ToList()
            };

            schedule.Add(scheduleItem);
        }

        return schedule;
    }

    public async Task<IEnumerable<ScheduleItem>> GetLecturerSchedule(string userId, DateOnly? start, int? days)
    {
        var timeTable = (await timeTableProvider.GetStaffTimeTable(userId, start ?? DateOnly.FromDateTime(DateTime.Now), days ?? DefaultScheduleDaysCount)).ToList();

        var schedule = new List<ScheduleItem>();
        foreach (var item in timeTable)
        {
            var scheduleItem = new ScheduleItem
            {
                Start = DateTime.Parse(item.StartTime),
                End = DateTime.Parse(item.EndTime),
                Name = item.Name.Translate(context.Language),
                CourseId = item.CourseId,
                CourseUnitId = item.UnitId,
                ClassType = new ClassType(item.ClasstypeId, item.ClasstypeName.Translate(context.Language)),
                GroupNumber = item.GroupNumber,
                Room = item.RoomId is not null ? new Room
                {
                    Id = item.RoomId.Value,
                    Name = item.RoomNumber
                } : null,
                Lecturers = new List<Lecturer>()
            };

            schedule.Add(scheduleItem);
        }

        return schedule;
    }

    public async Task<IEnumerable<Term>> GetTerms()
    {
        var terms = await termsProvider.GetTerms();

        return terms.Terms.Select(term => new Term
        {
            Id = term.Id,
            Name = term.Name.Translate(context.Language),
            StartDate = DateOnly.Parse(term.StartDate),
            EndDate = DateOnly.Parse(term.EndDate)
        });
    }
}
