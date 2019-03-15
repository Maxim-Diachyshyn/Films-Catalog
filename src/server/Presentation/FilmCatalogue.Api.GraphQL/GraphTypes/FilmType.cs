﻿using FilmCatalogue.Domain.DataTypes.Films;
using FilmCatalogue.Application.UseCases.Reviews.Requests;
using GraphQL.DataLoader;
using GraphQL.Types;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace FilmCatalogue.Api.GraphQL.GraphTypes
{
    public class FilmType : ObjectGraphType<Film>
    {
        public FilmType(IHttpContextAccessor accessor)
        {
            Field<IdGraphType>()
                .Name(nameof(Film.Id))
                .Resolve(x => (Guid)x.Source.Id);
            Field(x => x.Name);
            Field(x => x.ShowedDate, false, typeof(DateTimeGraphType));
            Field(x => x.AddedAt, false, typeof(DateTimeGraphType));
            Field<StringGraphType>()
                .Name(nameof(Film.Photo))
                .Resolve(x => x.Source.Photo?.Base64);
            Field<ListGraphType<ReviewType>>()
                .Name("Reviews")
                .ResolveAsync(async ctx => 
                {
                    var mediator = (IMediator)accessor.HttpContext.RequestServices.GetService(typeof(IMediator));
                    return await mediator.Send(new GetReviewsRequest(ctx.Source.Id));
                });
            Field<DecimalGraphType>()
                .Name("Rate")
                //context.TryAsyncResolve() maybe this for reviews than average
                .ResolveAsync(async ctx =>
                {
                    var mediator = (IMediator)accessor.HttpContext.RequestServices.GetService(typeof(IMediator));
                    return await mediator.Send(new GetRateRequest(ctx.Source.Id));
                });
        }
    }
}
