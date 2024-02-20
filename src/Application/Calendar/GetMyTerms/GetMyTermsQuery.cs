using App.Application.Contracts;
using App.Domain.Calendar;

namespace App.Application.Calendar.GetMyTerms;

public class GetMyTermsQuery : QueryBase<IEnumerable<Term>>;
