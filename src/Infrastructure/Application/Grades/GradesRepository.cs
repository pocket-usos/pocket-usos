using App.Domain.Courses;
using App.Domain.Grades;
using App.Infrastructure.Integration.Usos.Grades;

namespace App.Infrastructure.Application.Grades;

public class GradesRepository(IGradesProvider gradesProvider, ICourseRepository courseRepository) : IGradesRepository
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
                var course = await courseRepository.GetCourse(courseId);
                var termCourse = new TermCourse(courseId, course.Name);

                foreach (var (courseUnitId, courseUnitGrades) in courseUnits.CourseUnitsGrades)
                {
                    var courseUnit = await courseRepository.GetCourseUnit(courseUnitId);
                    var termCourseUnit = new TermCourseUnit(courseUnitId, courseUnit.Type);

                    foreach (var (sessionNumber, grade) in courseUnitGrades[0])
                    {
                        if (grade is null) continue;

                        var sessionGrade = CreateSessionGrade(sessionNumber, grade);
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

    private static SessionGrade CreateSessionGrade(string sessionNumber, GradeDto grade)
    {
        var sessionGrade = new SessionGrade
        {
            SessionNumber = sessionNumber,
            ExamId = grade.ExamId.ToString(),
            Grade = grade.ValueSymbol,
            GradeDescription = grade.ValueDescription["pl"],
            Passes = grade.Passes,
            CountsIntoAverage = grade.CountsIntoAverage == "T",
            Comment = grade.Comment,
            GradeTypeId = grade.GradeTypeId,
            ModifiedAt = DateTime.Parse(grade.DateModified),
            ModifiedBy = new GradeAuthor
            {
                Id = grade.ModificationAuthor.Id,
                FirstName = grade.ModificationAuthor.FirstName,
                LastName = grade.ModificationAuthor.LastName
            }
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
}
