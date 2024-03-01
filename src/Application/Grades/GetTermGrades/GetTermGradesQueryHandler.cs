using App.Application.Configuration.Queries;

namespace App.Application.Grades.GetTermGrades;

public class GetTermGradesQueryHandler(IGradesRepository gradesRepository) : IQueryHandler<GetTermGradesQuery, TermGrades>
{
    public async Task<TermGrades> Handle(GetTermGradesQuery query, CancellationToken cancellationToken)
    {
        var currentTerm = DateTime.Now.Year + "Z";

        var termsGrades = await gradesRepository.GetGradesForTerm(query.Term ?? currentTerm);

        return termsGrades;
    }
}
