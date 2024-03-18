using App.Application.Configuration;
using App.Application.Grades;
using App.Application.Shared;
using App.Infrastructure.Integration.Usos.Courses;
using App.Infrastructure.Integration.Usos.Grades;
using App.Infrastructure.Translations;

namespace App.Infrastructure.Application.Grades;

public class GradesRepository(IGradesProvider gradesProvider, ICoursesProvider coursesProvider, IExecutionContextAccessor context) : IGradesRepository
{
    public async Task<TermGrades> GetGradesForTerm(string term)
    {
        var grades = await gradesProvider.GetGradesForTerm(term);

        var termGrades = new TermGrades(term);
        var gradesToCalculateAverage = new List<decimal>();

        foreach (var (_, courses) in grades)
        {
            foreach (var (courseId, courseUnits) in courses)
            {
                var course = await GetCourse(courseId);
                var termCourse = new TermCourse(courseId, course.Name);

                foreach (var (courseUnitId, courseUnitGrades) in courseUnits.CourseUnitsGrades)
                {
                    var courseUnit = await GetCourseUnit(courseUnitId);
                    var termCourseUnit = new TermCourseUnit(courseUnitId, courseUnit.Type);

                    foreach (var (sessionNumber, grade) in courseUnitGrades[0])
                    {
                        if (grade is null) continue;

                        var sessionGrade = CreateSessionGrade(sessionNumber, grade, context.Language);
                        termCourseUnit.Grades.Add(sessionGrade);

                        if (sessionGrade.CountsIntoAverage && decimal.TryParse(sessionGrade.Grade.Replace(",", "."), out var decimalGrade))
                        {
                            gradesToCalculateAverage.Add(decimalGrade);
                        }
                    }

                    await GetGradesDistribution(termCourseUnit);

                    termCourse.Units.Add(termCourseUnit);
                }

                termGrades.Courses.Add(termCourse);
            }
        }

        if (gradesToCalculateAverage.Count > 0)
        {
            termGrades.AverageGrade = Math.Round(gradesToCalculateAverage.Sum() / gradesToCalculateAverage.Count, 2);
        }

        return termGrades;
    }

    private static SessionGrade CreateSessionGrade(string sessionNumber, GradeDto grade, string language)
    {
        var isModifiedAt = DateTime.TryParse(grade.DateAcquisition, out var modifiedAt);

        GradeAuthor? modifiedBy = null;
        if (grade.ModificationAuthor is not null)
        {
            modifiedBy = new GradeAuthor
            {
                Id = grade.ModificationAuthor.Id,
                FirstName = grade.ModificationAuthor.FirstName,
                LastName = grade.ModificationAuthor.LastName
            };
        }

        var sessionGrade = new SessionGrade
        {
            SessionNumber = sessionNumber,
            ExamId = grade.ExamId.ToString(),
            Grade = grade.ValueSymbol,
            GradeDescription = grade.ValueDescription.Translate(language),
            Passes = grade.Passes,
            CountsIntoAverage = grade.CountsIntoAverage == "T",
            Comment = grade.Comment,
            GradeTypeId = grade.GradeTypeId,
            ModifiedAt = isModifiedAt ? modifiedAt : null,
            ModifiedBy = modifiedBy,
        };
        return sessionGrade;
    }

    private async Task GetGradesDistribution(TermCourseUnit termCourseUnit)
    {
        var examId = termCourseUnit.Grades.SingleOrDefault()?.ExamId;

        if (examId is not null)
        {
            var gradesDistributionDto = await gradesProvider.GetExamGradesDistribution(examId);
            var gradesDistribution = gradesDistributionDto.GradesDistribution.Select(d => new GradesDistributionItem
            {
                Percentage = d.Percentage,
                Grade = d.GradeSymbol
            }).Where(d => d.Percentage > 0).ToArray();
            termCourseUnit.GradesDistribution = gradesDistribution;
        }
    }

    private async Task<Course> GetCourse(string id)
    {
        var course = await coursesProvider.GetCourse(id);

        return new Course(id, course.Name.Translate(context.Language));
    }

    private async Task<CourseUnit> GetCourseUnit(string id)
    {
        var classTypeId = await coursesProvider.GetCourseUnitTypeId(id);
        var classTypes = await coursesProvider.GetClassTypes();
        var classTypeDto = classTypes[classTypeId];
        var classType = new ClassType(classTypeDto.Id, classTypeDto.Name.Translate(context.Language));

        return new CourseUnit(id, classType);
    }

    private class Course(string id, string name)
    {
        public string Id { get; set; } = id;

        public string Name { get; set; } = name;
    }

    private class CourseUnit(string id, ClassType type)
    {
        public string Id { get; set; } = id;

        public ClassType Type { get; set; } = type;
    }
}
